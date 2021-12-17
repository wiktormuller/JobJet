﻿using MediatR;

namespace JobJetRestApi.Application.UseCases.JobOffers.Commands
{
    public class UpdateJobOfferCommand : IRequest
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal SalaryFrom { get; }
        public decimal SalaryTo { get; }

        public UpdateJobOfferCommand(
            int id,
            string name, 
            string description, 
            decimal salaryFrom, 
            decimal salaryTo)
        {
            Id = id;
            Name = name;
            Description = description;
            SalaryFrom = salaryFrom;
            SalaryTo = salaryTo;
        }
    }
}