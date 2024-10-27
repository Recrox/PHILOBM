using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PHILOBM.Models.Base;

namespace PHILOBM.Models;

public class Invoice : BaseEntity
{
    public Client Client { get; set; } = null!;
    public Car Car { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<Service> Services { get; set; } = new List<Service>();

    public decimal CalculSum()
    {
        return Services.Sum(service => service.CalculateCost());
    }
}

