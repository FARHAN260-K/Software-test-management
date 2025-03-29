using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class TestReportDataAccess : DataAccessBase
    {
        public void CreateTestReport(TestReport testReport)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO TestReports (TestCaseID, ExecutionDate, Result, Notes) VALUES (@TestCaseID, @ExecutionDate, @Result, @Notes)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestCaseID", testReport.TestCaseID);
                    command.Parameters.AddWithValue("@ExecutionDate", testReport.ExecutionDate);
                    command.Parameters.AddWithValue("@Result", testReport.Result);
                    command.Parameters.AddWithValue("@Notes", testReport.Notes);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public TestReport? ReadTestReport(int reportId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM TestReports WHERE ReportID = @ReportID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReportID", reportId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TestReport
                            {
                                ReportID = Convert.ToInt32(reader["ReportID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ExecutionDate = Convert.ToDateTime(reader["ExecutionDate"]),
                                Result = reader["Result"].ToString(),
                                Notes = reader["Notes"].ToString()
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<TestReport> ReadTestReportsByTestCase(int testCaseId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestReport> testReports = new List<TestReport>();
                string query = "SELECT * FROM TestReports WHERE TestCaseID = @TestCaseID ORDER BY ExecutionDate DESC";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestCaseID", testCaseId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testReports.Add(new TestReport
                            {
                                ReportID = Convert.ToInt32(reader["ReportID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ExecutionDate = Convert.ToDateTime(reader["ExecutionDate"]),
                                Result = reader["Result"].ToString(),
                                Notes = reader["Notes"].ToString()
                            });
                        }
                    }
                }
                return testReports;
            });
        }

        public List<TestReport> ReadAllTestReports()
        {
            return ExecuteWithConnection(connection =>
            {
                List<TestReport> testReports = new List<TestReport>();
                string query = "SELECT * FROM TestReports ORDER BY ExecutionDate DESC";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testReports.Add(new TestReport
                            {
                                ReportID = Convert.ToInt32(reader["ReportID"]),
                                TestCaseID = Convert.ToInt32(reader["TestCaseID"]),
                                ExecutionDate = Convert.ToDateTime(reader["ExecutionDate"]),
                                Result = reader["Result"].ToString(),
                                Notes = reader["Notes"].ToString()
                            });
                        }
                    }
                }
                return testReports;
            });
        }

        public void UpdateTestReport(TestReport testReport)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE TestReports SET TestCaseID = @TestCaseID, ExecutionDate = @ExecutionDate, Result = @Result, Notes = @Notes WHERE ReportID = @ReportID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReportID", testReport.ReportID);
                    command.Parameters.AddWithValue("@TestCaseID", testReport.TestCaseID);
                    command.Parameters.AddWithValue("@ExecutionDate", testReport.ExecutionDate);
                    command.Parameters.AddWithValue("@Result", testReport.Result);
                    command.Parameters.AddWithValue("@Notes", testReport.Notes);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteTestReport(int reportId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM TestReports WHERE ReportID = @ReportID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReportID", reportId);
                    command.ExecuteNonQuery();
                }
            });
        }
    }
} 