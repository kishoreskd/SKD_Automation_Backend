using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class LogApiDto
    {
        public int PluginId { get; set; }
        public string JobName { get; set; }
        public string Activity { get; set; }
        public string PluginKey { get; set; }
    }
}
