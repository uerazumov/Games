using LibraryClass;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DAL
{
    public class SqliteQuestionsProvider : SqliteAdapter, IQuestionsProvider
    {
        public SqliteQuestionsProvider()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS questions " +
                               "(question TEXT, answers TEXT)";
            var command = new SQLiteCommand(sql, Connection);
            command.ExecuteNonQuery();
        }

        public QuestionAnswers GetQuestionAnswers(int index)
        {
            var sql = $"SELECT * FROM questions WHERE rowid = {index}";
            var command = new SQLiteCommand(sql, Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var question = (string)reader["question"];
            var raw_answers = ((string)reader["answers"]).Split('|');
            var answers = new List<Answer>();
            foreach (var item in raw_answers)
            {
                if (item.StartsWith("!"))
                {
                    answers.Add(new Answer(item.Remove(0, 1), true));
                }
                else
                {
                    answers.Add(new Answer(item, false));
                }
            }

            return new QuestionAnswers(question, answers);
        }

        public long GetQuestionsCount()
        {
            var sql = "SELECT COUNT(*) FROM questions";
            var command = new SQLiteCommand(sql, Connection);
            return (long)command.ExecuteScalar();
        }

        public bool Update(List<QuestionAnswers> questions)
        {
            if ((questions == null) || (questions.Count == 0))
            {
                return false;
            }
            using (var command = new SQLiteCommand(Connection))
            {
                var sql = "DELETE FROM questions";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                using (var transaction = Connection.BeginTransaction())
                {
                    foreach (var item in questions)
                    {
                        var row_answers = "";
                        foreach (var answer in item.Answers)
                        {
                            if (answer.IsCorrect)
                            {
                                row_answers += "!";
                            }
                            row_answers += answer.Text + "|";
                        }
                        row_answers = row_answers.Remove(row_answers.Length - 1, 1);
                        sql = $"INSERT INTO questions (question, answers) VALUES ('{item.QuestionText}', '{row_answers}')";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }
    }
}
