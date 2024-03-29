﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JobJetRestApi.Application.Contracts.V1.Filters;
using JobJetRestApi.Application.Contracts.V1.Responses;

namespace JobJetRestApi.Application.UseCases.JobOffers.Queries
{
    public interface IJobOfferQueries
    {
        Task<(IEnumerable<JobOfferResponse> JobOffers, int TotalCount)> GetAllJobOffersAsync(JobOffersFilter usersFilter);
        Task<JobOfferResponse> GetJobOfferByIdAsync(int id);
    }
}