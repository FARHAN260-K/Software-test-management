using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class ProjectLogic
    {
        private readonly ProjectDataAccess _projectDataAccess;
        private readonly ComponentDataAccess _componentDataAccess;
        private readonly TestCaseDataAccess _testCaseDataAccess;
        private readonly TestReportDataAccess _testReportDataAccess;

        public ProjectLogic()
        {
            _projectDataAccess = new ProjectDataAccess();
            _componentDataAccess = new ComponentDataAccess();
            _testCaseDataAccess = new TestCaseDataAccess();
            _testReportDataAccess = new TestReportDataAccess();
        }

        public List<Project> GetProjects()
        {
            return _projectDataAccess.ReadAllProjects();
        }

        public Project? GetProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("Invalid project ID", nameof(projectId));
            }

            return _projectDataAccess.ReadProject(projectId);
        }

        public void CreateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            ValidateProject(project);

            // Check if project name already exists
            var existingProjects = _projectDataAccess.ReadAllProjects();
            if (existingProjects.Any(p => p.Name != null && project.Name != null && 
                p.Name.Equals(project.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A project with this name already exists.");
            }

            _projectDataAccess.CreateProject(project);
        }

        public void UpdateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            ValidateProject(project);

            // Check if project exists
            var existingProject = _projectDataAccess.ReadProject(project.ProjectID);
            if (existingProject == null)
            {
                throw new InvalidOperationException("Project not found.");
            }

            // Check if new name conflicts with existing projects
            var existingProjects = _projectDataAccess.ReadAllProjects();
            if (existingProjects.Any(p => p.ProjectID != project.ProjectID && 
                p.Name != null && project.Name != null &&
                p.Name.Equals(project.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A project with this name already exists.");
            }

            _projectDataAccess.UpdateProject(project);
        }

        public void DeleteProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("Invalid project ID", nameof(projectId));
            }

            // Check if project exists
            var project = _projectDataAccess.ReadProject(projectId);
            if (project == null)
            {
                throw new InvalidOperationException("Project not found.");
            }

            // Check if project has related components
            if (_projectDataAccess.HasRelatedComponents(projectId))
            {
                throw new InvalidOperationException("Cannot delete project with existing components. Please delete all components first.");
            }

            _projectDataAccess.DeleteProject(projectId);
        }

        public void DeleteProjectCascade(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("Invalid project ID", nameof(projectId));
            }

            // Check if project exists
            var project = _projectDataAccess.ReadProject(projectId);
            if (project == null)
            {
                throw new InvalidOperationException("Project not found.");
            }

            // Get all components for this project
            var components = _componentDataAccess.ReadComponentsByProject(projectId);

            // For each component, delete its test cases and test reports
            foreach (var component in components)
            {
                // Get all test cases for this component
                var testCases = _testCaseDataAccess.ReadTestCasesByComponent(component.ComponentID);

                // For each test case, delete its test reports
                foreach (var testCase in testCases)
                {
                    var testReports = _testReportDataAccess.ReadTestReportsByTestCase(testCase.TestCaseID);
                    foreach (var report in testReports)
                    {
                        _testReportDataAccess.DeleteTestReport(report.ReportID);
                    }

                    // Delete the test case
                    _testCaseDataAccess.DeleteTestCase(testCase.TestCaseID);
                }

                // Delete the component
                _componentDataAccess.DeleteComponent(component.ComponentID);
            }

            // Finally, delete the project
            _projectDataAccess.DeleteProject(projectId);
        }

        private void ValidateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException("Project name is required.", nameof(project));
            }

            if (project.Name.Length > 100)
            {
                throw new ArgumentException("Project name cannot exceed 100 characters.", nameof(project));
            }

            if (project.Description?.Length > 500)
            {
                throw new ArgumentException("Project description cannot exceed 500 characters.", nameof(project));
            }

            if (project.StartDate > project.EndDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.", nameof(project));
            }
        }
    }
} 