using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class TestHistoryLogic
    {
        private readonly TestHistoryDataAccess _testHistoryDataAccess;
        private readonly TestCaseDataAccess _testCaseDataAccess;
        private readonly UserDataAccess _userDataAccess;

        public TestHistoryLogic()
        {
            _testHistoryDataAccess = new TestHistoryDataAccess();
            _testCaseDataAccess = new TestCaseDataAccess();
            _userDataAccess = new UserDataAccess();
        }

        public List<TestHistory> GetTestHistories()
        {
            return _testHistoryDataAccess.ReadAllTestHistory();
        }

        public List<TestHistory> GetTestHistoriesByTestCase(int testCaseId)
        {
            if (testCaseId <= 0)
            {
                throw new ArgumentException("Invalid test case ID", nameof(testCaseId));
            }

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testCaseId);
            if (testCase == null)
            {
                throw new InvalidOperationException("Test case not found.");
            }

            return _testHistoryDataAccess.ReadTestHistoryByTestCase(testCaseId);
        }

        public List<TestHistory> GetTestHistoriesByUser(int userId)
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

            return _testHistoryDataAccess.ReadTestHistoryByUser(userId);
        }

        public TestHistory? GetTestHistory(int historyId)
        {
            if (historyId <= 0)
            {
                throw new ArgumentException("Invalid history ID", nameof(historyId));
            }

            return _testHistoryDataAccess.ReadTestHistory(historyId);
        }

        public void CreateTestHistory(TestHistory testHistory)
        {
            ValidateTestHistory(testHistory);

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testHistory.TestCaseID);
            if (testCase == null)
            {
                throw new InvalidOperationException("Test case not found.");
            }

            // Verify user exists
            var user = _userDataAccess.ReadUser(testHistory.UserID);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Set timestamp to current time if not specified
            if (testHistory.Timestamp == default)
            {
                testHistory.Timestamp = DateTime.Now;
            }

            _testHistoryDataAccess.CreateTestHistory(testHistory);
        }

        public void UpdateTestHistory(TestHistory testHistory)
        {
            ValidateTestHistory(testHistory);

            // Verify history entry exists
            var existingHistory = _testHistoryDataAccess.ReadTestHistory(testHistory.HistoryID);
            if (existingHistory == null)
            {
                throw new InvalidOperationException("Test history entry not found.");
            }

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testHistory.TestCaseID);
            if (testCase == null)
            {
                throw new InvalidOperationException("Test case not found.");
            }

            // Verify user exists
            var user = _userDataAccess.ReadUser(testHistory.UserID);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _testHistoryDataAccess.UpdateTestHistory(testHistory);
        }

        public void DeleteTestHistory(int historyId)
        {
            if (historyId <= 0)
            {
                throw new ArgumentException("Invalid history ID", nameof(historyId));
            }

            // Verify history entry exists
            var history = _testHistoryDataAccess.ReadTestHistory(historyId);
            if (history == null)
            {
                throw new InvalidOperationException("Test history entry not found.");
            }

            _testHistoryDataAccess.DeleteTestHistory(historyId);
        }

        private void ValidateTestHistory(TestHistory testHistory)
        {
            if (testHistory == null)
            {
                throw new ArgumentNullException(nameof(testHistory));
            }

            if (testHistory.TestCaseID <= 0)
            {
                throw new ArgumentException("Invalid test case ID", nameof(testHistory));
            }

            if (testHistory.UserID <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(testHistory));
            }

            if (string.IsNullOrWhiteSpace(testHistory.Action))
            {
                throw new ArgumentException("Action is required.", nameof(testHistory));
            }

            if (testHistory.Action.Length > 50)
            {
                throw new ArgumentException("Action cannot exceed 50 characters.", nameof(testHistory));
            }

            if (testHistory.Details?.Length > 1000)
            {
                throw new ArgumentException("Details cannot exceed 1000 characters.", nameof(testHistory));
            }
        }
    }
} 