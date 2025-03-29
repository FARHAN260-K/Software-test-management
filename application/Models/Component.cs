using System;

namespace SoftwareTestManager.Application.Models
{
    public class Component
    {
        public int ComponentID { get; set; }
        public int ProjectID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
} 