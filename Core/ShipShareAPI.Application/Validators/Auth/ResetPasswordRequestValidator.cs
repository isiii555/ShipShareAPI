using FluentValidation;
using ShipShareAPI.Application.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Auth
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator() { 
            RuleFor(r => r.Password).Equal(r => r.ConfirmPassword).WithMessage("Please check your password equality").NotNull().NotEmpty().WithMessage("Password field must be filled");
            RuleFor(r => r.ConfirmPassword).NotNull().NotEmpty().WithMessage("Password field must be filled");
        }
    }
}
