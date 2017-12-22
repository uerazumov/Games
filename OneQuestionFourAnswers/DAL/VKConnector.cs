using LoggingService;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Text;
using System.Net.Http;
using System;
using System.Configuration;

namespace DAL
{
    public class VKConnector
    {

        public bool CreateRec(int score)
        {
            if (!CreatePicture(score))
            {
                return false;
            }
            return PostResult();
        }

        public void LogOut()
        {
            Properties.Settings.Default.VkToken = "?";
            Properties.Settings.Default.UserID = "";
            Properties.Settings.Default.Save();
            GlobalLogger.Instance.Info("Данные пользователя были удалены");
        }

        private bool CreatePicture(int score)
        {
            try
            {
                var bitmap = (Bitmap)Image.FromFile(ConfigurationManager.AppSettings["newRecordImageTemplate"]);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    PrivateFontCollection pfc = new PrivateFontCollection();
                    pfc.AddFontFile(@"VisualResources\Resphekt.ttf");
                    using (var font = new Font(pfc.Families[0], 70))
                    {
                        var size = graphics.MeasureString(score.ToString(), font);
                        var startX = bitmap.Width / 2 - size.Width / 2;
                        graphics.DrawString(score.ToString(), font, Brushes.Black, new Point((int)startX, 411));
                    }
                }
                bitmap.Save("temp.png");
                GlobalLogger.Instance.Info("Изображение было успешно создано");
                return true;
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при создании изображения");
                return false;
            }
        }

        public string GetUserName()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["getUserNameUrl"] + Properties.Settings.Default.VkToken;
                var request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var raw = reader.ReadToEnd();
                            dynamic json = JsonConvert.DeserializeObject(raw);
                            GlobalLogger.Instance.Info("Имя пользователя успешно получено");
                            return (string)json.response.first_name;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении имени пользователя");
                return null;
            }
        }

        private string GetUploadServer()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["getUploadServerUrl"] + Properties.Settings.Default.VkToken;
                var request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var raw = reader.ReadToEnd();
                            dynamic json = JsonConvert.DeserializeObject(raw);
                            GlobalLogger.Instance.Info("Получение сервера VK прошло успешно");
                            return (string)json.response.upload_url;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении сервера VK");
                return "";
            }
        }

        private dynamic UploadPhoto()
        {
            try
            {
                var server = GetUploadServer();
                if (server == "")
                {
                    GlobalLogger.Instance.Error("Получение json загрузки фото было прервано из-за ошибки на предыдущем этапе");
                    return null;
                }
                using (var httpClient = new HttpClient())
                {
                    var form = new MultipartFormDataContent();
                    var bytes = File.ReadAllBytes("temp.png");
                    form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "photo", "temp.png");
                    var response = httpClient.PostAsync(server, form).Result;
                    response.EnsureSuccessStatusCode();
                    var raw = response.Content.ReadAsStringAsync().Result;
                    GlobalLogger.Instance.Info("Получение json загрузки фото прошло успешно");
                    return JsonConvert.DeserializeObject(raw);
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении json загрузки фото");
                return null;
            }
        }

        private string GetPhotoId()
        {
            try
            {
                var data = UploadPhoto();
                if (data == null)
                {
                    GlobalLogger.Instance.Error("Получение id фото было прервано из-за ошибки на предыдущем этапе");
                    return "";
                }
                var url = ConfigurationManager.AppSettings["getPhotoIdUrl"] + Properties.Settings.Default.VkToken;
                url += "&user_id=" + Properties.Settings.Default.UserID;
                url += "&photo=" + data.photo;
                url += "&server=" + data.server;
                url += "&hash=" + data.hash;
                var request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var raw = reader.ReadToEnd();
                            dynamic json = JsonConvert.DeserializeObject(raw);
                            GlobalLogger.Instance.Info("Получение id фото прошло успешно");
                            return (string)json.response[0].id;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении id фото");
                return "";
            }
        }

        private void DeletePhoto()
        {
            try
            {
                File.Delete("temp.png");
                GlobalLogger.Instance.Info("Файл temp.png был успешно удалён");
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Не удалось удалить файл temp.png. Текст ошибки " + e.Message);
            }
        }

        private bool PostResult()
        {
            try
            {
                var photoId = GetPhotoId();
                if (photoId == "")
                {
                    DeletePhoto();
                    GlobalLogger.Instance.Error("Размещение записи было прервано из-за ошибки на предыдущем этапе");
                    return false;
                }
                var url = ConfigurationManager.AppSettings["postUrlFirst"] + Properties.Settings.Default.VkToken;
                url += "&attachments=" + ConfigurationManager.AppSettings["postUrlSecond"] + "," + photoId;
                var request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var raw = reader.ReadToEnd();
                            dynamic json = JsonConvert.DeserializeObject(raw);
                            GlobalLogger.Instance.Info("Размещение записи прошло успешно. ID записи на стене: " + json.response.post_id.ToString());
                        }
                    }
                }
                DeletePhoto();
                return true;
            }
            catch (Exception e)
            {
                DeletePhoto();
                GlobalLogger.Instance.Error("Размещение записи завершилось ошибкой " + e.Message);
                return false;
            }
        }

        public string GetAuthUrl()
        {
            var url = ConfigurationManager.AppSettings["getAuthUrlFirst"];
            url += "client_id=" + Properties.Settings.Default.AppID;
            url += "&redirect_uri=" + ConfigurationManager.AppSettings["getAuthUrlSecond"];
            url += "&display=page";
            url += "&scope=wall,offline,photos";
            url += "&response_type=token";
            GlobalLogger.Instance.Info("URL создан: " + url);
            return url;
        }

        public void SaveToken(string token, string userID)
        {
            Properties.Settings.Default.VkToken = token;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.UserID = userID;
            Properties.Settings.Default.Save();
        }

        public bool IsTokenExist()
        {
            if (Properties.Settings.Default.VkToken != "?")
            {
                return true;
            }
            return false;
        }
    }
}
