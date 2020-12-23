using System;
using System.Diagnostics;
using System.Text;

namespace GitTool
{
    public class ProcessUtils
    {
        /// <summary>
        /// 按参数执行指定的进程，不显示窗口。
        /// </summary>
        /// <param name="fileName">命令行文件名，形如 cmd.exe, git-bash.exe</param>
        /// <param name="arguments">参数列表。</param>
        /// <param name="workingDirectory">工作目录。</param>
        /// <param name="timeoutMillimeter">超时时间。</param>
        /// <param name="encoding">返回值的编码格式。</param>
        /// <param name="stdinMessage">标准输入参数。</param>
        public static ProcessExecResult Exec(string fileName, string arguments, string workingDirectory = null,
            int? timeoutMillimeter = null, Encoding encoding = null, string stdinMessage = null)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments ?? "",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if (!string.IsNullOrWhiteSpace(workingDirectory)) // 设置工作目录。要配合UseShellExecute=false使用。如果UseShellExecute=true。则此值表示FileName所在的目录。
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return ExecProcess(processStartInfo, timeoutMillimeter, encoding, stdinMessage);
        }

        /// <summary>
        /// 执行单语句windows 命令行，使用/c退出，返回字符串只包含执行语句的结果。
        /// </summary>
        /// <param name="command">参数列表。</param>
        /// <param name="workingDirectory">工作目录。</param>
        /// <param name="timeoutMillimeter">超时时间。</param>
        /// <param name="encoding">返回值的编码格式。</param>
        /// <param name="stdinMessage">标准输入参数。</param>
        public static ProcessExecResult ExecCmdCommand(string command, string workingDirectory = null,
            int? timeoutMillimeter = null, Encoding encoding = null, string stdinMessage = null)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c " + command,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if (!string.IsNullOrWhiteSpace(workingDirectory)) // 设置工作目录。
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }

            return ExecProcess(processStartInfo, timeoutMillimeter, encoding, stdinMessage);
        }

        /// <summary>
        /// 执行进程，并返回结果。
        /// </summary>
        /// <param name="timeoutMillimeter">强行终止前执行的毫秒数。</param>
        /// <param name="processStartInfo">进程启动参数。</param>
        /// <param name="encoding">输出流的编码。</param>
        /// <param name="stdinMessage">标准输入参数。</param>
        /// <returns>执行结果。</returns>
        private static ProcessExecResult ExecProcess(ProcessStartInfo processStartInfo, int? timeoutMillimeter, Encoding encoding, string stdinMessage = null)
        {
            var execResult = new ProcessExecResult { StderrMessage = "", StdoutMessage = "" };

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            processStartInfo.StandardErrorEncoding = encoding;
            processStartInfo.StandardOutputEncoding = encoding;

            using (var process = new Process { StartInfo = processStartInfo })
            {
                // 注册标准输出的事件。
                process.OutputDataReceived += (sender, e) =>
                {
                    execResult.StdoutMessage += e.Data + Environment.NewLine;
                };
                // 注册错误输出的事件。
                process.ErrorDataReceived += (sender, e) =>
                {
                    execResult.StderrMessage += e.Data + Environment.NewLine;
                };

                process.Start();

                if (!string.IsNullOrWhiteSpace(stdinMessage))
                {
                    process.StandardInput.WriteLine(stdinMessage);
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(timeoutMillimeter ?? 5 * 60 * 1000); // 等待进程退出。

                execResult.ExitCode = process.ExitCode;
            }

            execResult.StderrMessage =
                string.IsNullOrWhiteSpace(execResult.StderrMessage)
                ? ""
                : execResult.StderrMessage.Trim();

            execResult.StdoutMessage =
                string.IsNullOrWhiteSpace(execResult.StdoutMessage)
                ? ""
                : execResult.StdoutMessage.Trim();

            return execResult;
        }
    }
}
