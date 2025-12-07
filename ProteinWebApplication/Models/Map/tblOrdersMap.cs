using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblOrdersMap : EntityTypeConfiguration<tblOrdersModel>
    {
        public tblOrdersMap()
        {
            HasKey(i => i.orderID);
            ToTable("tbl_orders");
        }
    }

}