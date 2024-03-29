﻿using FluentValidation;
using JobJetRestApi.Application.Contracts.V1.Requests;

namespace JobJetRestApi.Application.Validators.RequestsValidators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(request => request.UserName)
                .NotNull()
                .Length(1, 200);
        }
    }
}