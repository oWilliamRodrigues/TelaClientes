using Microsoft.Win32;

namespace Europa.Commons
{
    public class MimeMappingWrapper
    {

        public static string GetDefaultExtension(string mimeType)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            object value = key?.GetValue("Extension", null);
            return value?.ToString() ?? string.Empty;
        }

        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
