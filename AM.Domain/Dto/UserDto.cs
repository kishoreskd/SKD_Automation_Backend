﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class UserDto : AuditableEntity
    {
        public int Id { get; set; }
        public int UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int? EmployeeId { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
