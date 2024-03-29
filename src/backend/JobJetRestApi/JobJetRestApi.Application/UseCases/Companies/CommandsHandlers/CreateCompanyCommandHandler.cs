﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Application.UseCases.Companies.Commands;
using JobJetRestApi.Domain.Entities;
using JobJetRestApi.Domain.Repositories;
using MediatR;

namespace JobJetRestApi.Application.UseCases.Companies.CommandsHandlers
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository, 
            IUserRepository userRepository)
        {
            _userRepository = Guard.Against.Null(userRepository, nameof(userRepository));
            _companyRepository = Guard.Against.Null(companyRepository, nameof(companyRepository));
        }
        
        /// <exception cref="CompanyAlreadyExistsException"></exception>
        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            if (await _companyRepository.ExistsAsync(request.Name))
            {
                throw CompanyAlreadyExistsException.ForName(request.Name);
            }

            var company = new Company(request.Name, request.ShortName, request.Description, request.NumberOfPeople, request.CityName);
            
            var user = await _userRepository.GetByIdAsync(request.UserId);
            user.AddCompany(company);

            await _userRepository.UpdateAsync(user);

            return company.Id;
        }
    }
}