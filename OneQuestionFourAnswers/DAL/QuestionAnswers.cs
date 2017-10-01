using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class QuestionAnswers
    {
        public string Text { get; set; }
        public List<Answer> Answers;

        public QuestionAnswers(string text, List<Answer> answers)
        {
            Text = text;
            Answers = answers;
        }
    }
}
