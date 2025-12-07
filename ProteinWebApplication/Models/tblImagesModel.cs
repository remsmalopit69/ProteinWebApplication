using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblImagesModel
    {
        public int imageID { get; set; }
        public string imageName { get; set; }
        public string imagePath { get; set; }
        public string imageType { get; set; }
        public int? referenceID { get; set; }
        public int displayOrder { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int isArchive { get; set; }
    }
}