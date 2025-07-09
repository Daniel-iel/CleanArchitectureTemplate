using System.ComponentModel.DataAnnotations;

namespace RideSharingApp.Infrastructure.Database.Settings;

public class ConectionString
{
    public const string Key = "ConnectionStrings";

    [Required]
    public string RideConnection { get; set; }
}
