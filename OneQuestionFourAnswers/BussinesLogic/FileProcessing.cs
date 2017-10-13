using System;
using System.Collections.Generic;
using System.Linq;
using LibraryClass;

namespace BussinesLogic
{
    public class FileProcessing
    {
        private readonly RecordsTable _recordsTableTable = new RecordsTable(new List<Record> { new Record("Player 1", 500), new Record("Player 2", 400)});

        public void NewQuestion(out QuestionAnswers newQuestion)
        {
            //Метод берущий из БД вопрос по индексу и возвращающий нам новый вопрос

            //Пробный вопрос
            var answerList = new List<Answer>();
            var rnd = new Random();
            var r = rnd.Next(0, 4);
            for (int i = 0; i != 4; i++)
            {
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (i != r)
                {
                    answerList.Add(new Answer("неверный", false));
                }
                else
                {
                    answerList.Add(new Answer("верный", true));
                }
            }
            newQuestion = new QuestionAnswers("Текст вопроса", answerList);
        }

        public bool CheckAnswer(Answer selectedAnswer, ref int score)
        {
            if (selectedAnswer.IsCorrect)
            {
                score += 10;
                return true;
            }
            return false;
        }

        public bool CheckRecordIsBrocken(int score)
        {
            //Здесь будет проверка того, побил ли пользователь рекорд

            return true;
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
                        var incorrect = statistic.Select((item, index) => index).Where(index => statistic[index] != maxIndex);
                        var randomStatistic = (byte)incorrect.OrderBy(index => random.Next()).First();
                        var randomIndex = incorrect.OrderBy(item => randomStatistic).First();
                        var temp = statistic[i];
                        statistic[i] = randomStatistic;
                        statistic[randomIndex] = temp;
                    }
                }
            }
            return statistic;
        }

        public void CreateNewRecord(Record newRecord)
        {
            //здесь будет метод передающий новый рекорд в Дата Логику
        }

        public bool[] HintTwoAnswers(QuestionAnswers question)
        {
            var answers = question.Answers.Select(x => x.IsCorrect).ToArray();
            var random = new Random();
            var incorrect = answers.Select((item, index) => index).Where(index => answers[index] == false);
            var hintIndex = incorrect.OrderBy(index => random.Next()).First();
            answers[hintIndex] = true;
            return answers;
        }

        public RecordsTable GetRecordsTable()
        {
            //Здесь будет метод, запрашивающий и Дата Логики таблицу рекордов

            var countRecords = _recordsTableTable.Records.Count;
            if (countRecords < 3)
            {
                for (var i = 1; i != 4 - countRecords; i++)
                {
                    _recordsTableTable.Records.Add(new Record("-",0));
                }
            }
            return _recordsTableTable;
        }

        public void UpdateBaseOfQuestion()
        {
            //Здесь будет метод запускающий обновление базы вопросов
        }
    }
}