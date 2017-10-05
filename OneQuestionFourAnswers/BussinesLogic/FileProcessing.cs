using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic
{
    public class FileProcessing
    {
        public void StartNewGame(ref int newScore, ref string newName)
        {
            newScore = 0;
            newName = "";
        }
        public void NewQuestion(ref TimeSpan newTime, ref LibraryClass.QuestionAnswers newQuestion, ref bool[] allAnswersTrue)
        {
            //Метод берущий из БД вопрос по индексу и возвращающий нам новый вопрос
            allAnswersTrue = new bool[] { true, true, true, true };
            newTime = new TimeSpan(0, 1, 0);

            //Пробный вопрос
            LibraryClass.Answer AnswerA = new LibraryClass.Answer("верный", true);
            LibraryClass.Answer AnswerB = new LibraryClass.Answer("неверный", false);
            LibraryClass.Answer AnswerC = new LibraryClass.Answer("неверный", false);
            LibraryClass.Answer AnswerD = new LibraryClass.Answer("неверный", false);
            List<LibraryClass.Answer> AnswerList = new List<LibraryClass.Answer>();
            AnswerList.Add(AnswerA);
            AnswerList.Add(AnswerB);
            AnswerList.Add(AnswerC);
            AnswerList.Add(AnswerD);
            newQuestion = new LibraryClass.QuestionAnswers("Текст вопроса", AnswerList);
        }
        public bool CheckAnswer(LibraryClass.Answer selectedAnswer, ref int score, ref bool defeatRecord)
        {
            if(selectedAnswer.IsCorrect)
            {
                score += 10;
                return true;
            }
            else
            {
                //Здесь будет проверка того, побил ли пользователь рекорд
                return false;
            }
        }
        public byte[] HintStatistics(LibraryClass.QuestionAnswers question)
        {
            var max = new byte[] { 0, 1, 2, 3 };
            var statistic = new byte[] { 0, 0, 0, 0 };
            var random = new Random();
            byte number = 97;
            for (int i = 0; i != 3; i++)
            {
                statistic[i] = (byte)random.Next(1, number);
                number = (byte)(number - statistic[i] + 1);
            }
            statistic[3] = (byte)(100 - statistic[0] - statistic[1] - statistic[2]);
            var hintStatistic = statistic;
            for(int i = 0; i !=3; i++)
                for(int j = 1; j != 4; j++)
                {
                    if(statistic[i] < statistic[j])
                    {
                        byte temp = statistic[i];
                        statistic[i] = statistic[j];
                        statistic[j] = temp;
                        temp = max[i];
                        max[i] = max[j];
                        max[j] = temp;
                    }
                }
            for (int i = 0; i != 4;i++)
            {
                if (question.Answers[i].IsCorrect)
                {
                    byte randomValue = (byte)random.Next(0, 100);
                    if (randomValue < 90)
                    {
                        byte temp = hintStatistic[i];
                        hintStatistic[i] = hintStatistic[max[0]];
                        hintStatistic[max[0]] = temp;
                    }
                    else
                    {
                        var randomIndex = (int)random.Next(1, 3);
                        byte temp = hintStatistic[i];
                        hintStatistic[i] = hintStatistic[randomIndex];
                        hintStatistic[randomIndex] = temp;
                    }
                }
            }
            return hintStatistic;
        }
        public void CreateNewRecord(LibraryClass.Record newRecord)
        {
            //здесь будет метод передающий новый рекорд в Дата Логику
        }
        public bool[] HintTwoAnswers(LibraryClass.QuestionAnswers question)
        {
            var twoAnswers = new bool[] { question.Answers[0].IsCorrect, question.Answers[1].IsCorrect, question.Answers[2].IsCorrect, question.Answers[3].IsCorrect };
            var random = new Random();
            int randowIndex = random.Next(0, 3);
            if (!twoAnswers[randowIndex])
            {
                twoAnswers[randowIndex] = true;
            }
            else
            {
                twoAnswers[Math.Abs(randowIndex - random.Next(1, 3))] = true;
            }
            return twoAnswers;
        }
        public LibraryClass.RecordsTable GetRecordsTable()
        {
            //Здесь будет метод, запрашивающий и Дата Логики таблицу рекордов

            //Тестовая таблица рекордов
            LibraryClass.Record First = new LibraryClass.Record("Player 1", 500);
            LibraryClass.Record Second = new LibraryClass.Record("Player 2", 400);
            LibraryClass.Record Third = new LibraryClass.Record("Player 3", 300);
            List<LibraryClass.Record> listR = new List<LibraryClass.Record>();
            listR.Add(First);
            listR.Add(Second);
            listR.Add(Third);
            return new LibraryClass.RecordsTable(listR);
        }
    }
}
