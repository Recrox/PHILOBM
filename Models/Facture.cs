using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PHILOBM.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace PHILOBM.Models;

public class Facture
{

    [Key]
    public int Id { get; set; }
    public Client Client { get; set; } = null!;
    public Voiture Voiture { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<Service> Services { get; set; } = new List<Service>();

    public decimal CalculerTotal()
    {
        return Services.Sum(service => service.CalculerCout());
    }
    
public void CreerPDF()
{
    var document = new PdfDocument();
    var page = document.AddPage();
    var gfx = XGraphics.FromPdfPage(page);

    gfx.DrawString("Facture PHILO B.M", new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
        new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);
    document.Save("facture.pdf");
}
}

