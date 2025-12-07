using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblAdminUsersMap : EntityTypeConfiguration<tblAdminUsersModel>
    {
        public tblAdminUsersMap()
        {
            HasKey(i => i.adminID);
            ToTable("tbl_admin_users");
        }
    }
}