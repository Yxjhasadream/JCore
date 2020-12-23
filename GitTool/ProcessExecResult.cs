namespace GitTool
{
    public class ProcessExecResult
    {
        /// <summary>
        /// 进程的退出编码。
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// 标准输出返回的结果。
        /// </summary>
        public string StdoutMessage { get; set; }

        /// <summary>
        /// 错误输出返回的结果。
        /// </summary>
        public string StderrMessage { get; set; }
    }
}
