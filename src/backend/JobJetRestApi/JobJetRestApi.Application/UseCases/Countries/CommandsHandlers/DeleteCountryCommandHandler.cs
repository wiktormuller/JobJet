﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Application.UseCases.Countries.Commands;
using JobJetRestApi.Domain.Repositories;
using MediatR;

namespace JobJetRestApi.Application.UseCases.Countries.CommandsHandlers
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand>
    {
        private readonly ICountryRepository _countryRepository;
        
        public DeleteCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = Guard.Against.Null(countryRepository, nameof(countryRepository));
        }

        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            if (!await _countryRepository.ExistsAsync(request.Id))
            {
                throw CountryNotFoundException.ForId(request.Id);
            }

            var country = await _countryRepository.GetByIdAsync(request.Id);
            await _countryRepository.DeleteAsync(country);
            
            return Unit.Value;
        }
    }
}