﻿using System.Collections.Generic;

namespace JobJetRestApi.Domain.Entities
{
    public class TechnologyType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        
        // Relationships
        public ICollection<JobOffer> JobOffers { get; set; }

        private TechnologyType() {} // For EF purposes
        
        public TechnologyType(string name)
        {
            Name = name;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}