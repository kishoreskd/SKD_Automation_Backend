using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;

namespace AM.Domain.Entities
{
    public class Plugin : AuditableEntity
    {

        public Plugin()
        {
            this.PluginLogCol = new HashSet<PluginLog>();
        }

        public int PluginId { get; set; }
        public string PluginName { get; set; }
        public double ManualMinutes { get; set; }
        public double AutomatedMinutes { get; set; }
        public string Description { get; set; }
        public string PluginKey { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<PluginLog> PluginLogCol { get; set; }
    }
}
