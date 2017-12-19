using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using LibraryClass;
using LoggingService;

namespace DAL
{
    public class WebQuestionsLoader : IQuestionsLoader
    {
        public readonly int TotalPages;
        private readonly Random _random;

        public WebQuestionsLoader()
        {
            TotalPages = GetTotalPages();
            _random = new Random();
        }

        private static int GetTotalPages()
        {
            //REVIEW:Это бы в константы или настройки
            const string url = "https://baza-otvetov.ru/categories/view/1/0";
            int totalPages;
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                //REVIEW:Это тоже кандидат на константы или настройки. Хотя, парсить html-код - это буэ...
                const string xpath = "/html/body/div/div/div/div[2]/div[2]/div/a[5]";
                var pages = doc.DocumentNode.SelectSingleNode(xpath).GetAttributeValue("href", "");
                totalPages = Convert.ToInt16(pages.Split('/').Last()) / 10;
            }
            catch
            {
                totalPages = 0;
                GlobalLogger.Instance.Error("Произошла ошибка при получении кол-ва страниц с сайта " + url.ToString());
            }
            return totalPages;
        }

        public List<QuestionAnswers> GetQuestionsFromPage(int page)
        {
            var questions = new List<QuestionAnswers>();
            GlobalLogger.Instance.Debug("Обработка страницы " + page.ToString() + " из " + TotalPages.ToString());
            try
            {
                //REVIEW:В константы или настройки и дополнять на месте
                var url = $"https://baza-otvetov.ru/categories/view/1/{page * 10}";
                var web = new HtmlWeb();
                var doc = web.Load(url);
                //REVIEW:В константы или настройки
                const string xpath = "/html/body/div/div/div/div[2]/div[2]/table";
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
                    catch
                    {
                        GlobalLogger.Instance.Debug("Произошла ошибка при обработке " + i.ToString() + " строки");
                    }
                }
            }
            catch
            {
                GlobalLogger.Instance.Debug("Произошла ошибка при обработке страницы");
            }
            return questions;
        }

        private static List<Answer> GetAnswers(IReadOnlyCollection<HtmlNode> columns)
        {
            var answers = new List<Answer>();
            //REVIEW:Что будет, если columns - null?
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
            //REVIEW:columns is null приведёт к NRE на ровном месте. И количество проверить.
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
            //REVIEW: Опять NRE и OutOfRange
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
            //REVIEW:NRE, OutOfRange
            var column = columns.ElementAt(2);
            return column.InnerText.Trim().Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022");
        }

        public List<QuestionAnswers> LoadQuestions()
        {
            var questions = new List<QuestionAnswers>();
            for (var i = 0; i <= TotalPages; i++)
            {
                questions.AddRange(GetQuestionsFromPage(i));
            }
            return questions;
        }
    }
}
