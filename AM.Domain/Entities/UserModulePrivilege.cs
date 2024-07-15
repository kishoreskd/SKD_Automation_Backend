﻿namespace AM.Domain.Entities
{
    public class UserModulePrivilege : AuditableEntity
    {
        public int UserModulePrivilegeId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
    }
}