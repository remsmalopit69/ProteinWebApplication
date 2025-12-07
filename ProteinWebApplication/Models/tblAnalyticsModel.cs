using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblAnalyticsModel
    {
        public int analyticsID { get; set; }
        public string metricType { get; set; }
        public decimal metricValue { get; set; }
        public DateTime recordDate { get; set; }
        public DateTime createdAt { get; set; }
        public int isArchive { get; set; }
    }
}