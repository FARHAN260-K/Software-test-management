using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class TestStatusDataAccess : DataAccessBase
    {
        public void CreateTestStatus(TestStatus testStatus)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO TestStatuses (StatusName, Description) VALUES (@StatusName, @Description)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusName", testStatus.StatusName);
                    command.Parameters.AddWithValue("@Description", testStatus.Description);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public TestStatus? ReadTestStatus(int statusId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM TestStatuses WHERE StatusID = @StatusID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusID", statusId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TestStatus
                            {
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"].ToString(),
                                Description = reader["Description"].ToString()
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<TestStatus> ReadAllTestStatuses()
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestStatus> testStatuses = new List<TestStatus>();
                string query = "SELECT * FROM TestStatuses";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testStatuses.Add(new TestStatus
                            {
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
                return testStatuses;
            });
        }

        public void UpdateTestStatus(TestStatus testStatus)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE TestStatuses SET StatusName = @StatusName, Description = @Description WHERE StatusID = @StatusID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusID", testStatus.StatusID);
                    command.Parameters.AddWithValue("@StatusName", testStatus.StatusName);
                    command.Parameters.AddWithValue("@Description", testStatus.Description);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteTestStatus(int statusId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM TestStatuses WHERE StatusID = @StatusID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusID", statusId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public bool HasAssignedTestCases(int statusId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM TestCases WHERE Status = @StatusID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusID", statusId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }
    }
} 