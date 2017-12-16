using LibraryClass;

namespace DAL
{
    public interface IStatisticProvider
    {
        bool CreateReport(string path);
        void AddUsedQuestion(QuestionAnswers question);
        void AddChosenAnswer(Answer answer);
        void ClearReport();
    }
}
