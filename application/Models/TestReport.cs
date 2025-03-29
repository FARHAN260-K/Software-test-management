using System;

namespace SoftwareTestManager.Application.Models
{
    public class TestReport
    {
        public int ReportID { get; set; }
        public int TestCaseID { get; set; }
        public string? Result { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string? Notes { get; set; }
    }
} 