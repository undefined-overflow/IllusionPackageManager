using Client.Models.Api;
using Client.Models.Installed;
using Client.Repositories;
using Octokit;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static System.IO.File;

namespace Client.Managers
{
    public class PluginManager
    {
        public static readonly string CACHE_DIRECTORY = Path.Join(Path.GetTempPath(), "IPM", "Cache", "Plugins");

        private readonly Api _api;
        private readonly GitHubClient _github;
        private readonly PluginsRepository _plugins;

        public async Task Install(Guid plugin, GameInstalledModel game)
        {
            if (_plugins.TryGetValue(game.Guid, out var plugins))
            {
                var info = await _api.PluginInstallerInfo(plugin);
                await Task.WhenAll(info.Dependencies.Select(d => Install(d, game)));

                if (info.Type == PluginType.Git)
                {
                    var entity = await _api.GitPluginInstallerEntity(plugin, game.Guid);
                    var release = await _github.Repository.Release.GetLatest(entity.GitUser, entity.GitRepository);

                    var asset = release.Assets.First(a => Regex.IsMatch(a.Name, entity.FileMask));
                    await using var stream = await GetArchiveStream(asset.BrowserDownloadUrl);

                    using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
                    if (plugins.TryAdd(plugin, new()
                    {
                        Version = release.TagName,
                        Files = archive.Entries.Select(e => e.FullName).ToArray()
                    }))
                    {
                        archive.ExtractToDirectory(game.Path, true);
                    }
                }
            }
        }

        public PluginManager(Api api, GitHubClient github, PluginsRepository plugins) =>
            (_api, _github, _plugins) = (api, github, plugins);

        private static async Task<FileStream> GetArchiveStream(string url)
        {
            string path = Path.Join(CACHE_DIRECTORY, Path.GetFileName(url));
            if (!Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using HttpClient http = new();
                await using var stream = await http.GetStreamAsync(url);

                await using FileStream fileStream = new(path, System.IO.FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }

            return new(path, System.IO.FileMode.Open, FileAccess.Read);
        }
    }
}