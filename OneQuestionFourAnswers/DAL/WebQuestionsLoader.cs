using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using LibraryClass;
using LoggingService;
using System.Configuration;

namespace DAL
{
    public class WebQuestionsLoader : IQuestionsLoader
    {
        public readonly int TotalPages;
        private readonly Random _random;
        private int _processedPage = 0;

        public WebQuestionsLoader()
        {
            TotalPages = GetTotalPages();
            _random = new Random();
        }

        private static int GetTotalPages()
        {
            var url = ConfigurationManager.AppSettings["baseUrl"];
            int totalPages;
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var xpath = ConfigurationManager.AppSettings["xpath"];
                var pages = doc.DocumentNode.SelectSingleNode(xpath).GetAttributeValue("href", "");
                totalPages = Convert.ToInt16(pages.Split('/').Last()) / 10;
            }
            catch (Exception e)
            {
                totalPages = 0;
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении кол-ва страниц с сайта " + url.ToString());
            }
            return totalPages;
        }

        public List<QuestionAnswers> GetQuestionsFromPage(int page)
        {
            var questions = new List<QuestionAnswers>();
            GlobalLogger.Instance.Debug("Обработка страницы " + page.ToString() + " из " + TotalPages.ToString());
            try
            {
                var url = ConfigurationManager.AppSettings["questionsUrl"] + (page * 10).ToString();
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var xpath = ConfigurationManager.AppSettings["questionsXpath"];
                var table = doc.DocumentNode.SelectSingleNode(xpath);
                var rows = table.ChildNodes.Where(node => node.Name == "tr").Skip(1);
                for (int i = 0; i < rows.Count(); i++)
                {
                    var row = rows.ElementAt(i);
                    try
                    {
                        var columns = row.ChildNodes.Where(node => node.Name == "td").ToList();
                        var question = new QuestionAnswers(GetQuestionText(columns), GetAnswers(columns));
                        questions.Add(question);
                    }
                    catch (Exception e)
                    {
                        GlobalLogger.Instance.Debug("Произошла ошибка " + e.Message + " при обработке " + i.ToString() + " строки");
                    }
                }
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Debug("Произошла ошибка " + e.Message + " при обработке страницы");
            }
            return questions;
        }

        private static List<Answer> GetAnswers(IReadOnlyCollection<HtmlNode> columns)
        {
            var answers = new List<Answer>();
            if (columns == null)
            {
                GlobalLogger.Instance.Debug("В метод GetAnswers было подано значение null");
                throw new InvalidDataException();
            }
            answers.AddRange(GetIncorrectAnswers(columns).Select(x => new Answer(x, false)));
            answers.Add(new Answer(GetCorrectAnswer(columns), true));
            for (var i = 0; i < 4; i++)
            {
                if (answers[i].Text.Length > 18)
                {
                    GlobalLogger.Instance.Debug("Текст ответа превышает допустимую длину");
                    throw new InvalidDataException();
                }
            }
            return answers.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static string GetQuestionText(IEnumerable<HtmlNode> columns)
        {
            if (columns == null)
            {
                GlobalLogger.Instance.Debug("В метод GetQuestionText было подано значение null");
                throw new ArgumentNullException();
            }
            var column = columns.ElementAt(1);
            var question = column.ChildNodes.First(node => node.Name == "a").InnerText.Trim();
            if (question.Length > 180)
            {
                GlobalLogger.Instance.Debug("Текст вопроса превышает допустимую длину");
                throw new InvalidDataException();
            }
            return question.Replace("&amp;", "&").Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022");
        }

        private static IEnumerable<string> GetIncorrectAnswers(IEnumerable<HtmlNode> columns)
        {
            if (columns == null)
            {
                GlobalLogger.Instance.Debug("В метод GetIncorrectAnswers было подано значение null");
                throw new ArgumentNullException();
            }
            var column = columns.ElementAt(1);
            var variantsText = column.ChildNodes.First(node => node.Name == "div").InnerText.Trim();
            var variants = variantsText.Split(':')[1].Split(',').Select(x => x.Trim().Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022")).ToList(); 
            if (variants.Count != 3)
            {
                GlobalLogger.Instance.Debug("Некорректное кол-во ответов");
                throw new InvalidDataException();
            }
            if (variants.Distinct().Count() != variants.Count())
            {
                GlobalLogger.Instance.Debug("Ответы дублируются");
                throw new InvalidDataException();
            }
            return variants;
        }

        private static string GetCorrectAnswer(IEnumerable<HtmlNode> columns)
        {
            if (columns == null)
            {
                GlobalLogger.Instance.Debug("В метод GetCorrectAnswer было подано значение null");
                throw new ArgumentNullException();
            }
            var column = columns.ElementAt(2);
            return column.InnerText.Trim().Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022");
        }

        public List<QuestionAnswers> LoadQuestions()
        {
            var questions = new List<QuestionAnswers>();
            for (_processedPage = 0; _processedPage <= TotalPages; _processedPage++)
            {
                questions.AddRange(GetQuestionsFromPage(_processedPage));
            }
            _processedPage = -1;
            return questions;
        }

        public int GetUpdateProcent()
        {
            try
            {
                return (_processedPage * 100) / TotalPages;
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("При получении числа обработанных страниц было вызвано исключение " + e.Message);
                return -1;
            }
        }
    }
}
