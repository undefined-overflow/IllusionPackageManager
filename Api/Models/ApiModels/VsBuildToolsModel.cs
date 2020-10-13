namespace IPM.Models.ApiModels
{
    public sealed class VsBuildToolsModel
    {
        public string Version { get; init; }
        public string Url { get; init; }
        public string Path { get; init; }
        public string ProductId { get; init; }
        public string ChannelId { get; init; }
    }
}