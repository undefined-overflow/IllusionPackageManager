using Client.Models.Api;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client
{
    public sealed class Api : IDisposable
    {
        public string Url { get; }

        public Task<PluginInfoApiModel> PluginInstallerInfo(Guid guid) =>
            _client.GetFromJsonAsync<PluginInfoApiModel>($"{Url}/plugins/installers/infos/{guid}.json");

        public Task<GitPluginApiModel> GitPluginInstallerEntity(Guid plugin, Guid game) =>
            _client.GetFromJsonAsync<GitPluginApiModel>($"{Url}/plugins/installers/entities/{game}/{plugin}.json");

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        public Api(IConfiguration configuration) => Url = configuration["Api:Url"];

        private readonly HttpClient _client = new();
    }
}