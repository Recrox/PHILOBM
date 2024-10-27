using System.IO;

namespace PHILOBM.Constants;

public static class ConstantsSettings
{
    public const string BackupPath = "Backups";
    public const string DBName = "philoBM.db";
    public static readonly string DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public static readonly string RacinePath = Path.Combine("C:", "PhiloBM");
    public const int MaxBackupCount = 1000;
    public const bool ShowMessageBoxes = false;

}
