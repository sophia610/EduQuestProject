using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int QuizId { get; set; }
        public int TeacherID { get; set; }  // Foreign key to the teacher
        public string QuestionText { get; set; }
        public string Hint { get; set; } 
        public string TopicName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public Question() { }

        public Question(int questionID, int quizId, int teacherID, string questionText, string hint, string topicName, DateTime createdAt)
        {
            QuestionID = questionID;
            QuizId = quizId;
            TeacherID = teacherID;
            QuestionText = questionText;
            Hint = hint;
            TopicName = topicName;
            CreatedAt = createdAt;
        }
    }
}
