using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee Id required!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password required!");

            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role required!");
        }
    }
}
