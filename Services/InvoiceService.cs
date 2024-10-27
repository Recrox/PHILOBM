using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace PHILOBM.Services;

public class InvoiceService : BaseContextService<Invoice>, IInvoiceService
{
    public InvoiceService(PhiloBMContext context) : base(context)
    {

    }

    public void CreerPDF(Invoice invoice)
    {
        string directoryPath = "Factures";
        CréerDossierSiInexistant(directoryPath);

        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        double margin = 40;
        double yPoint = margin;
        double adresseHeight;

        DessinerTitre(gfx, page, ref yPoint);
        DessinerAdresseGarage(gfx, page, ref yPoint, margin, out adresseHeight);
        DessinerInformationsDroite(gfx, page, invoice, yPoint, margin);
        DessinerTVA(gfx, page, ref yPoint, margin);
        DessinerInformationsClient(gfx, page, invoice, ref yPoint, margin);
        DessinerTableauPlaqueEtKilometrage(gfx, page, invoice, ref yPoint, margin);
        DessinerTableauServices(gfx, document, page, invoice, ref yPoint, margin);
        DessinerTotal(gfx, document, page, invoice, ref yPoint, margin);
        DessinerMessagePaiement(gfx, document, page, ref yPoint, margin);

        SauvegarderDocument(document, directoryPath, invoice);
    }

    // Sous-méthodes pour chaque section

    private void CréerDossierSiInexistant(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private void DessinerTitre(XGraphics gfx, PdfPage page, ref double yPoint)
    {
        gfx.DrawString("Facture PHILO B.M", new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
            new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
        yPoint += 50;
    }

    private void DessinerAdresseGarage(XGraphics gfx, PdfPage page, ref double yPoint, double margin, out double adresseHeight)
    {
        string[] garageAddressLines = { "Rue Champ Courtin 16", "7522 Marquain", "0473/95.10.03" };
        adresseHeight = 20 * garageAddressLines.Length; // Calculer la hauteur totale de l'adresse

        foreach (var line in garageAddressLines)
        {
            gfx.DrawString(line, new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
        }
    }

    private void DessinerInformationsDroite(XGraphics gfx, PdfPage page, Invoice invoice, double yPoint, double margin)
    {
        double rightMargin = page.Width - margin;
        string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        double initialYPoint = yPoint; // Conserver le yPoint initial

        // Charger le logo de BMW
        string logoPath = Path.Combine("Assets", "bmw_logo.png");
        if (File.Exists(logoPath))
        {
            XImage logo = XImage.FromFile(logoPath);
            gfx.DrawImage(logo, rightMargin - 150, yPoint - 40, 150, 40); // Ajustez la position et la taille selon vos besoins
            yPoint += 40; // Ajustez yPoint pour laisser de l'espace pour l'image
        }

        gfx.DrawString($"Date : {dateTime}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        gfx.DrawString($"Facture n° : {invoice.Id}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        gfx.DrawString($"Adresse client : {invoice.Client.Address}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        // Alignez la hauteur des informations à droite avec celle de l'adresse du garage
        yPoint = initialYPoint; // Réinitialiser yPoint à la valeur initiale
    }



    private void DessinerTVA(XGraphics gfx, PdfPage page, ref double yPoint, double margin)
    {
        gfx.DrawString("TVA : BE 10 10 41 54 45", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
        yPoint += 40;
    }

    private void DessinerInformationsClient(XGraphics gfx, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        gfx.DrawString($"Client: {invoice.Client.FirstName} {invoice.Client.LastName}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
        yPoint += 20;
    }

    private void DessinerTableauPlaqueEtKilometrage(XGraphics gfx, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        // Tableau pour le numéro de plaque et kilométrage
        yPoint += 20;
        double cellHeight = 20;
        double tableWidth = page.Width - 2 * margin;
        double cellWidth = tableWidth / 2;

        gfx.DrawRectangle(XPens.Black, margin, yPoint, cellWidth, cellHeight);
        gfx.DrawRectangle(XPens.Black, margin + cellWidth, yPoint, cellWidth, cellHeight);
        gfx.DrawString("Numéro de plaque:", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Kilométrage:", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + cellWidth, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        yPoint += cellHeight;

        gfx.DrawRectangle(XPens.Black, margin, yPoint, cellWidth, cellHeight);
        gfx.DrawRectangle(XPens.Black, margin + cellWidth, yPoint, cellWidth, cellHeight);
        gfx.DrawString(invoice.Car.LicensePlate ?? "Non spécifié", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString(invoice.Car.Mileage.ToString(), new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin + cellWidth, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        yPoint += 30;
    }

    private void DessinerTableauServices(XGraphics gfx, PdfDocument document, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        double colWidth = (page.Width - 2 * margin) / 3;
        double cellHeight = 20;

        // Dessiner les en-têtes du tableau
        DessinerRectangle(gfx, margin, yPoint, colWidth, cellHeight);
        DessinerRectangle(gfx, margin + colWidth, yPoint, colWidth, cellHeight);
        DessinerRectangle(gfx, margin + 2 * colWidth, yPoint, colWidth, cellHeight);

        gfx.DrawString("Unité", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin, yPoint, colWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Description", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + colWidth, yPoint, colWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Prix", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + 2 * colWidth, yPoint, colWidth, cellHeight), XStringFormats.Center);
        yPoint += cellHeight;

        foreach (var service in invoice.Services)
        {
            // Vérifier si l'on doit ajouter une nouvelle page
            if (yPoint + cellHeight > page.Height - 50) // Laisser un marge de 50 points en bas
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPoint = 40; // Réinitialiser yPoint pour la nouvelle page
            }

            DessinerRectangle(gfx, margin, yPoint, colWidth, cellHeight);
            DessinerRectangle(gfx, margin + colWidth, yPoint, colWidth, cellHeight);
            DessinerRectangle(gfx, margin + 2 * colWidth, yPoint, colWidth, cellHeight);

            gfx.DrawString(service.Units.ToString(), new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin, yPoint, colWidth, cellHeight), XStringFormats.Center);
            gfx.DrawString(service.Description, new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin + colWidth, yPoint, colWidth, cellHeight), XStringFormats.CenterLeft);
            gfx.DrawString($"{service.CalculateCost()} €", new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin + 2 * colWidth, yPoint, colWidth, cellHeight), XStringFormats.CenterRight);
            yPoint += cellHeight;
        }
    }


    private void DessinerRectangle(XGraphics gfx, double x, double y, double width, double height)
    {
        gfx.DrawRectangle(XPens.Black, x, y, width, height);
    }

    private void DessinerTotal(XGraphics gfx, PdfDocument document, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        yPoint += 10;
        double colWidth = (page.Width - 2 * margin) / 3;
        double totalHeight = 20; // Hauteur de la zone de texte du total

        // Vérification si yPoint dépasse la hauteur de la page
        if (yPoint + totalHeight + 30 > page.Height - 50) // Laisser une marge de 50 points en bas
        {
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            yPoint = margin; // Réinitialiser yPoint pour la nouvelle page
        }

        // Placer le total à la fin de la page
        gfx.DrawString($"Total: {invoice.CalculSum()} €", new XFont("Verdana", 14, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + 2 * colWidth, yPoint, colWidth, totalHeight), XStringFormats.CenterRight);
        yPoint += totalHeight + 10; // Ajuster l'espacement après le total
    }

    private void DessinerMessagePaiement(XGraphics gfx, PdfDocument document, PdfPage page, ref double yPoint, double margin)
    {
        double messageHeight = 40; // Hauteur de la zone de texte pour le message de paiement

        // Vérification si yPoint dépasse la hauteur de la page
        if (yPoint + messageHeight > page.Height - 50) // Laisser une marge de 50 points en bas
        {
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            yPoint = margin; // Réinitialiser yPoint pour la nouvelle page
        }

        yPoint += 10; // Ajouter un espace avant le message de paiement
                      // Placer le message de paiement à la fin de la page
        gfx.DrawString("A verser sur le numéro de compte BE51 1262 0722 0675", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, messageHeight), XStringFormats.TopLeft);
        yPoint += messageHeight + 10; // Ajustez si nécessaire pour un espacement ultérieur
    }



    private void SauvegarderDocument(PdfDocument document, string directoryPath, Invoice invoice, bool useClientNameAndDate = true)
    {
        string fileName;

        if (useClientNameAndDate)
        {
            // Nom de fichier avec le prénom, le nom du client et la date
            fileName = $"Facture_{invoice.Client.FirstName}_{invoice.Client.LastName}_{invoice.Date:yyyyMMdd_HHmmss}.pdf";
        }
        else
        {
            // Nom de fichier avec l'ID de la facture
            fileName = $"Facture_{invoice.Id}.pdf";
        }

        string filePath = Path.Combine(directoryPath, fileName);
        document.Save(filePath);
        document.Close();
    }



    public async Task<IEnumerable<Invoice>> GetInvoicesForClientAsync(int selectedClientId)
    {
        var invoices = await _context.Invoices
            .Include(invoice => invoice.Client) // Inclut les détails du client si nécessaire
            .Where(invoice => invoice.Client.Id == selectedClientId) // Assurez-vous que Client est correctement référencé
            .ToListAsync(); // Exécute la requête et retourne la liste des factures

        return invoices;
    }
}
