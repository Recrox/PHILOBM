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

