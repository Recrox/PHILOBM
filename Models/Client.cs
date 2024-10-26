using System.ComponentModel.DataAnnotations;

namespace PHILOBM.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public List<Voiture> Voitures { get; set; } = new List<Voiture>();
}
