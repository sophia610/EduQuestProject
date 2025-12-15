using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }
        public Answer() { }
        public Answer(int answerId, int questionId, string answerText, bool isCorrect, string explanation)
        {
            AnswerId = answerId;
            QuestionId = questionId;
            AnswerText = answerText;
            IsCorrect = isCorrect;
            Explanation = explanation;
        }

    }
}
