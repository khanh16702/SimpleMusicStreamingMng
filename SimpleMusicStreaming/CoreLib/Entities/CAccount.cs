using CoreLib.DataTableToObject.Attributes;
using CoreLib.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CAccount : CBaseEntity
    {
        [DataNames("Username")]
        public string Username { get; set; }

        [DataNames("Password")]
        public string Password { get; set; }

        [DataNames("RetypedPassword")]
        public string RetypedPassword { get; set; }

        [DataNames("DisplayName")]
        public string DisplayName { get; set; }

        [DataNames("Email")]
        public string Email { get; set; }

        [DataNames("CountryId")]
        public int CountryId { get; set; }

        [DataNames("RoleId")]
        public int RoleId { get; set; }

        [DataNames("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DataNames("IsActive")]
        public bool IsActive { get; set; }
    }
}
