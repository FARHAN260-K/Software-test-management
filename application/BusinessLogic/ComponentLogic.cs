using System;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application.BusinessLogic
{
    public class ComponentLogic
    {
        private readonly ComponentDataAccess _componentDataAccess;
        private readonly ProjectDataAccess _projectDataAccess;
        private readonly TestCaseDataAccess _testCaseDataAccess;

        public ComponentLogic()
        {
            _componentDataAccess = new ComponentDataAccess();
            _projectDataAccess = new ProjectDataAccess();
            _testCaseDataAccess = new TestCaseDataAccess();
        }

        public List<Component> GetComponents()
        {
            return _componentDataAccess.ReadAllComponents();
        }

        public List<Component> GetComponentsByProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("Invalid project ID", nameof(projectId));
            }

            // Verify project exists
            var project = _projectDataAccess.ReadProject(projectId);
            if (project == null)
            {
                throw new InvalidOperationException("Project not found.");
            }

            return _componentDataAccess.ReadComponentsByProject(projectId);
        }

        public Component? GetComponent(int componentId)
        {
            if (componentId <= 0)
                throw new ArgumentException("Valid component ID is required.");

            return _componentDataAccess.ReadComponent(componentId);
        }

        public void CreateComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            // Validate required fields
            if (string.IsNullOrWhiteSpace(component.Name))
                throw new ArgumentException("Component name is required.");

            if (component.ProjectID <= 0)
                throw new ArgumentException("Valid project ID is required.");

            // Check if project exists
            var project = _projectDataAccess.ReadProject(component.ProjectID);
            if (project == null)
                throw new InvalidOperationException("Project not found.");

            // Check if component name already exists in the project
            var existingComponents = _componentDataAccess.ReadComponentsByProject(component.ProjectID);
            if (existingComponents.Any(c => c.Name != null && component.Name != null && 
                c.Name.Equals(component.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A component with this name already exists in the project.");
            }

            _componentDataAccess.CreateComponent(component);
        }

        public void UpdateComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            // Validate required fields
            if (component.ComponentID <= 0)
                throw new ArgumentException("Valid component ID is required.");

            if (string.IsNullOrWhiteSpace(component.Name))
                throw new ArgumentException("Component name is required.");

            if (component.ProjectID <= 0)
                throw new ArgumentException("Valid project ID is required.");

            // Check if component exists
            var existingComponent = _componentDataAccess.ReadComponent(component.ComponentID);
            if (existingComponent == null)
                throw new InvalidOperationException("Component not found.");

            // Check if project exists
            var project = _projectDataAccess.ReadProject(component.ProjectID);
            if (project == null)
                throw new InvalidOperationException("Project not found.");

            // Check if new name conflicts with existing components in the project
            var existingComponents = _componentDataAccess.ReadComponentsByProject(component.ProjectID);
            if (existingComponents.Any(c => c.ComponentID != component.ComponentID && 
                c.Name != null && component.Name != null &&
                c.Name.Equals(component.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A component with this name already exists in the project.");
            }

            _componentDataAccess.UpdateComponent(component);
        }

        public void DeleteComponent(int componentId)
        {
            if (componentId <= 0)
                throw new ArgumentException("Valid component ID is required.");

            // Check if component exists
            var component = _componentDataAccess.ReadComponent(componentId);
            if (component == null)
                throw new InvalidOperationException("Component not found.");

            // Check if component has any test cases
            var testCases = _testCaseDataAccess.ReadTestCasesByComponent(componentId);
            if (testCases.Any())
            {
                throw new InvalidOperationException("Cannot delete component with existing test cases.");
            }

            _componentDataAccess.DeleteComponent(componentId);
        }

        private void ValidateComponent(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (component.ProjectID <= 0)
            {
                throw new ArgumentException("Invalid project ID", nameof(component));
            }

            if (string.IsNullOrWhiteSpace(component.Name))
            {
                throw new ArgumentException("Component name is required.", nameof(component));
            }

            if (component.Name.Length > 100)
            {
                throw new ArgumentException("Component name cannot exceed 100 characters.", nameof(component));
            }

            if (component.Description?.Length > 500)
            {
                throw new ArgumentException("Component description cannot exceed 500 characters.", nameof(component));
            }
        }
    }
} 