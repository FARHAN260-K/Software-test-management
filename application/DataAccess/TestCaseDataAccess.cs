using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class TestCaseDataAccess : DataAccessBase
    {
        public void CreateTestCase(TestCase testCase)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "INSERT INTO TestCases (ComponentID, Name, Description, Priority, Status, AssignedUser, StatusID) " +
                    "VALUES (@ComponentID, @Name, @Description, @Priority, @Status, @AssignedUser, @StatusID)",
                    connection);

                command.Parameters.AddWithValue("@ComponentID", testCase.ComponentID);
                command.Parameters.AddWithValue("@Name", testCase.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", testCase.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Priority", testCase.Priority ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", testCase.Status ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignedUser", testCase.AssignedUser);
                command.Parameters.AddWithValue("@StatusID", testCase.StatusID);

                command.ExecuteNonQuery();
            });
        }

        public TestCase? ReadTestCase(int testCaseId)
        {
            TestCase? testCase = null;
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "SELECT tc.*, c.Name as ComponentName, c.Description as ComponentDescription " +
                    "FROM TestCases tc " +
                    "LEFT JOIN Components c ON tc.ComponentID = c.ComponentID " +
                    "WHERE tc.TestCaseID = @TestCaseID",
                    connection);

                command.Parameters.AddWithValue("@TestCaseID", testCaseId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    testCase = new TestCase
                    {
                        TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                        ComponentID = Convert.ToInt32(reader["ComponentID"]),
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : null,
                        Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                        AssignedUser = Convert.ToInt32(reader["AssignedUser"]),
                        StatusID = Convert.ToInt32(reader["StatusID"]),
                        ComponentName = reader["ComponentName"] != DBNull.Value ? reader["ComponentName"].ToString() : null,
                        ComponentDescription = reader["ComponentDescription"] != DBNull.Value ? reader["ComponentDescription"].ToString() : null
                    };
                }
            });
            return testCase;
        }

        public List<TestCase> ReadTestCasesByComponent(int componentId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestCase> testCases = new List<TestCase>();
                string query = "SELECT tc.*, c.Name as ComponentName, c.Description as ComponentDescription " +
                             "FROM TestCases tc " +
                             "LEFT JOIN Components c ON tc.ComponentID = c.ComponentID " +
                             "WHERE tc.ComponentID = @ComponentID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ComponentID", componentId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testCases.Add(new TestCase
                            {
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : null,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                                AssignedUser = Convert.ToInt32(reader["AssignedUser"]),
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                ComponentName = reader["ComponentName"] != DBNull.Value ? reader["ComponentName"].ToString() : null,
                                ComponentDescription = reader["ComponentDescription"] != DBNull.Value ? reader["ComponentDescription"].ToString() : null
                            });
                        }
                    }
                }
                return testCases;
            });
        }

        public List<TestCase> ReadTestCasesByUser(int userId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestCase> testCases = new List<TestCase>();
                string query = "SELECT tc.*, c.Name as ComponentName, c.Description as ComponentDescription " +
                             "FROM TestCases tc " +
                             "LEFT JOIN Components c ON tc.ComponentID = c.ComponentID " +
                             "WHERE tc.AssignedUser = @UserID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testCases.Add(new TestCase
                            {
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : null,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                                AssignedUser = Convert.ToInt32(reader["AssignedUser"]),
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                ComponentName = reader["ComponentName"] != DBNull.Value ? reader["ComponentName"].ToString() : null,
                                ComponentDescription = reader["ComponentDescription"] != DBNull.Value ? reader["ComponentDescription"].ToString() : null
                            });
                        }
                    }
                }
                return testCases;
            });
        }

        public List<TestCase> ReadTestCasesByStatus(int statusId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestCase> testCases = new List<TestCase>();
                string query = "SELECT tc.*, c.Name as ComponentName, c.Description as ComponentDescription " +
                             "FROM TestCases tc " +
                             "LEFT JOIN Components c ON tc.ComponentID = c.ComponentID " +
                             "WHERE tc.StatusID = @StatusID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatusID", statusId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testCases.Add(new TestCase
                            {
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : null,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                                AssignedUser = Convert.ToInt32(reader["AssignedUser"]),
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                ComponentName = reader["ComponentName"] != DBNull.Value ? reader["ComponentName"].ToString() : null,
                                ComponentDescription = reader["ComponentDescription"] != DBNull.Value ? reader["ComponentDescription"].ToString() : null
                            });
                        }
                    }
                }
                return testCases;
            });
        }

        public List<TestCase> ReadAllTestCases()
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestCase> testCases = new List<TestCase>();
                string query = "SELECT tc.*, c.Name as ComponentName, c.Description as ComponentDescription " +
                             "FROM TestCases tc " +
                             "LEFT JOIN Components c ON tc.ComponentID = c.ComponentID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testCases.Add(new TestCase
                            {
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : null,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                                AssignedUser = Convert.ToInt32(reader["AssignedUser"]),
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                ComponentName = reader["ComponentName"] != DBNull.Value ? reader["ComponentName"].ToString() : null,
                                ComponentDescription = reader["ComponentDescription"] != DBNull.Value ? reader["ComponentDescription"].ToString() : null
                            });
                        }
                    }
                }
                return testCases;
            });
        }

        public void UpdateTestCase(TestCase testCase)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "UPDATE TestCases SET ComponentID = @ComponentID, Name = @Name, Description = @Description, " +
                    "Priority = @Priority, Status = @Status, AssignedUser = @AssignedUser, StatusID = @StatusID " +
                    "WHERE TestCaseID = @TestCaseID",
                    connection);

                command.Parameters.AddWithValue("@TestCaseID", testCase.TestCaseID);
                command.Parameters.AddWithValue("@ComponentID", testCase.ComponentID);
                command.Parameters.AddWithValue("@Name", testCase.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", testCase.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Priority", testCase.Priority ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", testCase.Status ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignedUser", testCase.AssignedUser);
                command.Parameters.AddWithValue("@StatusID", testCase.StatusID);

                command.ExecuteNonQuery();
            });
        }

        public void DeleteTestCase(int testCaseId)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "DELETE FROM TestCases WHERE TestCaseID = @TestCaseID",
                    connection);

                command.Parameters.AddWithValue("@TestCaseID", testCaseId);

                command.ExecuteNonQuery();
            });
        }

        public bool HasRelatedTestReports(int testCaseId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM TestReports WHERE TestCaseID = @TestCaseID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestCaseID", testCaseId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }
    }
} 