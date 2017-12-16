using System;
using System.Linq;
using LibraryClass;
using DAL;
using System.Collections.Generic;
using LoggingService;

namespace BussinesLogic
{
    public class FileProcessing
    {
        private IStatisticProvider _excelStatisticSaver = new StatisticProvider();

        private IQuestionsProvider _questionsProvider = new QuestionsProvider();

        private IHighScoresProvider _highScoresProvider = new HighScoresProvider();

        private List<int> _availableQuestions;

        public void CreateReport(string path)
        {
            if(_excelStatisticSaver.CreateReport(path))
            {
                GlobalLogger.Instance.Info("Создан отчёт Excel");
            }
            else
            {
                GlobalLogger.Instance.Error("Произошла ошибка создании отчёта Excel");
            }
        }

        public void AddUsedQuestion(QuestionAnswers question)
        {
            _excelStatisticSaver.AddUsedQuestion(question);
        }

        public void AddChosenAnswer(Answer answer)
        {
            _excelStatisticSaver.AddChosenAnswer(answer);
        }

        public void ClearReport()
        {
            _excelStatisticSaver.ClearReport();
        }

        public void RefreshQuestions()
        {
            _availableQuestions = Enumerable.Range(1, (int)_questionsProvider.GetQuestionsCount()).ToList();
            GlobalLogger.Instance.Info("Список использованных вопросов был очищен");
        }

        public QuestionAnswers NewQuestion()
        {
            var random = new Random();
            if(_availableQuestions.Count == 0)
            {
                RefreshQuestions();
            }
            var index = random.Next(0, _availableQuestions.Count);
            var question = _questionsProvider.GetQuestionAnswers(_availableQuestions[index]);
            _availableQuestions.RemoveAt(index);
            GlobalLogger.Instance.Info("Был выбран случайный вопрос с номером " + index.ToString());
            GlobalLogger.Instance.Info("Текст вопроса " + question.QuestionText);
            return question;
        }

        public bool CheckAnswer(Answer selectedAnswer)
        {
            if (selectedAnswer.IsCorrect)
            {
                return true;
            }
            return false;
        }

        public bool CheckRecordIsBrocken(int gameScore)
        {
            var tableOfReckords = GetRecordsTable();
            for (var i = 0; i != 3; i++)
            {
                if ((tableOfReckords.Records[i] == null)||(tableOfReckords.Records[i].Score < gameScore))
                {
                    GlobalLogger.Instance.Info("Рекорд был побит");
                    return true;
                }
            }
            GlobalLogger.Instance.Info("Рекорд не был побит");
            return false;
        }

        public byte[] HintStatistics(QuestionAnswers question)
        {
            var statistic = new byte[] {0, 0, 0, 0};
            var random = new Random();
            byte number = 97;
            for (var i = 0; i != 3; i++)
            {
                statistic[i] = (byte) random.Next(1, number + 1);
                number = (byte) (number - statistic[i] + 1);
            }
            statistic[3] = (byte) (100 - statistic[0] - statistic[1] - statistic[2]);
            var maxValue = statistic.Max();
            var maxIndex = Array.IndexOf(statistic, maxValue);
            for (var i = 0; i != 4; i++)
            {
                if (question.Answers[i].IsCorrect)
                {
                    byte randomValue = (byte) random.Next(0, 101);
                    if (randomValue < 90)
                    {
                        var temp = statistic[i];
                        statistic[i] = statistic[maxIndex];
                        statistic[maxIndex] = temp;
                    }
                    else
                    {
                        var incorrect = statistic.Select((item, index) => index).Where(index => statistic[index] != maxIndex) as int[];
                        if (incorrect != null)
                        {
                            var randomStatistic = (byte)incorrect.OrderBy(index => random.Next()).First();
                            var randomIndex = incorrect.OrderBy(item => randomStatistic).First();
                            var temp = statistic[i];
                            statistic[i] = randomStatistic;
                            statistic[randomIndex] = temp;
                        }
                    }
                }
            }
            GlobalLogger.Instance.Info("Подсказка Статистика вернула cледующие значения: " + statistic[0].ToString() + "%  " + statistic[1].ToString() + "%  " + statistic[2].ToString() + "%  " + statistic[3].ToString() + "%");
            return statistic;
        }

        public void CreateNewRecord(Record newRecord)
        {
            if(_highScoresProvider.Add(newRecord))
            {
                GlobalLogger.Instance.Info("Рекорд успешно добавлен в БД");
            }
            else
            {
                GlobalLogger.Instance.Error("Произошла ошибка при добавление нового рекорда в БД");
            }
        }

        public bool[] HintTwoAnswers(QuestionAnswers question)
        {
            var answers = question.Answers.Select(x => x.IsCorrect).ToArray();
            var random = new Random();
            var incorrect = answers.Select((item, index) => index).Where(index => answers[index] == false);
            var hintIndex = incorrect.OrderBy(index => random.Next()).First();
            answers[hintIndex] = true;
            var logText = "Подсказка Убрать Два Неверных ответа убрала ответы под номерами: ";
            for(var i = 0; i < 4; i++) if (!answers[i]) logText += i.ToString() + " ";
            GlobalLogger.Instance.Info(logText);
            return answers;
        }

        public RecordsTable GetRecordsTable()
        {
            var recordsTable = _highScoresProvider.GetTable();
            var countRecords = recordsTable.Records.Count;
            for (var i = 1; i != 4 - countRecords; i++)
            {
                recordsTable.Records.Add(new Record("-",0));
            }
            return recordsTable;
        }

        public void UpdateBaseOfQuestion()
        {
            var loader = new QuestionsLoader();
            var questions = loader.LoadQuestions();
            if(_questionsProvider.Update(questions))
            {
                GlobalLogger.Instance.Info("Обновление прошло успешно");
            }
            else
            {
                GlobalLogger.Instance.Error("Обновление базы не было осуществело");
            }
        }
    }
}