using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class PluginLogService : RepositoryService<PluginLog>, IPluginLogService
    {
        private readonly AutomationDbService _service;

        public PluginLogService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(PluginLog obj)
        {
            _service.PluginLog.Update(obj);
        }

       

    }
}
