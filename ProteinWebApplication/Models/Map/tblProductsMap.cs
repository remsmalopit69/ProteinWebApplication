using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblProductsMap : EntityTypeConfiguration<tblProductsModel>
    {
        public tblProductsMap()
        {
            HasKey(i => i.productID);
            ToTable("tbl_products");
        }
    }
}