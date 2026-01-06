using DBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class SubjectDB : BaseDB<Subject>
    {
        protected override string GetTableName() => "subjects";
        protected override string GetPrimaryKeyName() => "subject_id";

        protected override async Task<Subject> CreateModelAsync(object[] row)
        {
            return new Subject
            {
                subject_id = Convert.ToInt32(row[0]),
                subject_name = row[1]?.ToString()
            };
        }
        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            return await SelectAllAsync();
        }
        public async Task<int> InsertSubjectAsync(Subject subject)
        {
            Dictionary<string, object> values = new()
            {
                { "subject_name", subject.subject_name }
            };
            return await InsertAsync(values);
        }
        public async Task<int> UpdateSubjectAsync(Subject subject)
        {
            Dictionary<string, object> values = new()
            {
                { "subject_name", subject.subject_name }
            };
            Dictionary<string, object> filter = new()
            {
                { "subject_id", subject.subject_id }
            };
            return await UpdateAsync(values, filter);
        }
        public async Task<int> DeleteSubjectAsync(int subjectId)
        {
            Dictionary<string, object> filter = new()
            {
                { "subject_id", subjectId }
            };
            return await DeleteAsync(filter);
        }
    }
}
