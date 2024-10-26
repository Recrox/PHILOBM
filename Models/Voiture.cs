using PHILOBM.Models.Base;

namespace PHILOBM.Models;

// Classe pour la voiture, liée à un client
public class Voiture : BaseEntity
{
    public string? NumeroPlaque { get; set; }
    public string? NumeroChassis { get; set; }
    public int Kilometrage { get; set; }
    public int ClientId { get; set; } // Clé étrangère vers Client
    public Client? Proprietaire { get; set; }
}
