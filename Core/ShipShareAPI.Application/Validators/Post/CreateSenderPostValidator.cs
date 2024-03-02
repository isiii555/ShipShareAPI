using FluentValidation;
using ShipShareAPI.Application.Dto.Post.SenderPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Validators.Post
{
    public class CreateSenderPostValidator : AbstractValidator<CreateSenderPostRequest>
    {
        public CreateSenderPostValidator() {
            RuleFor(p => p.Title).NotNull().WithMessage("Title field must be filled!");
            RuleFor(p => p.ItemWeight).GreaterThan(0).NotEmpty().WithMessage("Item weight field must be filled!");
            RuleFor(p => p.ItemType).NotNull().WithMessage("Item type field must be filled!");
            RuleFor(p => p.Description).NotNull().WithMessage("Item description field must be filled!");
            RuleFor(p => p.StartDestination).NotNull().WithMessage("Start destination field must be filled!");
            RuleFor(p => p.EndDestination).NotNull().WithMessage("End destination field must be filled!");
            RuleFor(p => p.DeadlineDate).NotNull().WithMessage("End destination field must be filled!");
            RuleFor(p => p.ItemPhotos).NotNull().WithMessage("Item photos field must be filled!");
        }
    }
}
