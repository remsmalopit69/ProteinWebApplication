using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblProductsModel
    {
        public int productID { get; set; }
        public int categoryID { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public decimal price { get; set; }
        public int stockQuantity { get; set; }
        public int displayOrder { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int isArchive { get; set; }
    }
}