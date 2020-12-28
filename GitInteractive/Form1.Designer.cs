namespace GitInteractive
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Push = new System.Windows.Forms.Button();
            this.LocalPath = new System.Windows.Forms.TextBox();
            this.address = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CommitText = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Account = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RemoteGitUrl = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Repositories = new System.Windows.Forms.DataGridView();
            this.SaveAccount = new System.Windows.Forms.Button();
            this.NewLocalPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NewRemoteGitUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Loading = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Repositories)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Loading)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Push
            // 
            this.Push.Location = new System.Drawing.Point(523, 214);
            this.Push.Name = "Push";
            this.Push.Size = new System.Drawing.Size(75, 23);
            this.Push.TabIndex = 8;
            this.Push.Text = "提交";
            this.Push.UseVisualStyleBackColor = true;
            this.Push.Click += new System.EventHandler(this.Push_Click);
            // 
            // LocalPath
            // 
            this.LocalPath.Location = new System.Drawing.Point(121, 52);
            this.LocalPath.Name = "LocalPath";
            this.LocalPath.Size = new System.Drawing.Size(477, 21);
            this.LocalPath.TabIndex = 6;
            // 
            // address
            // 
            this.address.AutoSize = true;
            this.address.Location = new System.Drawing.Point(17, 55);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(77, 12);
            this.address.TabIndex = 3;
            this.address.Text = "本地文件路径";
            this.address.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "说明";
            // 
            // CommitText
            // 
            this.CommitText.Location = new System.Drawing.Point(121, 110);
            this.CommitText.Name = "CommitText";
            this.CommitText.Size = new System.Drawing.Size(477, 87);
            this.CommitText.TabIndex = 7;
            this.CommitText.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "帐号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Account
            // 
            this.Account.Location = new System.Drawing.Point(109, 16);
            this.Account.Name = "Account";
            this.Account.Size = new System.Drawing.Size(200, 21);
            this.Account.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(325, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "密码";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Password
            // 
            this.Password.AcceptsTab = true;
            this.Password.Location = new System.Drawing.Point(360, 16);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(200, 21);
            this.Password.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "远程Git地址";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // RemoteGitUrl
            // 
            this.RemoteGitUrl.Location = new System.Drawing.Point(121, 16);
            this.RemoteGitUrl.Name = "RemoteGitUrl";
            this.RemoteGitUrl.Size = new System.Drawing.Size(477, 21);
            this.RemoteGitUrl.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.Repositories);
            this.panel1.Controls.Add(this.SaveAccount);
            this.panel1.Controls.Add(this.NewLocalPath);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.NewRemoteGitUrl);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Add);
            this.panel1.Controls.Add(this.delete);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Password);
            this.panel1.Controls.Add(this.Account);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(705, 508);
            this.panel1.TabIndex = 16;
            // 
            // Repositories
            // 
            this.Repositories.AllowUserToAddRows = false;
            this.Repositories.AllowUserToDeleteRows = false;
            this.Repositories.AllowUserToResizeColumns = false;
            this.Repositories.AllowUserToResizeRows = false;
            this.Repositories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Repositories.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Repositories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Repositories.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Repositories.Location = new System.Drawing.Point(109, 61);
            this.Repositories.MultiSelect = false;
            this.Repositories.Name = "Repositories";
            this.Repositories.RowTemplate.Height = 23;
            this.Repositories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Repositories.Size = new System.Drawing.Size(584, 232);
            this.Repositories.TabIndex = 23;
            this.Repositories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Repositories_CellClick);
            // 
            // SaveAccount
            // 
            this.SaveAccount.Location = new System.Drawing.Point(618, 16);
            this.SaveAccount.Name = "SaveAccount";
            this.SaveAccount.Size = new System.Drawing.Size(75, 23);
            this.SaveAccount.TabIndex = 3;
            this.SaveAccount.Text = "保存";
            this.SaveAccount.UseVisualStyleBackColor = true;
            this.SaveAccount.Click += new System.EventHandler(this.SaveAccount_Click);
            // 
            // NewLocalPath
            // 
            this.NewLocalPath.Location = new System.Drawing.Point(109, 428);
            this.NewLocalPath.Name = "NewLocalPath";
            this.NewLocalPath.Size = new System.Drawing.Size(584, 21);
            this.NewLocalPath.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 428);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "本地文件路径";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // NewRemoteGitUrl
            // 
            this.NewRemoteGitUrl.Location = new System.Drawing.Point(109, 386);
            this.NewRemoteGitUrl.Name = "NewRemoteGitUrl";
            this.NewRemoteGitUrl.Size = new System.Drawing.Size(584, 21);
            this.NewRemoteGitUrl.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 386);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "远程Git地址";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(293, 471);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 17;
            this.Add.Text = "新增";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(618, 299);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 20;
            this.delete.Text = "删除";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "配置列表";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.address);
            this.panel2.Controls.Add(this.LocalPath);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.RemoteGitUrl);
            this.panel2.Controls.Add(this.Push);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.CommitText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(711, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 508);
            this.panel2.TabIndex = 17;
            // 
            // Loading
            // 
            this.Loading.BackColor = System.Drawing.SystemColors.Window;
            this.Loading.Image = ((System.Drawing.Image)(resources.GetObject("Loading.Image")));
            this.Loading.Location = new System.Drawing.Point(677, 190);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(68, 66);
            this.Loading.TabIndex = 14;
            this.Loading.TabStop = false;
            this.Loading.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Loading);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1366, 508);
            this.panel3.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 508);
            this.Controls.Add(this.panel3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Repositories)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Loading)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Push;
        private System.Windows.Forms.TextBox LocalPath;
        private System.Windows.Forms.Label address;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox CommitText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Account;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RemoteGitUrl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox NewLocalPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox NewRemoteGitUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SaveAccount;
        private System.Windows.Forms.DataGridView Repositories;
        private System.Windows.Forms.PictureBox Loading;
        private System.Windows.Forms.Panel panel3;
    }
}

