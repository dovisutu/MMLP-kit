using MmlpLib.Models;

namespace MmlpLib.Extensions
{
    public static class EnumerationExtension
    {
        /// <summary>
        /// 从给定的目录出发，根据给定的模组标识符，枚举这些模组的所有命名空间文件夹
        /// </summary>
        /// <param name="assetDirectory">目标仓库的assets/文件夹</param>
        /// <param name="targetModIdentifiers">需检索的模组标识符。如该项为空集合，则枚举所有模组标识符</param>
        /// <param name="config">枚举使用的配置</param>
        /// <returns>检索得到的命名空间文件夹</returns>
        public static IEnumerable<DirectoryInfo> EnumerateNamespaces(
            this DirectoryInfo assetDirectory, IEnumerable<string> targetModIdentifiers, Config config)
            => from modDirectory in assetDirectory.EnumerateDirectories()
               let modIdentifier = modDirectory.Name
               where !targetModIdentifiers.Any()                                   // 未提供列表，全部打包
                   || targetModIdentifiers.Contains(modIdentifier)                 // 有列表，仅打包列表中的项
               where !config.Base.ExclusionMods.Contains(modIdentifier)            // 没有被明确排除
               from namespaceDirectory in modDirectory.EnumerateDirectories()
               let namespaceName = namespaceDirectory.Name
               where !config.Base.ExclusionNamespaces.Contains(namespaceName)      // 没有被明确排除
               where namespaceName.ValidateNamespace()                             // 不是非法名称
               select namespaceDirectory;

        /// <summary>
        /// 按照默认的合并规则，合并给定序列中目标地址重合的提供器
        /// </summary>
        /// <param name="providers">需合并的提供器序列</param>
        /// <returns>经合并后的提供器序列</returns>
        public static IEnumerable<IResourceFileProvider> MergeProviders(
            this IEnumerable<IResourceFileProvider> providers)
            => from provider in providers
               group provider by provider.Destination into destinationGroup
               select destinationGroup
                   .Aggregate(seed: null as IResourceFileProvider,
                              (accumulate, next)
                                  => next.ApplyTo(
                                      accumulate));
        
        /// <summary>
        /// 对给定序列中的每个提供器执行内容替换
        /// </summary>
        /// <param name="providers">需替换的提供器序列</param>
        /// <param name="config">替换配置</param>
        /// <returns>替换后的提供器序列</returns>
        public static IEnumerable<IResourceFileProvider> ReplaceContent(
            this IEnumerable<IResourceFileProvider> providers, Config config)
            => from provider in providers
               select config.Floating.CharacterReplacement
                            .Aggregate(seed: provider,
                                       (accumulate, replacement)
                                           => accumulate.ReplaceContent(
                                               replacement.Key,
                                               replacement.Value));

        /// <summary>
        /// 对给定序列中的每个提供器执行目标路径替换
        /// </summary>
        /// <param name="providers">需替换的提供器序列</param>
        /// <param name="config">替换配置</param>
        /// <returns>替换后的提供器序列</returns>
        public static IEnumerable<IResourceFileProvider> ReplaceDestination(
            this IEnumerable<IResourceFileProvider> providers, Config config)
            => from provider in providers
               select config.Floating.DestinationReplacement
                            .Aggregate(seed: provider,
                                       (accumulate, replacement)
                                           => accumulate.ReplaceDestination(
                                               replacement.Key,
                                               replacement.Value));
    }
}
