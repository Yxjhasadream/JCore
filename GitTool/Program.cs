using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            GitUtils.SetAccount("", "");
            var remote = "http://git.server.tongbu.com/youxiaojun/MyList.git";
            var account = "664105020@qq.com";
            var password = "a5625845";
            GitUtils.GitClone(remote, account, password);
            //Console.WriteLine("自动 git 命令");

            //var git = new CommandRunner("git", @"D:\MyRepository\GitHub\JCore\GitTool\bin\Debug\Xiongmao_Android");
            //var status = git.Run("status");

            //Console.WriteLine(status);
            //Console.WriteLine("按 Enter 退出程序……");
            //Console.ReadLine();
        }
    }
}
