using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblCategoriesMap : EntityTypeConfiguration<tblCategoriesModel>
    {
        public tblCategoriesMap()
        {
            HasKey(i => i.categoryID);
            ToTable("tbl_categories");
        }
    }
}