using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using JobJetRestApi.Application.Contracts.V1.Filters;
using JobJetRestApi.Application.Contracts.V1.Responses;
using JobJetRestApi.Application.Exceptions;
using JobJetRestApi.Application.UseCases.JobOffers.Queries;
using JobJetRestApi.Domain.Enums;
using JobJetRestApi.Infrastructure.Factories;

namespace JobJetRestApi.Infrastructure.Queries
{
    public class JobOfferQueries : IJobOfferQueries
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        
        private readonly record struct JobOfferRecord(int Id, string Name, string Description, decimal SalaryFrom, decimal SalaryTo,
            string WorkSpecification, DateTime CreatedAt, int AddressId, string Town, string Street, string ZipCode, string CountryName, decimal Latitude, decimal Longitude,
            int SeniorityId, string SeniorityName, int EmploymentTypeId, string EmploymentTypeName,
            int CompanyId, string CompanyName);

        private readonly record struct TechnologyTypeRecord(int TechnologyTypeId, string TechnologyTypeName);
        
        public JobOfferQueries(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = Guard.Against.Null(sqlConnectionFactory, nameof(sqlConnectionFactory));
        }
    
        public async Task<(IEnumerable<JobOfferResponse> JobOffers, int TotalCount)> GetAllJobOffersAsync(JobOffersFilter usersFilter)
        {
            using var connection = _sqlConnectionFactory.GetOpenConnection();

            var queryBuilder = new StringBuilder(@"
                SELECT 
                    [JobOffer].Id,
                    [JobOffer].Name,
                    [JobOffer].Description,
                    [JobOffer].SalaryFrom,
                    [JobOffer].SalaryTo,
                    [JobOffer].WorkSpecification,
                    [JobOffer].CreatedAt,
                    [Address].Id AS AddressId,
                    [Address].Town,
                    [Address].Street,
                    [Address].ZipCode,
                    [Country].Name AS CountryName,
                    [Address].Latitude,
                    [Address].Longitude,
                    [Seniority].Id AS SeniorityId,
                    [Seniority].Name AS SeniorityName,
                    [EmploymentType].Id AS EmploymentTypeId,
                    [EmploymentType].Name AS EmploymentTypeName,
                    [Company].Id AS CompanyId,
                    [Company].Name AS CompanyName,
                    [TechnologyType].Id AS TechnologyTypeId,
                    [TechnologyType].Name AS TechnologyTypeName
                FROM [JobOffers] AS [JobOffer] 
                LEFT JOIN Address AS [Address]
                    ON [JobOffer].AddressId = [Address].Id
                INNER JOIN Countries AS [Country]
                    ON [Address].CountryId = [Country].Id
                LEFT JOIN JobOfferTechnologyType AS [JobOfferTechnologyType]
                    ON [JobOffer].Id = [JobOfferTechnologyType].JobOffersId
                LEFT JOIN TechnologyTypes AS [TechnologyType]
                    ON [JobOfferTechnologyType].TechnologyTypesId = [TechnologyType].Id
                LEFT JOIN SeniorityLevels AS [Seniority]
                    ON [JobOffer].SeniorityId = [Seniority].Id
                LEFT JOIN EmploymentTypes AS [EmploymentType]
                    ON [JobOffer].EmploymentTypeId = [EmploymentType].Id
                LEFT JOIN Companies AS [Company]
                    ON [JobOffer].CompanyId = [Company].Id
                    
                --@WHERE
                
                --@ORDERBY
                --@OFFSET
                --@FETCH;"
                );
            
            
            var parameters = BuildConditionsAndGetDynamicParameters(queryBuilder, usersFilter);

            var jobOfferMap = new Dictionary<int, JobOfferDto>();
            
            await connection.QueryAsync<JobOfferRecord, TechnologyTypeRecord, bool>(
                queryBuilder.ToString(), 
                param: parameters, 
                splitOn: "TechnologyTypeId",
                map: (jobOfferRecord, technologyTypeRecord) =>
                {
                    JobOfferDto jobOfferDto;
                    
                    if (!jobOfferMap.TryGetValue(jobOfferRecord.Id, out jobOfferDto))
                    {
                        jobOfferDto = new JobOfferDto
                        {
                            Id = jobOfferRecord.Id,
                            AddressId = jobOfferRecord.AddressId,
                            CompanyId = jobOfferRecord.CompanyId,
                            CompanyName = jobOfferRecord.CompanyName,
                            CountryName = jobOfferRecord.CountryName,
                            Description = jobOfferRecord.Description,
                            EmploymentTypeId = jobOfferRecord.EmploymentTypeId,
                            EmploymentTypeName = jobOfferRecord.EmploymentTypeName,
                            Latitude = jobOfferRecord.Latitude,
                            Longitude = jobOfferRecord.Longitude,
                            Name = jobOfferRecord.Name,
                            SalaryFrom = jobOfferRecord.SalaryFrom,
                            SalaryTo = jobOfferRecord.SalaryTo,
                            SeniorityId = jobOfferRecord.SeniorityId,
                            SeniorityName = jobOfferRecord.SeniorityName,
                            WorkSpecification = jobOfferRecord.WorkSpecification,
                            Createdt = jobOfferRecord.CreatedAt,
                            Town = jobOfferRecord.Town,
                            Street = jobOfferRecord.Street,
                            ZipCode = jobOfferRecord.ZipCode
                        };
                        jobOfferMap.Add(jobOfferRecord.Id, jobOfferDto);
                    }
                    
                    jobOfferDto.TechnologyTypes.Add(new TechnologyTypeDto
                    {
                        TechnologyTypeId = technologyTypeRecord.TechnologyTypeId,
                        TechnologyTypeName = technologyTypeRecord.TechnologyTypeName
                    });
                    
                    return true;
                });

            var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM [JobOffers];"); // @TODO - Implement real total count

            var jobOffers = jobOfferMap.Values.Select(x => new JobOfferResponse(
                x.Id,
                x.Name,
                x.Description,
                x.SalaryFrom,
                x.SalaryTo,
                x.CountryName,
                x.Town,
                x.Street,
                x.ZipCode,
                x.Latitude,
                x.Longitude,
                x.TechnologyTypes
                    .Select(technologyType => technologyType.TechnologyTypeName)
                    .ToList(),
                x.SeniorityName,
                x.EmploymentTypeName,
                Enum.Parse<WorkSpecification>(x.WorkSpecification),
                x.Createdt,
                x.CompanyId,
                x.CompanyName
                ));
            
            return (jobOffers, totalCount);
        }

        /// <exception cref="JobOfferNotFoundException"></exception>
        public async Task<JobOfferResponse> GetJobOfferByIdAsync(int id)
        {
            using var connection = _sqlConnectionFactory.GetOpenConnection();

            const string query = @"
                SELECT 
                    [JobOffer].Id,
                    [JobOffer].Name,
                    [JobOffer].Description,
                    [JobOffer].SalaryFrom,
                    [JobOffer].SalaryTo,
                    [JobOffer].WorkSpecification,
                    [JobOffer].CreatedAt,
                    [Address].Id AS AddressId,
                    [Address].Town,
                    [Address].Street,
                    [Address].ZipCode,
                    [Country].Name AS CountryName,
                    [Address].Latitude,
                    [Address].Longitude,
                    [TechnologyType].Id AS TechnologyTypeId,
                    [TechnologyType].Name AS TechnologyTypeName,
                    [Seniority].Id AS SeniorityId,
                    [Seniority].Name AS SeniorityName,
                    [EmploymentType].Id AS EmploymentTypeId,
                    [EmploymentType].Name AS EmploymentTypeName,
                    [Company].Id AS CompanyId,
                    [Company].Name AS CompanyName
                FROM [JobOffers] AS [JobOffer] 
                LEFT JOIN Address AS [Address]
                    ON [JobOffer].AddressId = [Address].Id
                INNER JOIN Countries AS [Country]
                    ON [Address].CountryId = [Country].Id
                LEFT JOIN JobOfferTechnologyType AS [JobOfferTechnologyType]
                    ON [JobOffer].Id = [JobOfferTechnologyType].JobOffersId
                LEFT JOIN TechnologyTypes AS [TechnologyType]
                    ON [JobOfferTechnologyType].TechnologyTypesId = [TechnologyType].Id
                LEFT JOIN SeniorityLevels AS [Seniority]
                    ON [JobOffer].SeniorityId = [Seniority].Id
                LEFT JOIN EmploymentTypes AS [EmploymentType]
                    ON [JobOffer].EmploymentTypeId = [EmploymentType].Id
                LEFT JOIN Companies AS [Company]
                    ON [JobOffer].CompanyId = [Company].Id
                    
                --@WHERE
                
                --@ORDERBY
                --@OFFSET
                --@FETCH;"
                ;
            
            var jobOfferMap = new Dictionary<int, JobOfferDto>();
            
            await connection.QueryAsync<JobOfferRecord, TechnologyTypeRecord, bool>(
                query, 
                param: new { Id = id }, 
                splitOn: "TechnologyTypeId",
                map: (jobOfferRecord, technologyTypeRecord) =>
                {
                    JobOfferDto jobOfferDto;
                    
                    if (!jobOfferMap.TryGetValue(jobOfferRecord.Id, out jobOfferDto))
                    {
                        jobOfferDto = new JobOfferDto
                        {
                            Id = jobOfferRecord.Id,
                            AddressId = jobOfferRecord.AddressId,
                            CompanyId = jobOfferRecord.CompanyId,
                            CompanyName = jobOfferRecord.CompanyName,
                            CountryName = jobOfferRecord.CountryName,
                            Description = jobOfferRecord.Description,
                            EmploymentTypeId = jobOfferRecord.EmploymentTypeId,
                            EmploymentTypeName = jobOfferRecord.EmploymentTypeName,
                            Latitude = jobOfferRecord.Latitude,
                            Longitude = jobOfferRecord.Longitude,
                            Name = jobOfferRecord.Name,
                            SalaryFrom = jobOfferRecord.SalaryFrom,
                            SalaryTo = jobOfferRecord.SalaryTo,
                            SeniorityId = jobOfferRecord.SeniorityId,
                            SeniorityName = jobOfferRecord.SeniorityName,
                            WorkSpecification = jobOfferRecord.WorkSpecification,
                            Createdt = jobOfferRecord.CreatedAt,
                            Town = jobOfferRecord.Town,
                            Street = jobOfferRecord.Street,
                            ZipCode = jobOfferRecord.ZipCode
                        };
                        jobOfferMap.Add(jobOfferRecord.Id, jobOfferDto);
                    }
                    
                    jobOfferDto.TechnologyTypes.Add(new TechnologyTypeDto
                    {
                        TechnologyTypeId = technologyTypeRecord.TechnologyTypeId,
                        TechnologyTypeName = technologyTypeRecord.TechnologyTypeName
                    });
                    
                    return true;
                });

            var queriedJobOffer = jobOfferMap.Values.FirstOrDefault();

            if (queriedJobOffer is null || queriedJobOffer.Id == 0)
            {
                throw JobOfferNotFoundException.ForId(id);
            }

            var jobOffer = new JobOfferResponse(
                queriedJobOffer.Id,
                queriedJobOffer.Name,
                queriedJobOffer.Description,
                queriedJobOffer.SalaryFrom,
                queriedJobOffer.SalaryTo,
                queriedJobOffer.CountryName,
                queriedJobOffer.Town,
                queriedJobOffer.Street,
                queriedJobOffer.ZipCode,
                queriedJobOffer.Latitude,
                queriedJobOffer.Longitude,
                queriedJobOffer.TechnologyTypes
                    .Select(technologyType => technologyType.TechnologyTypeName)
                    .ToList(),
                queriedJobOffer.SeniorityName,
                queriedJobOffer.EmploymentTypeName,
                Enum.Parse<WorkSpecification>(queriedJobOffer.WorkSpecification),
                queriedJobOffer.Createdt,
                queriedJobOffer.CompanyId,
                queriedJobOffer.CompanyName
                );

            return jobOffer;
        }

        public DynamicParameters BuildConditionsAndGetDynamicParameters(StringBuilder queryBuilder, JobOffersFilter usersFilter)
        {
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (usersFilter.CountryId.HasValue)
            {
                conditions.Add("[Address].CountryId = @CountryId");
                parameters.Add("CountryId", usersFilter.CountryId.Value);
            }

            if (usersFilter.TechnologyIds is not null && usersFilter.TechnologyIds.Any())
            {
                conditions.Add(@"
                    WHERE JobOfferTechnologyType.JobOffersId IN (SELECT JobOffer.Id 
                        FROM [JobOfferTechnologyType] 
                            JOIN [JobOffers] AS JobOffer
                                ON [JobOfferTechnologyType].JobOffersId = [JobOffer].Id
                        WHERE [JobOfferTechnologyType].TechnologyTypesId  IN @TechnologyTypeIds"); 
                parameters.Add("TechnologyTypeIds", usersFilter.TechnologyIds);
            }

            if (usersFilter.EmploymentTypeId.HasValue)
            {
                conditions.Add("[EmploymentType].Id = @EmploymentTypeId");
                parameters.Add("EmploymentTypeId", usersFilter.EmploymentTypeId.Value);
            }

            if (usersFilter.SeniorityLevelId.HasValue)
            {
                conditions.Add("[Seniority].Id = @SeniorityLevelId");
                parameters.Add("SeniorityLevelId", usersFilter.SeniorityLevelId.Value);
            }

            if (! string.IsNullOrEmpty(usersFilter.WorkSpecification))
            {
                conditions.Add("[JobOffer].WorkSpecification = @WorkSpecification");
                parameters.Add("WorkSpecification", usersFilter.WorkSpecification);
            }

            if (usersFilter.SalaryFrom.HasValue)
            {
                conditions.Add("[JobOffer].SalaryFrom >= @SalaryFrom");
                parameters.Add("SalaryFrom", usersFilter.SalaryFrom.Value);
            }

            if (usersFilter.SalaryTo.HasValue)
            {
                conditions.Add("[JobOffer].SalaryTo >= @SalaryTo");
                parameters.Add("SalaryTo", usersFilter.SalaryTo.Value);
            }

            if (!string.IsNullOrEmpty(usersFilter.GeneralSearchByText))
            {
                conditions.Add(@"[JobOffer].Description LIKE @GeneralSearchByText
                    OR [JobOffer].Name LIKE @GeneralSearchByText");
                parameters.Add("GeneralSearchByText", $"%{usersFilter.GeneralSearchByText}%");
            }

            queryBuilder.Replace("--@WHERE", conditions.Any()
                ? $"WHERE {string.Join(" AND ", conditions)}" 
                : string.Empty);

            queryBuilder.Replace("--@ORDERBY", $"ORDER BY [JobOffer].Id"); // @TODO - Implement better sorting
            queryBuilder.Replace("--@OFFSET", $"OFFSET {usersFilter.PageNumber} ROWS");
            queryBuilder.Replace("--@FETCH", $"FETCH NEXT {usersFilter.GetNormalizedPageSize()} ROWS ONLY");

            return parameters;
        }
    }
    
    public class JobOfferDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal SalaryFrom { get; set; }
        public decimal SalaryTo { get; set; }
        public string WorkSpecification { get; set; }
        public DateTime Createdt { get; set; }
        public int AddressId { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int TechnologyTypeId { get; set; }
        public string TechnologyTypeName { get; set; }
        public int SeniorityId { get; set; }
        public string SeniorityName { get; set; }
        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<TechnologyTypeDto> TechnologyTypes { get; set; } = new List<TechnologyTypeDto>();
    }

    public class TechnologyTypeDto
    {
        public int TechnologyTypeId { get; set; }
        public string TechnologyTypeName { get; set; }
    }
}