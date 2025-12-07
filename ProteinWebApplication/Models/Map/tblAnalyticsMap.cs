using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblAnalyticsMap : EntityTypeConfiguration<tblAnalyticsModel>
    {
        public tblAnalyticsMap()
        {
            HasKey(i => i.analyticsID);
            ToTable("tbl_analytics");
        }
    }
}