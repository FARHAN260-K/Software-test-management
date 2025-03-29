using System;
using MySql.Data.MySqlClient;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.DataAccess
{
    public class UserDataAccess : DataAccessBase
    {
        public void CreateUser(User user)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "INSERT INTO Users (Name, Email, Password, RoleID) VALUES (@Name, @Email, @Password, @RoleID)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleID", user.RoleID.HasValue ? (object)user.RoleID.Value : DBNull.Value);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public User? ReadUser(int userId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                RoleID = reader["RoleID"] != DBNull.Value ? Convert.ToInt32(reader["RoleID"]) : null
                            };
                        }
                        return null;
                    }
                }
            });
        }

        public List<User> ReadAllUsers()
        {
            return ExecuteWithConnection(connection =>
            {
                List<User> users = new List<User>();
                string query = "SELECT * FROM Users";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                RoleID = reader["RoleID"] != DBNull.Value ? Convert.ToInt32(reader["RoleID"]) : null
                            });
                        }
                    }
                }
                return users;
            });
        }

        public void UpdateUser(User user)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "UPDATE Users SET Name = @Name, Email = @Email, Password = @Password, RoleID = @RoleID WHERE UserID = @UserID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.UserID);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleID", user.RoleID.HasValue ? (object)user.RoleID.Value : DBNull.Value);
                    
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteUser(int userId)
        {
            ExecuteWithConnection(connection =>
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public bool HasAssignedTestCases(int userId)
        {
            return ExecuteWithConnection(connection =>
            {
                string query = "SELECT COUNT(*) FROM TestCases WHERE AssignedUser = @UserID";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            });
        }

        public List<User> ReadUsersByRole(int roleId)
        {
            List<User> users = new List<User>();
            ExecuteWithConnection(connection =>
            {
                string query = "SELECT * FROM Users WHERE RoleID = @RoleID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = reader.GetInt32("UserID"),
                                Username = reader.GetString("Username"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                Password = reader.GetString("Password"),
                                RoleID = reader.GetInt32("RoleID")
                            });
                        }
                    }
                }
            });
            return users;
        }
    }
} 