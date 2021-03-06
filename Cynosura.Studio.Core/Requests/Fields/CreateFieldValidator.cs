﻿using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class CreateFieldValidator : AbstractValidator<CreateField>
    {
        public CreateFieldValidator()
        {
            RuleFor(x => x.Type).NotEmpty().When(x => x.EntityId == null && x.EnumId == null);
            RuleFor(x => x.Type).Empty().When(x => x.EntityId != null || x.EnumId != null);
            RuleFor(x => x.EntityId).NotEmpty().When(x => x.Type == null && x.EnumId == null);
            RuleFor(x => x.EntityId).Empty().When(x => x.Type != null || x.EnumId != null);
            RuleFor(x => x.EnumId).NotEmpty().When(x => x.Type == null && x.EntityId == null);
            RuleFor(x => x.EnumId).Empty().When(x => x.Type != null || x.EntityId != null);
        }

    }
}
