using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class TestReportLogic
    {
        private readonly TestReportDataAccess _testReportDataAccess;
        private readonly TestCaseDataAccess _testCaseDataAccess;

        public TestReportLogic()
        {
            _testReportDataAccess = new TestReportDataAccess();
            _testCaseDataAccess = new TestCaseDataAccess();
        }

        public List<TestReport> GetTestReports()
        {
            return _testReportDataAccess.ReadAllTestReports();
        }

        public TestReport? GetTestReport(int reportId)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Invalid report ID", nameof(reportId));
            }

            return _testReportDataAccess.ReadTestReport(reportId);
        }

        public List<TestReport> GetTestReportsByTestCase(int testCaseId)
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

            return _testReportDataAccess.ReadTestReportsByTestCase(testCaseId);
        }

        public void CreateTestReport(TestReport testReport)
        {
            ValidateTestReport(testReport);

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testReport.TestCaseID);
            if (testCase == null)
            {
                throw new InvalidOperationException("Test case not found.");
            }

            _testReportDataAccess.CreateTestReport(testReport);
        }

        public void UpdateTestReport(TestReport testReport)
        {
            ValidateTestReport(testReport);

            // Verify report exists
            var existingReport = _testReportDataAccess.ReadTestReport(testReport.ReportID);
            if (existingReport == null)
            {
                throw new InvalidOperationException("Test report not found.");
            }

            // Verify test case exists
            var testCase = _testCaseDataAccess.ReadTestCase(testReport.TestCaseID);
            if (testCase == null)
            {
                throw new InvalidOperationException("Test case not found.");
            }

            _testReportDataAccess.UpdateTestReport(testReport);
        }

        public void DeleteTestReport(int reportId)
        {
            if (reportId <= 0)
            {
                throw new ArgumentException("Invalid report ID", nameof(reportId));
            }

            // Verify report exists
            var report = _testReportDataAccess.ReadTestReport(reportId);
            if (report == null)
            {
                throw new InvalidOperationException("Test report not found.");
            }

            _testReportDataAccess.DeleteTestReport(reportId);
        }

        private void ValidateTestReport(TestReport testReport)
        {
            if (testReport == null)
            {
                throw new ArgumentNullException(nameof(testReport));
            }

            if (testReport.TestCaseID <= 0)
            {
                throw new ArgumentException("Valid test case ID is required.", nameof(testReport));
            }

            if (string.IsNullOrWhiteSpace(testReport.Result))
            {
                throw new ArgumentException("Result is required.", nameof(testReport));
            }

            if (testReport.Result.Length > 50)
            {
                throw new ArgumentException("Result cannot exceed 50 characters.", nameof(testReport));
            }

            if (testReport.Notes?.Length > 1000)
            {
                throw new ArgumentException("Notes cannot exceed 1000 characters.", nameof(testReport));
            }

            if (testReport.ExecutionDate == default)
            {
                throw new ArgumentException("Execution date is required.", nameof(testReport));
            }
        }
    }
} 