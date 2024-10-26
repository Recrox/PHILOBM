namespace PHILOBM.Models.Base;

// Classe abstraite pour représenter un service
public abstract class Service
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public decimal Prix { get; set; }
    public decimal CalculerCout()
    {
        return Prix; // Vous pouvez ajuster cette logique selon vos besoins
    }
}
