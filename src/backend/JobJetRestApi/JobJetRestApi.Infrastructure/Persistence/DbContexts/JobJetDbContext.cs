﻿using JobJetRestApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobJetRestApi.Infrastructure.Persistence.DbContexts
{
    public class JobJetDbContext : DbContext
    {
        public JobJetDbContext()
        {
            
        }
        
        public JobJetDbContext(DbContextOptions<JobJetDbContext> options) : base(options)
        {
        }

        /*
        // The OnConfiguring() method allows us to select and configure the data source to be used with a context using DbContextOptionsBuilder.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        */

        // The OnModelCreating() method allows us to configure the model using ModelBuilder Fluent API.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobOffer>().Property(j => j.SalaryFrom).HasPrecision(10, 2);
            modelBuilder.Entity<JobOffer>().Property(j => j.SalaryTo).HasPrecision(10, 2);

            modelBuilder.Entity<Address>().Property(a => a.Latitude).HasPrecision(9, 6);
            modelBuilder.Entity<Address>().Property(a => a.Longitude).HasPrecision(9,6);
        }

        // Entities
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<TechnologyType> TechnologyTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; } // Does it make sense?
        public DbSet<Seniority> SeniorityLevels { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}