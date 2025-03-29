using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using SoftwareTestManager.Application.DataAccess;
using SoftwareTestManager.Application.BusinessLogic;
using SoftwareTestManager.Application.Models;

namespace SoftwareTestManager.Application
{
    public class Program
    {
        // Business Logic instances
        private static readonly ProjectLogic _projectLogic = new ProjectLogic();
        private static readonly ComponentLogic _componentLogic = new ComponentLogic();
        private static readonly TestCaseLogic _testCaseLogic = new TestCaseLogic();
        private static readonly UserLogic _userLogic = new UserLogic();
        private static readonly UserRoleLogic _userRoleLogic = new UserRoleLogic();
        private static readonly TestReportLogic _testReportLogic = new TestReportLogic();
        private static readonly TestStatusLogic _testStatusLogic = new TestStatusLogic();
        private static readonly TestHistoryLogic _testHistoryLogic = new TestHistoryLogic();

        public static void Main(string[] args)
        {
            Console.WriteLine("Software Test Manager Application");
            
            try
            {
                DataAccessBase.ExecuteWithConnection(connection =>
                {
                    Console.WriteLine("Connected to MySQL!");
                });
                
                bool exitApplication = false;
                
                while (!exitApplication)
                {
                    // Display main menu
                    Console.WriteLine("\n===== MAIN MENU =====");
                    Console.WriteLine("1. Projects");
                    Console.WriteLine("2. Components");
                    Console.WriteLine("3. Test Cases");
                    Console.WriteLine("4. Users");
                    Console.WriteLine("5. User Roles");
                    Console.WriteLine("6. Test Reports");
                    Console.WriteLine("7. Test Statuses");
                    Console.WriteLine("8. Test Histories");
                    Console.WriteLine("0. Exit Application");
                    Console.Write("Select an option: ");
                    
                    string? mainMenuChoice = Console.ReadLine() ?? string.Empty;
                    
                    switch (mainMenuChoice)
                    {
                        case "1": // Projects
                            ManageProjects();
                            break;
                        case "2": // Components
                            ManageComponents();
                            break;
                        case "3": // Test Cases
                            ManageTestCases();
                            break;
                        case "4": // Users
                            ManageUsers();
                            break;
                        case "5": // User Roles
                            ManageUserRoles();
                            break;
                        case "6": // Test Reports
                            ManageTestReports();
                            break;
                        case "7": // Test Statuses
                            ManageTestStatuses();
                            break;
                        case "8": // Test Histories
                            ManageTestHistories();
                            break;
                        case "0": // Exit
                            exitApplication = true;
                            Console.WriteLine("Exiting application. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ManageProjects()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Projects submenu
                Console.WriteLine("\n===== PROJECTS MENU =====");
                Console.WriteLine("1. Create Project");
                Console.WriteLine("2. Read Project(s)");
                Console.WriteLine("3. Update Project");
                Console.WriteLine("4. Delete Project");
                Console.WriteLine("5. Delete Project with Cascade");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Project ---");
                        Console.Write("Name: ");
                        string? name = Console.ReadLine() ?? string.Empty;
                        Console.Write("Description: ");
                        string? description = Console.ReadLine() ?? string.Empty;
                        
                        DateTime startDate, endDate;
                        Console.Write("Start Date (yyyy-MM-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out startDate))
                        {
                            Console.Write("End Date (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out endDate))
                            {
                                CreateProject(name, description, startDate, endDate);
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format for End Date.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format for Start Date.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Project(s) ---");
                        Console.WriteLine("1. Read all projects");
                        Console.WriteLine("2. Read specific project");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine();
                        
                        if (readChoice == "1")
                        {
                            ReadProject();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter Project ID: ");
                            if (int.TryParse(Console.ReadLine(), out int projectId))
                            {
                                ReadProject(projectId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Project ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Project ---");
                        Console.Write("Project ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateProjectId))
                        {
                            Console.Write("New Name: ");
                            string? updateName = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Description: ");
                            string? updateDescription = Console.ReadLine() ?? string.Empty;
                            
                            DateTime updateStartDate, updateEndDate;
                            Console.Write("New Start Date (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out updateStartDate))
                            {
                                Console.Write("New End Date (yyyy-MM-dd): ");
                                if (DateTime.TryParse(Console.ReadLine(), out updateEndDate))
                                {
                                    UpdateProject(updateProjectId, updateName, updateDescription, updateStartDate, updateEndDate);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid date format for End Date.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format for Start Date.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Project ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Project ---");
                        Console.Write("Project ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteProjectId))
                        {
                            DeleteProject(deleteProjectId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Project ID.");
                        }
                        break;
                    
                    case "5": // Delete with Cascade
                        Console.WriteLine("\n--- Delete Project with Cascade ---");
                        Console.WriteLine("WARNING: This will delete the project and ALL related components, test cases, and test reports.");
                        Console.Write("Project ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int cascadeDeleteProjectId))
                        {
                            Console.Write("Are you sure? (y/n): ");
                            string? confirm = Console.ReadLine()?.ToLower() ?? string.Empty;
                            if (confirm == "y" || confirm == "yes")
                            {
                                _projectLogic.DeleteProjectCascade(cascadeDeleteProjectId);
                            }
                            else
                            {
                                Console.WriteLine("Deletion cancelled.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Project ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void CreateProject(string? Name, string? Description, DateTime StartDate, DateTime EndDate)
        {
            // Implementation for creating a project
            Console.WriteLine($"Creating project: {Name}");
            
            try
            {
                Project newProject = new Project
                {
                    Name = Name,
                    Description = Description,
                    StartDate = StartDate,
                    EndDate = EndDate
                };
                
                _projectLogic.CreateProject(newProject);
                Console.WriteLine("Project created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
            }
        }

        public static void ReadProject(int? ProjectID = null)
        {
            try
            {
                if (ProjectID.HasValue)
                {
                    // Retrieve a specific project
                    Console.WriteLine($"Reading project with ID: {ProjectID}");
                    
                    Project? project = _projectLogic.GetProject(ProjectID.Value);
                    
                    if (project != null)
                    {
                        Console.WriteLine($"Project ID: {project.ProjectID}");
                        Console.WriteLine($"Name: {project.Name ?? "N/A"}");
                        Console.WriteLine($"Description: {project.Description ?? "N/A"}");
                        Console.WriteLine($"Start Date: {project.StartDate}");
                        Console.WriteLine($"End Date: {project.EndDate}");
                        Console.WriteLine("Components:");
                        
                        // Get components for this project
                        List<Component> components = _componentLogic.GetComponentsByProject(project.ProjectID);
                        
                        if (components.Count > 0)
                        {
                            foreach (var component in components)
                            {
                                Console.WriteLine($"  - Component ID: {component.ComponentID}");
                                Console.WriteLine($"    Name: {component.Name}");
                                Console.WriteLine($"    Description: {component.Description}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("  No components found for this project.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No project found with the specified ID.");
                    }
                }
                else
                {
                    // Retrieve all projects
                    Console.WriteLine("Reading all projects");
                    
                    List<Project> projects = _projectLogic.GetProjects();
                    
                    if (projects.Count > 0)
                    {
                        foreach (var project in projects)
                        {
                            Console.WriteLine($"Project ID: {project.ProjectID}");
                            Console.WriteLine($"Name: {project.Name ?? "N/A"}");
                            Console.WriteLine($"Description: {project.Description ?? "N/A"}");
                            Console.WriteLine($"Start Date: {project.StartDate}");
                            Console.WriteLine($"End Date: {project.EndDate}");
                            Console.WriteLine("----------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No projects found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading project(s): {ex.Message}");
            }
        }
        
        public static void UpdateProject(int ProjectID, string? Name, string? Description, DateTime StartDate, DateTime EndDate)
        {
            Console.WriteLine($"Updating project with ID: {ProjectID}");
            
            try
            {
                // Get the existing project first
                Project? existingProject = _projectLogic.GetProject(ProjectID);
                
                if (existingProject == null)
                {
                    Console.WriteLine("No project found with the specified ID.");
                    return;
                }
                
                // Update the project properties
                existingProject.Name = Name;
                existingProject.Description = Description;
                existingProject.StartDate = StartDate;
                existingProject.EndDate = EndDate;
                
                // Update the project in the database
                _projectLogic.UpdateProject(existingProject);
                Console.WriteLine("Project updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating project: {ex.Message}");
            }
        }
        
        public static void DeleteProject(int ProjectID)
        {
            Console.WriteLine($"Deleting project with ID: {ProjectID}");
            
            try
            {
                _projectLogic.DeleteProject(ProjectID);
                Console.WriteLine("Project deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting project: {ex.Message}");
            }
        }

        private static void DisplayProjects(List<Project> projects)
        {
            if (projects.Count == 0)
            {
                Console.WriteLine("No projects found.");
            }
            else
            {
                foreach (var project in projects)
                {
                    DisplayProject(project);
                }
            }
        }

        private static void DisplayProject(Project project)
        {
            if (project == null)
            {
                Console.WriteLine("No project found with the specified ID.");
            }
            else
            {
                Console.WriteLine($"Project ID: {project.ProjectID}");
                Console.WriteLine($"Name: {project.Name ?? "N/A"}");
                Console.WriteLine($"Description: {project.Description ?? "N/A"}");
                Console.WriteLine($"Start Date: {project.StartDate}");
                Console.WriteLine($"End Date: {project.EndDate}");
                Console.WriteLine("----------------------------");
            }
        }

        private static void ManageComponents()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Components submenu
                Console.WriteLine("\n===== COMPONENTS MENU =====");
                Console.WriteLine("1. Create Component");
                Console.WriteLine("2. Read Component(s)");
                Console.WriteLine("3. Update Component");
                Console.WriteLine("4. Delete Component");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Component ---");
                        Console.Write("Project ID: ");
                        if (int.TryParse(Console.ReadLine(), out int projectId))
                        {
                            Console.Write("Name: ");
                            string? name = Console.ReadLine() ?? string.Empty;
                            Console.Write("Description: ");
                            string? description = Console.ReadLine() ?? string.Empty;
                            
                            CreateComponent(projectId, name, description);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Project ID.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Component(s) ---");
                        Console.WriteLine("1. Read all components");
                        Console.WriteLine("2. Read components for a project");
                        Console.WriteLine("3. Read specific component");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine();
                        
                        if (readChoice == "1")
                        {
                            ReadComponent();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter Project ID: ");
                            if (int.TryParse(Console.ReadLine(), out int projectIdForComponents))
                            {
                                ReadComponent(null, projectIdForComponents);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Project ID.");
                            }
                        }
                        else if (readChoice == "3")
                        {
                            Console.Write("Enter Component ID: ");
                            if (int.TryParse(Console.ReadLine(), out int componentId))
                            {
                                ReadComponent(componentId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Component ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Component ---");
                        Console.Write("Component ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateComponentId))
                        {
                            Console.Write("New Project ID: ");
                            if (int.TryParse(Console.ReadLine(), out int newProjectId))
                            {
                                Console.Write("New Name: ");
                                string? updateName = Console.ReadLine() ?? string.Empty;
                                Console.Write("New Description: ");
                                string? updateDescription = Console.ReadLine() ?? string.Empty;
                                
                                UpdateComponent(updateComponentId, newProjectId, updateName, updateDescription);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Project ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Component ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Component ---");
                        Console.Write("Component ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteComponentId))
                        {
                            DeleteComponent(deleteComponentId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Component ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void CreateComponent(int ProjectID, string? Name, string? Description)
        {
            Console.WriteLine($"Creating component: {Name} for Project ID: {ProjectID}");
            
            try
            {
                Component newComponent = new Component
                {
                    ProjectID = ProjectID,
                    Name = Name,
                    Description = Description
                };
                
                _componentLogic.CreateComponent(newComponent);
                Console.WriteLine("Component created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating component: {ex.Message}");
            }
        }
        
        public static void ReadComponent(int? ComponentID = null, int? ProjectID = null)
        {
            try
            {
                if (ComponentID.HasValue)
                {
                    // Retrieve a specific component
                    Console.WriteLine($"Reading component with ID: {ComponentID}");
                    
                    Component? component = _componentLogic.GetComponent(ComponentID.Value);
                    
                    if (component != null)
                    {
                        Console.WriteLine($"Component ID: {component.ComponentID}");
                        Console.WriteLine($"Name: {component.Name ?? "N/A"}");
                        Console.WriteLine($"Description: {component.Description ?? "N/A"}");
                        Console.WriteLine($"Project ID: {component.ProjectID}");
                        
                        // Get the project information
                        Project? project = _projectLogic.GetProject(component.ProjectID);
                        if (project != null)
                        {
                            Console.WriteLine($"Project Name: {project.Name ?? "N/A"}");
                            Console.WriteLine($"Project Description: {project.Description ?? "N/A"}");
                        }
                        
                        Console.WriteLine("----------------------------");
                    }
                    else
                    {
                        Console.WriteLine("No component found with the specified ID.");
                    }
                }
                else if (ProjectID.HasValue)
                {
                    // Retrieve all components for a project
                    Console.WriteLine($"Reading all components for Project ID: {ProjectID}");
                    
                    List<Component> components = _componentLogic.GetComponentsByProject(ProjectID.Value);
                    
                    if (components.Count > 0)
                    {
                        // Get the project information
                        Project? project = _projectLogic.GetProject(ProjectID.Value);
                        string projectName = project?.Name ?? "N/A";
                        string projectDescription = project?.Description ?? "N/A";
                        
                        foreach (var component in components)
                        {
                            Console.WriteLine($"Component ID: {component.ComponentID}");
                            Console.WriteLine($"Name: {component.Name ?? "N/A"}");
                            Console.WriteLine($"Description: {component.Description ?? "N/A"}");
                            Console.WriteLine($"Project ID: {component.ProjectID}");
                            Console.WriteLine($"Project Name: {projectName}");
                            Console.WriteLine($"Project Description: {projectDescription}");
                            Console.WriteLine("----------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No components found for the specified project.");
                    }
                }
                else
                {
                    // Retrieve all components
                    Console.WriteLine("Reading all components");
                    
                    List<Component> components = _componentLogic.GetComponents();
                    
                    if (components.Count > 0)
                    {
                        foreach (var component in components)
                        {
                            Console.WriteLine($"Component ID: {component.ComponentID}");
                            Console.WriteLine($"Name: {component.Name ?? "N/A"}");
                            Console.WriteLine($"Description: {component.Description ?? "N/A"}");
                            Console.WriteLine($"Project ID: {component.ProjectID}");
                            
                            // Get the project information
                            Project? project = _projectLogic.GetProject(component.ProjectID);
                            if (project != null)
                            {
                                Console.WriteLine($"Project Name: {project.Name ?? "N/A"}");
                                Console.WriteLine($"Project Description: {project.Description ?? "N/A"}");
                            }
                            
                            Console.WriteLine("----------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No components found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading component(s): {ex.Message}");
            }
        }
        
        public static void UpdateComponent(int ComponentID, int ProjectID, string? Name, string? Description)
        {
            Console.WriteLine($"Updating component with ID: {ComponentID}");
            
            try
            {
                // Get the existing component first
                Component? existingComponent = _componentLogic.GetComponent(ComponentID);
                
                if (existingComponent == null)
                {
                    Console.WriteLine("No component found with the specified ID.");
                    return;
                }
                
                // Update the component properties
                existingComponent.ProjectID = ProjectID;
                existingComponent.Name = Name;
                existingComponent.Description = Description;
                
                // Update the component in the database
                _componentLogic.UpdateComponent(existingComponent);
                Console.WriteLine("Component updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating component: {ex.Message}");
            }
        }
        
        public static void DeleteComponent(int ComponentID)
        {
            Console.WriteLine($"Deleting component with ID: {ComponentID}");
            
            try
            {
                _componentLogic.DeleteComponent(ComponentID);
                Console.WriteLine("Component deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting component: {ex.Message}");
            }
        }

        private static void ManageTestCases()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Test Cases submenu
                Console.WriteLine("\n===== TEST CASES MENU =====");
                Console.WriteLine("1. Create Test Case");
                Console.WriteLine("2. Read Test Case(s)");
                Console.WriteLine("3. Update Test Case");
                Console.WriteLine("4. Delete Test Case");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Test Case ---");
                        Console.Write("Component ID: ");
                        if (int.TryParse(Console.ReadLine(), out int componentId))
                        {
                            Console.Write("Name: ");
                            string? name = Console.ReadLine() ?? string.Empty;
                            Console.Write("Description: ");
                            string? description = Console.ReadLine() ?? string.Empty;
                            Console.Write("Priority: ");
                            string? priority = Console.ReadLine() ?? string.Empty;
                            Console.Write("Status: ");
                            string? status = Console.ReadLine() ?? string.Empty;
                            Console.Write("Assigned User ID: ");
                            if (int.TryParse(Console.ReadLine(), out int assignedUser))
                            {
                                Console.Write("Status ID: ");
                                if (int.TryParse(Console.ReadLine(), out int statusId))
                                {
                                    CreateTestCase(componentId, description, priority, status, assignedUser, statusId);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Status ID.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Assigned User ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Component ID.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Test Case(s) ---");
                        Console.WriteLine("1. Read all test cases");
                        Console.WriteLine("2. Read test cases for a component");
                        Console.WriteLine("3. Read test cases for a user");
                        Console.WriteLine("4. Read specific test case");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine();
                        
                        if (readChoice == "1")
                        {
                            var allTestCases = _testCaseLogic.GetTestCases();
                            DisplayTestCases(allTestCases);
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter Component ID: ");
                            if (int.TryParse(Console.ReadLine(), out int componentIdForTestCases))
                            {
                                var componentTestCases = _testCaseLogic.GetTestCasesByComponent(componentIdForTestCases);
                                DisplayTestCases(componentTestCases);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Component ID.");
                            }
                        }
                        else if (readChoice == "3")
                        {
                            Console.Write("Enter User ID: ");
                            if (int.TryParse(Console.ReadLine(), out int userIdForTestCases))
                            {
                                var userTestCases = _testCaseLogic.GetTestCasesByUser(userIdForTestCases);
                                DisplayTestCases(userTestCases);
                            }
                            else
                            {
                                Console.WriteLine("Invalid User ID.");
                            }
                        }
                        else if (readChoice == "4")
                        {
                            Console.Write("Enter Test Case ID: ");
                            if (int.TryParse(Console.ReadLine(), out int specificTestCaseId))
                            {
                                var specificTestCase = _testCaseLogic.GetTestCase(specificTestCaseId);
                                DisplayTestCase(specificTestCase);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Test Case ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Test Case ---");
                        Console.Write("Test Case ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateTestCaseId))
                        {
                            Console.Write("New Component ID: ");
                            if (int.TryParse(Console.ReadLine(), out int newComponentId))
                            {
                                Console.Write("New Description: ");
                                string? updateDescription = Console.ReadLine() ?? string.Empty;
                                Console.Write("New Priority: ");
                                string? updatePriority = Console.ReadLine() ?? string.Empty;
                                Console.Write("New Status: ");
                                string? updateStatus = Console.ReadLine() ?? string.Empty;
                                Console.Write("New Assigned User ID: ");
                                if (int.TryParse(Console.ReadLine(), out int updateAssignedUser))
                                {
                                    Console.Write("New Status ID: ");
                                    if (int.TryParse(Console.ReadLine(), out int updateStatusId))
                                    {
                                        UpdateTestCase(updateTestCaseId, newComponentId, updateDescription, updatePriority, updateStatus, updateAssignedUser, updateStatusId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Status ID.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Assigned User ID.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Component ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Test Case ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Test Case ---");
                        Console.Write("Test Case ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteTestCaseId))
                        {
                            _testCaseLogic.DeleteTestCase(deleteTestCaseId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Test Case ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void DisplayTestCases(List<TestCase> testCases)
        {
            if (testCases.Count == 0)
            {
                Console.WriteLine("No test cases found.");
            }
            else
            {
                foreach (var testCase in testCases)
                {
                    DisplayTestCase(testCase);
                }
            }
        }

        private static void DisplayTestCase(TestCase? testCase)
        {
            if (testCase == null)
            {
                Console.WriteLine("No test case found with the specified ID.");
                return;
            }

            Console.WriteLine($"Test Case ID: {testCase.TestCaseID}");
            Console.WriteLine($"Name: {testCase.Name ?? "N/A"}");
            Console.WriteLine($"Description: {testCase.Description ?? "N/A"}");
            Console.WriteLine($"Component ID: {testCase.ComponentID}");
            Console.WriteLine($"Component Name: {testCase.ComponentName ?? "N/A"}");
            Console.WriteLine($"Component Description: {testCase.ComponentDescription ?? "N/A"}");
            Console.WriteLine($"Priority: {testCase.Priority ?? "N/A"}");
            Console.WriteLine($"Status: {testCase.Status ?? "N/A"}");
            Console.WriteLine($"Assigned User ID: {testCase.AssignedUser}");
            Console.WriteLine($"Status ID: {testCase.StatusID}");
            Console.WriteLine("----------------------------");
        }

        public static void CreateTestCase(int ComponentID, string? Description, string? Priority, string? Status, int AssignedUser, int StatusID)
        {
            Console.WriteLine($"Creating test case for Component ID: {ComponentID}");
            
            try
            {
                TestCase newTestCase = new TestCase
                {
                    ComponentID = ComponentID,
                    Description = Description,
                    Priority = Priority,
                    Status = Status,
                    AssignedUser = AssignedUser,
                    StatusID = StatusID
                };
                
                _testCaseLogic.CreateTestCase(newTestCase);
                Console.WriteLine("Test case created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test case: {ex.Message}");
            }
        }
        
        public static void ReadTestCase(int? TestCaseID = null, int? ComponentID = null, int? UserID = null)
        {
            try
            {
                if (TestCaseID.HasValue)
                {
                    // Retrieve a specific test case
                    Console.WriteLine($"Reading test case with ID: {TestCaseID}");
                    
                    TestCase? testCase = _testCaseLogic.GetTestCase(TestCaseID.Value);
                    
                    if (testCase != null)
                    {
                        DisplayTestCaseDetails(testCase);
                    }
                    else
                    {
                        Console.WriteLine("No test case found with the specified ID.");
                    }
                }
                else if (ComponentID.HasValue)
                {
                    // Retrieve all test cases for a component
                    Console.WriteLine($"Reading all test cases for Component ID: {ComponentID}");
                    
                    List<TestCase> testCases = _testCaseLogic.GetTestCasesByComponent(ComponentID.Value);
                    DisplayTestCases(testCases);
                }
                else if (UserID.HasValue)
                {
                    // Retrieve all test cases for a user
                    Console.WriteLine($"Reading all test cases for User ID: {UserID}");
                    
                    List<TestCase> testCases = _testCaseLogic.GetTestCasesByUser(UserID.Value);
                    DisplayTestCases(testCases);
                }
                else
                {
                    // Retrieve all test cases
                    Console.WriteLine("Reading all test cases");
                    
                    List<TestCase> testCases = _testCaseLogic.GetTestCases();
                    DisplayTestCases(testCases);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading test case(s): {ex.Message}");
            }
        }
        
        public static void UpdateTestCase(int TestCaseID, int ComponentID, string? Description, string? Priority, string? Status, int AssignedUser, int StatusID)
        {
            Console.WriteLine($"Updating test case with ID: {TestCaseID}");
            
            try
            {
                // Get the existing test case first
                TestCase? existingTestCase = _testCaseLogic.GetTestCase(TestCaseID);
                
                if (existingTestCase == null)
                {
                    Console.WriteLine("No test case found with the specified ID.");
                    return;
                }
                
                // Update the test case properties
                existingTestCase.ComponentID = ComponentID;
                existingTestCase.Description = Description;
                existingTestCase.Priority = Priority;
                existingTestCase.Status = Status;
                existingTestCase.AssignedUser = AssignedUser;
                existingTestCase.StatusID = StatusID;
                
                // Update the test case in the database
                _testCaseLogic.UpdateTestCase(existingTestCase);
                Console.WriteLine("Test case updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating test case: {ex.Message}");
            }
        }
        
        public static void DeleteTestCase(int TestCaseID)
        {
            Console.WriteLine($"Deleting test case with ID: {TestCaseID}");
            
            try
            {
                _testCaseLogic.DeleteTestCase(TestCaseID);
                Console.WriteLine("Test case deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting test case: {ex.Message}");
            }
        }

        private static void DisplayTestCaseDetails(TestCase testCase)
        {
            Console.WriteLine($"Test Case ID: {testCase.TestCaseID}");
            Console.WriteLine($"Description: {testCase.Description ?? "N/A"}");
            Console.WriteLine($"Priority: {testCase.Priority ?? "N/A"}");
            Console.WriteLine($"Status: {testCase.Status ?? "N/A"}");
            Console.WriteLine($"Assigned User ID: {testCase.AssignedUser}");
            Console.WriteLine($"Status ID: {testCase.StatusID}");
            
            // Get component information
            Component? component = _componentLogic.GetComponent(testCase.ComponentID);
            if (component != null)
            {
                Console.WriteLine($"Component ID: {component.ComponentID}");
                Console.WriteLine($"Component Name: {component.Name ?? "N/A"}");
                Console.WriteLine($"Component Description: {component.Description ?? "N/A"}");
            }
            
            // Get assigned user information
            User? user = _userLogic.GetUser(testCase.AssignedUser);
            if (user != null)
            {
                Console.WriteLine($"Assigned User Name: {user.Name ?? "N/A"}");
                Console.WriteLine($"Assigned User Email: {user.Email ?? "N/A"}");
            }
            
            Console.WriteLine("----------------------------");
        }

        private static void ManageUsers()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Users submenu
                Console.WriteLine("\n===== USERS MENU =====");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Read User(s)");
                Console.WriteLine("3. Update User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create User ---");
                        Console.Write("Username: ");
                        string? username = Console.ReadLine() ?? string.Empty;
                        Console.Write("Password: ");
                        string? password = Console.ReadLine() ?? string.Empty;
                        Console.Write("Email: ");
                        string? email = Console.ReadLine() ?? string.Empty;
                        Console.Write("Name: ");
                        string? name = Console.ReadLine() ?? string.Empty;
                        Console.Write("Role ID: ");
                        if (int.TryParse(Console.ReadLine(), out int roleId))
                        {
                            var newUser = new User
                            {
                                Username = username,
                                Password = password,
                                Email = email,
                                Name = name,
                                RoleID = roleId
                            };
                            _userLogic.CreateUser(newUser);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Role ID.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read User(s) ---");
                        Console.WriteLine("1. Read all users");
                        Console.WriteLine("2. Read specific user");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine();
                        
                        if (readChoice == "1")
                        {
                            var users = _userLogic.GetUsers();
                            DisplayUsers(users);
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter User ID: ");
                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                var user = _userLogic.GetUser(userId);
                                DisplayUser(user);
                            }
                            else
                            {
                                Console.WriteLine("Invalid User ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update User ---");
                        Console.Write("User ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateUserId))
                        {
                            Console.Write("New Username: ");
                            string? updateUsername = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Password: ");
                            string? updatePassword = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Email: ");
                            string? updateEmail = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Name: ");
                            string? updateName = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Role ID: ");
                            if (int.TryParse(Console.ReadLine(), out int updateRoleId))
                            {
                                var updatedUser = new User
                                {
                                    UserID = updateUserId,
                                    Username = updateUsername,
                                    Password = updatePassword,
                                    Email = updateEmail,
                                    Name = updateName,
                                    RoleID = updateRoleId
                                };
                                _userLogic.UpdateUser(updatedUser);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Role ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid User ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete User ---");
                        Console.Write("User ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteUserId))
                        {
                            _userLogic.DeleteUser(deleteUserId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid User ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void DisplayUsers(List<User> users)
        {
            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");
            }
            else
            {
                foreach (var user in users)
                {
                    DisplayUser(user);
                }
            }
        }

        private static void DisplayUser(User? user)
        {
            if (user == null)
            {
                Console.WriteLine("No user found with the specified ID.");
                return;
            }

            Console.WriteLine($"User ID: {user.UserID}");
            Console.WriteLine($"Username: {user.Username ?? "N/A"}");
            Console.WriteLine($"Name: {user.Name ?? "N/A"}");
            Console.WriteLine($"Email: {user.Email ?? "N/A"}");
            Console.WriteLine($"Role ID: {user.RoleID ?? 0}");
            Console.WriteLine("----------------------------");
        }

        private static void ManageUserRoles()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display User Roles submenu
                Console.WriteLine("\n===== USER ROLES MENU =====");
                Console.WriteLine("1. Create User Role");
                Console.WriteLine("2. Read User Role(s)");
                Console.WriteLine("3. Update User Role");
                Console.WriteLine("4. Delete User Role");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create User Role ---");
                        Console.Write("Role Name: ");
                        string? roleName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Role Description: ");
                        string? roleDescription = Console.ReadLine() ?? string.Empty;
                        
                        CreateUserRole(roleName, roleDescription);
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read User Role(s) ---");
                        Console.WriteLine("1. Read all user roles");
                        Console.WriteLine("2. Read specific user role");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine();
                        
                        if (readChoice == "1")
                        {
                            ReadUserRole();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter User Role ID: ");
                            if (int.TryParse(Console.ReadLine(), out int userRoleId))
                            {
                                var userRole = _userRoleLogic.GetUserRole(userRoleId);
                                DisplayUserRole(userRole);
                            }
                            else
                            {
                                Console.WriteLine("Invalid User Role ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update User Role ---");
                        Console.Write("User Role ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateUserRoleId))
                        {
                            Console.Write("New Name: ");
                            string? updateName = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Description: ");
                            string? updateDescription = Console.ReadLine() ?? string.Empty;
                            
                            UpdateUserRole(updateUserRoleId, updateName, updateDescription);
                        }
                        else
                        {
                            Console.WriteLine("Invalid User Role ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete User Role ---");
                        Console.Write("User Role ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteUserRoleId))
                        {
                            _userRoleLogic.DeleteUserRole(deleteUserRoleId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid User Role ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void CreateUserRole(string? RoleName, string? Description)
        {
            Console.WriteLine($"Creating user role: {RoleName}");
            
            try
            {
                UserRole newRole = new UserRole
                {
                    RoleName = RoleName,
                    Description = Description
                };
                
                _userRoleLogic.CreateUserRole(newRole);
                Console.WriteLine("User role created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user role: {ex.Message}");
            }
        }
        
        public static void ReadUserRole(int? RoleID = null)
        {
            try
            {
                if (RoleID.HasValue)
                {
                    // Retrieve a specific user role
                    Console.WriteLine($"Reading user role with ID: {RoleID}");
                    
                    UserRole? role = _userRoleLogic.GetUserRole(RoleID.Value);
                    
                    if (role != null)
                    {
                        DisplayUserRoleDetails(role);
                    }
                    else
                    {
                        Console.WriteLine("No user role found with the specified ID.");
                    }
                }
                else
                {
                    // Retrieve all user roles
                    Console.WriteLine("Reading all user roles");
                    
                    List<UserRole> roles = _userRoleLogic.GetUserRoles();
                    
                    if (roles.Count > 0)
                    {
                        foreach (var role in roles)
                        {
                            DisplayUserRoleDetails(role);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No user roles found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading user role(s): {ex.Message}");
            }
        }
        
        public static void UpdateUserRole(int RoleID, string? RoleName, string? Description)
        {
            Console.WriteLine($"Updating user role with ID: {RoleID}");
            
            try
            {
                // Get the existing user role first
                UserRole? existingRole = _userRoleLogic.GetUserRole(RoleID);
                
                if (existingRole == null)
                {
                    Console.WriteLine("No user role found with the specified ID.");
                    return;
                }
                
                // Update the user role properties
                existingRole.RoleName = RoleName;
                existingRole.Description = Description;
                
                // Update the user role in the database
                _userRoleLogic.UpdateUserRole(existingRole);
                Console.WriteLine("User role updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user role: {ex.Message}");
            }
        }
        
        public static void DeleteUserRole(int RoleID)
        {
            Console.WriteLine($"Deleting user role with ID: {RoleID}");
            
            try
            {
                _userRoleLogic.DeleteUserRole(RoleID);
                Console.WriteLine("User role deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user role: {ex.Message}");
            }
        }

        private static void DisplayUserRoleDetails(UserRole role)
        {
            Console.WriteLine($"Role ID: {role.RoleID}");
            Console.WriteLine($"Role Name: {role.RoleName ?? "N/A"}");
            Console.WriteLine($"Description: {role.Description ?? "N/A"}");
            
            // Get users with this role
            List<User> users = _userLogic.GetUsersByRole(role.RoleID);
            if (users.Count > 0)
            {
                Console.WriteLine("Users with this role:");
                foreach (var user in users)
                {
                    Console.WriteLine($"  - {user.Name ?? "N/A"} (ID: {user.UserID})");
                }
            }
            
            Console.WriteLine("----------------------------");
        }

        private static void DisplayUserRole(UserRole? role)
        {
            if (role == null)
            {
                Console.WriteLine("No user role found with the specified ID.");
                return;
            }

            Console.WriteLine($"Role ID: {role.RoleID}");
            Console.WriteLine($"Role Name: {role.RoleName ?? "N/A"}");
            Console.WriteLine($"Description: {role.Description ?? "N/A"}");
            Console.WriteLine("----------------------------");
        }

        private static void ManageTestHistories()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Test Histories submenu
                Console.WriteLine("\n===== TEST HISTORIES MENU =====");
                Console.WriteLine("1. Create Test History");
                Console.WriteLine("2. Read Test History(ies)");
                Console.WriteLine("3. Update Test History");
                Console.WriteLine("4. Delete Test History");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine() ?? string.Empty;
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Test History ---");
                        Console.Write("Test Case ID: ");
                        if (int.TryParse(Console.ReadLine(), out int testCaseId))
                        {
                            Console.Write("User ID: ");
                            if (int.TryParse(Console.ReadLine(), out int userId))
                            {
                                Console.Write("Action: ");
                                string? action = Console.ReadLine() ?? string.Empty;
                                Console.Write("Details: ");
                                string? details = Console.ReadLine() ?? string.Empty;
                                
                                CreateTestHistory(testCaseId, userId, action, details);
                            }
                            else
                            {
                                Console.WriteLine("Invalid User ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Test Case ID.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Test History(ies) ---");
                        Console.WriteLine("1. Read all test histories");
                        Console.WriteLine("2. Read specific test history");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine() ?? string.Empty;
                        
                        if (readChoice == "1")
                        {
                            ReadTestHistory();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter History ID: ");
                            if (int.TryParse(Console.ReadLine(), out int historyId))
                            {
                                ReadTestHistory(historyId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid History ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Test History ---");
                        Console.Write("History ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateHistoryId))
                        {
                            Console.Write("Action: ");
                            string? action = Console.ReadLine() ?? string.Empty;
                            Console.Write("Details: ");
                            string? details = Console.ReadLine() ?? string.Empty;
                            
                            UpdateTestHistory(updateHistoryId, action, details);
                        }
                        else
                        {
                            Console.WriteLine("Invalid History ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Test History ---");
                        Console.Write("History ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteHistoryId))
                        {
                            DeleteTestHistory(deleteHistoryId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid History ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void CreateTestHistory(int TestCaseID, int UserID, string? Action, string? Details)
        {
            Console.WriteLine($"Creating test history for Test Case ID: {TestCaseID}");
            
            try
            {
                TestHistory newHistory = new TestHistory
                {
                    TestCaseID = TestCaseID,
                    UserID = UserID,
                    Action = Action,
                    Details = Details,
                    Timestamp = DateTime.Now
                };
                
                _testHistoryLogic.CreateTestHistory(newHistory);
                Console.WriteLine("Test history created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test history: {ex.Message}");
            }
        }
        
        public static void ReadTestHistory(int? HistoryID = null, int? TestCaseID = null, int? UserID = null)
        {
            try
            {
                if (HistoryID.HasValue)
                {
                    // Retrieve a specific test history
                    Console.WriteLine($"Reading test history with ID: {HistoryID}");
                    
                    TestHistory? history = _testHistoryLogic.GetTestHistory(HistoryID.Value);
                    
                    if (history != null)
                    {
                        DisplayTestHistoryDetails(history);
                    }
                    else
                    {
                        Console.WriteLine("No test history found with the specified ID.");
                    }
                }
                else if (TestCaseID.HasValue)
                {
                    // Retrieve all test histories for a test case
                    Console.WriteLine($"Reading all test histories for Test Case ID: {TestCaseID}");
                    
                    List<TestHistory> histories = _testHistoryLogic.GetTestHistoriesByTestCase(TestCaseID.Value);
                    
                    if (histories.Count > 0)
                    {
                        foreach (var history in histories)
                        {
                            DisplayTestHistoryDetails(history);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test histories found for the specified test case.");
                    }
                }
                else if (UserID.HasValue)
                {
                    // Retrieve all test histories for a user
                    Console.WriteLine($"Reading all test histories for User ID: {UserID}");
                    
                    List<TestHistory> histories = _testHistoryLogic.GetTestHistoriesByUser(UserID.Value);
                    
                    if (histories.Count > 0)
                    {
                        foreach (var history in histories)
                        {
                            DisplayTestHistoryDetails(history);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test histories found for the specified user.");
                    }
                }
                else
                {
                    // Retrieve all test histories
                    Console.WriteLine("Reading all test histories");
                    
                    List<TestHistory> histories = _testHistoryLogic.GetTestHistories();
                    
                    if (histories.Count > 0)
                    {
                        foreach (var history in histories)
                        {
                            DisplayTestHistoryDetails(history);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test histories found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading test history(ies): {ex.Message}");
            }
        }
        
        public static void UpdateTestHistory(int HistoryID, string? Action, string? Details)
        {
            Console.WriteLine($"Updating test history with ID: {HistoryID}");
            
            try
            {
                // Get the existing test history first
                TestHistory? existingHistory = _testHistoryLogic.GetTestHistory(HistoryID);
                
                if (existingHistory == null)
                {
                    Console.WriteLine("No test history found with the specified ID.");
                    return;
                }
                
                // Update the test history properties
                existingHistory.Action = Action;
                existingHistory.Details = Details;
                
                // Update the test history in the database
                _testHistoryLogic.UpdateTestHistory(existingHistory);
                Console.WriteLine("Test history updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating test history: {ex.Message}");
            }
        }
        
        public static void DeleteTestHistory(int HistoryID)
        {
            Console.WriteLine($"Deleting test history with ID: {HistoryID}");
            
            try
            {
                _testHistoryLogic.DeleteTestHistory(HistoryID);
                Console.WriteLine("Test history deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting test history: {ex.Message}");
            }
        }

        private static void DisplayTestHistoryDetails(TestHistory history)
        {
            Console.WriteLine($"History ID: {history.HistoryID}");
            Console.WriteLine($"Action: {history.Action ?? "N/A"}");
            Console.WriteLine($"Details: {history.Details ?? "N/A"}");
            Console.WriteLine($"Timestamp: {history.Timestamp}");
            
            // Get test case information
            TestCase? testCase = _testCaseLogic.GetTestCase(history.TestCaseID);
            if (testCase != null)
            {
                Console.WriteLine($"Test Case ID: {testCase.TestCaseID}");
                Console.WriteLine($"Test Case Description: {testCase.Description ?? "N/A"}");
            }
            
            // Get user information
            User? user = _userLogic.GetUser(history.UserID);
            if (user != null)
            {
                Console.WriteLine($"User ID: {user.UserID}");
                Console.WriteLine($"User Name: {user.Name ?? "N/A"}");
                Console.WriteLine($"User Email: {user.Email ?? "N/A"}");
            }
            
            Console.WriteLine("----------------------------");
        }

        public static void CreateTestReport(int TestCaseID, string? Result, DateTime ExecutionDate, string? Notes)
        {
            Console.WriteLine($"Creating test report for Test Case ID: {TestCaseID}");
            
            try
            {
                TestReport newReport = new TestReport
                {
                    TestCaseID = TestCaseID,
                    Result = Result,
                    ExecutionDate = ExecutionDate,
                    Notes = Notes
                };
                
                _testReportLogic.CreateTestReport(newReport);
                Console.WriteLine("Test report created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test report: {ex.Message}");
            }
        }
        
        public static void ReadTestReport(int? ReportID = null, int? TestCaseID = null)
        {
            try
            {
                if (ReportID.HasValue)
                {
                    // Retrieve a specific test report
                    Console.WriteLine($"Reading test report with ID: {ReportID}");
                    
                    TestReport? report = _testReportLogic.GetTestReport(ReportID.Value);
                    
                    if (report != null)
                    {
                        DisplayTestReportDetails(report);
                    }
                    else
                    {
                        Console.WriteLine("No test report found with the specified ID.");
                    }
                }
                else if (TestCaseID.HasValue)
                {
                    // Retrieve all test reports for a test case
                    Console.WriteLine($"Reading all test reports for Test Case ID: {TestCaseID}");
                    
                    List<TestReport> reports = _testReportLogic.GetTestReportsByTestCase(TestCaseID.Value);
                    
                    if (reports.Count > 0)
                    {
                        foreach (var report in reports)
                        {
                            DisplayTestReportDetails(report);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test reports found for the specified test case.");
                    }
                }
                else
                {
                    // Retrieve all test reports
                    Console.WriteLine("Reading all test reports");
                    
                    List<TestReport> reports = _testReportLogic.GetTestReports();
                    
                    if (reports.Count > 0)
                    {
                        foreach (var report in reports)
                        {
                            DisplayTestReportDetails(report);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test reports found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading test report(s): {ex.Message}");
            }
        }
        
        public static void UpdateTestReport(int ReportID, string? Result, DateTime ExecutionDate, string? Notes)
        {
            Console.WriteLine($"Updating test report with ID: {ReportID}");
            
            try
            {
                // Get the existing test report first
                TestReport? existingReport = _testReportLogic.GetTestReport(ReportID);
                
                if (existingReport == null)
                {
                    Console.WriteLine("No test report found with the specified ID.");
                    return;
                }
                
                // Update the test report properties
                existingReport.Result = Result;
                existingReport.ExecutionDate = ExecutionDate;
                existingReport.Notes = Notes;
                
                // Update the test report in the database
                _testReportLogic.UpdateTestReport(existingReport);
                Console.WriteLine("Test report updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating test report: {ex.Message}");
            }
        }
        
        public static void DeleteTestReport(int ReportID)
        {
            Console.WriteLine($"Deleting test report with ID: {ReportID}");
            
            try
            {
                _testReportLogic.DeleteTestReport(ReportID);
                Console.WriteLine("Test report deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting test report: {ex.Message}");
            }
        }

        private static void DisplayTestReportDetails(TestReport report)
        {
            Console.WriteLine($"Report ID: {report.ReportID}");
            Console.WriteLine($"Result: {report.Result ?? "N/A"}");
            Console.WriteLine($"Execution Date: {report.ExecutionDate}");
            Console.WriteLine($"Notes: {report.Notes ?? "N/A"}");
            Console.WriteLine("----------------------------");
        }

        private static void DisplayTestStatusDetails(TestStatus status)
        {
            Console.WriteLine($"Status ID: {status.StatusID}");
            Console.WriteLine($"Name: {status.Name ?? "N/A"}");
            Console.WriteLine($"Description: {status.Description ?? "N/A"}");
            
            // Get test cases with this status
            List<TestCase> testCases = _testCaseLogic.GetTestCasesByStatus(status.StatusID);
            if (testCases.Count > 0)
            {
                Console.WriteLine("Test cases with this status:");
                foreach (var testCase in testCases)
                {
                    Console.WriteLine($"  - Test Case ID: {testCase.TestCaseID}");
                    Console.WriteLine($"    Description: {testCase.Description ?? "N/A"}");
                }
            }
            
            Console.WriteLine("----------------------------");
        }

        private static void ManageTestStatuses()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Test Statuses submenu
                Console.WriteLine("\n===== TEST STATUSES MENU =====");
                Console.WriteLine("1. Create Test Status");
                Console.WriteLine("2. Read Test Status(es)");
                Console.WriteLine("3. Update Test Status");
                Console.WriteLine("4. Delete Test Status");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine() ?? string.Empty;
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Test Status ---");
                        Console.Write("Name: ");
                        string? statusName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Description: ");
                        string? statusDescription = Console.ReadLine() ?? string.Empty;
                        
                        CreateTestStatus(statusName, statusDescription);
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Test Status(es) ---");
                        Console.WriteLine("1. Read all test statuses");
                        Console.WriteLine("2. Read specific test status");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine() ?? string.Empty;
                        
                        if (readChoice == "1")
                        {
                            ReadTestStatus();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter Status ID: ");
                            if (int.TryParse(Console.ReadLine(), out int statusId))
                            {
                                ReadTestStatus(statusId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Status ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Test Status ---");
                        Console.Write("Status ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateStatusId))
                        {
                            Console.Write("New Name: ");
                            string? name = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Description: ");
                            string? description = Console.ReadLine() ?? string.Empty;
                            
                            UpdateTestStatus(updateStatusId, name, description);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Status ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Test Status ---");
                        Console.Write("Status ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteStatusId))
                        {
                            DeleteTestStatus(deleteStatusId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Status ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void CreateTestStatus(string? Name, string? Description)
        {
            Console.WriteLine($"Creating test status: {Name}");
            
            try
            {
                TestStatus newStatus = new TestStatus
                {
                    Name = Name,
                    Description = Description
                };
                
                _testStatusLogic.CreateTestStatus(newStatus);
                Console.WriteLine("Test status created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test status: {ex.Message}");
            }
        }
        
        public static void ReadTestStatus(int? StatusID = null)
        {
            try
            {
                if (StatusID.HasValue)
                {
                    // Retrieve a specific test status
                    Console.WriteLine($"Reading test status with ID: {StatusID}");
                    
                    TestStatus? status = _testStatusLogic.GetTestStatus(StatusID.Value);
                    
                    if (status != null)
                    {
                        DisplayTestStatusDetails(status);
                    }
                    else
                    {
                        Console.WriteLine("No test status found with the specified ID.");
                    }
                }
                else
                {
                    // Retrieve all test statuses
                    Console.WriteLine("Reading all test statuses");
                    
                    List<TestStatus> statuses = _testStatusLogic.GetTestStatuses();
                    
                    if (statuses.Count > 0)
                    {
                        foreach (var status in statuses)
                        {
                            DisplayTestStatusDetails(status);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No test statuses found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading test status(es): {ex.Message}");
            }
        }
        
        public static void UpdateTestStatus(int StatusID, string? Name, string? Description)
        {
            Console.WriteLine($"Updating test status with ID: {StatusID}");
            
            try
            {
                // Get the existing test status first
                TestStatus? existingStatus = _testStatusLogic.GetTestStatus(StatusID);
                
                if (existingStatus == null)
                {
                    Console.WriteLine("No test status found with the specified ID.");
                    return;
                }
                
                // Update the test status properties
                existingStatus.Name = Name;
                existingStatus.Description = Description;
                
                // Update the test status in the database
                _testStatusLogic.UpdateTestStatus(existingStatus);
                Console.WriteLine("Test status updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating test status: {ex.Message}");
            }
        }
        
        public static void DeleteTestStatus(int StatusID)
        {
            Console.WriteLine($"Deleting test status with ID: {StatusID}");
            
            try
            {
                _testStatusLogic.DeleteTestStatus(StatusID);
                Console.WriteLine("Test status deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting test status: {ex.Message}");
            }
        }

        private static void ManageTestReports()
        {
            bool returnToMainMenu = false;
            
            while (!returnToMainMenu)
            {
                // Display Test Reports submenu
                Console.WriteLine("\n===== TEST REPORTS MENU =====");
                Console.WriteLine("1. Create Test Report");
                Console.WriteLine("2. Read Test Report(s)");
                Console.WriteLine("3. Update Test Report");
                Console.WriteLine("4. Delete Test Report");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select an option: ");
                
                string? choice = Console.ReadLine() ?? string.Empty;
                
                switch (choice)
                {
                    case "1": // Create
                        Console.WriteLine("\n--- Create Test Report ---");
                        Console.Write("Test Case ID: ");
                        if (int.TryParse(Console.ReadLine(), out int reportTestCaseId))
                        {
                            Console.Write("Result: ");
                            string? result = Console.ReadLine() ?? string.Empty;
                            Console.Write("Execution Date (yyyy-MM-dd HH:mm:ss): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime executionDate))
                            {
                                Console.Write("Notes: ");
                                string? notes = Console.ReadLine() ?? string.Empty;
                                
                                CreateTestReport(reportTestCaseId, result, executionDate, notes);
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Test Case ID.");
                        }
                        break;
                    
                    case "2": // Read
                        Console.WriteLine("\n--- Read Test Report(s) ---");
                        Console.WriteLine("1. Read all test reports");
                        Console.WriteLine("2. Read reports for a test case");
                        Console.WriteLine("3. Read specific report");
                        Console.Write("Select an option: ");
                        string? readChoice = Console.ReadLine() ?? string.Empty;
                        
                        if (readChoice == "1")
                        {
                            ReadTestReport();
                        }
                        else if (readChoice == "2")
                        {
                            Console.Write("Enter Test Case ID: ");
                            if (int.TryParse(Console.ReadLine(), out int testCaseId))
                            {
                                ReadTestReport(null, testCaseId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Test Case ID.");
                            }
                        }
                        else if (readChoice == "3")
                        {
                            Console.Write("Enter Report ID: ");
                            if (int.TryParse(Console.ReadLine(), out int reportId))
                            {
                                ReadTestReport(reportId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Report ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                        }
                        break;
                    
                    case "3": // Update
                        Console.WriteLine("\n--- Update Test Report ---");
                        Console.Write("Report ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateReportId))
                        {
                            Console.Write("Result: ");
                            string? result = Console.ReadLine() ?? string.Empty;
                            Console.Write("Execution Date (yyyy-MM-dd HH:mm:ss): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime executionDate))
                            {
                                Console.Write("Notes: ");
                                string? notes = Console.ReadLine() ?? string.Empty;
                                
                                UpdateTestReport(updateReportId, result, executionDate, notes);
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Report ID.");
                        }
                        break;
                    
                    case "4": // Delete
                        Console.WriteLine("\n--- Delete Test Report ---");
                        Console.Write("Report ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteReportId))
                        {
                            DeleteTestReport(deleteReportId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Report ID.");
                        }
                        break;
                    
                    case "0": // Return
                        returnToMainMenu = true;
                        break;
                    
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }

    public class Project
    {
        public int ProjectID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Component
    {
        public int ComponentID { get; set; }
        public int ProjectID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class TestCase
    {
        public int TestCaseID { get; set; }
        public int ComponentID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public int AssignedUser { get; set; }
        public int StatusID { get; set; }
        public string? ComponentName { get; set; }
        public string? ComponentDescription { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? RoleID { get; set; }
    }

    public class UserRole
    {
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
    }

    public class TestReport
    {
        public int ReportID { get; set; }
        public int TestCaseID { get; set; }
        public string? Result { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string? Notes { get; set; }
    }

    public class TestStatus
    {
        public int StatusID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? StatusName { get; set; }
    }

    public class TestHistory
    {
        public int HistoryID { get; set; }
        public int TestCaseID { get; set; }
        public int UserID { get; set; }
        public string? Action { get; set; }
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
        public int ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? Notes { get; set; }
    }
} 