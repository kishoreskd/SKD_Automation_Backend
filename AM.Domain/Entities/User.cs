﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Entities
{
    public class User : AuditableEntity
    {
        public User()
        {
            this.UserModulePrivileges = new HashSet<UserModulePrivilege>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public int? EmployeeId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<UserModulePrivilege> UserModulePrivileges { get; set; }
    }
}
