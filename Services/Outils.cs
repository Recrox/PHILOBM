using System.IO;

namespace PHILOBM.Services;

public static class Outils
{
    /// <summary>
    /// Crée un dossier si il n'existe pas déjà.
    /// </summary>
    /// <param name="path">Le chemin du dossier à créer.</param>
    public static void CréerDossierSiInexistant(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

