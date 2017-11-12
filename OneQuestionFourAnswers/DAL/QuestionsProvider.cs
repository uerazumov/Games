using System.Collections.Generic;
using LibraryClass;

namespace DAL
{
    public class QuestionsProvider : IQuestionsProvider
    {
        private IQuestionsProvider _provider;

        public QuestionsProvider()
        {
            _provider = new SqliteQuestionsProvider();
        }

        public QuestionAnswers GetQuestionAnswers(int index)
        {
            return _provider.GetQuestionAnswers(index);
        }

        public long GetQuestionsCount()
        {
            return _provider.GetQuestionsCount();
        }

        public bool Update(List<QuestionAnswers> questions)
        {
            return _provider.Update(questions);
        }
    }
}
