﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using JobJetRestApi.Domain.Entities;
using JobJetRestApi.Domain.Repositories;
using JobJetRestApi.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace JobJetRestApi.Infrastructure.Repositories
{
    public class EmploymentTypeRepository : IEmploymentTypeRepository
    {
        private readonly JobJetDbContext _jobJetDbContext;

        public EmploymentTypeRepository(JobJetDbContext jobJetDbContext)
        {
            _jobJetDbContext = Guard.Against.Null(jobJetDbContext, nameof(jobJetDbContext));
        }

        public async Task<EmploymentType> GetByIdAsync(int id)
        {
            return await _jobJetDbContext.EmploymentTypes.FindAsync(id);
        }

        public async Task<List<EmploymentType>> GetAllAsync()
        {
            return await _jobJetDbContext.EmploymentTypes.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await GetByIdAsync(id) is not null;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            var employmentType = await _jobJetDbContext.EmploymentTypes
                .FirstOrDefaultAsync(x => x.Name == name);

            return employmentType is not null;
        }

        public async Task<int> CreateAsync(EmploymentType employmentType)
        {
            await _jobJetDbContext.AddAsync(employmentType);
            await _jobJetDbContext.SaveChangesAsync();

            return employmentType.Id;
        }

        public async Task DeleteAsync(EmploymentType employmentType)
        {
            _jobJetDbContext.EmploymentTypes.Remove(employmentType);
            await _jobJetDbContext.SaveChangesAsync();
        }
    }
}