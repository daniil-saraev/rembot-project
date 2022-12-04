using Rembot.Core.Models;

namespace Rembot.Core.Interfaces;

public interface IReferalService
{
    Task<uint> CountReferals(string phoneNumber);

    Task AddReferal(string thisUserPhoneNumber, string linkOwnerPhoneNumber);
}