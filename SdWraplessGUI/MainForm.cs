using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using SdWrapCore.SdWrap;


namespace SdWraplessGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //加密区块列表初始化
            {
                ListView lv = this.lvSdWrapPatch;
                lv.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(lv, true);

                ColumnHeader ch0 = new() { Name = "index", Text = string.Empty, Width = 50 };

                ColumnHeader ch1 = new() { Name = "offset", Text = "偏移", Width = 100, TextAlign = HorizontalAlignment.Center };
                ColumnHeader ch2 = new() { Name = "length", Text = "长度", Width = 100, TextAlign = HorizontalAlignment.Center };
                ColumnHeader ch3 = new() { Name = "sig1", Text = "标记1", Width = 100, TextAlign = HorizontalAlignment.Center };
                ColumnHeader ch4 = new() { Name = "sig2", Text = "标记2", Width = 100, TextAlign = HorizontalAlignment.Center };
                ColumnHeader ch5 = new() { Name = "reserve1", Text = "预留1", Width = 100, TextAlign = HorizontalAlignment.Center };
                ColumnHeader ch6 = new() { Name = "mode", Text = "模式", Width = 120, TextAlign = HorizontalAlignment.Center };

                lv.Columns.Add(ch0);

                lv.Columns.Add(ch1);
                lv.Columns.Add(ch2);
                lv.Columns.Add(ch3);
                lv.Columns.Add(ch4);
                lv.Columns.Add(ch5);
                lv.Columns.Add(ch6);
            }
        }

        private readonly SdWrapProgram mProgram = new();

        /// <summary>
        /// 修改SdWrap主程序路径
        /// </summary>
        /// <param name="filepath">SdWrap主程序路径</param>
        private void ProgramFilePathOnChanged(string filepath)
        {
            this.Clear();

            //加载SdWrap
            SdWrapProgram program = this.mProgram;
            if (!program.Load(filepath))
            {
                PopErrorMessage(program.LastError);
                return;
            }

            SdWrapStub stub = program.Stub!;

            //刷新文件信息
            {
                this.tbFilePath.Text = filepath;

                this.tbSdWrapVersion.Text = $"{stub.Level}";
                this.tbSdWrapSize.Text = $"{program.Size:X8}";
                this.tbSdWrapEncryptConfigSize.Text = $"{stub.Size:X8}";
                this.tbSdWrapConfigSize.Text = $"{stub.AlignSize:X8}";
                this.tbSdWrapMode.Text = $"{stub.Config.Mode}";

                this.tbMainExeVersion.Text = program.ExecutableVersion;
                this.tbMainExeSize.Text = $"{program.ExecutableSize:X8}";
            }

            //刷新补丁列表
            {
                ListView lv = this.lvSdWrapPatch;
                ListView.ColumnHeaderCollection cols = lv.Columns;

                lv.BeginUpdate();

                ReadOnlyCollection<SdWrapPatch> patches = stub.Patches;
                for (int i = 0; i < patches.Count; ++i)
                {
                    SdWrapPatch swp = patches[i];

                    //第0行由 ListViewItem.Text显示
                    ListViewItem.ListViewSubItem[] items = new ListViewItem.ListViewSubItem[cols.Count - 1];

                    items[cols["offset"].Index - 1] = new() { Text = $"{swp.Position:X8}" };
                    items[cols["length"].Index - 1] = new() { Text = $"{swp.Length:X8}" };
                    items[cols["sig1"].Index - 1] = new() { Text = $"{swp.Signature1:X8}" };
                    items[cols["sig2"].Index - 1] = new() { Text = $"{swp.Signature2:X8}" };
                    items[cols["reserve1"].Index - 1] = new() { Text = $"{swp.Reserve1:X8}" };
                    items[cols["mode"].Index - 1] = new() { Text = $"{swp.Mode}" };

                    Color bkColor = swp.Mode switch
                    {
                        SdWrapPatchFlags.ExecutableOnly => Color.FromArgb(189, 255, 198),
                        SdWrapPatchFlags.File => Color.FromArgb(255, 180, 176),
                        SdWrapPatchFlags.Memory => Color.FromArgb(186, 217, 255),
                        _ => Color.FromArgb(239, 239, 239),
                    };

                    ListViewItem lvi = new() { BackColor = bkColor, UseItemStyleForSubItems = true, Text = $"{i}" };
                    lvi.SubItems.AddRange(items);

                    lv.Items.Add(lvi);
                }

                lv.EndUpdate();
            }

            //刷新界面功能
            {
                this.btnExtract.Enabled = true;
            }
        }

        /// <summary>
        /// 清除界面信息
        /// </summary>
        private void Clear()
        {
            this.tbFilePath.Clear();

            this.tbSdWrapVersion.Clear();
            this.tbSdWrapSize.Clear();
            this.tbSdWrapEncryptConfigSize.Clear();
            this.tbSdWrapConfigSize.Clear();
            this.tbSdWrapMode.Clear();

            this.tbMainExeVersion.Clear();
            this.tbMainExeSize.Clear();

            this.lvSdWrapPatch.Items.Clear();

            this.btnExtract.Enabled = false;
        }

        //选择文件
        private void BtnSelectFile_Click(object sender, System.EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".exe",
                Filter = "可执行程序(*.exe)|*.exe|所有文件(*.*)|*.*",
                Multiselect = false,
                RestoreDirectory = true,
                ShowHelp = false,
                Title = "选择SdWrap主程序",
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.ProgramFilePathOnChanged(ofd.FileName);
            }
        }

        //提取文件
        private void BtnExtract_Click(object sender, System.EventArgs e)
        {
            SdWrapProgram program = this.mProgram;
            if (!program.Extract())
            {
                PopErrorMessage(program.LastError);
                return;
            }
            MessageBox.Show("提取成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //响应拖拽
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //获取拖拽数据
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is null)
            {
                return;
            }

            if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Any())
            {
                this.ProgramFilePathOnChanged(files[0]);
            }
        }

        /// <summary>
        /// 弹出错误消息框
        /// </summary>
        /// <param name="text"></param>
        private static void PopErrorMessage(string text)
        {
            MessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}