using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int TeacherId { get; set; }
        public string QuizName { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public int TopicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public List<Question> Questions { get; set; } = new List<Question>();
        public Quiz() { }
        public Quiz(int quizId, int teacherId, string quizName, string description, int subjectId, int topicId, DateTime createdAt, bool isActive, List<Question> questions)
        {
            QuizId = quizId;
            TeacherId = teacherId;
            QuizName = quizName;
            Description = description;
            SubjectId = subjectId;
            TopicId = topicId;
            CreatedAt = createdAt;
            IsActive = isActive;
            Questions = questions;
        }

        public Quiz(int quizId, int teacherId, string quizName, string description, int subjectId, int topicId, DateTime createdAt, bool isActive)
        {
            QuizId = quizId;
            TeacherId = teacherId;
            QuizName = quizName;
            Description = description;
            SubjectId = subjectId;
            TopicId = topicId;
            CreatedAt = createdAt;
            IsActive = isActive;
        }
    }
}
