using FluentValidation;
using ShipShareAPI.Application.Dto.Post.TravellerPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Post.TravellerPost
{
    public class CreateTravellerPostValidator : AbstractValidator<CreateTravellerPostRequest>
    {
        public CreateTravellerPostValidator()
        {
            RuleFor(p => p.Title).NotNull().WithMessage("Title field must be filled!");;
            RuleFor(p => p.Description).NotNull().WithMessage("Item description field must be filled!");
            RuleFor(p => p.StartDestination).NotNull().WithMessage("Start destination field must be filled!");
            RuleFor(p => p.EndDestination).NotNull().WithMessage("End destination field must be filled!");
            RuleFor(p => p.DeadlineDate).NotNull().WithMessage("End destination field must be filled!");
        }
    }
}
