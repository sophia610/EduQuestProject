using DBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class QuizDB : BaseDB<Quiz>
    {
        protected override string GetTableName() => "quizzes";
        protected override string GetPrimaryKeyName() => "quiz_id";

        protected override async Task<Quiz> CreateModelAsync(object[] row)
        {
            return new Quiz
            {
                QuizId = Convert.ToInt32(row[0]),
                TeacherId = Convert.ToInt32(row[1]),
                QuizName = row[2]?.ToString() ?? "",
                Description = row[3]?.ToString() ?? "",
                SubjectId = Convert.ToInt32(row[4]),
                TopicId = Convert.ToInt32(row[5]),
                CreatedAt = row[6] != DBNull.Value ? Convert.ToDateTime(row[6]) : DateTime.Now,
                IsActive = row.Length > 7 && row[7] != DBNull.Value ? Convert.ToBoolean(row[7]) : true
            };
        }

        // ✅ יצירת חידון חדש
        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            Dictionary<string, object> values = new()
            {
                { "teacher_id", quiz.TeacherId },
                { "quiz_name", quiz.QuizName },
                { "description", quiz.Description ?? "" },
                { "subject_id", quiz.SubjectId },
                { "topic_id", quiz.TopicId },
                { "created_at", DateTime.Now },
                { "is_active", true }
            };

            return await base.InsertGetObjAsync(values);
        }

        // ✅ שליפת חידונים של מורה
        public async Task<List<Quiz>> GetQuizzesByTeacherAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId }
            };

            var quizzes = await SelectAllAsync(filter);

            // טעינת שאלות לכל חידון
            var questionDb = new QuestionDB();
            foreach (var quiz in quizzes)
            {
                quiz.Questions = await questionDb.GetQuestionsByQuizIdAsync(quiz.QuizId);
            }

            return quizzes;
        }

        // ✅ שליפת חידון לפי ID עם השאלות
        public async Task<Quiz> GetQuizWithQuestionsAsync(int quizId)
        {
            Dictionary<string, object> filter = new()
            {
                { "quiz_id", quizId }
            };

            var results = await SelectAllAsync(filter);
            if (results.Count == 0) return null;

            var quiz = results[0];

            // טעינת השאלות
            var questionDb = new QuestionDB();
            quiz.Questions = await questionDb.GetQuestionsByQuizIdAsync(quizId);

            return quiz;
        }

        // ✅ מחיקת חידון (עם כל השאלות)
        public async Task<int> DeleteQuizAsync(int quizId)
        {
            try
            {
                var questionDb = new QuestionDB();
                var questions = await questionDb.GetQuestionsByQuizIdAsync(quizId);

                // מחיקת כל השאלות
                foreach (var q in questions)
                {
                    await questionDb.DeleteQuestionAsync(q.QuestionID);
                }

                // מחיקת החידון
                Dictionary<string, object> filter = new()
                {
                    { "quiz_id", quizId }
                };
                return await base.DeleteAsync(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteQuizAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ עדכון חידון
        public async Task<int> UpdateQuizAsync(Quiz quiz)
        {
            Dictionary<string, object> values = new()
            {
                { "quiz_name", quiz.QuizName },
                { "description", quiz.Description ?? "" },
                { "subject_id", quiz.SubjectId },
                { "topic_id", quiz.TopicId },
                { "is_active", quiz.IsActive }
            };

            Dictionary<string, object> filter = new()
            {
                { "quiz_id", quiz.QuizId }
            };

            return await base.UpdateAsync(values, filter);
        }
    }

}
