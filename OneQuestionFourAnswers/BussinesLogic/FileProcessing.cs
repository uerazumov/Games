using System;
using System.Collections.Generic;
using System.Linq;
using LibraryClass;

namespace BussinesLogic
{
    public class FileProcessing
    {

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
                        var randomIndex = random.Next(1, 4);
                        var temp = statistic[i];
                        statistic[i] = statistic[Math.Abs(randomIndex - random.Next(1, 4))];
                        statistic[Math.Abs(randomIndex - random.Next(1, 4))] = temp;
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
            var twoAnswers = new[]
            {
                question.Answers[0].IsCorrect, question.Answers[1].IsCorrect, question.Answers[2].IsCorrect,
                question.Answers[3].IsCorrect
            };
            var random = new Random();
            var randomIndex = random.Next(0, 4);
            if (!twoAnswers[randomIndex])
            {
                twoAnswers[randomIndex] = true;
            }
            else
            {
                twoAnswers[Math.Abs(randomIndex - random.Next(1, 4))] = true;
            }
            return twoAnswers;
        }

        public RecordsTable GetRecordsTable()
        {
            //Здесь будет метод, запрашивающий и Дата Логики таблицу рекордов

            //Тестовая таблица рекордов
            var first = new Record("Player 1", 500);
            var second = new Record("Player 2", 400);
            var third = new Record("Player 3", 300);
            var listR = new List<Record> {first, second, third};
            return new RecordsTable(listR);
        }

        public void UpdateBaseOfQuestion()
        {
            //Здесь будет метод запускающий обновление базы вопросов
        }
    }
}