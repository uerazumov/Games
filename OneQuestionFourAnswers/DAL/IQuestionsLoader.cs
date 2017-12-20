using LibraryClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IQuestionsLoader
    {
        List<QuestionAnswers> LoadQuestions();
        int GetUpdateProcent();
    }
}
