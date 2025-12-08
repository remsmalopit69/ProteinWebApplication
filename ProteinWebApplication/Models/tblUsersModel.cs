using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProteinWebApplication.Models
{
    public class tblUsersModel
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int isArchive { get; set; }
        public string role { get; set; } // "admin" or "customer"

    }
}