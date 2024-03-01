using FluentValidation;
using ShipShareAPI.Application.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Auth
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator() {
            RuleFor(s => s.Email).EmailAddress().WithMessage("Email must be entered here.").NotNull().NotEmpty().WithMessage("Email field must be filled.");
            RuleFor(s => s.Password).NotNull().NotEmpty().WithMessage("Password field must be filled.");
            RuleFor(s => s.UserName).MinimumLength(5).NotNull().NotEmpty().WithMessage("Email field must be filled.");
        }
    }
}
