using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GitTool;
using Server.Core.Serialization.Json;

namespace GitInteractive
{
    public partial class Form1 : Form
    {
        private const string SettingDir = "settings.json";
        private static Settings _settings;
        private static BindingList<RepositorySetting> _bindingRepositories;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GetSettings();
                Account.Text = _settings.Account;
                Password.Text = _settings.Password;
                Loading.BringToFront();
                _bindingRepositories = new BindingList<RepositorySetting>(_settings.Repositories);
                Repositories.DataSource = _bindingRepositories;
                Repositories.Columns[0].Width = 400;
                Repositories.Columns[1].Width = 50;
                Repositories.Columns[2].Width = 100;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// 获取配置。
        /// </summary>
        private static void GetSettings()
        {
            string text;
            using (var reader = new StreamReader(SettingDir))
            {
                text = reader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                _settings = new Settings { Repositories = new List<RepositorySetting>() };
            }
            else
            {
                var settings = JsonSerializer.Default.Deserialize<Settings>(text);
                settings.Password = string.IsNullOrWhiteSpace(settings.Password) ? "" : Utils.DESDecrypt(settings.Password);
                _settings = settings;
            }
        }

        /// <summary>
        /// 更新配置。
        /// </summary>
        private static void SetSettings()
        {
            var temp = new Settings();
            temp.Password = Utils.DESEncrypt(_settings.Password);
            temp.Account = _settings.Account;
            temp.Repositories = _settings.Repositories;
            var text = JsonSerializer.Default.Serialize(temp);
            using (var writer = new StreamWriter(SettingDir))
            {
                writer.Write(text);
            }
        }

        /// <summary>
        /// 提交按钮。Push当前文件夹的文件到Git。
        /// </summary>
        private void Push_Click(object sender, EventArgs e)
        {
            var dir = LocalPath.Text;
            var message = CommitText.Text;
            try
            {
                Loading.Visible = true;
                GitUtils.GitPush(dir, message);
                Loading.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Loading.Visible = false;
            }
        }

        /// <summary>
        /// 删除一项配置。
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            if (Repositories.SelectedRows.Count <= 0)
            {
                return;
            }

            foreach (DataGridViewRow row in Repositories.SelectedRows)
            {
                for (int i = 0; i < _bindingRepositories.Count; i++)
                {
                    var item = _bindingRepositories[i];
                    if (item.LocalPath == row.Cells[0].Value.ToString() && item.RemoteUrl == row.Cells[1].Value.ToString())
                    {
                        _bindingRepositories.RemoveAt(i);
                        break;
                    }
                }
            }

            SetSettings();
        }

        /// <summary>
        /// 更新帐号密码。
        /// </summary>
        private void SaveAccount_Click(object sender, EventArgs e)
        {
            _settings.Account = Account.Text;
            _settings.Password = Password.Text;
            SetSettings();
            GitUtils.Init();
        }

        /// <summary>
        /// 添加新的本地目录和git仓库映射的配置项。
        /// 需要检查目录是否是空目录，以及能够正确git clone。
        /// 这里不需要做git目录的重复性校验。因为理论上可以允许clone同一个git仓库到本地的多个目录。没有影响。
        /// 而本地目录的重复性校验，在判断本地目录为空的时候就隐性的处理了。
        ///   因为如果是空的，则必定不会在现有列表中存在。
        ///   如果非空。则弹窗提示，让用户自行处理为空目录。
        /// </summary>
        private void Add_Click(object sender, EventArgs e)
        {
            var repository = new RepositorySetting();
            var path = NewLocalPath.Text;
            var remote = NewRemoteGitUrl.Text;
            var remark = NewRemark.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("此文件路径不存在,请检查！");
                return;
            }

            if (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0)
            {
                MessageBox.Show("此文件夹中包含文件,请使用空文件夹进行初始化。");
                return;
            }

            if (string.IsNullOrWhiteSpace(_settings.Account))
            {
                MessageBox.Show("请先保存帐号。");
                return;
            }

            if (string.IsNullOrWhiteSpace(_settings.Password))
            {
                MessageBox.Show("请先保存密码。");
                return;
            }

            try
            {
                Loading.Visible = true;
                GitUtils.GitClone(remote, _settings.Account, _settings.Password, path);
                Loading.Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Git初始化失败：" + exception.Message);
                Loading.Visible = false;
                return;
            }

            repository.LocalPath = path;
            repository.RemoteUrl = remote;
            repository.Remark = remark;
            _bindingRepositories.Add(repository);
            SetSettings();
            NewRemoteGitUrl.Text = "";
            NewLocalPath.Text = "";
            NewRemark.Text = "";
        }

        /// <summary>
        /// 选择具体某个配置。
        /// </summary>
        private void Repositories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var cells = Repositories.SelectedCells;
            if (cells.Count==3)
            {
                LocalPath.Text = cells[0].Value.ToString();
                RemoteGitUrl.Text = cells[1].Value.ToString();
                Remark.Text = cells[2].Value.ToString();
            }
        }
    }
}
