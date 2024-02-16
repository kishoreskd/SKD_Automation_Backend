using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class PluginValidator : AbstractValidator<Plugin>
    {
        public PluginValidator()
        {
            RuleFor(x => x.PluginName).NotEmpty().WithMessage("Plugin name name required!");

            RuleFor(x => x.ManualMinutes).NotEmpty().WithMessage("Manual minutes name required!");

            RuleFor(x => x.AutomatedMinutes).NotEmpty().WithMessage("Automated minutes name required!");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description required!");
        }
    }
}
