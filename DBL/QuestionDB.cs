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
            // ✅ סדר העמודות בטבלה שלך: question_id, topic_id, teacher_id, question_text, hint, created_at
            Question q = new Question
            {
                QuestionID = Convert.ToInt32(row[0]),        // question_id
                TopicName = row[1]?.ToString(),              // topic_id (נשמר כ-string)
                TeacherID = Convert.ToInt32(row[2]),         // teacher_id
                QuestionText = row[3]?.ToString(),           // question_text
                Hint = row.Length > 4 && row[4] != DBNull.Value ? row[4].ToString() : "", // hint
                CreatedAt = row.Length > 5 && row[5] != DBNull.Value && row[5] != null
                    ? Convert.ToDateTime(row[5])             // created_at
                    : DateTime.Now
            };
            return q;
        }

        // ✅ הוספת שאלה חדשה עם תשובות
        public async Task<Question> InsertQuestionWithAnswersAsync(Question question, List<Answer> answers)
        {
            try
            {
                // המרה נכונה של topic_id
                int topicId;
                if (!int.TryParse(question.TopicName, out topicId))
                {
                    throw new Exception("Topic ID must be a valid integer");
                }

                Dictionary<string, object> questionValues = new()
                {
                    { "quiz_id", question.QuizId },
                    { "teacher_id", question.TeacherID },
                    { "question_text", question.QuestionText },
                    { "topic_id", topicId },
                    { "hint", question.Hint ?? "" },
                    { "created_at", DateTime.Now }
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

                    // ✅ טעינת התשובות למודל שנשמר
                    insertedQuestion.Answers = answers;
                }

                return insertedQuestion;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InsertQuestionWithAnswersAsync: {ex.Message}");
                throw;
            }
        }

        // הוספת שאלה פשוטה
        public async Task<Question> InsertQuestionAsync(Question question)
        {
            try
            {
                int topicId;
                if (!int.TryParse(question.TopicName, out topicId))
                {
                    throw new Exception("Topic ID must be a valid integer");
                }

                Dictionary<string, object> values = new()
                {
                    { "teacher_id", question.TeacherID },
                    { "question_text", question.QuestionText },
                    { "topic_id", topicId },
                    { "hint", question.Hint ?? "" },
                    { "created_at", DateTime.Now }
                };

                return await base.InsertGetObjAsync(values);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InsertQuestionAsync: {ex.Message}");
                throw;
            }
        }

        // שליפת שאלות אחרונות של מורה
        public async Task<List<Question>> GetRecentByTeacherAsync(int teacherId)
        {
            Dictionary<string, object> filter = new()
            {
                { "teacher_id", teacherId }
            };

            var results = await SelectAllAsync(filter);
            results.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            return results.Take(10).ToList();
        }

        // שליפת כל השאלות
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await SelectAllAsync();
        }

        // שליפת שאלות לפי נושא
        public async Task<List<Question>> GetQuestionsByTopicAsync(int topicId)
        {
            Dictionary<string, object> filter = new()
            {
                { "topic_id", topicId }
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
            try
            {
                // ✅ מחיקת תשובות קודם
                var answerDb = new AnswerDB();
                await answerDb.DeleteAnswersByQuestionIdAsync(questionId);

                // מחיקת השאלה
                Dictionary<string, object> filter = new()
                {
                    { "question_id", questionId }
                };
                return await base.DeleteAsync(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteQuestionAsync: {ex.Message}");
                throw;
            }
        }

        // עדכון שאלה
        public async Task<int> UpdateQuestionAsync(Question question)
        {
            int topicId;
            if (!int.TryParse(question.TopicName, out topicId))
            {
                throw new Exception("Topic ID must be a valid integer");
            }

            Dictionary<string, object> values = new()
            {
                { "topic_id", topicId },
                { "question_text", question.QuestionText },
                { "hint", question.Hint ?? "" }
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
        public async Task<List<Question>> GetQuestionsByQuizIdAsync(int quizId)
        {
            Dictionary<string, object> filter = new()
    {
        { "quiz_id", quizId }
    };

            var questions = await SelectAllAsync(filter);

            // טעינת תשובות לכל שאלה
            var answerDb = new AnswerDB();
            foreach (var q in questions)
            {
                q.Answers = await answerDb.GetAnswersByQuestionIdAsync(q.QuestionID);
            }

            return questions;
        }
    }
}