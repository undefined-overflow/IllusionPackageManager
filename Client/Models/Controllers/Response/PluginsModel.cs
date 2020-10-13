using Client.Models.Installed;
using System;

namespace Client.Models.Controllers.Response
{
    public class PluginsModel
    {
        public PluginsModel(Guid guid, PluginInstalledModel model) =>
            (Guid, Version) = (guid, model.Version);

        public Guid Guid { get; init; }
        public string Version { get; init; }
    }
}