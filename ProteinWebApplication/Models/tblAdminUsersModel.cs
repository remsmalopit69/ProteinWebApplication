using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblAdminUsersModel
    {
        public int adminID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int isArchive { get; set; }
    }
}