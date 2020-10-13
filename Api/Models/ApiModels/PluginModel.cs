namespace IPM.Models.ApiModels
{
    public sealed class PluginModel
    {
        public string Uuid { get; init; }
        public string Script { get; init; }
        public string Game { get; init; }
        public string[] Dependencies { get; init; }
    }
}