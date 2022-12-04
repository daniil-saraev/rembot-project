namespace Rembot.Core.Interfaces;

public interface IDiscountService
{
    Task<decimal> CalculateDiscount(string phoneNumber);
}