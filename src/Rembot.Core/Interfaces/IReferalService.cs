using Rembot.Core.Entities;

namespace Rembot.Core.Interfaces;

public interface IReferalService
{
    Task<string> GenerateLink();

    Task<uint> CountReferals();

    Task AddReferal(User user);
}