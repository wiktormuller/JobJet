﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using JobJetRestApi.Infrastructure.Services;

namespace JobJetRestApi.Web.Installers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI()
                .AddInMemoryStorage();
            
            services.AddHealthChecks()

            // Azure storage
            /*.AddCheck(
                "JobJetAzureStorageDB - Check",
                new AzureSqlConnectionHealthCheck(configuration.GetConnectionString("DefaultConnection")),
                HealthStatus.Unhealthy,
                new string[] {"jobjetazurestoragedb"})*/
            
            // Google Geocoding API
            .AddUrlGroup(
                new Uri("https://google.com"), // @TODO - Add custom logic to check health of geocoding
                "Google Geocoding API - Check",
                HealthStatus.Unhealthy,
                new string[] {"googlegeocodingapi"})
            
            // Project OSRM API
            .AddUrlGroup(
                new Uri("http://router.project-osrm.org/route/v1/driving/54.369201,18.483172;54.370176,18.460660?geometries=geojson&overview=full"),
                "Project OSRM API - Check",
                HealthStatus.Unhealthy,
                new string[] {"projectosrmapi"})
            
            // React client
            .AddUrlGroup(
                new Uri("https://google.com"), // @TODO - Publish frontend before implement the healthcheck
                "JobJet React Client - Check",
                HealthStatus.Unhealthy,
                new string[] {"jobjetreactclient"});
        }
    }
}