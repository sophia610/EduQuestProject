using DBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class AnswerDB : BaseDB<Answer>
    {
        protected override string GetTableName() => "answers";
        protected override string GetPrimaryKeyName() => "answer_id";

        protected override async Task<Answer> CreateModelAsync(object[] row)
        {
            Answer a = new Answer
            {
                AnswerId = Convert.ToInt32(row[0]),
                QuestionId = Convert.ToInt32(row[1]),
                AnswerText = row[2]?.ToString(),
                IsCorrect = Convert.ToBoolean(row[3]),
                Explanation = row[4]?.ToString()
            };
            return a;
        }

        // הוספת תשובה
        public async Task<Answer> InsertAnswerAsync(Answer answer)
        {
            Dictionary<string, object> values = new()
            {
                { "question_id", answer.QuestionId },
                { "answer_text", answer.AnswerText },
                { "is_correct", answer.IsCorrect },
                { "explanation", answer.Explanation ?? "" }
            };
            return await base.InsertGetObjAsync(values);
        }

        // שליפת תשובות לשאלה
        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            return await SelectAllAsync(filter);
        }

        // מחיקת תשובה
        public async Task<int> DeleteAnswerAsync(int answerId)
        {
            Dictionary<string, object> filter = new()
            {
                { "answer_id", answerId }
            };
            return await base.DeleteAsync(filter);
        }

        // עדכון תשובה
        public async Task<int> UpdateAnswerAsync(Answer answer)
        {
            Dictionary<string, object> values = new()
            {
                { "answer_text", answer.AnswerText },
                { "is_correct", answer.IsCorrect },
                { "explanation", answer.Explanation }
            };
            Dictionary<string, object> filter = new()
            {
                { "answer_id", answer.AnswerId }
            };
            return await base.UpdateAsync(values, filter);
        }
       

    }
}


