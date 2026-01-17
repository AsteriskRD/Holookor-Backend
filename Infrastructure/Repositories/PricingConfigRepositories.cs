using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace HolookorBackend.Infrastructure.Repositories
{
    public class PricingConfigRepositories : BaseRespositories<PricingConfig>, IPriceingConfigRepo
    {
        private readonly HolookorSystem _context;
        public PricingConfigRepositories(HolookorSystem context) : base(context) 
        {
            _context = context;
        }

        public async Task<PricingConfig?> GetActiveAsync()
        {
            try
            {
                return await _context.PricingConfigs
                    .FirstOrDefaultAsync(p => p.IsActive);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving the active PricingConfig.", ex);
            }
        }
    }
}
