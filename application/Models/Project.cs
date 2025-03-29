using System;

namespace SoftwareTestManager.Application.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 