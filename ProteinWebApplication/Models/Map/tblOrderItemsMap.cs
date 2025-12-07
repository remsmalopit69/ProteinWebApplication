using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models.Map
{
    public class tblOrderItemsMap : EntityTypeConfiguration<tblOrderItemsModel>
    {
        public tblOrderItemsMap()
        {
            HasKey(i => i.orderItemID);
            ToTable("tbl_order_items");
        }
    }
}