using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLL;
using Models;

namespace DBL
{
    public class TopicDB : BaseDB<Topic>
    {
        protected override string GetTableName() => "topics";
        protected override string GetPrimaryKeyName() => "topic_id";

        protected override async Task<Topic> CreateModelAsync(object[] row)
        {
            return new Topic
            {
                topic_id = Convert.ToInt32(row[0]),   // 1
                subject_id = Convert.ToInt32(row[1]),   // 2
                topic_name = row[2]?.ToString()         // 3
            };
        }

        // שליפת כל הטופיקים
        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await SelectAllAsync();
        }

        // שליפת טופיקים לפי subject_id
        public async Task<List<Topic>> GetBySubjectAsync(int subjectId)
        {
            Dictionary<string, object> filter = new()
            {
                { "subject_id", subjectId }
            };

            return await SelectAllAsync(filter);
        }

        // הוספת טופיק
        public async Task<int> InsertTopicAsync(Topic t)
        {
            Dictionary<string, object> values = new()
            {
                { "subject_id", t.subject_id },
                { "topic_name", t.topic_name }
            };

            return await InsertAsync(values);
        }

        // עדכון טופיק
        public async Task<int> UpdateTopicAsync(Topic t)
        {
            Dictionary<string, object> values = new()
            {
                { "subject_id", t.subject_id },
                { "topic_name", t.topic_name }
            };

            Dictionary<string, object> filter = new()
            {
                { "topic_id", t.topic_id }
            };

            return await UpdateAsync(values, filter);
        }

        // מחיקת טופיק
        public async Task<int> DeleteTopicAsync(int id)
        {
            Dictionary<string, object> filter = new()
            {
                { "topic_id", id }
            };

            return await DeleteAsync(filter);
        }
    }
}
