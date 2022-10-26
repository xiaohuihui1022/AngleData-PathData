using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AngleDataToPathData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        Lib lib = new Lib();
        Thread main;
        bool isFirst = true;
        string AdofaiSpectralFile = "";
        public string OpenFile()
        {
            string strFileName = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Adofai谱面文件(*.adofai)|*.adofai";
            ofd.ValidateNames = true; // 验证用户输入是否是一个有效的Windows文件名
            ofd.CheckFileExists = true; // 验证路径的有效性
            ofd.CheckPathExists = true;// 验证路径的有效性
            if (ofd.ShowDialog() == DialogResult.OK) // 用户点击确认按钮，发送确认消息
            {
                strFileName = ofd.FileName;// 获取在文件对话框中选定的路径或者字符串
            }
            return strFileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 打开文件
            AdofaiSpectralFile = OpenFile();
            main = new Thread(MainThread);
            main.Start();
        }

        private void MainThread()
        {
            if (AdofaiSpectralFile == "")
            {
                status.Text = "打开文件失败";
                return;
            }
            status.Text = "已获取谱面文件" + Path.GetFileName(AdofaiSpectralFile) + ".";
            StreamReader file = File.OpenText(AdofaiSpectralFile);
            if (isFirst)
            {
                // 初始化map
                lib.MapSet();
                isFirst = false;
            }
            // String2Json
            try
            {
                JObject Adofai = lib.AdofaiParse(file.ReadToEnd());
                file.Close();
                if (isBackup.Checked)
                {
                    File.Copy(AdofaiSpectralFile, @".\backup_" + Path.GetFileName(AdofaiSpectralFile));
                }
                if (!Adofai.ContainsKey("angleData"))
                {
                    status.Text = "此铺面没有angleData类";
                    Application.Exit();
                }

                string pathData = "";
                string lastPath = "";
                foreach (int a in Adofai["angleData"])
                {
                    status.Text = "正在处理" + Path.GetFileName(AdofaiSpectralFile) + ".";
                    string path = "";
                    if (a == 360)
                    {
                        path = lib.FlipPath(lastPath);
                    }
                    else if (a < 0)
                    {
                        path = lib.Map[360 + a].ToString();
                    }
                    else
                    {
                        path = lib.Map[a].ToString();
                    }
                    status.Text = "正在处理" + Path.GetFileName(AdofaiSpectralFile) + "..";
                    lastPath = path;
                    pathData += path;
                    status.Text = "正在处理" + Path.GetFileName(AdofaiSpectralFile) + "...";
                }
                Adofai.Remove("angleData");
                Adofai.Add("pathData", pathData);
                File.WriteAllText(AdofaiSpectralFile, JsonConvert.SerializeObject(Adofai, Formatting.Indented));
                status.Text = "已处理完成";
            }
            catch (Exception e)
            {
                file.Close();
                status.Text = "发生了意料之外的错误，可能是您的谱面文件格式有误\n" +
                    "请打开json.cn网站并且将谱面文件内容复制进去进行检验";
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            
        }
    }
}
