using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Topic
    {
        public int topic_id { get; set; }
        public int subject_id { get; set; }
        public string topic_name { get; set; }

        public Topic() { }

        public Topic(int topicId, int subjectId, string topicName)
        {
            this.topic_id = topicId;
            this.subject_id = subjectId;
            this.topic_name = topicName;
        }
    }
}
