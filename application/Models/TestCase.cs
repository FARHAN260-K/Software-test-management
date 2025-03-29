using System;

namespace SoftwareTestManager.Application.Models
{
    public class TestCase
    {
        public int TestCaseID { get; set; }
        public int ComponentID { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public int AssignedUser { get; set; }
        public int StatusID { get; set; }
    }
} 