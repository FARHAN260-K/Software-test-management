using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class ComponentDataAccess : DataAccessBase
    {
        public void CreateComponent(Component component)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "INSERT INTO Components (ProjectID, Name, Description) VALUES (@ProjectID, @Name, @Description)",
                    connection);

                command.Parameters.AddWithValue("@ProjectID", component.ProjectID);
                command.Parameters.AddWithValue("@Name", component.Name);
                command.Parameters.AddWithValue("@Description", component.Description ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            });
        }

        public Component? ReadComponent(int componentId)
        {
            Component? component = null;
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "SELECT * FROM Components WHERE ComponentID = @ComponentID",
                    connection);

                command.Parameters.AddWithValue("@ComponentID", componentId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    component = new Component
                    {
                        ComponentID = Convert.ToInt32(reader["ComponentID"]),
                        ProjectID = Convert.ToInt32(reader["ProjectID"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                    };
                }
            });
            return component;
        }

        public List<Component> ReadComponentsByProject(int projectId)
        {
            return ExecuteWithConnection(connection =>
            {
                List<Component> components = new List<Component>();
                string query = "SELECT * FROM Components WHERE ProjectID = @ProjectID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            components.Add(new Component
                            {
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                ProjectID = Convert.ToInt32(reader["ProjectID"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
                return components;
            });
        }

        public List<Component> ReadAllComponents()
        {
            return ExecuteWithConnection(connection =>
            {
                List<Component> components = new List<Component>();
                string query = "SELECT * FROM Components";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            components.Add(new Component
                            {
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                ProjectID = Convert.ToInt32(reader["ProjectID"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
                return components;
            });
        }

        public void UpdateComponent(Component component)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "UPDATE Components SET ProjectID = @ProjectID, Name = @Name, Description = @Description WHERE ComponentID = @ComponentID",
                    connection);

                command.Parameters.AddWithValue("@ComponentID", component.ComponentID);
                command.Parameters.AddWithValue("@ProjectID", component.ProjectID);
                command.Parameters.AddWithValue("@Name", component.Name);
                command.Parameters.AddWithValue("@Description", component.Description ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            });
        }

        public void DeleteComponent(int componentId)
        {
            ExecuteWithConnection(connection =>
            {
                using var command = new MySqlCommand(
                    "DELETE FROM Components WHERE ComponentID = @ComponentID",
                    connection);

                command.Parameters.AddWithValue("@ComponentID", componentId);

                command.ExecuteNonQuery();
            });
        }

        public bool HasRelatedTestCases(int componentId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM TestCases WHERE ComponentID = @ComponentID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ComponentID", componentId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }
    }
} 