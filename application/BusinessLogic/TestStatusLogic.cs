using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class TestStatusLogic
    {
        private readonly TestStatusDataAccess _testStatusDataAccess;
        private readonly TestCaseDataAccess _testCaseDataAccess;

        public TestStatusLogic()
        {
            _testStatusDataAccess = new TestStatusDataAccess();
            _testCaseDataAccess = new TestCaseDataAccess();
        }

        public List<TestStatus> GetTestStatuses()
        {
            return _testStatusDataAccess.ReadAllTestStatuses();
        }

        public TestStatus? GetTestStatus(int statusId)
        {
            if (statusId <= 0)
            {
                throw new ArgumentException("Invalid status ID", nameof(statusId));
            }

            return _testStatusDataAccess.ReadTestStatus(statusId);
        }

        public void CreateTestStatus(TestStatus testStatus)
        {
            ValidateTestStatus(testStatus);

            // Check for duplicate status names
            var existingStatuses = _testStatusDataAccess.ReadAllTestStatuses();
            if (existingStatuses.Any(s => s.StatusName != null && testStatus.StatusName != null && 
                s.StatusName.Equals(testStatus.StatusName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A test status with this name already exists.");
            }

            _testStatusDataAccess.CreateTestStatus(testStatus);
        }

        public void UpdateTestStatus(TestStatus testStatus)
        {
            ValidateTestStatus(testStatus);

            // Verify status exists
            var existingStatus = _testStatusDataAccess.ReadTestStatus(testStatus.StatusID);
            if (existingStatus == null)
            {
                throw new InvalidOperationException("Test status not found.");
            }

            // Check for duplicate status names, excluding the current status
            var existingStatuses = _testStatusDataAccess.ReadAllTestStatuses();
            if (existingStatuses.Any(s => s.StatusID != testStatus.StatusID && 
                s.StatusName != null && testStatus.StatusName != null &&
                s.StatusName.Equals(testStatus.StatusName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A test status with this name already exists.");
            }

            _testStatusDataAccess.UpdateTestStatus(testStatus);
        }

        public void DeleteTestStatus(int statusId)
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

            // Check if status is in use
            var testCases = _testCaseDataAccess.ReadAllTestCases();
            if (testCases.Any(tc => tc.StatusID == statusId))
            {
                throw new InvalidOperationException("Cannot delete test status that is in use by test cases.");
            }

            _testStatusDataAccess.DeleteTestStatus(statusId);
        }

        private void ValidateTestStatus(TestStatus testStatus)
        {
            if (testStatus == null)
            {
                throw new ArgumentNullException(nameof(testStatus));
            }

            if (string.IsNullOrWhiteSpace(testStatus.StatusName))
            {
                throw new ArgumentException("Status name is required.", nameof(testStatus));
            }

            if (testStatus.StatusName.Length > 50)
            {
                throw new ArgumentException("Status name cannot exceed 50 characters.", nameof(testStatus));
            }

            if (testStatus.Description?.Length > 200)
            {
                throw new ArgumentException("Description cannot exceed 200 characters.", nameof(testStatus));
            }
        }
    }
} 