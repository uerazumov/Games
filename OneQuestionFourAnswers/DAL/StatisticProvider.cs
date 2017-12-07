using LibraryClass;


namespace DAL
{
    public class StatisticProvider : IStatisticProvider
    {
        private IStatisticProvider _provider;

        public StatisticProvider()
        {
            _provider = new ExcelStatisticProvider();
        }

        public void AddChosenAnswer(Answer answer)
        {
            _provider.AddChosenAnswer(answer);
        }

        public void AddUsedQuestion(QuestionAnswers question)
        {
            _provider.AddUsedQuestion(question);
        }

        public void ClearReport()
        {
            _provider.ClearReport();
        }

        public bool CreateReport()
        {
            return _provider.CreateReport();
        }
    }
}
