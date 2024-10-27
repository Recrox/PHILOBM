using PHILOBM.Models.Base;

namespace PHILOBM.Models;

public class Car : BaseEntity
{
    public string? LicensePlate { get; set; } // License plate number
    public string? ChassisNumber { get; set; } // Chassis number
    public int Mileage { get; set; } // Mileage
    public int ClientId { get; set; } // Foreign key to Client
    public Client? Owner { get; set; } // Owner

    public string? Brand { get; set; } // Car brand
    public string? Model { get; set; } // Car model

    public List<Service> Services { get; set; } = new List<Service>();
}

