using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Interfaces.Repositories
{
    public interface IPriceingConfigRepo : IBaseRepositoriesResponse<PricingConfig>
    {
        Task<PricingConfig?> GetActiveAsync();
    }
}
