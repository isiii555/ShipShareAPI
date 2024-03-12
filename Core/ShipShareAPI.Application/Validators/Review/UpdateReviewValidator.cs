using FluentValidation;
using ShipShareAPI.Application.Dto.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Review
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
    {
        public UpdateReviewValidator() {
            RuleFor(r => r.Rating).NotEmpty().WithMessage("Rating field must be filled!");
        }
    }
}
