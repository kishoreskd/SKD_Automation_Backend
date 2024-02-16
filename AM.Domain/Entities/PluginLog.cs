

namespace AM.Domain.Entities
{
    public class PluginLog : AuditableEntity
    {
        public int PluginLogId { get; set; }
        public string JobName { get; set; }
        public string Activity { get; set; }

        public int PluginId { get; set; }
        public Plugin Plugin { get; set; }
    }
}
