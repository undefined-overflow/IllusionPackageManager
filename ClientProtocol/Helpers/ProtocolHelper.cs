using Microsoft.Win32;

namespace ClientProtocol.Helpers
{
    internal class ProtocolHelper
    {
        internal static void Register()
        {
            using RegistryKey protocolKey = Registry.ClassesRoot.CreateSubKey(_protocol);
            protocolKey.SetValue(null, $"URL:{_protocolName} Protocol");
            protocolKey.SetValue("URL Protocol", "");

            string location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            using RegistryKey iconKey = protocolKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue(null, $"{location},1");

            using RegistryKey commandKey = protocolKey.CreateSubKey(@"shell\open\command");
            commandKey.SetValue(null, $"\"{location}\" \"%1\"");
        }

        private const string _protocol = "ipm";
        private const string _protocolName = "IllusinPackageManager";
    }
}