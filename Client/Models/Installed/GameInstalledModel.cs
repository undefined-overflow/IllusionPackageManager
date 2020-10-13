using System;

namespace Client.Models.Installed
{
    public class GameInstalledModel
    {
        public Guid Guid { get; init; }
        public string Path { get; set; }
    }
}