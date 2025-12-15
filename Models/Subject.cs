using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Subject
    {
        public int subject_id { get; set; }
        public string subject_name { get; set; }

        public Subject() { }

        public Subject(int id, string name)
        {
            this.subject_id = id;
            this.subject_name = name;
        }
    }

}
