﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Application.UseCases.SeniorityLevel.Commands;
using MediatR;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Domain.Repositories;

namespace JobJetRestApi.Application.UseCases.SeniorityLevel.CommandsHandlers
{
    public class UpdateSeniorityLevelCommandHandler : IRequestHandler<UpdateSeniorityLevelCommand>
    {
        private readonly ISeniorityRepository _seniorityRepository;
        
        public UpdateSeniorityLevelCommandHandler(ISeniorityRepository seniorityRepository)
        {
            _seniorityRepository = Guard.Against.Null(seniorityRepository, nameof(seniorityRepository));
        }

        /// <exception cref="SeniorityLevelNotFoundException"></exception>
        /// <exception cref="SeniorityLevelAlreadyExistsException"></exception>
        public async Task<Unit> Handle(UpdateSeniorityLevelCommand request, CancellationToken cancellationToken)
        {
            if (! await _seniorityRepository.ExistsAsync(request.Id))
            {
                throw SeniorityLevelNotFoundException.ForId(request.Id);
            }

            if (await _seniorityRepository.ExistsAsync(request.Name))
            {
                throw SeniorityLevelAlreadyExistsException.ForName(request.Name);
            }

            var seniority = await _seniorityRepository.GetByIdAsync(request.Id);
            seniority.UpdateName(request.Name);

            await _seniorityRepository.UpdateAsync();
            
            return Unit.Value;
        }
    }
}