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
            return new Question
            {
                QuestionText = row[0].ToString(),
                TopicName = row[1].ToString(),
                CreatedAt = Convert.ToDateTime(row[2])
            };
        }

        public async Task<List<Question>> GetRecentByTeacherAsync(int teacherId)
        {
            string sql = @"SELECT q.question_text, t.topic_name, q.created_at
                           FROM questions q
                           JOIN topics t ON q.topic_id = t.topic_id
                           WHERE q.teacher_id = @id
                           ORDER BY q.created_at DESC
                           LIMIT 5";

            Dictionary<string, object> p = new()
            {
                { "@id", teacherId }
            };

            return await SelectAllAsync(sql, p);
        }
    }
}