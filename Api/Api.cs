using IPM.Models.ApiModels;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IPM
{
    public sealed class Api : IDisposable
    {
        private readonly WebClient _webClient = new WebClient();

        private const string _packagesApi = "http://localhost:3001/public/packages";
        private const string _toolchain = "http://localhost:3001/public/toolchain";

        public async Task<PluginModel> GetPlugin(string uuid)
        {
            var data = await _webClient.DownloadStringTaskAsync($"{_packagesApi}/entries/{uuid}.json");
            return JsonSerializer.Deserialize<PluginModel>(data);
        }

        public async Task<VsBuildToolsModel[]> GetVsBuildTools()
        {
            var data = await _webClient.DownloadStringTaskAsync($"{_toolchain}/vs-build-tools.json");
            return JsonSerializer.Deserialize<VsBuildToolsModel[]>(data);
        }

        public Task<string> GetPluginScript(string uuid) => _webClient.DownloadStringTaskAsync($"{_packagesApi}/scripts/{uuid}.js");

        public void Dispose()
        {
            _webClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}