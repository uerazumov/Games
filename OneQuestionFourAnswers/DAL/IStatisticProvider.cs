using LibraryClass;

namespace DAL
{
    public interface IStatisticProvider
    {
        bool CreateReport();
        void AddUsedQuestion(QuestionAnswers question);
        void AddChosenAnswer(Answer answer);
        void ClearReport();
    }
}
