using FluentValidation;
using ShipShareAPI.Application.Dto.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Review
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewValidator() {
            RuleFor(r => r.Rating).NotEmpty().WithMessage("Rating field must be filled!");
        }
    }
}
