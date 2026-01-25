
    using DBLL;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace DBL
    {
        public class ResultDB : BaseDB<Result>
        {
            protected override string GetTableName() => "results";
            protected override string GetPrimaryKeyName() => "result_id";

        protected override async Task<Result> CreateModelAsync(object[] row)
        {
            return new Result
            {
                result_id = Convert.ToInt32(row[0]),
                student_id = Convert.ToInt32(row[1]),
                question_id = Convert.ToInt32(row[2]),
                quiz_id = Convert.ToInt32(row[3]),
                is_correct = Convert.ToInt32(row[4]),
                used_hint = Convert.ToInt32(row[5]),
                time_taken = Convert.ToInt32(row[6]), 
                rating = row[7] != DBNull.Value ? Convert.ToInt32(row[7]) : 0,
                score = row[8] != DBNull.Value ? Convert.ToInt32(row[8]) : 0, 
                answered_at = row[9] != DBNull.Value ? Convert.ToDateTime(row[9]) : DateTime.Now
            };
        }

        // שמירת תשובת תלמיד לשאלה
        public async Task<Result> InsertResultAsync(Result result)
        {
            Dictionary<string, object> values = new()
    {
        { "student_id", result.student_id },
        { "question_id", result.question_id },
        { "quiz_id", result.quiz_id },
        { "is_correct", result.is_correct },
        { "used_hint", result.used_hint },
        { "time_taken", result.time_taken }, 
        { "rating", result.rating },
        { "score", result.score }, 
        { "answered_at", DateTime.Now }
    };

            return await base.InsertGetObjAsync(values);
        }

        //  שמירת ציון סופי של חידון (עדכון כל התוצאות של החידון)
        public async Task UpdateQuizScoreAsync(int studentId, int quizId, int score)
        {
            Dictionary<string, object> values = new()
    {
        { "score", score }
    };

            Dictionary<string, object> filter = new()
    {
        { "student_id", studentId },
        { "quiz_id", quizId }
    };

            await base.UpdateAsync(values, filter);
        }

        // ✅ סטטיסטיקות זמן ממוצע לשאלה
        public async Task<double> GetQuestionAverageTimeAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
    {
        { "question_id", questionId }
    };
            var results = await SelectAllAsync(filter);

            return results.Count > 0 ? results.Average(r => r.time_taken) : 0;
        }

        // ✅ סטטיסטיקות ציון ממוצע של חידון
        public async Task<double> GetQuizAverageScoreAsync(int quizId)
        {
            Dictionary<string, object> filter = new()
    {
        { "quiz_id", quizId }
    };
            var results = await SelectAllAsync(filter);

            var uniqueStudents = results.GroupBy(r => r.student_id)
                                         .Select(g => g.First().score)
                                         .Where(s => s > 0)
                                         .ToList();

            return uniqueStudents.Count > 0 ? uniqueStudents.Average() : 0;
        }
        public async Task<List<Result>> GetStudentQuizResultsAsync(int studentId, int quizId)
        {
            Dictionary<string, object> filter = new()
            {
                { "student_id", studentId },
                { "quiz_id", quizId }
            };
            return await SelectAllAsync(filter);
        }

        //  בדיקה האם תלמיד כבר פתר חידון
        public async Task<bool> HasStudentTakenQuizAsync(int studentId, int quizId)
        {
            var results = await GetStudentQuizResultsAsync(studentId, quizId);
            return results.Count > 0;
        }

        //  חישוב ציון של תלמיד בחידון
        public async Task<int> CalculateQuizScoreAsync(int studentId, int quizId)
        {
            var results = await GetStudentQuizResultsAsync(studentId, quizId);
            if (results.Count == 0) return 0;

            int correctAnswers = results.Count(r => r.is_correct == 1);
            return (int)Math.Round((double)correctAnswers / results.Count * 100);
        }

        //  שליפת כל התוצאות של תלמיד (כל החידונים)
        public async Task<List<Result>> GetAllStudentResultsAsync(int studentId)
        {
            Dictionary<string, object> filter = new()
            {
                { "student_id", studentId }
            };
            return await SelectAllAsync(filter);
        }
        public async Task<List<Result>> GetAllQuizResultsAsync(int quizId)
        {
            Dictionary<string, object> filter = new()
    {
        { "quiz_id", quizId }
    };
            return await SelectAllAsync(filter);
        }

        //  סטטיסטיקות לשאלה - כמה תלמידים ענו נכון/שגוי
        public async Task<Dictionary<string, int>> GetQuestionStatsAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            var results = await SelectAllAsync(filter);

            return new Dictionary<string, int>
            {
                { "total_attempts", results.Count },
                { "correct", results.Count(r => r.is_correct == 1) },
                { "incorrect", results.Count(r => r.is_correct == 0) },
                { "used_hint", results.Count(r => r.used_hint == 1) },
                { "average_rating", results.Count > 0 ? (int)results.Average(r => r.rating) : 0 }
            };
        }

        //  דירוג ממוצע של שאלה
        public async Task<double> GetQuestionAverageRatingAsync(int questionId)
        {
            Dictionary<string, object> filter = new()
            {
                { "question_id", questionId }
            };
            var results = await SelectAllAsync(filter);

            var rated = results.Where(r => r.rating > 0).ToList();
            return rated.Count > 0 ? rated.Average(r => r.rating) : 0;
        }

        //  עדכון דירוג שאלה
        public async Task<int> UpdateRatingAsync(int resultId, int rating)
        {
            Dictionary<string, object> values = new()
            {
                { "rating", rating }
            };

            Dictionary<string, object> filter = new()
            {
                { "result_id", resultId }
            };

            return await base.UpdateAsync(values, filter);
        }
    }
    }

