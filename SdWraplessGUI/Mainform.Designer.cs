namespace SdWraplessGUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox gbViewFile;
            System.Windows.Forms.Button btnSelectFile;
            System.Windows.Forms.GroupBox gbSdWrapInfo;
            System.Windows.Forms.Label labelSdWrapPatch;
            System.Windows.Forms.Label labelSdWrapMode;
            System.Windows.Forms.Label labelSdWrapEncryptConfigSize;
            System.Windows.Forms.Label labelMainExeSize;
            System.Windows.Forms.Label labelMainExeVersion;
            System.Windows.Forms.Label labelSdWrapConfigSize;
            System.Windows.Forms.Label labelSdWrapSize;
            System.Windows.Forms.Label labelVersion;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tbFilePath = new System.Windows.Forms.TextBox();
            lvSdWrapPatch = new System.Windows.Forms.ListView();
            tbSdWrapMode = new System.Windows.Forms.TextBox();
            tbSdWrapEncryptConfigSize = new System.Windows.Forms.TextBox();
            tbMainExeSize = new System.Windows.Forms.TextBox();
            tbMainExeVersion = new System.Windows.Forms.TextBox();
            tbSdWrapConfigSize = new System.Windows.Forms.TextBox();
            tbSdWrapSize = new System.Windows.Forms.TextBox();
            tbSdWrapVersion = new System.Windows.Forms.TextBox();
            btnExtract = new System.Windows.Forms.Button();
            gbViewFile = new System.Windows.Forms.GroupBox();
            btnSelectFile = new System.Windows.Forms.Button();
            gbSdWrapInfo = new System.Windows.Forms.GroupBox();
            labelSdWrapPatch = new System.Windows.Forms.Label();
            labelSdWrapMode = new System.Windows.Forms.Label();
            labelSdWrapEncryptConfigSize = new System.Windows.Forms.Label();
            labelMainExeSize = new System.Windows.Forms.Label();
            labelMainExeVersion = new System.Windows.Forms.Label();
            labelSdWrapConfigSize = new System.Windows.Forms.Label();
            labelSdWrapSize = new System.Windows.Forms.Label();
            labelVersion = new System.Windows.Forms.Label();
            gbViewFile.SuspendLayout();
            gbSdWrapInfo.SuspendLayout();
            SuspendLayout();
            // 
            // gbViewFile
            // 
            gbViewFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbViewFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            gbViewFile.Controls.Add(btnSelectFile);
            gbViewFile.Controls.Add(tbFilePath);
            gbViewFile.Location = new System.Drawing.Point(12, 3);
            gbViewFile.Name = "gbViewFile";
            gbViewFile.Padding = new System.Windows.Forms.Padding(0);
            gbViewFile.Size = new System.Drawing.Size(760, 44);
            gbViewFile.TabIndex = 0;
            gbViewFile.TabStop = false;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSelectFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnSelectFile.Location = new System.Drawing.Point(706, 15);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new System.Drawing.Size(47, 23);
            btnSelectFile.TabIndex = 1;
            btnSelectFile.Text = "...";
            btnSelectFile.UseMnemonic = false;
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += BtnSelectFile_Click;
            // 
            // tbFilePath
            // 
            tbFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbFilePath.Location = new System.Drawing.Point(6, 15);
            tbFilePath.MaxLength = 1024;
            tbFilePath.Name = "tbFilePath";
            tbFilePath.ReadOnly = true;
            tbFilePath.Size = new System.Drawing.Size(694, 23);
            tbFilePath.TabIndex = 0;
            tbFilePath.TabStop = false;
            // 
            // gbSdWrapInfo
            // 
            gbSdWrapInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbSdWrapInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            gbSdWrapInfo.Controls.Add(lvSdWrapPatch);
            gbSdWrapInfo.Controls.Add(labelSdWrapPatch);
            gbSdWrapInfo.Controls.Add(tbSdWrapMode);
            gbSdWrapInfo.Controls.Add(labelSdWrapMode);
            gbSdWrapInfo.Controls.Add(tbSdWrapEncryptConfigSize);
            gbSdWrapInfo.Controls.Add(labelSdWrapEncryptConfigSize);
            gbSdWrapInfo.Controls.Add(tbMainExeSize);
            gbSdWrapInfo.Controls.Add(labelMainExeSize);
            gbSdWrapInfo.Controls.Add(tbMainExeVersion);
            gbSdWrapInfo.Controls.Add(tbSdWrapConfigSize);
            gbSdWrapInfo.Controls.Add(tbSdWrapSize);
            gbSdWrapInfo.Controls.Add(tbSdWrapVersion);
            gbSdWrapInfo.Controls.Add(labelMainExeVersion);
            gbSdWrapInfo.Controls.Add(labelSdWrapConfigSize);
            gbSdWrapInfo.Controls.Add(labelSdWrapSize);
            gbSdWrapInfo.Controls.Add(labelVersion);
            gbSdWrapInfo.Location = new System.Drawing.Point(12, 50);
            gbSdWrapInfo.Name = "gbSdWrapInfo";
            gbSdWrapInfo.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            gbSdWrapInfo.Size = new System.Drawing.Size(760, 454);
            gbSdWrapInfo.TabIndex = 1;
            gbSdWrapInfo.TabStop = false;
            // 
            // lvSdWrapPatch
            // 
            lvSdWrapPatch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvSdWrapPatch.AutoArrange = false;
            lvSdWrapPatch.FullRowSelect = true;
            lvSdWrapPatch.GridLines = true;
            lvSdWrapPatch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lvSdWrapPatch.LabelWrap = false;
            lvSdWrapPatch.Location = new System.Drawing.Point(6, 128);
            lvSdWrapPatch.Name = "lvSdWrapPatch";
            lvSdWrapPatch.ShowGroups = false;
            lvSdWrapPatch.Size = new System.Drawing.Size(747, 319);
            lvSdWrapPatch.TabIndex = 16;
            lvSdWrapPatch.UseCompatibleStateImageBehavior = false;
            lvSdWrapPatch.View = System.Windows.Forms.View.Details;
            // 
            // labelSdWrapPatch
            // 
            labelSdWrapPatch.AutoSize = true;
            labelSdWrapPatch.Location = new System.Drawing.Point(6, 108);
            labelSdWrapPatch.Name = "labelSdWrapPatch";
            labelSdWrapPatch.Size = new System.Drawing.Size(56, 17);
            labelSdWrapPatch.TabIndex = 15;
            labelSdWrapPatch.Text = "加密区块";
            labelSdWrapPatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelSdWrapPatch.UseMnemonic = false;
            // 
            // tbSdWrapMode
            // 
            tbSdWrapMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbSdWrapMode.Location = new System.Drawing.Point(6, 82);
            tbSdWrapMode.MaxLength = 1024;
            tbSdWrapMode.Name = "tbSdWrapMode";
            tbSdWrapMode.ReadOnly = true;
            tbSdWrapMode.Size = new System.Drawing.Size(747, 23);
            tbSdWrapMode.TabIndex = 14;
            tbSdWrapMode.TabStop = false;
            // 
            // labelSdWrapMode
            // 
            labelSdWrapMode.AutoSize = true;
            labelSdWrapMode.Location = new System.Drawing.Point(6, 62);
            labelSdWrapMode.Name = "labelSdWrapMode";
            labelSdWrapMode.Size = new System.Drawing.Size(32, 17);
            labelSdWrapMode.TabIndex = 13;
            labelSdWrapMode.Text = "模式";
            labelSdWrapMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelSdWrapMode.UseMnemonic = false;
            // 
            // tbSdWrapEncryptConfigSize
            // 
            tbSdWrapEncryptConfigSize.Location = new System.Drawing.Point(234, 36);
            tbSdWrapEncryptConfigSize.MaxLength = 256;
            tbSdWrapEncryptConfigSize.Name = "tbSdWrapEncryptConfigSize";
            tbSdWrapEncryptConfigSize.ReadOnly = true;
            tbSdWrapEncryptConfigSize.Size = new System.Drawing.Size(108, 23);
            tbSdWrapEncryptConfigSize.TabIndex = 12;
            tbSdWrapEncryptConfigSize.TabStop = false;
            // 
            // labelSdWrapEncryptConfigSize
            // 
            labelSdWrapEncryptConfigSize.AutoSize = true;
            labelSdWrapEncryptConfigSize.Location = new System.Drawing.Point(234, 16);
            labelSdWrapEncryptConfigSize.Name = "labelSdWrapEncryptConfigSize";
            labelSdWrapEncryptConfigSize.Size = new System.Drawing.Size(80, 17);
            labelSdWrapEncryptConfigSize.TabIndex = 11;
            labelSdWrapEncryptConfigSize.Text = "加密配置大小";
            labelSdWrapEncryptConfigSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelSdWrapEncryptConfigSize.UseMnemonic = false;
            // 
            // tbMainExeSize
            // 
            tbMainExeSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbMainExeSize.Location = new System.Drawing.Point(645, 36);
            tbMainExeSize.MaxLength = 256;
            tbMainExeSize.Name = "tbMainExeSize";
            tbMainExeSize.ReadOnly = true;
            tbMainExeSize.Size = new System.Drawing.Size(108, 23);
            tbMainExeSize.TabIndex = 10;
            tbMainExeSize.TabStop = false;
            // 
            // labelMainExeSize
            // 
            labelMainExeSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelMainExeSize.AutoSize = true;
            labelMainExeSize.Location = new System.Drawing.Point(645, 16);
            labelMainExeSize.Name = "labelMainExeSize";
            labelMainExeSize.Size = new System.Drawing.Size(68, 17);
            labelMainExeSize.TabIndex = 9;
            labelMainExeSize.Text = "主程序大小";
            labelMainExeSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelMainExeSize.UseMnemonic = false;
            // 
            // tbMainExeVersion
            // 
            tbMainExeVersion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tbMainExeVersion.Location = new System.Drawing.Point(531, 36);
            tbMainExeVersion.MaxLength = 256;
            tbMainExeVersion.Name = "tbMainExeVersion";
            tbMainExeVersion.ReadOnly = true;
            tbMainExeVersion.Size = new System.Drawing.Size(108, 23);
            tbMainExeVersion.TabIndex = 8;
            tbMainExeVersion.TabStop = false;
            // 
            // tbSdWrapConfigSize
            // 
            tbSdWrapConfigSize.Location = new System.Drawing.Point(348, 36);
            tbSdWrapConfigSize.MaxLength = 256;
            tbSdWrapConfigSize.Name = "tbSdWrapConfigSize";
            tbSdWrapConfigSize.ReadOnly = true;
            tbSdWrapConfigSize.Size = new System.Drawing.Size(108, 23);
            tbSdWrapConfigSize.TabIndex = 7;
            tbSdWrapConfigSize.TabStop = false;
            // 
            // tbSdWrapSize
            // 
            tbSdWrapSize.Location = new System.Drawing.Point(120, 36);
            tbSdWrapSize.MaxLength = 256;
            tbSdWrapSize.Name = "tbSdWrapSize";
            tbSdWrapSize.ReadOnly = true;
            tbSdWrapSize.Size = new System.Drawing.Size(108, 23);
            tbSdWrapSize.TabIndex = 6;
            tbSdWrapSize.TabStop = false;
            // 
            // tbSdWrapVersion
            // 
            tbSdWrapVersion.Location = new System.Drawing.Point(6, 36);
            tbSdWrapVersion.MaxLength = 256;
            tbSdWrapVersion.Name = "tbSdWrapVersion";
            tbSdWrapVersion.ReadOnly = true;
            tbSdWrapVersion.Size = new System.Drawing.Size(108, 23);
            tbSdWrapVersion.TabIndex = 5;
            tbSdWrapVersion.TabStop = false;
            // 
            // labelMainExeVersion
            // 
            labelMainExeVersion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelMainExeVersion.AutoSize = true;
            labelMainExeVersion.Location = new System.Drawing.Point(531, 16);
            labelMainExeVersion.Name = "labelMainExeVersion";
            labelMainExeVersion.Size = new System.Drawing.Size(68, 17);
            labelMainExeVersion.TabIndex = 4;
            labelMainExeVersion.Text = "主程序版本";
            labelMainExeVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelMainExeVersion.UseMnemonic = false;
            // 
            // labelSdWrapConfigSize
            // 
            labelSdWrapConfigSize.AutoSize = true;
            labelSdWrapConfigSize.Location = new System.Drawing.Point(348, 16);
            labelSdWrapConfigSize.Name = "labelSdWrapConfigSize";
            labelSdWrapConfigSize.Size = new System.Drawing.Size(68, 17);
            labelSdWrapConfigSize.TabIndex = 3;
            labelSdWrapConfigSize.Text = "配置总大小";
            labelSdWrapConfigSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelSdWrapConfigSize.UseMnemonic = false;
            // 
            // labelSdWrapSize
            // 
            labelSdWrapSize.AutoSize = true;
            labelSdWrapSize.Location = new System.Drawing.Point(120, 16);
            labelSdWrapSize.Name = "labelSdWrapSize";
            labelSdWrapSize.Size = new System.Drawing.Size(44, 17);
            labelSdWrapSize.TabIndex = 1;
            labelSdWrapSize.Text = "壳大小";
            labelSdWrapSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelSdWrapSize.UseMnemonic = false;
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Location = new System.Drawing.Point(6, 16);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new System.Drawing.Size(32, 17);
            labelVersion.TabIndex = 0;
            labelVersion.Text = "版本";
            labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelVersion.UseMnemonic = false;
            // 
            // btnExtract
            // 
            btnExtract.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnExtract.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btnExtract.Location = new System.Drawing.Point(674, 510);
            btnExtract.Name = "btnExtract";
            btnExtract.Size = new System.Drawing.Size(98, 40);
            btnExtract.TabIndex = 2;
            btnExtract.Text = "提取";
            btnExtract.UseMnemonic = false;
            btnExtract.UseVisualStyleBackColor = true;
            btnExtract.Click += BtnExtract_Click;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 562);
            Controls.Add(btnExtract);
            Controls.Add(gbSdWrapInfo);
            Controls.Add(gbViewFile);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            MinimumSize = new System.Drawing.Size(800, 600);
            Name = "MainForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SdWrapless - 20241212";
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            gbViewFile.ResumeLayout(false);
            gbViewFile.PerformLayout();
            gbSdWrapInfo.ResumeLayout(false);
            gbSdWrapInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.TextBox tbMainExeVersion;
        private System.Windows.Forms.TextBox tbSdWrapConfigSize;
        private System.Windows.Forms.TextBox tbSdWrapSize;
        private System.Windows.Forms.TextBox tbSdWrapVersion;
        private System.Windows.Forms.TextBox tbMainExeSize;
        private System.Windows.Forms.TextBox tbSdWrapEncryptConfigSize;
        private System.Windows.Forms.TextBox tbSdWrapMode;
        private System.Windows.Forms.ListView lvSdWrapPatch;
        private System.Windows.Forms.Button btnExtract;
    }
}