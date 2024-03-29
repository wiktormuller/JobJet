﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Application.UseCases.TechnologyType.Commands;
using JobJetRestApi.Domain.Repositories;
using MediatR;

namespace JobJetRestApi.Application.UseCases.TechnologyType.CommandsHandlers
{
    public class DeleteTechnologyTypeCommandHandler : IRequestHandler<DeleteTechnologyTypeCommand>
    {
        private readonly ITechnologyTypeRepository _technologyTypeRepository;
        
        public DeleteTechnologyTypeCommandHandler(ITechnologyTypeRepository technologyTypeRepository)
        {
            _technologyTypeRepository = Guard.Against.Null(technologyTypeRepository, nameof(technologyTypeRepository));
        }
        
        public async Task<Unit> Handle(DeleteTechnologyTypeCommand request, CancellationToken cancellationToken)
        {
            if (! await _technologyTypeRepository.ExistsAsync(request.Id))
            {
                throw TechnologyTypeNotFoundException.ForId(request.Id);
            }

            var technologyType = await _technologyTypeRepository.GetByIdAsync(request.Id);

            await _technologyTypeRepository.DeleteAsync(technologyType);
            
            return Unit.Value;
        }
    }
}