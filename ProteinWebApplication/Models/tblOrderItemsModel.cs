using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblOrderItemsModel
    {
        public int orderItemID { get; set; }
        public int orderID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subtotal { get; set; }
        public DateTime createdAt { get; set; }
        public int isArchive { get; set; }
    }
}