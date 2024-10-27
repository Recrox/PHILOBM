using PHILOBM.Models.Base;

namespace PHILOBM.Models;

public class Car : BaseEntity
{
    public string? LicensePlate { get; set; } // Numéro de plaque
    public string? ChassisNumber { get; set; } // Numéro de châssis
    public int Mileage { get; set; } // Kilométrage
    public int ClientId { get; set; } // Foreign key to Client
    public Client? Owner { get; set; } // Propriétaire
}
