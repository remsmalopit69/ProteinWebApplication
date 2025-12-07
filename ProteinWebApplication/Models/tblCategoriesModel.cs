using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblCategoriesModel
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string categoryDescription { get; set; }
        public int displayOrder { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int isArchive { get; set; }
    }
}