using System;
using System.Collections.Generic;

namespace Client.Models.Api
{
    public enum PluginType
    {
        Git
    }

    public class PluginInfoApiModel
    {
        public PluginType Type { get; init; }
        public IReadOnlyList<Guid> Dependencies { get; init; }
    }
}