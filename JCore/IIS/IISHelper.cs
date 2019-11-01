using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Web.Administration;

namespace JCore.IIS
{
    public class IISHelper
    {
        public static void OutPutCurrentServicesIISSitesInfos()
        {
            var res = getSiteList();
            Console.ReadKey();

            var path = @"./IISSiteInfo.txt";
            if (!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    Console.WriteLine("本地IIS服务器下的站点信息：");
                    sw.WriteLine("本地IIS服务器下的站点信息：");
                    foreach (var siteInfo in res)
                    {
                        Console.WriteLine("站点名称：" + siteInfo.Name);
                        sw.WriteLine("站点名称：" + siteInfo.Name);
                        sw.WriteLine("站点IISId：" + siteInfo.IISId);
                        Console.WriteLine("*********************Start*************************");
                        sw.WriteLine("*********************Start*************************");
                        Console.WriteLine("站点路径：" + siteInfo.Path);
                        sw.WriteLine("站点路径：" + siteInfo.Path);
                        Console.WriteLine("是否是APP站点：" + siteInfo.IsApp);
                        sw.WriteLine("是否是APP站点：" + siteInfo.IsApp);
                        Console.WriteLine("该站点下的子站点信息：");
                        sw.WriteLine("该站点下的子站点信息：");
                        foreach (var child in siteInfo.Children)
                        {
                            Console.WriteLine(" 子站点名称：" + child.Name);
                            sw.WriteLine(" 子站点名称：" + child.Name);
                            Console.WriteLine(" 子站点路径：" + child.Path);
                            sw.WriteLine(" 子站点路径：" + child.Path);
                            Console.WriteLine(" 子站点是否是APP站点：" + child.IsApp);
                            sw.WriteLine(" 子站点是否是APP站点：" + child.IsApp);
                            if (child.Children.Count > 0)
                            {
                                // 发送邮件通知该子站点中还有子站点，需要注意！
                                Console.WriteLine(" 该子站点中还有子站点。请注意！！！");
                                sw.WriteLine(" 该子站点中还有子站点。请注意！！！");
                            }
                        }

                        Console.WriteLine("*********************End*************************");
                        sw.WriteLine("*********************End*************************");
                    }
                }
            }
        }

        /// <summary>
        /// 获取本地IIS服务器上的所有站点。
        /// </summary>
        /// <returns></returns>
        private static List<ScannedSiteInfo> getSiteList()
        {
            var result = new List<ScannedSiteInfo>();

            var iisManager = new ServerManager();
            var sites = iisManager.Sites;

            foreach (var site in sites)
            {
                var bindings = new List<string>();
                foreach (var binding in site.Bindings)
                {
                    if (binding.Protocol == "http")
                    {
                        bindings.Add(binding.BindingInformation);
                    }
                }
                // 如果当前站点的域名不存在HTTP协议的域名。表示站点可能是FTP等站点，直接跳过，放弃记录该站点。
                if (bindings.Count == 0)
                {
                    continue;
                }

                var scanned = new ScannedSiteInfo();
                scanned.Name = site.Name;
                scanned.IISId = Convert.ToInt32(site.Id);
                scanned.Domains = GetDomain(bindings);
                scanned.Path = site.Applications[0].VirtualDirectories[0].PhysicalPath;
                scanned.IsApp = true;
                scanned.Children = new List<ScannedSiteInfo>();
                var applications = site.Applications;
                for (var i = 1; i < applications.Count; i++)
                {
                    var application = applications[i];
                    var subSite = new ScannedSiteInfo();
                    subSite.Name = application.Path;
                    subSite.Path = application.VirtualDirectories[0].PhysicalPath;
                    subSite.IsApp = true;
                    scanned.Children.Add(subSite);
                }
                result.Add(scanned);
            }

            return result;
        }

        /// <summary>
        /// 获取当前服务器的IP地址（通常为内网地址）
        /// </summary>
        private static string GetCurrentServerIp()
        {
            var name = Dns.GetHostName();
            var ipAddresses = Dns.GetHostAddresses(name);
            foreach (var ipa in ipAddresses)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipa.ToString();
                }
            }

            return "获取不到当前服务器的IP，请注意此异常。";
        }

        /// <summary>
        /// 根据输入的域名对象，返回经过处理后的string类型的域名对象，一个站点下的多个域名以\n分割。
        /// </summary>
        private static string GetDomain(List<string> domains)
        {
            string[] domainArray;
            var res = "";
            // 如果只有一条域名。（域名list集合必定超过1个元素因为不可能有不存在域名的站点。）
            if (domains.Count == 1)
            {
                domainArray = domains[0].Split(':');
                if (string.IsNullOrWhiteSpace(domainArray[2]))
                {
                    domainArray[2] = GetCurrentServerIp();
                }
                res = Convert.ToInt32(domainArray[1]) == 80 ? domainArray[2] : domainArray[2] + ":" + domainArray[1];
                return res;
            }

            foreach (var domain in domains)
            {
                domainArray = domain.Split(':');
                if (string.IsNullOrWhiteSpace(domainArray[2]))
                {
                    domainArray[2] = GetCurrentServerIp();
                }
                var temp = Convert.ToInt32(domainArray[1]) == 80
                    ? domainArray[2] + "\n"
                    : domainArray[2] + ":" + domainArray[1] + "\n";
                res += temp;
            }

            return res.Substring(0, res.Length - 1);
        }

    }

    public class ScannedSiteInfo
    {
        /// <summary>
        /// 物理ID。
        /// </summary>
        public int IISId;

        /// <summary>
        /// 站点名称，形如 /www.test.com。
        /// </summary>
        public string Name;

        /// <summary>
        /// 站点物理路径。形如 D:\MySvn\a.com。
        /// </summary>
        public string Path;

        /// <summary>
        /// 判断是否是一个应用程序。
        /// </summary>
        public bool IsApp;

        /// <summary>
        /// 站点的域名。
        /// </summary>
        public string Domains;

        /// <summary>
        /// 站点下的子站点。
        /// </summary>
        public List<ScannedSiteInfo> Children;

        /// <inheritdoc />
        public override string ToString()
        {
            return "IISId:" + IISId + ",Name:" + Name + ",Path:" + Path + ",ChildrenCount:" + Children.Count;
        }
    }
}
