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
        public int TeacherID { get; set; }  // Foreign key to the teacher
        public string QuestionText { get; set; }
        public string TopicName { get; set; }
        public DateTime CreatedAt { get; set; }

        public Question() { }

        public Question(int id, int teacherId, string text, string topic, DateTime createdAt)
        {
            QuestionID = id;
            TeacherID = teacherId;
            QuestionText = text;
            TopicName = topic;
            CreatedAt = createdAt;
        }
    }
}
