using LoggingService;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Text;
using System.Net.Http;

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
                //REVIEW: В настройки, ресурсы, константы
                var bitmap = (Bitmap)Image.FromFile(@"VisualResources\Images\NewRecordTemplate.jpg");
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
            catch
            {
                //REVIEW: А какая ошибка-то? Вот, я увидел, что у меня софтина хлопнулась с этой строкой - как мне это поможет разобраться?
                //REVIEW: Тут как минимум может быть 3 ошибки разных
                GlobalLogger.Instance.Error("Произошла ошибка при создании изображения");
                return false;
            }
        }

        public string GetUserName()
        {
            try
            {
                //REVIEW:В константы урлу
                var url = "https://api.vk.com/method/account.getProfileInfo?access_token=" + Properties.Settings.Default.VkToken;
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
            catch
            {
                //REVIEW:Опять же, какая ошибка?
                GlobalLogger.Instance.Error("Произошла ошибка при получении имени пользователя");
                return "Введите Имя";
            }
        }

        private string GetUploadServer()
        {
            try
            {
                //REVIEW:Урлу в константы
                var url = "https://api.vk.com/method/photos.getWallUploadServer?access_token=" + Properties.Settings.Default.VkToken;
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
            catch
            {
                //REVIEW:Ошибку выводить точно
                GlobalLogger.Instance.Error("Произошла ошибка при получении сервера VK");
                return "error";
            }
        }

        private dynamic UploadPhoto()
        {
            try
            {
                var server = GetUploadServer();
                //REVIEW:А вот тут лучше enum возвращать или обрабатывать Exception. А то сразу много вопросов. А если Error? или ERROR? Или " error "?
                if (server == "error")
                {
                    GlobalLogger.Instance.Error("Произошла ошибка при получении json загрузки фото");
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
            catch
            {
                //REVIEW:Какая ошибка?
                GlobalLogger.Instance.Error("Произошла ошибка при получении json загрузки фото");
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
                    GlobalLogger.Instance.Error("Произошла ошибка при получении id фото");
                    return "error";
                }
                //REVIEW:урлу - в константы
                var url = "https://api.vk.com/method/photos.saveWallPhoto?access_token=" + Properties.Settings.Default.VkToken;
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
            catch
            {
                //REVIEW:Какая ошибка?
                GlobalLogger.Instance.Error("Произошла ошибка при получении id фото");
                //REVIEW: Лучше тогда уж null или пустую строку. Или throw; , чтобы снаружи поняли, что что-то пошло не так
                return "error";
            }
        }

        private void DeletePhoto()
        {
            try
            {
                File.Delete("temp.png");
                GlobalLogger.Instance.Info("Файл temp.png был успешно удалён");
            }
            catch
            {
                GlobalLogger.Instance.Error("Не удалось удалить файл temp.png");
            }
        }

        private bool PostResult()
        {
            try
            {
                var photoId = GetPhotoId();
                if (photoId == "error")
                {
                    DeletePhoto();
                    GlobalLogger.Instance.Error("Размещение записи завершилось ошибкой");
                    return false;
                }
                //REVIEW: Урлы в константы
                var url = "https://api.vk.com/method/wall.post?access_token=" + Properties.Settings.Default.VkToken;
                url += "&attachments=https://github.com/Julistian/SAPR-15-1_Razumov/tree/master/OneQuestionFourAnswers," + photoId;
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
            catch
            {
                DeletePhoto();
                GlobalLogger.Instance.Error("Размещение записи завершилось ошибкой");
                return false;
            }
        }

        public string GetAuthUrl()
        {
            //REVIEW:Урлы в константы
            var url = "https://oauth.vk.com/authorize?";
            url += "client_id=" + Properties.Settings.Default.AppID;
            url += "&redirect_uri=https://oauth.vk.com/blank.html";
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
