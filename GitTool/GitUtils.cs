using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;

namespace GitTool
{
    public static class GitUtils
    {
        private const string FileName = "git";
        private static readonly Regex FileRegex = new Regex("\\s*modified:\\s*(?<file>.*)\\s*");
        private static readonly Regex DateRegex = new Regex("new Date\\(\\d*\\.?\\d*\\)");

        public static void Init()
        {
            SetCredentialStore();
            SetQuotePath();
        }

        /// <summary>
        /// 设置帐号密码为持久化保存。设置credential.helper 为store模式。
        /// </summary>
        public static string SetCredentialStore()
        {
            var arguments = "config --global credential.helper store";
            var res = ExecGitCommand(arguments);
            return res;
        }

        /// <summary>
        /// 配置取消中文转译。
        /// </summary>
        /// <returns></returns>
        public static string SetQuotePath()
        {
            var arguments = "config --global core.quotepath false";
            var res = ExecGitCommand(arguments);
            return res;
        }

        /// <summary>
        /// git clone 操作。
        ///
        /// 考虑做一个本地的持久化，如果外部没有传进来帐号和密码。则使用本地记录的。如果本地也没有，则提示让用户进行帐号密码的初始化。
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static string GitClone(string remote, string account, string password)
        {
            account = WebUtility.UrlEncode(account);
            password = WebUtility.UrlEncode(password);
            var dir = Directory.GetCurrentDirectory();
            var uri = new Uri(remote);
            var address = $"http://{account}:{password}@{uri.Host + uri.AbsolutePath}";
            var arguments = $"clone {address}";//格式为http://username:password@address 需要注意的是。username和password需要进行一下转译。
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// 对指定目录进行git pull操作。
        /// </summary>
        public static string GitPull(string dir)
        {
            var arguments = "pull";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// git push操作。
        /// push之前需要做一下几个步骤。
        /// 1. 首先把本地的修改都stash起来。
        /// 2. 接着`git pull`，拿到最新的文件。这样必定不会产生要merge的情况。因为每次提交之前就已经是最新的文件版本了。
        ///    然后将stash pop。根据返回的信息确定是否有冲突。如果没有冲突。直接走4.有冲突则进行3。有冲突则返回的信息会是。
        /// 3. 有冲突。用`git checkout --theirs .`把远程的变化都否定。
        /// 4. 接着执行git add . 先把所有文件提交到暂存区stage。
        /// 5. 然后利用git status 获取到所有有变动的文件名称。在这些文件中进行尾缀为data.js的筛选。
        /// 6. 把筛选出来的data.js文件一个一个的进行diff操作。如果只有日期发生了变化，则不进行提交。
        ///     先用git rev-parse HEAD 拿到最新的远程库的sha1，
        ///     然后用git show sha1:filename 的命令来获取data.js的上个版本的值。再根据filename直接读当前文件的内容。
        ///     用正则replace，Date(1582182705579.44) 把Date(xxxx)替换成Date() 这样data.js文件的内容就做了一次清理。
        ///     接着比较两个文件剩余字符串是否一致，一致则不提交(用git reset sha1 filename 的方式回退到最新的远程库版本)，不一致则提交（忽略）。
        /// 7. 处理完前面的add和reset操作，开始commit，增加注释，commit完之后push上去。
        /// </summary>
        public static void GitPush(string dir, string message)
        {
            if (IsNullOrWhiteSpace(message))
            {
                throw new GitTolException("每次提交必须要添加注释！");
            }

            GitStash(dir);
            GitPull(dir);
            var stash = GitStash(dir, "pop");

            var cancel = false;
            if (stash.Contains("conflict"))
            {
                //MessageBox.Show("该文件已经被人改动过，请确认是否要覆盖。");
                GitCheckout(dir, "--theirs ."); // 把远端的所有都忽略。

                //if (!cancel)// 取消覆盖。
                //{
                //    cancel = false;
                //}
            }

            if (cancel)
            {
                return;
            }

            GitAdd(dir);
            var status = GitStatus(dir);
            if (!status.Contains("nothing to commit"))
            {
                var changeFiles = GetChangeFiles(status);
                DiffDataJsFiles(dir, changeFiles); // 后续可能会有其余的文件diff处理。
                GitCommit(dir, message);
            }

            if (status.Contains("use \"git push\""))
            {
                var arguments = "push";
                ExecGitCommand(arguments, dir);
            }

            GitStash(dir, "pop"); // 如果之前有冲突。可能会导致pop不干净，这里在末尾再次pop一下。
        }

        /// <summary>
        /// git commit操作。
        /// </summary>
        /// <param name="dir">在指定的路径下执行git commit</param>
        /// <param name="message">commit时提交的信息。</param>
        public static void GitCommit(string dir, string message)
        {
            var arguments = $"commit -m \"{message}\"";
            ExecGitCommand(arguments, dir);
        }

        public static string GitStash(string dir, string mode = "")
        {
            var arguments = $"stash {mode}";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// 对git目录下的特殊的（目前是只处理data.js后续可能会增加别的）有变动的文件进行diff操作，如果只是除了日期发生了变动，那就revert回去。
        /// </summary>
        /// <param name="dir">git根目录。</param>
        /// <param name="changeFiles">有变化的文件名称列表。</param>
        private static void DiffDataJsFiles(string dir, List<string> changeFiles)
        {
            var preSha1 = GetPreSha1(dir);
            foreach (var file in changeFiles)
            {
                if (!file.Contains("data.js"))
                {
                    continue;
                }

                var preVersionFile = GetSpecialSha1File(dir, preSha1, file);
                string currentFile;
                using (var reader = new StreamReader($"{dir}\\{file}"))
                {
                    currentFile = reader.ReadToEnd();
                }

                // 因为 GetSpecialSha1File 读到的文件内容有 bom， StreamReader 读出来的似乎没有 bom ，
                // 为了统一。都去除一下 utf-8 的 bom。(默认都采用 utf-8 读 git 的操作结果，所以只 trim 这个)。
                currentFile = DateRegex.Replace(currentFile, "new Date()").Trim('\uFEFF');
                preVersionFile = DateRegex.Replace(preVersionFile, "new Date()").Trim('\uFEFF');
                if (currentFile == preVersionFile)
                {
                    GitReset(dir, preSha1, file);
                    GitCheckout(dir, file);
                }
            }
        }

        /// <summary>
        /// 将指定文件reset到指定的sha1版本。
        /// </summary>
        /// <param name="dir">git根目录。</param>
        /// <param name="sha1">sha1。</param>
        /// <param name="fileName">文件名称。</param>
        public static string GitReset(string dir, string sha1, string fileName)
        {
            var arguments = $"reset {sha1} {fileName}";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// 获取指定sha1的文件内容。
        /// </summary>
        /// <param name="dir">git根目录。</param>
        /// <param name="sha1">sha1。</param>
        /// <param name="fileName">文件名称。</param>
        public static string GetSpecialSha1File(string dir, string sha1, string fileName)
        {
            var arguments = $"show {sha1}:{fileName}";
            var res = ExecGitCommand(arguments, dir, null, new UTF8Encoding(false));
            return res;
        }

        /// <summary>
        /// 获取所有有变动的文件名称列表。方便后续进行diff处理。
        /// </summary>
        /// <param name="status">git status 得到的返回结果。</param>
        private static List<string> GetChangeFiles(string status)
        {
            var res = new List<string>();
            using (var reader = new StringReader(status))
            {
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    // 读到有modified的，表明开始读取变动文件，目前只会针对data.js来做特殊处理。
                    if (line != null && line.Contains("modified:"))
                    {
                        var fileName = GetChangeFile(line);
                        res.Add(fileName);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 利用正则，匹配出发生过变动的文件名称。
        /// </summary>
        /// <param name="modifiedText">变动的描述，形如 modified:  test.txt</param>
        /// <returns></returns>
        private static string GetChangeFile(string modifiedText)
        {
            var match = FileRegex.Match(modifiedText);
            if (match.Success)
            {
                var val = match.Groups["file"].Value;
                return val;
            }
            return "";
        }

        /// <summary>
        /// 在指定目录执行git status 操作，同时返回status的结果。
        /// </summary>
        public static string GitStatus(string dir)
        {
            var arguments = "status";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// 在指定目录执行 git add 操作，将所有文件都添加到stage中。
        /// </summary>
        public static string GitAdd(string dir)
        {
            var arguments = "add .";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        public static string GitCheckout(string dir, string fileName)
        {
            var arguments = $"checkout {fileName}";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// 获取指定目录的上一个版本的CommitId。
        /// </summary>
        public static string GetPreSha1(string dir)
        {
            var arguments = "rev-parse HEAD";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        /// <summary>
        /// Exec帮助类，将在这里处理异常情况，如果有发现ExitCode不为0，则抛出error信息。
        /// 参数涵义可以直接查看<see cref="ProcessUtils.Exec"/>的参数表。
        /// </summary>
        private static string ExecGitCommand(string arguments, string workingDirectory = null,
            int? timeoutMillimeter = null, Encoding encoding = null, string stdinMessage = null)
        {
            var res = ProcessUtils.Exec(FileName, arguments, workingDirectory, timeoutMillimeter, encoding, stdinMessage);
            if (res.ExitCode != 0 && res.ExitCode != 1)
            {
                throw new GitTolException(res.StderrMessage);
            }

            return res.StdoutMessage ?? res.StderrMessage;
        }
    }
}
