using MmlpLib.Extensions;
using Packer.Helpers;
using MmlpLib.Helpers;
using MmlpLib.Models;
using MmlpLib.Models.Providers;
using Serilog;
using System.IO.Compression;

namespace Packer
{
    static class Program
    {
        // System.CommandLine.DragonFruit支持
        public static async Task Main(string version, bool increment = false)
        {
            Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .MinimumLevel.Debug()
             .CreateLogger();

            var config = ConfigHelpers.RetrieveConfig(version);
            Log.Information("开始对版本 {0} 的打包", config.Base.Version);

            var targetModIdentifiers = increment ? GitHelpers.EnumerateChangedMods(config.Base.Version)
                : Enumerable.Empty<string>();

            var rawProviders =
                from namespaceDirectory in new DirectoryInfo($"./projects/{config.Base.Version}/assets")
                                           .EnumerateNamespaces(targetModIdentifiers, config)
                from provider in namespaceDirectory.EnumerateProviders(config)
                select provider;

            var providers = rawProviders.ReplaceDestination(config)
                                        .MergeProviders()
                                        .ReplaceContent(config);

            var initials = from file in new DirectoryInfo($"./projects/{config.Base.Version}")
                                                 .EnumerateFiles()
                                select (file.Name == "pack.mcmeta")
                                    ? McMetaProvider.Create(file, file.Name) // 类型推断不出要用接口
                                    : new RawFile(file, file.Name) as IResourceFileProvider;

            string packName = $"./Minecraft-Mod-Language-Package-{config.Base.Version}.zip";
            await using var stream = File.Create(packName);

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen: true))
            {
                await Task.WhenAll(from provider in providers.Concat(initials)
                                   select provider.WriteToArchive(archive));
            }

            Log.Information("对版本 {0} 的打包结束。", version);

            var md5 = stream.ComputeMD5();

            Log.Information("打包文件的 MD5 值：{0}", md5);
            File.WriteAllText($"./{config.Base.Version}.md5", md5);
        }
    }
}
