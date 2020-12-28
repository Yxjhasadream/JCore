using System.Collections.Generic;

namespace GitInteractive
{
    /// <summary>
    /// 用户Git交互的相关配置。
    /// </summary>
    public class Settings
    {
        public string Account;

        public string Password;

        public List<RepositorySetting> Repositories;
    }

    /// <summary>
    /// 本地路径与远程git路径的映射匹配结构。
    /// </summary>
    public class RepositorySetting
    {
        /// <summary>
        /// 本地路径。
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// 远程git库地址。
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 备注信息。
        /// </summary>
        public string Remark { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return RemoteUrl + "|" + LocalPath;
        }
    }
}
