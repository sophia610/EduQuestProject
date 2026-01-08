using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Result
    {
       public int result_id { get; set; }
        public int student_id { get; set; }
        public int question_id { get; set; }
        public int quiz_id { get; set; }
        //public int score { get; set; } // צריך ציון החידון, הזהות של החידון?
        public int is_correct { get; set; }
        public int used_hint { get; set; }
        public int rating { get; set;}
        public DateTime answered_at { get; set; }

        public Result() { }

        public Result(int result_id, int student_id, int question_id, int quiz_id, int is_correct, int used_hint, int rating, DateTime answered_at)
        {
            this.result_id = result_id;
            this.student_id = student_id;
            this.question_id = question_id;
            this.quiz_id = quiz_id;
            this.is_correct = is_correct;
            this.used_hint = used_hint;
            this.rating = rating;
            this.answered_at = answered_at;
        }
    }
}
