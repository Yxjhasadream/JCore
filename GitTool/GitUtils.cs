using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace GitTool
{
    public static class GitUtils
    {
        private const string FileName = "git";
        private static readonly Regex FileRegex = new Regex("\\s*modified:\\s*(?<file>.*)\\s*");
        private static readonly Regex DateRegex = new Regex("new Date\\(\\d*\\)");

        /// <summary>
        /// 设置帐号密码为持久化保存。设置credential.helper 为store模式。
        /// </summary>
        public static string SetAccount(string account, string password)
        {
            var arguments = "config --global credential.helper store";
            var res = ExecGitCommand(arguments);
            return res;
        }

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
        /// 1. 先`git pull`，拿到最新的文件。这样必定不会产生要merge的情况。因为每次提交之前就已经是最新的文件版本了。
        ///    然后根据pull的返回信息，来做决定是否要提示用户远程的文件已经变更。确定则覆盖，取消则不做操作。pull的结果如果是Already up to date.说明远程仓库的文件没人动过，可以继续操作。
        /// 2. 接着执行git add . 先把所有文件提交到暂存区stage。
        /// 3. 然后利用git status 获取到所有有变动的文件名称。在这些文件中进行尾缀为data.js的筛选。
        /// 4. 把筛选出来的data.js文件一个一个的进行diff操作。如果只有日期发生了变化，则不进行提交。
        ///     先用git rev-parse HEAD 拿到最新的远程库的sha1，
        ///     然后用git show sha1:filename 的命令来获取data.js的上个版本的值。再根据filename直接读当前文件的内容。
        ///     用正则replace，Date(1582182705579.44) 把Date(xxxx)替换成Date() 这样data.js文件的内容就做了一次清理。
        ///     接着比较两个文件剩余字符串是否一致，一致则不提交(用git reset sha1 filename 的方式回退到最新的远程库版本)，不一致则提交（忽略）。
        /// 5. 处理完前面的add和reset操作，开始commit，增加注释，commit完之后push上去。
        /// </summary>
        public static void GitPush(string dir, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new GitTolException("每次提交必须要添加注释！");
            }
            var pull = GitPull(dir);
            var cancel = true;
            if (pull != "Already up to date.")
            {
                //MessageBox.Show("该文件已经被人改动过，请确认是否要覆盖。");
                if (cancel)// 取消覆盖。
                {
                    cancel = false;
                }
            }

            if (cancel)
            {
                return;
            }

            GitAdd(dir);
            var status = GitStatus(dir);
            if (status.Contains("nothing to commit"))
            {
                return;
            }

            var changeFiles = GetChangeFiles(status);
            DiffChangeFiles(dir, changeFiles);
            GitCommit(dir, message);
            var arguments = "push";
            ExecGitCommand(arguments, dir);
        }

        private static void GitCommit(string dir, string message)
        {
            var arguments = "commit";
            ExecGitCommand(arguments, dir);
        }


        private static void DiffChangeFiles(string dir, List<string> changeFiles)
        {
            foreach (var file in changeFiles)
            {
                var preSha1 = GetPreSha1(dir);
                var preVersionFile = GetSpecialSha1File(dir, preSha1, file);
                var currentFile = "";
                using (var reader = new StreamReader($"{dir}\\{file}"))
                {
                    currentFile = reader.ReadToEnd();
                }

                currentFile = DateRegex.Replace(currentFile, "new Date()");
                preVersionFile = DateRegex.Replace(preVersionFile, "new Date()");
                if (currentFile == preVersionFile)
                {
                    GitReset(dir, preSha1, file);
                }
            }
        }

        public static string GitReset(string dir, string sha1, string fileName)
        {
            var arguments = $"reset {sha1} {fileName}";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        private static string GetSpecialSha1File(string dir, string sha1, string fileName)
        {
            var arguments = $"show {sha1} {fileName}";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        private static List<string> GetChangeFiles(string status)
        {
            var res = new List<string>();
            using (var reader = new StringReader(status))
            {
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    // 读到有modified的，表明开始读取变动文件了。
                    if (line != null && line.Contains("modified:"))
                    {
                        var fileName = GetChangeFile(line);
                        res.Add(fileName);
                    }
                }
            }

            return res;
        }


        private static string GetChangeFile(string file)
        {
            var match = FileRegex.Match(file);
            if (match.Success)
            {
                var val = match.Groups["file"].Value;
                return val;
            }
            return "";
        }

        private static string GitStatus(string dir)
        {
            var arguments = "status";
            var res = ExecGitCommand(arguments, dir);
            return res;
        }

        private static string GitAdd(string dir)
        {
            var arguments = "add .";
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
            if (res.ExitCode != 0)
            {
                throw new GitTolException(res.StderrMessage);
            }

            return res.StdoutMessage;
        }
    }
}
