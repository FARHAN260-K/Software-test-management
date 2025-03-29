using System;
using System.Text.RegularExpressions;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class UserLogic
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly UserRoleDataAccess _userRoleDataAccess;

        public UserLogic()
        {
            _userDataAccess = new UserDataAccess();
            _userRoleDataAccess = new UserRoleDataAccess();
        }

        public List<User> GetUsers()
        {
            return _userDataAccess.ReadAllUsers();
        }

        public List<User> GetUsersByRole(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID", nameof(roleId));
            }

            // Verify role exists
            var role = _userRoleDataAccess.ReadUserRole(roleId);
            if (role == null)
            {
                throw new InvalidOperationException("Role not found.");
            }

            return _userDataAccess.ReadUsersByRole(roleId);
        }

        public User? GetUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            return _userDataAccess.ReadUser(userId);
        }

        public void CreateUser(User user)
        {
            ValidateUser(user);

            // Verify role exists if specified
            if (user.RoleID.HasValue)
            {
                var role = _userRoleDataAccess.ReadUserRole(user.RoleID.Value);
                if (role == null)
                {
                    throw new InvalidOperationException("User role not found.");
                }
            }

            _userDataAccess.CreateUser(user);
        }

        public void UpdateUser(User user)
        {
            ValidateUser(user);

            // Verify user exists
            var existingUser = _userDataAccess.ReadUser(user.UserID);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Verify role exists if specified
            if (user.RoleID.HasValue)
            {
                var role = _userRoleDataAccess.ReadUserRole(user.RoleID.Value);
                if (role == null)
                {
                    throw new InvalidOperationException("User role not found.");
                }
            }

            _userDataAccess.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            // Verify user exists
            var user = _userDataAccess.ReadUser(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Check if user has assigned test cases
            if (_userDataAccess.HasAssignedTestCases(userId))
            {
                throw new InvalidOperationException("Cannot delete user with assigned test cases. Please reassign or delete the test cases first.");
            }

            _userDataAccess.DeleteUser(userId);
        }

        private void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Name is required.", nameof(user));
            }

            if (user.Name.Length > 100)
            {
                throw new ArgumentException("Name cannot exceed 100 characters.", nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email is required.", nameof(user));
            }

            if (user.Email.Length > 100)
            {
                throw new ArgumentException("Email cannot exceed 100 characters.", nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Password is required.", nameof(user));
            }

            if (user.Password.Length > 100)
            {
                throw new ArgumentException("Password cannot exceed 100 characters.", nameof(user));
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
} 