namespace Rembot.Core.Interfaces;

public interface ICashbackService
{
    Task<decimal> CalculateCashback();
}