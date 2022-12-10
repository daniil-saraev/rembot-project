using System.ComponentModel.DataAnnotations;

namespace Rembot.Core.Entities;

public class Referal
{
    [Key]
    public string GuestPhoneNumber { get; private set; }
    public string HostPhoneNumber { get; private set; }

    public Referal(string guestPhoneNumber, string hostPhoneNumber)
    {
        GuestPhoneNumber = guestPhoneNumber;
        HostPhoneNumber = hostPhoneNumber;
    } 
}