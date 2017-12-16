using LibraryClass;
using System.Collections.Generic;
using LoggingService;
using ExcelLibrary.SpreadSheet;
using System;

namespace DAL
{
    public class ExcelStatisticProvider : IStatisticProvider
    {
        private List<QuestionAnswers> _usedQuestions;
        private List<Answer> _chosenAnswers;
        private int _rightAnswers;

        public bool CreateReport(string path)
        {
            try
            {
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Report");
                workbook.Worksheets.Add(worksheet);
                for (int i = 0; i < 100; i++) worksheet.Cells[i, 0] = new Cell("");
                worksheet.Cells[0, 0] = new Cell("Текст вопроса");
                worksheet.Cells[0, 1] = new Cell("Выбранный ответ");
                for (int i = 0; i < _usedQuestions.Count; i++)
                {
                    worksheet.Cells[i + 2, 0] = new Cell(_usedQuestions[i].QuestionText);
                    worksheet.Cells[i + 2, 1] = new Cell(_chosenAnswers[i].Text);
                }
                worksheet.Cells[_usedQuestions.Count + 3, 0] = new Cell("Процент верных ответов");
                double procent = _rightAnswers * 100 / _usedQuestions.Count;
                worksheet.Cells[_usedQuestions.Count + 3, 1] = new Cell(procent.ToString() + "%");
                workbook.Save(path);
                GlobalLogger.Instance.Info("Был создан отчёт Excel");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddUsedQuestion(QuestionAnswers question)
        {
            _usedQuestions.Add(question);
        }

        public void AddChosenAnswer(Answer answer)
        {
            _chosenAnswers.Add(answer);
            if(answer.IsCorrect)
            {
                _rightAnswers++;
            }
        }

        public void ClearReport()
        {
            _usedQuestions = new List<QuestionAnswers>();
            _chosenAnswers = new List<Answer>();
        }
    }
}