using System;

namespace SoftwareTestManager.Application.Models
{
    public class TestHistory
    {
        public int HistoryID { get; set; }
        public int TestCaseID { get; set; }
        public int UserID { get; set; }
        public string? Action { get; set; }
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 