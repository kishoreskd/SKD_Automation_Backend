using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("Department name required!");
        }
    }
}
