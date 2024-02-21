using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Entities
{
    public class Dashbord
    {
        public int totalPlugins { get; set; }
        public double totalManualMiniutes { get; set; }
        public double totalAutomatedMinutes { get; set; }
    }
}
