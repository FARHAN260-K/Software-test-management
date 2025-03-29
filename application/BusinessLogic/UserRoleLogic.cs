using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class UserRoleLogic
    {
        private readonly UserRoleDataAccess _userRoleDataAccess;

        public UserRoleLogic()
        {
            _userRoleDataAccess = new UserRoleDataAccess();
        }

        public List<UserRole> GetUserRoles()
        {
            return _userRoleDataAccess.ReadAllUserRoles();
        }

        public UserRole? GetUserRole(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID", nameof(roleId));
            }

            return _userRoleDataAccess.ReadUserRole(roleId);
        }

        public void CreateUserRole(UserRole userRole)
        {
            ValidateUserRole(userRole);

            // Check if role name already exists
            var existingRoles = _userRoleDataAccess.ReadAllUserRoles();
            if (existingRoles.Any(r => r.RoleName != null && userRole.RoleName != null && 
                r.RoleName.Equals(userRole.RoleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A role with this name already exists.");
            }

            _userRoleDataAccess.CreateUserRole(userRole);
        }

        public void UpdateUserRole(UserRole userRole)
        {
            ValidateUserRole(userRole);

            // Verify role exists
            var existingRole = _userRoleDataAccess.ReadUserRole(userRole.RoleID);
            if (existingRole == null)
            {
                throw new InvalidOperationException("Role not found.");
            }

            // Check if new name conflicts with existing roles
            var existingRoles = _userRoleDataAccess.ReadAllUserRoles();
            if (existingRoles.Any(r => r.RoleID != userRole.RoleID && 
                r.RoleName != null && userRole.RoleName != null &&
                r.RoleName.Equals(userRole.RoleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A role with this name already exists.");
            }

            _userRoleDataAccess.UpdateUserRole(userRole);
        }

        public void DeleteUserRole(int roleId)
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

            // Check if role has assigned users
            if (_userRoleDataAccess.HasAssignedUsers(roleId))
            {
                throw new InvalidOperationException("Cannot delete role with assigned users. Please reassign or delete the users first.");
            }

            _userRoleDataAccess.DeleteUserRole(roleId);
        }

        private void ValidateUserRole(UserRole userRole)
        {
            if (userRole == null)
            {
                throw new ArgumentNullException(nameof(userRole));
            }

            if (string.IsNullOrWhiteSpace(userRole.RoleName))
            {
                throw new ArgumentException("Role name is required.", nameof(userRole));
            }

            if (userRole.RoleName.Length > 50)
            {
                throw new ArgumentException("Role name cannot exceed 50 characters.", nameof(userRole));
            }

            if (userRole.Description?.Length > 200)
            {
                throw new ArgumentException("Description cannot exceed 200 characters.", nameof(userRole));
            }
        }
    }
} 