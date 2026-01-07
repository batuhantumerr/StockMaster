using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Core.Entities
{
    public class AppUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Gerçek hayatta Hashlenmeli ama şimdilik düz tutalım
        public string Role { get; set; } // "Admin", "User" vs.
    }
}
