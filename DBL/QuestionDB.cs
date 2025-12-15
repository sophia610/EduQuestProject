using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBLL;
using Models;
using MySql.Data.MySqlClient;

namespace DBL
{
    public class QuestionDB : BaseDB<Question>
    {
        protected override string GetTableName() => "questions";
        protected override string GetPrimaryKeyName() => "question_id";

        protected override async Task<Question> CreateModelAsync(object[] row)
        {
            Question q = new Question
            {
                QuestionID = Convert.ToInt32(row[0]),
                TeacherID = Convert.ToInt32(row[1]),
                QuestionText = row[2]?.ToString(),
                TopicName = row[3]?.ToString(),
                CreatedAt = row.Length > 4 && row[4] != DBNull.Value && row[4] != null
                    ? Convert.ToDateTime(row[4])
                    : DateTime.Now  
            };
            return q;
        }

        // הוספת שאלה חדשה עם תשובות
        public async Task<Question> InsertQuestionWithAnswersAsync(Question question, List<Answer> answers)
        {
            Dictionary<string, object> questionValues = new()
    {
        { "teacher_id", question.TeacherID },
        { "question_text", question.QuestionText },
        { "topic_id", int.Parse(question.TopicName) }, // ⬅️ חשוב! המרה ל-int
        { "hint", question.Hint ?? "" },
        { "created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } // ⬅️ פורמט נכון
    };

            var insertedQuestion = await base.InsertGetObjAsync(questionValues);

            if (insertedQuestion != null && answers != null && answers.Count > 0)
            {
                var answerDb = new AnswerDB();
                foreach (var answer in answers)
                {
                    answer.QuestionId = insertedQuestion.QuestionID;
                    await answerDb.InsertAnswerAsync(answer);
                }
            }
            
            return insertedQuestion;
        }

        // הוספת שאלה פשוטה
        public async Task<Question> InsertQuestionAsync(Question question)
        {
            Dictionary<string, object> values = new()
            {
                { "teacher_id", question.TeacherID },
                { "question_text", question.QuestionText },
                { "topic_id", question.TopicName },
                { "hint", "" },
                { "created_at", DateTime.Now }
            };
            return await base.InsertGetObjAsync(values);
        }

        // שליפת שאלות אחרונות של מורה
        public async Task<List<Question>> GetRecentByTeacherAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId }
            };

            var results = await SelectAllAsync(filter);

            // Sort by created_at DESC and take top 10
            results.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            return results.Take(10).ToList();
        }

        // שליפת כל השאלות (לתלמידים)
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await SelectAllAsync();
        }

        // שליפת שאלות לפי נושא
        public async Task<List<Question>> GetQuestionsByTopicAsync(string topicName)
        {
            Dictionary<string, object> filter = new()
            {
                { "topic_name", topicName }
            };
            return await SelectAllAsync(filter);
        }

        // שליפת שאלה לפי ID עם התשובות שלה
        public async Task<Question> GetQuestionWithAnswersAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            var results = await SelectAllAsync(filter);

            if (results.Count == 0) return null;

            var question = results[0];

            // Get answers for this question
            var answerDb = new AnswerDB();
            question.Answers = await answerDb.GetAnswersByQuestionIdAsync(questionId);

            return question;
        }

        // שליפת שאלה לפי ID
        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            var results = await SelectAllAsync(filter);
            return results.Count > 0 ? results[0] : null;
        }

        // מחיקת שאלה
        public async Task<int> DeleteQuestionAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            return await base.DeleteAsync(filter);
        }

        // עדכון שאלה
        public async Task<int> UpdateQuestionAsync(Question question)
        {
            Dictionary<string, object> values = new()
            {
                { "topic_id", question.TopicName },
                { "question_text", question.QuestionText }
            };
            Dictionary<string, object> filter = new()
            {
                { "question_id", question.QuestionID }
            };
            return await base.UpdateAsync(values, filter);
        }

        // ספירת שאלות של מורה
        public async Task<int> GetQuestionCountAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId }
            };
            var results = await SelectAllAsync(filter);
            return results.Count;
        }
    }
}