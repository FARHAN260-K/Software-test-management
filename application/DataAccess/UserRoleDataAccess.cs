using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class UserRoleDataAccess : DataAccessBase
    {
        public void CreateUserRole(UserRole userRole)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO UserRoles (RoleName, Description) VALUES (@RoleName, @Description)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", userRole.RoleName);
                    command.Parameters.AddWithValue("@Description", userRole.Description);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public UserRole? ReadUserRole(int roleId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM UserRoles WHERE RoleID = @RoleID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserRole
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString(),
                                Description = reader["Description"].ToString()
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<UserRole> ReadAllUserRoles()
        {
            return ExecuteWithConnection(connection =>
            {
                List<UserRole> userRoles = new List<UserRole>();
                string query = "SELECT * FROM UserRoles";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(new UserRole
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
                return userRoles;
            });
        }

        public void UpdateUserRole(UserRole userRole)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE UserRoles SET RoleName = @RoleName, Description = @Description WHERE RoleID = @RoleID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", userRole.RoleID);
                    command.Parameters.AddWithValue("@RoleName", userRole.RoleName);
                    command.Parameters.AddWithValue("@Description", userRole.Description);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteUserRole(int roleId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM UserRoles WHERE RoleID = @RoleID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public bool HasAssignedUsers(int roleId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM Users WHERE RoleID = @RoleID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }
    }
} 