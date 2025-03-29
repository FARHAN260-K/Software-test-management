using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class TestCaseLogic
    {
        private readonly TestCaseDataAccess _testCaseDataAccess;
        private readonly ComponentDataAccess _componentDataAccess;
        private readonly UserDataAccess _userDataAccess;
        private readonly TestStatusDataAccess _testStatusDataAccess;

        public TestCaseLogic()
        {
            _testCaseDataAccess = new TestCaseDataAccess();
            _componentDataAccess = new ComponentDataAccess();
            _userDataAccess = new UserDataAccess();
            _testStatusDataAccess = new TestStatusDataAccess();
        }

        public List<TestCase> GetTestCases()
        {
            return _testCaseDataAccess.ReadAllTestCases();
        }

        public List<TestCase> GetTestCasesByComponent(int componentId)
        {
            if (componentId <= 0)
            {
                throw new ArgumentException("Invalid component ID", nameof(componentId));
            }

            // Verify component exists
            var component = _componentDataAccess.ReadComponent(componentId);
            if (component == null)
            {
                throw new InvalidOperationException("Component not found.");
            }

            return _testCaseDataAccess.ReadTestCasesByComponent(componentId);
        }

        public List<TestCase> GetTestCasesByUser(int userId)
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

            return _testCaseDataAccess.ReadTestCasesByUser(userId);
        }

        public List<TestCase> GetTestCasesByStatus(int statusId)
        {
            if (statusId <= 0)
            {
                throw new ArgumentException("Invalid status ID", nameof(statusId));
            }

            // Verify status exists
            var status = _testStatusDataAccess.ReadTestStatus(statusId);
            if (status == null)
            {
                throw new InvalidOperationException("Test status not found.");
            }

            return _testCaseDataAccess.ReadTestCasesByStatus(statusId);
        }

        public TestCase? GetTestCase(int testCaseId)
        {
            if (testCaseId <= 0)
                throw new ArgumentException("Valid test case ID is required.");

            return _testCaseDataAccess.ReadTestCase(testCaseId);
        }

        public void CreateTestCase(TestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException(nameof(testCase));

            ValidateTestCase(testCase);

            // Verify component exists
            var component = _componentDataAccess.ReadComponent(testCase.ComponentID);
            if (component == null)
                throw new InvalidOperationException("Component not found.");

            // Verify user exists
            var user = _userDataAccess.ReadUser(testCase.AssignedUser);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Verify status exists
            var status = _testStatusDataAccess.ReadTestStatus(testCase.StatusID);
            if (status == null)
                throw new InvalidOperationException("Test status not found.");

            _testCaseDataAccess.CreateTestCase(testCase);
        }

        public void UpdateTestCase(TestCase testCase)
        {
            if (testCase == null)
                throw new ArgumentNullException(nameof(testCase));

            ValidateTestCase(testCase);

            // Verify test case exists
            var existingTestCase = _testCaseDataAccess.ReadTestCase(testCase.TestCaseID);
            if (existingTestCase == null)
                throw new InvalidOperationException("Test case not found.");

            // Verify component exists
            var component = _componentDataAccess.ReadComponent(testCase.ComponentID);
            if (component == null)
                throw new InvalidOperationException("Component not found.");

            // Verify user exists
            var user = _userDataAccess.ReadUser(testCase.AssignedUser);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Verify status exists
            var status = _testStatusDataAccess.ReadTestStatus(testCase.StatusID);
            if (status == null)
                throw new InvalidOperationException("Test status not found.");

            _testCaseDataAccess.UpdateTestCase(testCase);
        }

        public void DeleteTestCase(int testCaseId)
        {
            if (testCaseId <= 0)
                throw new ArgumentException("Valid test case ID is required.");

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testCaseId);
            if (testCase == null)
                throw new InvalidOperationException("Test case not found.");

            _testCaseDataAccess.DeleteTestCase(testCaseId);
        }

        private void ValidateTestCase(TestCase testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }

            if (testCase.ComponentID <= 0)
            {
                throw new ArgumentException("Invalid component ID", nameof(testCase));
            }

            if (testCase.AssignedUser <= 0)
            {
                throw new ArgumentException("Invalid assigned user ID", nameof(testCase));
            }

            if (testCase.StatusID <= 0)
            {
                throw new ArgumentException("Invalid status ID", nameof(testCase));
            }

            if (!string.IsNullOrWhiteSpace(testCase.Name) && testCase.Name.Length > 100)
            {
                throw new ArgumentException("Test case name cannot exceed 100 characters.", nameof(testCase));
            }

            if (!string.IsNullOrWhiteSpace(testCase.Description) && testCase.Description.Length > 500)
            {
                throw new ArgumentException("Test case description cannot exceed 500 characters.", nameof(testCase));
            }

            if (!string.IsNullOrWhiteSpace(testCase.Priority) && testCase.Priority.Length > 50)
            {
                throw new ArgumentException("Priority cannot exceed 50 characters.", nameof(testCase));
            }

            if (!string.IsNullOrWhiteSpace(testCase.Status) && testCase.Status.Length > 50)
            {
                throw new ArgumentException("Status cannot exceed 50 characters.", nameof(testCase));
            }
        }
    }
} 