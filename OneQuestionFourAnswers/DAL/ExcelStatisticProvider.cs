using LibraryClass;
using System.Collections.Generic;
using LoggingService;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
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
                File.Copy(@"VisualResources\ReportTemplate.xlsx", path, true);
                var info = new FileInfo(path);
                using (var package = new ExcelPackage(info))
                {
                    var table = package.Workbook.Worksheets[1];
                    for (int i = 0; i < _usedQuestions.Count; i++)
                    {
                        table.Cells[i + 2, 1].Value = _usedQuestions[i].QuestionText;
                        var answerCell = table.Cells[i + 2, 2];
                        answerCell.Value = _chosenAnswers[i].Text;
                        var fill = answerCell.Style.Fill;
                        fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(_chosenAnswers[i].IsCorrect ? Color.Green : Color.OrangeRed);
                    }
                    var procent = _rightAnswers * 100 / _usedQuestions.Count;
                    table.Cells[2, 4].Value = procent.ToString() + "%";
                    table.Column(1).AutoFit();
                    table.Column(2).AutoFit();
                    package.Save();
                }
                return true;
            }
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Создание отчёта завершилось с ошибкой " + e.Message);
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
            if (answer.IsCorrect)
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