using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name required!");
        }
    }
}
