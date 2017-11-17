using System;
using System.Linq;
using LibraryClass;
using DAL;
using System.Collections.Generic;

namespace BussinesLogic
{
    public class FileProcessing
    {
        private IQuestionsProvider _questions_provider = new QuestionsProvider();

        private IHighScoresProvider _high_scores_provider = new HighScoresProvider();

        private List<int> _availableQuestions;

        public void RefreshQuestions()
        {
            _availableQuestions = Enumerable.Range(1, (int)_questions_provider.GetQuestionsCount()).ToList();
        }

        public QuestionAnswers NewQuestion()
        {
            var random = new Random();
            var index = random.Next(0, _availableQuestions.Count);
            var question = _questions_provider.GetQuestionAnswers(_availableQuestions[index]);
            _availableQuestions.RemoveAt(index);
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
                if (tableOfReckords.Records[i] == null)
                {
                    return true;
                }
                if (tableOfReckords.Records[i].Score < gameScore)
                {
                    return true;
                }
            }
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
            return statistic;
        }

        public void CreateNewRecord(Record newRecord)
        {
            _high_scores_provider.Add(newRecord);
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
            var recordsTable = _high_scores_provider.GetTable();
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
            _questions_provider.Update(questions);
        }
    }
}