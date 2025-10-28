using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Answers
    {
        public int answer_id { get; set; }
        public int question_id { get; set; }
        public int answer_text { get; set; }
        public int is_correct { get; set; }
        public string explanation { get; set; }
    }
}
