﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JobJetRestApi.Application.Contracts.V1.Responses;
using JobJetRestApi.Application.UseCases.Dashboards.Queries;
using JobJetRestApi.Web.Contracts.V1.ApiRoutes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobJetRestApi.Web.Controllers.V1;

[ApiController]
public class DashboardsController : ControllerBase
{
    private readonly IDashboardQueries _dashboardQueries;
    
    public DashboardsController(IDashboardQueries dashboardQueries)
    {
        _dashboardQueries = dashboardQueries;
    }
    
    [Route(ApiRoutes.Dashboards.GetAverageSalaryForTechnologies)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AverageSalaryForTechnologiesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AverageSalaryForTechnologiesResponse>>> GetAverageSalariesForTechnologies()
    {
        var averageSalaries = await _dashboardQueries.GetAverageSalariesForTechnologies();

        return Ok(averageSalaries);
    }

    [Route(ApiRoutes.Dashboards.GetAverageSalaryForCountries)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AverageSalaryForCountriesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AverageSalaryForCountriesResponse>>> GetAverageSalariesForCountries()
    {
        var averageSalaries = await _dashboardQueries.GetAverageSalariesForCountries();

        return Ok(averageSalaries);
    }

    [Route(ApiRoutes.Dashboards.GetAverageSalaryForSeniorityLevels)]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AverageSalaryForSeniorityLevelsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AverageSalaryForSeniorityLevelsResponse>>> GetAverageSalariesForSeniorityLevels()
    {
        var averageSalaries = await _dashboardQueries.GetAverageSalariesForSeniorityLevels();

        return Ok(averageSalaries);
    }
}