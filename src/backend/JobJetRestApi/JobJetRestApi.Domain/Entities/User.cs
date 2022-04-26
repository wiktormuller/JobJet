﻿using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using JobJetRestApi.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace JobJetRestApi.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public List<Company> Companies { get; private set; } = new List<Company>();

        private User() : base()
        {
        }

        public bool IsOwnerOfCompany(int companyId)
        {
            return Companies.Any(company => company.Id == companyId);
        }

        public void DeleteCompany(int companyId)
        {
            Guard.Against.Null(companyId, nameof(companyId));
            
            if (!IsOwnerOfCompany(companyId))
            {
                CannotDeleteCompanyInformationException.YouAreNotCompanyOwner(companyId);
            }

            Companies.RemoveAll(company => company.Id == companyId);
        }

        public void UpdateCompanyInformation(int companyId, string description, int numberOfPeople)
        {
            Guard.Against.Null(description, nameof(description));
            Guard.Against.NegativeOrZero(numberOfPeople, nameof(numberOfPeople));
            
            if (!IsOwnerOfCompany(companyId))
            {
                CannotUpdateCompanyInformationException.YouAreNotCompanyOwner(companyId);
            }

            Companies.First(company => company.Id == companyId).Update(description, numberOfPeople);
        }

        public void AddJobOffer(Company company, JobOffer jobOffer)
        {
            Guard.Against.Null(company, nameof(company));
            Guard.Against.Null(jobOffer, nameof(jobOffer));
            
            if (!IsOwnerOfCompany(company.Id))
            {
                throw CannotCreateJobOfferException.YouAreNotCompanyOwner(company.Id);
            }

            Companies
                .First(existingCompany => existingCompany.Id == company.Id)
                .AddJobOffer(jobOffer);
        }

        public void AddCompany(Company company)
        {
            Guard.Against.Null(company);
            
            if (Companies.Any(existingCompany => existingCompany.Name == company.Name))
            {
                throw UserCannotHaveCompaniesWithTheSameNamesException.ForName(company.Name);
            }

            Companies.Add(company);
        }
        
        public User(string email, string username)
        {
            Email = email;
            UserName = username;
        }
        
        public void UpdateName(string name)
        {
            UserName = name;
        }
    }
}