using LibraryClass;
using System.Collections.Generic;

namespace DAL
{
    public class ExcelStatisticSaver
    {
        private List<QuestionAnswers> _usedQuestions;
        private List<Answer> _chosenAnswers;

        public ExcelStatisticSaver()
        {

        }

        public void CreateReport()
        {

        }

        public void AddUsedQuestion(QuestionAnswers question)
        {
            _usedQuestions.Add(question);
        }

        public void AddChosenAnswer(Answer answer)
        {
            _chosenAnswers.Add(answer);
        }

        public void ClearReport()
        {
            _usedQuestions = new List<QuestionAnswers>();
            _chosenAnswers = new List<Answer>();
        }
    }
}