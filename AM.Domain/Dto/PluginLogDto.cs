using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class PluginLogDto
    {
        public int PluginLogId { get; set; }
        public int PluginId { get; set; }

        public string JobName { get; set; }
        public string Activity { get; set; }

        public int CreatedEmployeeId { get; set; }
        public int? LastModifiedEmployeeId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
