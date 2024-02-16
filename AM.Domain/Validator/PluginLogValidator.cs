using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class PluginLogValidator : AbstractValidator<PluginLog>
    {
        public PluginLogValidator()
        {
            RuleFor(x => x.JobName).NotEmpty().WithMessage("Job name required!");

            RuleFor(x => x.Activity).NotEmpty().WithMessage("Activity required!");

            RuleFor(x => x.PluginId).GreaterThan(0).WithMessage("Plugin id required!");
        }
    }
}
