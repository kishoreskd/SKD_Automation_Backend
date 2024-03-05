using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using FluentValidation;

namespace AM.Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name required!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password required!");

            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role required!");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("A valid email is required");

        }
    }
}
