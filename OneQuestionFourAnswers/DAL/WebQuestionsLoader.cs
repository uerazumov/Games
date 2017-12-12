using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LibraryClass;

namespace DAL
{
    public class WebQuestionsLoader : IQuestionsLoader
    {
        public readonly int TotalPages;
        private readonly Random _random;
        private readonly List<int> _bannedPages;

        public WebQuestionsLoader()
        {
            TotalPages = GetTotalPages();
            _random = new Random();
            _bannedPages = new List<int>();
        }

        private static int GetTotalPages()
        {
            const string url = "https://baza-otvetov.ru/categories/view/1/0";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            const string xpath = "/html/body/div/div/div/div[2]/div[2]/div/a[5]";
            var pages = doc.DocumentNode.SelectSingleNode(xpath).GetAttributeValue("href", "");
            return Convert.ToInt16(pages.Split('/').Last()) / 10;
        }

        public List<QuestionAnswers> GetQuestionsFromPage(int page)
        {
            Console.WriteLine($"Обработка страницы {page} из {TotalPages}");
            var url = $"https://baza-otvetov.ru/categories/view/1/{page * 10}";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            const string xpath = "/html/body/div/div/div/div[2]/div[2]/table";
            var table = doc.DocumentNode.SelectSingleNode(xpath);
            var rows = table.ChildNodes.Where(node => node.Name == "tr").Skip(1);
            var questions = new List<QuestionAnswers>();
            foreach (var row in rows)
            {
                try
                {
                    var columns = row.ChildNodes.Where(node => node.Name == "td").ToList();
                    var question = new QuestionAnswers(GetQuestionText(columns), GetAnswers(columns));
                    questions.Add(question);
                }
                catch (Exception e)
                {
                    // TODO: Обработать ошибку
                }
            }
            return questions;
        }

        private static List<Answer> GetAnswers(IReadOnlyCollection<HtmlNode> columns)
        {
            var answers = new List<Answer>();
            answers.AddRange(GetIncorrectAnswers(columns).Select(x => new Answer(x, false)));
            answers.Add(new Answer(GetCorrectAnswer(columns), true));
            for (var i = 0; i < 4; i++)
            {
                if (answers[i].Text.Length > 18)
                {
                    throw new InvalidDataException();
                }
            }
            return answers.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static string GetQuestionText(IEnumerable<HtmlNode> columns)
        {
            var column = columns.ElementAt(1);
            var question = column.ChildNodes.First(node => node.Name == "a").InnerText.Trim();
            if (question.Length > 180)
            {
                throw new InvalidDataException();
            }
            return question.Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022");
        }

        private static IEnumerable<string> GetIncorrectAnswers(IEnumerable<HtmlNode> columns)
        {
            var column = columns.ElementAt(1);
            var variantsText = column.ChildNodes.First(node => node.Name == "div").InnerText.Trim();
            var variants = variantsText.Split(':')[1].Split(',').Select(x => x.Trim().Replace("&quot;", "\u0022").Replace("'", "\u2019").Replace("«", "\u0022").Replace("»", "\u0022")).ToList(); 
            if (variants.Count != 3)
            {
                throw new InvalidDataException();
            }
            return variants;
        }

        private static string GetCorrectAnswer(IEnumerable<HtmlNode> columns)
        {
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
