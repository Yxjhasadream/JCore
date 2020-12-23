using GitTool;
using NUnit.Framework;

namespace Tests.GitTool
{
    [TestFixture]
    public class GitToolTests
    {
        [Test]
        public static void GitClone()
        {
            var remote = "http://git.server.tongbu.com/youxiaojun/MyList.git";
            var account = "664105020@qq.com";
            var password = "a5625845";
            GitUtils.GitClone(remote, account, password);
        }

        [Test]
        public static void SetAccount()
        {
            GitUtils.SetAccount("", "");
        }


        [Test]
        public static void SetQuotePath()
        {
            GitUtils.SetQuotePath();
        }

        [Test]
        public static void GitPull()
        {
            var dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug\\MyList";
            GitUtils.GitPull(dir);
            dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug";
            GitUtils.GitPull(dir);
        }

        [Test]
        public static void GitPush()
        {
            var dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug\\MyList";
            GitUtils.GitPush(dir,"提交");
        }

        [Test]
        public static void GetPreSHA1()
        {
            var dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug\\MyList";
            var message = GitUtils.GetPreSha1(dir);
            dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug";
            var message1 = GitUtils.GetPreSha1(dir);
        }
    }
}
