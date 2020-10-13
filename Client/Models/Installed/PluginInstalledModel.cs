using System.Collections.Generic;

namespace Client.Models.Installed
{
    public class PluginInstalledModel
    {
        public string Version { get; init; }
        public IReadOnlyList<string> Files { get; init; }
    }
}