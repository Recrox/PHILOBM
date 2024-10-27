namespace PHILOBM.Models.Base;

public class Service : BaseEntity
{
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Units { get; set; }


    public decimal CalculateCost()
    {
        return Price; 
    }
}
