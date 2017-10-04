using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic
{
    public class FileProcessing
    {
        public void StartNewGame(ref int newscore, ref string newname)
        {
            newscore = 0;
            newname = "";
        }
        public void NewQuestion(ref TimeSpan newtime, ref LibraryClass.QuestionAnswers newquestion, ref bool[] allanswerstrue)
        {
            //Метод берущий из БД вопрос по индексу и возвращающий нам новый вопрос
            allanswerstrue = new bool[] { true, true, true, true };
            newtime = new TimeSpan(0, 1, 0);

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
            newquestion = new LibraryClass.QuestionAnswers("Текст вопроса", AnswerList);
        }
        public bool CheckAnswer(LibraryClass.Answer selectedanswer, ref int score, ref bool defeatrecord)
        {
            if(selectedanswer.IsCorrect)
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
            byte[] statistic = new byte[] { 0, 0, 0, 0 };
            Random rnd = new Random();
            byte n = 97;
            for (int i = 0; i != 3; i++)
            {
                statistic[i] = (byte)rnd.Next(1, n);
                n = (byte)(n - statistic[i] + 1);
            }
            statistic[3] = (byte)(100 - statistic[0] - statistic[1] - statistic[2]);
            byte[] hintstatistic = statistic;
            byte[] max = new byte[] { 0, 1, 2, 3 };
            for(int i = 0; i !=3; i++)
                for(int j = 1; j != 4; j++)
                {
                    if(statistic[i] < statistic[j])
                    {
                        byte t = statistic[i];
                        statistic[i] = statistic[j];
                        statistic[j] = t;
                        t = max[i];
                        max[i] = max[j];
                        max[j] = t;
                    }
                }
            for (int i = 0; i != 4;i++)
            {
                if (question.Answers[i].IsCorrect)
                {
                    byte RND = (byte)rnd.Next(0, 100);
                    if (RND < 85)
                    {
                        byte t = hintstatistic[i];
                        hintstatistic[i] = hintstatistic[max[0]];
                        hintstatistic[max[0]] = t;
                    }
                    else if (RND < 75)
                    {
                        byte t = hintstatistic[i];
                        hintstatistic[i] = hintstatistic[max[1]];
                        hintstatistic[max[1]] = t;
                    }
                    else if (RND < 65)
                    {
                        byte t = hintstatistic[i];
                        hintstatistic[i] = hintstatistic[max[2]];
                        hintstatistic[max[2]] = t;
                    }
                    else
                    {
                        byte t = hintstatistic[i];
                        hintstatistic[i] = hintstatistic[max[3]];
                        hintstatistic[max[3]] = t;
                    }
                }
            }
            return hintstatistic;
        }
        public void CreateNewRecord(LibraryClass.Record newrecord)
        {
            //здесь будет метод передающий новый рекорд в Дата Логику
        }
        public bool[] HintTwoAnswers(LibraryClass.QuestionAnswers question)
        {
            bool[] twoanswers = new bool[] { question.Answers[0].IsCorrect, question.Answers[1].IsCorrect, question.Answers[2].IsCorrect, question.Answers[3].IsCorrect };
            Random rnd = new Random();
            int t = rnd.Next(0, 3);
            if (!twoanswers[t])
            {
                twoanswers[t] = true;
            }
            else
            {
                twoanswers[Math.Abs(t - rnd.Next(1, 3))] = true;
            }
            return twoanswers;
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
