using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class TestHistoryDataAccess : DataAccessBase
    {
        public void CreateTestHistory(TestHistory testHistory)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO TestHistory (TestCaseID, ChangedBy, ChangeDate, OldStatus, NewStatus, Notes) VALUES (@TestCaseID, @ChangedBy, @ChangeDate, @OldStatus, @NewStatus, @Notes)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestCaseID", testHistory.TestCaseID);
                    command.Parameters.AddWithValue("@ChangedBy", testHistory.ChangedBy);
                    command.Parameters.AddWithValue("@ChangeDate", testHistory.ChangeDate);
                    command.Parameters.AddWithValue("@OldStatus", testHistory.OldStatus);
                    command.Parameters.AddWithValue("@NewStatus", testHistory.NewStatus);
                    command.Parameters.AddWithValue("@Notes", testHistory.Notes);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public TestHistory? ReadTestHistory(int historyId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM TestHistory WHERE HistoryID = @HistoryID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HistoryID", historyId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TestHistory
                            {
                                HistoryID = Convert.ToInt32(reader["HistoryID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ChangedBy = Convert.ToInt32(reader["ChangedBy"]),
                                ChangeDate = Convert.ToDateTime(reader["ChangeDate"]),
                                OldStatus = reader["OldStatus"].ToString(),
                                NewStatus = reader["NewStatus"].ToString(),
                                Notes = reader["Notes"].ToString()
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<TestHistory> ReadTestHistoryByTestCase(int testCaseId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestHistory> testHistory = new List<TestHistory>();
                string query = "SELECT * FROM TestHistory WHERE TestCaseID = @TestCaseID ORDER BY ChangeDate DESC";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestCaseID", testCaseId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testHistory.Add(new TestHistory
                            {
                                HistoryID = Convert.ToInt32(reader["HistoryID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ChangedBy = Convert.ToInt32(reader["ChangedBy"]),
                                ChangeDate = Convert.ToDateTime(reader["ChangeDate"]),
                                OldStatus = reader["OldStatus"].ToString(),
                                NewStatus = reader["NewStatus"].ToString(),
                                Notes = reader["Notes"].ToString()
                            });
                        }
                    }
                }
                return testHistory;
            });
        }

        public List<TestHistory> ReadTestHistoryByUser(int userId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestHistory> testHistory = new List<TestHistory>();
                string query = "SELECT * FROM TestHistory WHERE ChangedBy = @UserID ORDER BY ChangeDate DESC";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testHistory.Add(new TestHistory
                            {
                                HistoryID = Convert.ToInt32(reader["HistoryID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ChangedBy = Convert.ToInt32(reader["ChangedBy"]),
                                ChangeDate = Convert.ToDateTime(reader["ChangeDate"]),
                                OldStatus = reader["OldStatus"].ToString(),
                                NewStatus = reader["NewStatus"].ToString(),
                                Notes = reader["Notes"].ToString()
                            });
                        }
                    }
                }
                return testHistory;
            });
        }

        public List<TestHistory> ReadAllTestHistory()
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestHistory> testHistory = new List<TestHistory>();
                string query = "SELECT * FROM TestHistory ORDER BY ChangeDate DESC";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testHistory.Add(new TestHistory
                            {
                                HistoryID = Convert.ToInt32(reader["HistoryID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ChangedBy = Convert.ToInt32(reader["ChangedBy"]),
                                ChangeDate = Convert.ToDateTime(reader["ChangeDate"]),
                                OldStatus = reader["OldStatus"].ToString(),
                                NewStatus = reader["NewStatus"].ToString(),
                                Notes = reader["Notes"].ToString()
                            });
                        }
                    }
                }
                return testHistory;
            });
        }

        public void UpdateTestHistory(TestHistory testHistory)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE TestHistory SET TestCaseID = @TestCaseID, ChangedBy = @ChangedBy, ChangeDate = @ChangeDate, OldStatus = @OldStatus, NewStatus = @NewStatus, Notes = @Notes WHERE HistoryID = @HistoryID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HistoryID", testHistory.HistoryID);
                    command.Parameters.AddWithValue("@TestCaseID", testHistory.TestCaseID);
                    command.Parameters.AddWithValue("@ChangedBy", testHistory.ChangedBy);
                    command.Parameters.AddWithValue("@ChangeDate", testHistory.ChangeDate);
                    command.Parameters.AddWithValue("@OldStatus", testHistory.OldStatus);
                    command.Parameters.AddWithValue("@NewStatus", testHistory.NewStatus);
                    command.Parameters.AddWithValue("@Notes", testHistory.Notes);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteTestHistory(int historyId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM TestHistory WHERE HistoryID = @HistoryID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HistoryID", historyId);
                    command.ExecuteNonQuery();
                }
            });
        }
    }
} 