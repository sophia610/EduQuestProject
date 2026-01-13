using Models;
using DBL;

namespace ConsoleUnitTesting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
           // await TestInsertCustomerAsync();
           await InsertQuizzesWithQuestionsAsync();
         }
        public static async Task InsertQuizzesWithQuestionsAsync()
        {
            var quizDb = new QuizDB();
            var questionDb = new QuestionDB();
            var subjectDb = new SubjectDB();
            var topicDb = new TopicDB();

            // שליפת מקצועות ונושאים קיימים
            var subjects = await subjectDb.GetAllSubjectsAsync();
            var allTopics = await topicDb.GetAllTopicsAsync();

            int[] teacherIds = { 5, 6, 7, 10 };

            foreach (int teacherId in teacherIds)
            {
                Console.WriteLine($"\n=== Creating quizzes for Teacher {teacherId} ===");

                // חידון 1: מתמטיקה
                var mathSubject = subjects.FirstOrDefault(s => s.subject_name.Contains("Math"));
                if (mathSubject != null)
                {
                    var mathTopics = allTopics.Where(t => t.subject_id == mathSubject.subject_id).ToList();
                    if (mathTopics.Any())
                    {
                        var quiz1 = new Quiz
                        {
                            TeacherId = teacherId,
                            QuizName = "Algebra Fundamentals",
                            Description = "Test your knowledge of basic algebra concepts including equations, variables, and expressions.",
                            SubjectId = mathSubject.subject_id,
                            TopicId = mathTopics[0].topic_id,
                            CreatedAt = DateTime.Now,
                            IsActive = true
                        };

                        var savedQuiz1 = await quizDb.CreateQuizAsync(quiz1);
                        Console.WriteLine($"✓ Created: {savedQuiz1.QuizName}");

                        // שאלה 1
                        var q1 = new Question
                        {
                            QuizId = savedQuiz1.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What is the value of x in the equation: 2x + 5 = 15?",
                            Hint = "Subtract 5 from both sides first",
                            TopicName = mathTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers1 = new List<Answer>
                        {
                            new Answer { AnswerText = "x = 5", IsCorrect = true, Explanation = "2x + 5 = 15 → 2x = 10 → x = 5" },
                            new Answer { AnswerText = "x = 10", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "x = 7", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "x = 3", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q1, answers1);

                        // שאלה 2
                        var q2 = new Question
                        {
                            QuizId = savedQuiz1.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "Simplify: 3(x + 4) - 2x",
                            Hint = "Distribute the 3 first, then combine like terms",
                            TopicName = mathTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers2 = new List<Answer>
                        {
                            new Answer { AnswerText = "x + 12", IsCorrect = true, Explanation = "3x + 12 - 2x = x + 12" },
                            new Answer { AnswerText = "x + 4", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "5x + 12", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "x + 8", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q2, answers2);

                        // שאלה 3
                        var q3 = new Question
                        {
                            QuizId = savedQuiz1.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What is the slope of the line passing through points (2,3) and (4,7)?",
                            Hint = "Use the formula: slope = (y2-y1)/(x2-x1)",
                            TopicName = mathTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers3 = new List<Answer>
                        {
                            new Answer { AnswerText = "2", IsCorrect = true, Explanation = "(7-3)/(4-2) = 4/2 = 2" },
                            new Answer { AnswerText = "1/2", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "4", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "-2", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q3, answers3);
                        Console.WriteLine($"  → Added 3 questions");
                    }
                }

                // חידון 2: מדעים
                var scienceSubject = subjects.FirstOrDefault(s => s.subject_name.Contains("Science") || s.subject_name.Contains("Biology"));
                if (scienceSubject != null)
                {
                    var scienceTopics = allTopics.Where(t => t.subject_id == scienceSubject.subject_id).ToList();
                    if (scienceTopics.Any())
                    {
                        var quiz2 = new Quiz
                        {
                            TeacherId = teacherId,
                            QuizName = "Cell Biology Basics",
                            Description = "Explore the fundamental concepts of cell structure and function.",
                            SubjectId = scienceSubject.subject_id,
                            TopicId = scienceTopics[0].topic_id,
                            CreatedAt = DateTime.Now,
                            IsActive = true
                        };

                        var savedQuiz2 = await quizDb.CreateQuizAsync(quiz2);
                        Console.WriteLine($"✓ Created: {savedQuiz2.QuizName}");

                        // שאלה 1
                        var q1 = new Question
                        {
                            QuizId = savedQuiz2.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What is the powerhouse of the cell?",
                            Hint = "This organelle produces energy (ATP)",
                            TopicName = scienceTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers1 = new List<Answer>
                        {
                            new Answer { AnswerText = "Mitochondria", IsCorrect = true, Explanation = "Mitochondria produce ATP through cellular respiration" },
                            new Answer { AnswerText = "Nucleus", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Ribosome", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Chloroplast", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q1, answers1);

                        // שאלה 2
                        var q2 = new Question
                        {
                            QuizId = savedQuiz2.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "Which organelle is responsible for protein synthesis?",
                            Hint = "Think about where proteins are made",
                            TopicName = scienceTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers2 = new List<Answer>
                        {
                            new Answer { AnswerText = "Ribosome", IsCorrect = true, Explanation = "Ribosomes translate mRNA into proteins" },
                            new Answer { AnswerText = "Golgi apparatus", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Lysosome", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Endoplasmic reticulum", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q2, answers2);

                        // שאלה 3
                        var q3 = new Question
                        {
                            QuizId = savedQuiz2.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What process do plants use to make food?",
                            Hint = "This process requires sunlight and chlorophyll",
                            TopicName = scienceTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers3 = new List<Answer>
                        {
                            new Answer { AnswerText = "Photosynthesis", IsCorrect = true, Explanation = "Plants convert light energy into chemical energy" },
                            new Answer { AnswerText = "Respiration", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Digestion", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Fermentation", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q3, answers3);

                        // שאלה 4
                        var q4 = new Question
                        {
                            QuizId = savedQuiz2.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What is the function of the cell membrane?",
                            Hint = "Think about what separates the inside from outside",
                            TopicName = scienceTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers4 = new List<Answer>
                        {
                            new Answer { AnswerText = "Controls what enters and exits the cell", IsCorrect = true, Explanation = "The cell membrane is selectively permeable" },
                            new Answer { AnswerText = "Stores genetic information", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Produces energy", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "Breaks down waste", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q4, answers4);
                        Console.WriteLine($"  → Added 4 questions");
                    }
                }

                // חידון 3: היסטוריה או אנגלית
                var englishSubject = subjects.FirstOrDefault(s => s.subject_name.Contains("English") || s.subject_name.Contains("History"));
                if (englishSubject != null)
                {
                    var englishTopics = allTopics.Where(t => t.subject_id == englishSubject.subject_id).ToList();
                    if (englishTopics.Any())
                    {
                        var quiz3 = new Quiz
                        {
                            TeacherId = teacherId,
                            QuizName = "Grammar Essentials",
                            Description = "Master the basics of English grammar and sentence structure.",
                            SubjectId = englishSubject.subject_id,
                            TopicId = englishTopics[0].topic_id,
                            CreatedAt = DateTime.Now,
                            IsActive = true
                        };

                        var savedQuiz3 = await quizDb.CreateQuizAsync(quiz3);
                        Console.WriteLine($"✓ Created: {savedQuiz3.QuizName}");

                        // שאלה 1
                        var q1 = new Question
                        {
                            QuizId = savedQuiz3.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "Which sentence is grammatically correct?",
                            Hint = "Pay attention to subject-verb agreement",
                            TopicName = englishTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers1 = new List<Answer>
                        {
                            new Answer { AnswerText = "The students are studying for their exams.", IsCorrect = true, Explanation = "Plural subject requires plural verb" },
                            new Answer { AnswerText = "The students is studying for their exams.", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "The student are studying for their exams.", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "The students was studying for their exams.", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q1, answers1);

                        // שאלה 2
                        var q2 = new Question
                        {
                            QuizId = savedQuiz3.QuizId,
                            TeacherID = teacherId,
                            QuestionText = "What is the past tense of 'run'?",
                            Hint = "This is an irregular verb",
                            TopicName = englishTopics[0].topic_id.ToString(),
                            CreatedAt = DateTime.Now
                        };

                        var answers2 = new List<Answer>
                        {
                            new Answer { AnswerText = "ran", IsCorrect = true, Explanation = "Run is irregular: run, ran, run" },
                            new Answer { AnswerText = "runned", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "running", IsCorrect = false, Explanation = "" },
                            new Answer { AnswerText = "runs", IsCorrect = false, Explanation = "" }
                        };

                        await questionDb.InsertQuestionWithAnswersAsync(q2, answers2);
                        Console.WriteLine($"  → Added 2 questions");
                    }
                }
            }

            Console.WriteLine("\n✅ All quizzes created successfully!");
        }
        public static async Task TestInsertCustomerAsync()
        {
            Customers customers = new Customers();
            CustomerDB db = new CustomerDB();
            customers.FullName = "Shahar Hartshtein";
            customers.Email = "shahartt@gmail.com";
            customers.Role = 0;
            customers.Password = "shaha4";
            customers = await db.InsertGetObjAsync(customers);
            List<Customers> allCustomers = await db.GetAllAsync();
            foreach (var c in allCustomers)
            {
                Console.WriteLine($"- {c.user_id}: {c.FullName} ({c.Email}) | Role: {(c.Role == 0 ? "Student" : "Teacher")}");
            }
        }
    }
}
