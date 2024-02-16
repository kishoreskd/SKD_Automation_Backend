using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class PluginService : RepositoryService<Plugin>, IPluginService
    {
        private readonly AutomationDbService _context;

        public PluginService(AutomationDbService context) : base(context)
        {
            this._context = context;
        }

        public void Update(Plugin obj)
        {
            _context.Plugin.Update(obj);
        }


        public void DetachLocal(List<Plugin> col)
        {
        }

        //public IQueryable<Plugin> GetAllPluginWithChild()
        //{
        //    IQueryable<Plugin> plugin = _context.Plugin.Include(e => e.PluginLogCol).Include(e => e.Department);
        //    return plugin;
        //}
    }
}
