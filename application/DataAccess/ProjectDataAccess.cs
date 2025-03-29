using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class ProjectDataAccess : DataAccessBase
    {
        public void CreateProject(Project project)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO Projects (Name, Description, StartDate, EndDate) VALUES (@Name, @Description, @StartDate, @EndDate)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", project.Name);
                    command.Parameters.AddWithValue("@Description", project.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public Project? ReadProject(int projectId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM Projects WHERE ProjectID = @ProjectID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Project
                            {
                                ProjectID = Convert.ToInt32(reader["ProjectID"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : DateTime.MaxValue
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<Project> ReadAllProjects()
        {
            return ExecuteWithConnection(connection =>
            {
                List<Project> projects = new List<Project>();
                string query = "SELECT * FROM Projects";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(new Project
                            {
                                ProjectID = Convert.ToInt32(reader["ProjectID"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : DateTime.MaxValue
                            });
                        }
                    }
                }
                return projects;
            });
        }

        public void UpdateProject(Project project)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE Projects SET Name = @Name, Description = @Description, StartDate = @StartDate, EndDate = @EndDate WHERE ProjectID = @ProjectID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", project.ProjectID);
                    command.Parameters.AddWithValue("@Name", project.Name);
                    command.Parameters.AddWithValue("@Description", project.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteProject(int projectId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM Projects WHERE ProjectID = @ProjectID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public bool HasRelatedComponents(int projectId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM Components WHERE ProjectID = @ProjectID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }
    }
} 