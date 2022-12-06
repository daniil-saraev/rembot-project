using Rembot.Core.Models;

namespace Rembot.Core.Interfaces;

public interface IReferalService
{
    Task<uint> CountReferals(string phoneNumber);

    Task<IEnumerable<UserDto>> GetListOfReferals(string phoneNumber);
}