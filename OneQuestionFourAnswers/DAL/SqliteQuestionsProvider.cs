using LibraryClass;
using System.Collections.Generic;
using System.Data.SQLite;
using LoggingService;
using System;

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
            string question;
            var answers = new List<Answer>();
            try
            {
                var sql = $"SELECT * FROM questions WHERE rowid = {index}";
                var command = new SQLiteCommand(sql, Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                question = (string)reader["question"];
                var raw_answers = ((string)reader["answers"]).Split('|');
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
            }
            catch (Exception e)
            {
                question = "";
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при получении вопроса из БД по индексу " + index.ToString());
            }

            return new QuestionAnswers(question, answers);
        }

        public long GetQuestionsCount()
        {
            long count;
            try
            {
                var sql = "SELECT COUNT(*) FROM questions";
                var command = new SQLiteCommand(sql, Connection);
                count = (long)command.ExecuteScalar();
            }
            catch (Exception e)
            {
                count = 0;
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " получении кол-ва вопросов от БД");
            }
            return count;
        }

        public bool Update(List<QuestionAnswers> questions)
        {
            if ((questions == null) || (questions.Count == 0))
            {
                GlobalLogger.Instance.Error("Отсутствуют вопросы для занесения в БД при обновлении");
                return false;
            }
            try
            {
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
            catch (Exception e)
            {
                GlobalLogger.Instance.Error("Произошла ошибка " + e.Message + " при внесении вопросов в БД");
                return false;
            }
        }
    }
}
