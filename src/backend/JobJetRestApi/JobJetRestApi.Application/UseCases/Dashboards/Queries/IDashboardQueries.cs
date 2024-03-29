﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JobJetRestApi.Application.Contracts.V1.Responses;

namespace JobJetRestApi.Application.UseCases.Dashboards.Queries;

public interface IDashboardQueries
{
    Task<IEnumerable<AverageSalaryForTechnologiesResponse>> GetAverageSalariesForTechnologies();
    Task<IEnumerable<AverageSalaryForCountriesResponse>> GetAverageSalariesForCountries();
    Task<IEnumerable<AverageSalaryForSeniorityLevelsResponse>> GetAverageSalariesForSeniorityLevels();
}