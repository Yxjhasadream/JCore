using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace ConsoleApp1
{
    class Program
    {
        private static string NginxConfPath = "D:\\Nginx\\sites-enabled,D:\\Nginx\\sites-enabled\\store.emmmd.com.conf";
        private static Regex regex = new Regex(@":\d+");

        static void Main(string[] args)
        {


        }

        private static void Monggo()
        {
            var path = "";
            var Conn = new SQLiteConnection($"Data Source={path};Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10");
            SQLiteCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "select ROOT from REPOSITORY order by id desc limit 1";
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine(ds.Tables[0].Rows[0]["ROOT"]);
                }
            }
        }

        private static List<DockerInfo> GetDockerConfigs()
        {
            var arguments =
@" ps --no-trunc --format 'table {{.ID}}@_@{{.Image}}@_@{{.Command}}@_@{{.CreatedAt}}@_@{{.RunningFor}}@_@{{.Ports}}@_@{{.Status}}@_@{{.Size}}@_@{{.Names}}@_@{{.Mounts}}'";
            var res = new List<DockerInfo>();
            var psi = new ProcessStartInfo("docker", arguments) { RedirectStandardOutput = true };
            var proc = Process.Start(psi);
            if (proc == null)
            {
                Console.WriteLine("Can not exec.");
            }
            else
            {
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }
                        var dockerInfo = new DockerInfo();
                        var props = line.Split("@_@");
                        var command = props[2].Trim('"').Trim();
                        if (!command.StartsWith("dotnet", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue; // 只处理 dotnet 的程序。
                        }

                        dockerInfo.ContainerID = props[0];
                        dockerInfo.Image = props[1];
                        dockerInfo.Command = command;
                        dockerInfo.Created = props[3];
                        dockerInfo.Elapsed = props[4];
                        dockerInfo.Ports = GetPort(props[5]);
                        dockerInfo.Status = props[6];
                        dockerInfo.Size = props[7];
                        dockerInfo.Name = props[8];
                        dockerInfo.Mounts = props[9];
                        res.Add(dockerInfo);
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
            }

            using (var read = new StreamReader("C:\\Users\\admin\\Desktop\\notepad++\\docker"))
            {
                while (!read.EndOfStream)
                {
                    var line = read.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    var dockerInfo = new DockerInfo();
                    var props = line.Split("@_@");
                    var command = props[2].Trim('"').Trim();
                    if (!command.StartsWith("dotnet", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue; // 只处理 dotnet 的程序。
                    }

                    dockerInfo.ContainerID = props[0];
                    dockerInfo.Image = props[1];
                    dockerInfo.Command = command;
                    dockerInfo.Created = props[3];
                    dockerInfo.Elapsed = props[4];
                    dockerInfo.Ports = GetPort(props[5]);
                    dockerInfo.Status = props[6];
                    dockerInfo.Size = props[7];
                    dockerInfo.Name = props[8];
                    dockerInfo.Mounts = props[9];
                    res.Add(dockerInfo);
                }
            }

            return res;
        }

        private static string GetPort(string input)
        {
            var match = regex.Match(input);
            if (match.Success)
            {
                return match.Value.Trim(':');
            }

            return "";
        }

        private static IList<NginxServerConfig> GetNginxConfigs()
        {
            var paths = NginxConfPath.Split(',');
            var res = new List<NginxServerConfig>();
            if (paths.Length == 1 && !Directory.Exists(paths[0])) // 仅仅只有一个文件且不是目录的情况下，直接用文件解析
            {
                var conf = ResolveNginxConfByFile(paths[0]);
                res.AddRange(conf);
                return res;
            }

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    // 如果是个目录，则读取该目录下的所有.conf文件。
                    var filePaths = Directory.GetFiles(path);
                    foreach (var filePath in filePaths)
                    {
                        var conf = ResolveNginxConfByFile(filePath);
                        res.AddRange(conf);
                    }
                }
                else
                {
                    var conf = ResolveNginxConfByFile(path);
                    res.AddRange(conf);
                }
            }

            return res;
        }

        private static List<NginxServerConfig> ResolveNginxConfByFile(string path)
        {
            if (path.EndsWith('~'))
            {
                return new List<NginxServerConfig>();
            }
            var res = new List<NginxServerConfig>();
            var nginxSections = new Stack<NginxSection>();
            var upstream = new NginxSection();
            // 10 代表\n   32 代表'' 每次读到分号为止，算是一个命令结束。通常也不会一个命令换行拼写。就以一行为一个命令的模式暂时处理吧。碰到不兼容的再说好了。
            var stack = new Stack<char>();
            using (var read = new StreamReader(path))
            {
                while (!read.EndOfStream)
                {
                    var character = (char)read.Read();
                    if (character == '}')
                    {
                        var command = GetCommand(stack); // 括号内的key value对。
                        var section = GetSection(stack); // 节的名称和对应可能的配置。

                        if (section.name == Upstream) // 处理负载均衡。同时把负载均衡节点从结果集中过滤掉。
                        {
                            upstream.name = section.value.Trim();
                            if (command.name == Server)
                            {
                                upstream.value = command.value.Trim().Split(' ')[0];
                            }
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(command.value))
                        {
                            nginxSections.Push(command);
                            nginxSections.Push(section);
                        }

                    }
                    // 上面一步把{}中的内容获取完毕。
                    // 这时候继续往前pop。pop到一个分号为止。这样pop出来的内容，就是 xxxxx {} 的节点名称。可以尝试匹配是否是location或是server_name节点
                    stack.Push(character);
                }
            }

            var count = nginxSections.Count;
            NginxServerConfig serverConfig = null;
            NginxLocation location = null;
            for (int i = 0; i < count; i++)
            {
                var item = nginxSections.Pop();

                switch (item.name)
                {
                    case Server:
                        serverConfig = new NginxServerConfig();
                        serverConfig.Locations = new List<NginxLocation>();
                        location = new NginxLocation();
                        res.Add(serverConfig);
                        break;
                    case ServerName:
                        serverConfig.ServerName = item.value;
                        break;
                    case Location:
                        location.VirtualPath = item.value;
                        break;
                    case ProxyPass:
                        if (string.IsNullOrWhiteSpace(upstream.name) || string.IsNullOrWhiteSpace(upstream.value))
                        {
                            location.ProxyPass = item.value;
                        }
                        else
                        {
                            location.ProxyPass = item.value.Replace(upstream.name, upstream.value);
                        }
                        serverConfig.Locations.Add(location);
                        location = new NginxLocation();
                        break;
                }
            }

            return res;
        }

        private static NginxSection GetSection(Stack<char> stack)
        {
            var sb = new StringBuilder();
            char character;
            while (stack.Count > 0 && (character = stack.Pop()) != '\n')
            {
                sb.Append(character);
            }

            Reverse(sb);
            var line = sb.ToString().Trim();
            var location = line.Split(' ');
            if (location.Length == 1)// 只有一个表示是server节。
            {
                return new NginxSection { name = location[0].Trim(';'), value = "" };
            }

            return new NginxSection
            {
                name = location[0].Trim(';'),
                value = line.Replace(location[0], "").Trim(';')
            };
        }

        private static NginxSection GetCommand(Stack<char> stack)
        {
            var res = new NginxSection();
            var sb = new StringBuilder();
            char character;
            // 把括号中的内容读出来，视为某个节。 然后作为字典返回回去。
            while ((character = stack.Pop()) != '{')
            {
                sb.Append(character);
            }

            Reverse(sb);

            using (var sr = new StringReader(sb.ToString()))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith(ServerName)) // 读取servername节点。
                    {
                        var domains = line.Replace(ServerName, "").Trim().Trim(';'); ; // 将域名都解析出来。
                        res.name = ServerName;
                        res.value = domains;
                        return res;
                    }

                    if (line.StartsWith(ProxyPass)) // 读取proxpass
                    {
                        var proxy_pass = line.Replace(ProxyPass, "").Trim().Trim(';'); // 因为都只会有一个，不会出现多个的情况，所以不用做字典排重。
                        res.name = ProxyPass;
                        res.value = proxy_pass;
                        return res;
                    }

                    if (line.StartsWith(Server)) // 读取负载均衡的server。
                    {
                        var server = line.Replace(Server, "").Trim();
                        res.name = Server;
                        res.value = server;
                        return res;
                    }
                }
            }

            return res;
        }


        private static void Reverse(StringBuilder input)
        {
            var count = input.Length;
            var maxIndex = count - 1;
            for (int i = 0; i < count / 2; i++)
            {
                var temp = input[i];
                input[i] = input[maxIndex - i];
                input[maxIndex - i] = temp;
            }
        }

        public class DockerInfo
        {
            public string ContainerID;
            public string Image;
            public string Command;
            public string Created;
            public string Elapsed;
            public string Status;
            public string Ports;
            public string Size;
            public string Name;
            public string Mounts;

            /// <inheritdoc />
            public override string ToString()
            {
                return $"{ContainerID.Substring(0, 8)}|{Image}|{Command}|{Created}|{Elapsed}|{Status}|{Ports}|{Name}|{Mounts}";
            }
        }

        public class NginxServerConfig
        {
            public string ServerName;
            public IList<NginxLocation> Locations;

            /// <inheritdoc />
            public override string ToString()
            {
                return ServerName + "||" + Locations.Count;
            }
        }

        public class NginxLocation
        {
            public string VirtualPath;
            public string ProxyPass;

            /// <inheritdoc />
            public override string ToString()
            {
                return VirtualPath + "||" + ProxyPass;
            }
        }

        public class NginxSection
        {
            public string name;
            public string value;

            /// <inheritdoc />
            public override string ToString()
            {
                return name + "||" + value;
            }
        }


        private const string ServerName = "server_name";
        private const string Location = "location";
        private const string ProxyPass = "proxy_pass";
        private const string Server = "server";
        private const string Upstream = "upstream";
    }
}
