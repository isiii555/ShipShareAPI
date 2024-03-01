using FluentValidation;
using ShipShareAPI.Application.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Auth
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator() {
            RuleFor(s => s.Email).EmailAddress().WithMessage("Email must be entered here.").NotNull().NotEmpty().WithMessage("Email field must be filled.");
            RuleFor(s => s.Password).NotNull().NotEmpty().WithMessage("Email field must be filled.");
        }
    }
}
