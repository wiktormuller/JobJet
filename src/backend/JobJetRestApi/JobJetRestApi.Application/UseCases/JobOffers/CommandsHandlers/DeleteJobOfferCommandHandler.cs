﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Application.Repositories;
using JobJetRestApi.Application.UseCases.JobOffers.Commands;
using MediatR;

namespace JobJetRestApi.Application.UseCases.JobOffers.CommandsHandlers
{
    public class DeleteJobOfferCommandHandler : IRequestHandler<DeleteJobOfferCommand>
    {
        private readonly IJobOfferRepository _jobOfferRepository;

        public DeleteJobOfferCommandHandler(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = Guard.Against.Null(jobOfferRepository, nameof(jobOfferRepository));
        }

        public async Task<Unit> Handle(DeleteJobOfferCommand request, CancellationToken cancellationToken)
        {
            if (! await _jobOfferRepository.ExistsAsync(request.Id))
            {
                throw JobOfferNotFoundException.ForId(request.Id);
            }

            var jobOffer = await _jobOfferRepository.GetByIdAsync(request.Id);
            await _jobOfferRepository.DeleteAsync(jobOffer);

            return Unit.Value;
        }
    }
}