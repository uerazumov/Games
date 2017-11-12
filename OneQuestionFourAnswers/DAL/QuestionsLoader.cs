using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryClass;

namespace DAL
{
    public class QuestionsLoader : IQuestionsLoader
    {
        private IQuestionsLoader _loader;

        public QuestionsLoader()
        {
            _loader = new WebQuestionsLoader();
        }

        public List<QuestionAnswers> LoadQuestions()
        {
            return _loader.LoadQuestions();
        }
    }
}
