using LibraryClass;
using System.Collections.Generic;

namespace DAL
{
    public interface IQuestionsProvider 
    {
        long GetQuestionsCount();
        QuestionAnswers GetQuestionAnswers(int index);
        bool Update(List<QuestionAnswers> questions);
    }
}
