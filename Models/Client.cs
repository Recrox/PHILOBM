using PHILOBM.Models.Base;

namespace PHILOBM.Models;

public class Client : BaseEntity
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public List<Voiture> Voitures { get; set; } = new List<Voiture>();
}
