using System.Collections.Generic;

namespace LibraryClass
{
    public class QuestionAnswers
    {
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }

        public QuestionAnswers(string answertext, List<Answer> answers)
        {
            QuestionText = answertext;
            Answers = answers;
        }
    }
}
