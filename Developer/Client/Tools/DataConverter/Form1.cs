using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataConverter
{
    public partial class Form1 : Form
    {
        enum EType
        {
            Csv,
            Xml,
            Json,
            Lua,
            Binary,
            Count,
        }
        enum ECode
        {
            UTF8,
            CB2312,
            Count,
        }
        public class ListItem
        {
            private object _Value;
            public object Value
            {
                get { return _Value; }
            }

            private String _Display;
            public String Display
            {
                get
                {
                    if (MulSheed) return _Display + "[多表格]";
                    else return _Display;
                }
            }

            public String tableName
            {
                get
                {
                    return _Display;
                }
            }

            private bool _MulSheed;
            public bool MulSheed
            {
                get { return _MulSheed; }
                set { _MulSheed = value; }
            }

            public ListItem(object value, String display)
            {
                _Value = value;
                _Display = display;
            }
        }

        IniParser m_pIniParser;
        string m_strSrcDir="";
        string m_strDestDir="";
        string m_strDestSvrDir = "";
        public Form1(string[] args)
        {
            InitializeComponent();

            LoadConfig("Config.ini");
            {
                List<ListItem> listListItem = new List<ListItem>();
                for (int i = 0; i < (int)EType.Count; ++i)
                    listListItem.Add(new ListItem((EType)i, ((EType)i).ToString()));

                this.Type.DataSource = listListItem;
                this.Type.DisplayMember = "Display";
                this.Type.ValueMember = "Value";

                this.Type.SelectedItem = listListItem[(int)EType.Csv];
            }

            {
                List<ListItem> listListItem = new List<ListItem>();
                for (int i = 0; i < (int)ECode.Count; ++i)
                    listListItem.Add(new ListItem((ECode)i, ((ECode)i).ToString()));

                this.Code.DataSource = listListItem;
                this.Code.DisplayMember = "Display";
                this.Code.ValueMember = "Value";
            }

            if (args!=null && args.Length >= 2)
            {
                m_strSrcDir = args[0];
                m_strDestDir = args[1];
                EType type = EType.Csv;
                if (args.Length == 3 )
                {
                    type = (EType)Enum.Parse(typeof(EType), args[2]);
                }

                RefreshSrcDataList();
                Export(m_strDestDir, true, type);

                System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
                myTimer.Tick += new EventHandler(Callback);
                myTimer.Enabled = true;
                int interval = 100;
                if (args.Length == 4 && int.TryParse(args[3], out interval))
                    myTimer.Interval = interval;
                else
                    myTimer.Interval = 100;
                return;
            }
        }
        private void Callback(object sender, EventArgs e)
        {
            this.Close();
        } 

        void LoadConfig(string config)
        {
            m_pIniParser = new IniParser(config);
            m_strSrcDir = m_pIniParser.readStringData("conf", "src");

            m_strDestDir = m_pIniParser.readStringData("conf", "dest-" + (EType.Csv).ToString());
            m_strDestSvrDir = m_pIniParser.readStringData("conf", "svrdest-" + (EType.Csv).ToString());

            if (string.IsNullOrEmpty(m_strSrcDir))
                m_strSrcDir = "";
            SRC_DIR.Text = m_strSrcDir;
            DEST_SRC.Text = m_strDestDir;
            SVRDST_SRC.Text = m_strDestSvrDir;

            RefreshSrcDataList();
        }

        private void CONVERT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_strDestDir))
            {
                MessageBox.Show("请先选择导出目录!");
                return;
            }
            bool All = EXPORT_ALL.Checked;
            if (!All && (this.SRC_LIST.SelectedItems ==null || this.SRC_LIST.SelectedItems.Count<=0))
            {
                MessageBox.Show("请先选择导出的文件!");
                return;
            }
            Export(m_strDestDir,All);
        }
        private void CONVERT_SVR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_strDestSvrDir))
            {
                MessageBox.Show("请先选择导出目录!");
                return;
            }
            bool All = EXPORT_ALL.Checked;
            if (!All && (this.SRC_LIST.SelectedItems == null || this.SRC_LIST.SelectedItems.Count <= 0))
            {
                MessageBox.Show("请先选择导出的文件!");
                return;
            }
            Export(m_strDestSvrDir, All);
        }

        void Export(string output,bool bAll, EType type = EType.Count)
        {
            Encoding code = GetCurCode();
            if(type == EType.Count) type = (EType)((ListItem)Type.SelectedItem).Value;
            List<ListItem> vItem = new List<ListItem>();
            if (bAll)
            {
                for (int i = 0; i < this.SRC_LIST.Items.Count; ++i)
                {
                    vItem.Add(((ListItem)this.SRC_LIST.Items[i]));
                }
            }
            else
            {
                for (int i = 0; i < this.SRC_LIST.SelectedItems.Count; ++i)
                {
                    vItem.Add(((ListItem)this.SRC_LIST.SelectedItems[i]));
                }
            }
            for (int i = 0; i < vItem.Count; ++i)
            {
                ListItem item = vItem[i];
                string path = (string)item.Value;
                ExcelUtility excel = new ExcelUtility(path);
                if (output[output.Length - 1] != '/')
                    output += "/";

                string strFile = "";
                if (type == EType.Csv)
                {
                    strFile = output + item.tableName + ".csv";
                    excel.ConvertToCSV(strFile, code, item.MulSheed);
                }
                else if (type == EType.Json)
                {
                    strFile = output + item.tableName + ".json";
                    excel.ConvertToJson(strFile, code);
                }
                else if (type == EType.Lua)
                {
                    strFile = output + item.tableName + ".lua";
                    excel.ConvertToLua(strFile, code);
                }
                else if (type == EType.Xml)
                {
                    strFile = output + item.tableName + ".xml";
                    excel.ConvertToXml(strFile);
                }
                else if (type == EType.Binary)
                {
                    strFile = output + item.tableName + ".bytes";
                    excel.ConvertToBinary(strFile);
                }

                this.LOG.Items.Add(strFile + "-->" + "转化成功!!");
            }
        }

        private void Code_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_strDestDir = m_pIniParser.readStringData("conf", GetCurTypeKey());
            m_strDestSvrDir = m_pIniParser.readStringData("conf", GetCurTypeKey("svr"));
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SRC_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public Encoding GetCurCode()
        {
            ECode code = (ECode)((ListItem)Code.SelectedItem).Value;
            if(code == ECode.CB2312) return Encoding.GetEncoding("gb2312");
            return Encoding.UTF8;
        }

        public string GetCurTypeKey(string label="")
        {
            return label + "dest-" + ((EType)((ListItem)Type.SelectedItem).Value).ToString();
        }

        private void SCR_DIR_SELECT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.SelectedPath = Directory.GetCurrentDirectory();
            path.ShowDialog();
            m_strSrcDir = path.SelectedPath.Replace("\\","/");
            SRC_DIR.Text = m_strSrcDir;
            m_pIniParser.writeData("conf", "src", m_strSrcDir);
        }

        void RefreshSrcDataList()
        {
            if (string.IsNullOrEmpty(m_strSrcDir)) return;
            List<string> vFiles = new List<string>();
            GetFileList(m_strSrcDir, ref vFiles, false);
            this.SRC_LIST.ClearSelected();

         //   DataTable srcListData = ((DataTable)this.SRC_LIST.DataSource);
            List<ListItem> listListItem = new List<ListItem>();
            for (int i =0; i < vFiles.Count; ++i)
            {
                listListItem.Add( new ListItem(vFiles[i], Path.GetFileNameWithoutExtension(vFiles[i])));
            }
            this.SRC_LIST.DataSource = listListItem;
            this.SRC_LIST.ValueMember = "Value";
            this.SRC_LIST.DisplayMember = "Display";
        }

        private void DEST_DIR_SELECT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.SelectedPath = Directory.GetCurrentDirectory();
            path.ShowDialog();
            m_strDestDir = path.SelectedPath.Replace("\\", "/");
            DEST_SRC.Text = m_strDestDir;
            m_pIniParser.writeData("conf", GetCurTypeKey(), m_strDestDir);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //------------------------------------------------------
        void GetFileList(string basePath, ref List<string> fileInfoList, bool bIncludeSub = true)
        {
            if (string.IsNullOrEmpty(basePath))
                return;

            if (!Directory.Exists(basePath))
                return;

            if (fileInfoList == null)
            {
                fileInfoList = new List<string>();
            }

            basePath = basePath.ToLower();
            basePath = basePath.Replace("\\", "/");
            if (basePath[basePath.Length - 1] == '/')
                basePath = basePath.Substring(0, basePath.Length - 1);

            var curDirectoryInfo = new DirectoryInfo(basePath);

            var FilesInfo = curDirectoryInfo.GetFiles();

            foreach (var info in FilesInfo)
            {
                if (!info.FullName.EndsWith(".xlsx") || info.FullName.Contains("~$")) continue;
                fileInfoList.Add(info.FullName);
            }

            if(bIncludeSub)
            {
                var childrenDirectories = curDirectoryInfo.GetDirectories();

                for (int i = 0; i < childrenDirectories.Length; ++i)
                {
                    var directory = childrenDirectories[i];

                    string childPath = string.Format("{0}/{1}", basePath, directory.Name.ToLower());
                    GetFileList(childPath, ref fileInfoList);
                }
            }

        }

        private void RefreshList_Click(object sender, EventArgs e)
        {
            RefreshSrcDataList();
        }

        private void SRC_LIST_DoubleClick(object sender, EventArgs e)
        {
            ListItem item = ((ListItem)SRC_LIST.SelectedItem);
            item.MulSheed = !item.MulSheed;

            SRC_LIST.Refresh();
        }
        private Color RowBackColorAlt = Color.FromArgb(200, 200, 200);//交替色 
        private Color RowBackColorSel = Color.FromArgb(150, 200, 250);//选择项目颜色 
        private void SRC_LIST_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(RowBackColorSel);
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }
            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();//焦点框 

            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            ListItem item = (ListItem)SRC_LIST.Items[e.Index];
            e.Graphics.DrawString(item.Display, e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
        }

        private void SVR_DEST_DIR_SELECT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.SelectedPath = Directory.GetCurrentDirectory();
            path.ShowDialog();
            m_strDestSvrDir = path.SelectedPath.Replace("\\", "/");
            SVRDST_SRC.Text = m_strDestSvrDir;
            m_pIniParser.writeData("conf", GetCurTypeKey("svr"), m_strDestSvrDir);
        }
    }
}
