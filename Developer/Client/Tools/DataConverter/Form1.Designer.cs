namespace DataConverter
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
            this.LOG = new System.Windows.Forms.ListBox();
            this.Type = new System.Windows.Forms.ComboBox();
            this.Code = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SRC_LIST = new System.Windows.Forms.ListBox();
            this.CONVERT = new System.Windows.Forms.Button();
            this.SRC_DIR = new System.Windows.Forms.TextBox();
            this.DEST_SRC = new System.Windows.Forms.TextBox();
            this.SCR_DIR_SELECT = new System.Windows.Forms.Button();
            this.DEST_DIR_SELECT = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RefreshList = new System.Windows.Forms.Button();
            this.EXPORT_ALL = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SVR_DEST_DIR_SELECT = new System.Windows.Forms.Button();
            this.SVRDST_SRC = new System.Windows.Forms.TextBox();
            this.CONVERT_SVR = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LOG
            // 
            this.LOG.FormattingEnabled = true;
            this.LOG.ItemHeight = 12;
            this.LOG.Location = new System.Drawing.Point(8, 403);
            this.LOG.Name = "LOG";
            this.LOG.Size = new System.Drawing.Size(408, 124);
            this.LOG.TabIndex = 0;
            // 
            // Type
            // 
            this.Type.FormattingEnabled = true;
            this.Type.Location = new System.Drawing.Point(94, 97);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(75, 20);
            this.Type.TabIndex = 1;
            this.Type.SelectedIndexChanged += new System.EventHandler(this.Type_SelectedIndexChanged);
            // 
            // Code
            // 
            this.Code.FormattingEnabled = true;
            this.Code.Location = new System.Drawing.Point(250, 97);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(78, 20);
            this.Code.TabIndex = 2;
            this.Code.SelectedIndexChanged += new System.EventHandler(this.Code_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "格式类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "编码类型：";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(2, 390);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 141);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LOG";
            // 
            // SRC_LIST
            // 
            this.SRC_LIST.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.SRC_LIST.FormattingEnabled = true;
            this.SRC_LIST.ItemHeight = 12;
            this.SRC_LIST.Location = new System.Drawing.Point(12, 127);
            this.SRC_LIST.Name = "SRC_LIST";
            this.SRC_LIST.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.SRC_LIST.Size = new System.Drawing.Size(404, 232);
            this.SRC_LIST.TabIndex = 8;
            this.SRC_LIST.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SRC_LIST_DrawItem);
            this.SRC_LIST.SelectedIndexChanged += new System.EventHandler(this.SRC_LIST_SelectedIndexChanged);
            this.SRC_LIST.DoubleClick += new System.EventHandler(this.SRC_LIST_DoubleClick);
            // 
            // CONVERT
            // 
            this.CONVERT.Location = new System.Drawing.Point(231, 362);
            this.CONVERT.Name = "CONVERT";
            this.CONVERT.Size = new System.Drawing.Size(93, 23);
            this.CONVERT.TabIndex = 9;
            this.CONVERT.Text = "客户端导出";
            this.CONVERT.UseVisualStyleBackColor = true;
            this.CONVERT.Click += new System.EventHandler(this.CONVERT_Click);
            // 
            // SRC_DIR
            // 
            this.SRC_DIR.Enabled = false;
            this.SRC_DIR.Location = new System.Drawing.Point(77, 12);
            this.SRC_DIR.Name = "SRC_DIR";
            this.SRC_DIR.Size = new System.Drawing.Size(309, 21);
            this.SRC_DIR.TabIndex = 10;
            this.SRC_DIR.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DEST_SRC
            // 
            this.DEST_SRC.Enabled = false;
            this.DEST_SRC.Location = new System.Drawing.Point(113, 43);
            this.DEST_SRC.Name = "DEST_SRC";
            this.DEST_SRC.Size = new System.Drawing.Size(273, 21);
            this.DEST_SRC.TabIndex = 11;
            this.DEST_SRC.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // SCR_DIR_SELECT
            // 
            this.SCR_DIR_SELECT.Location = new System.Drawing.Point(389, 11);
            this.SCR_DIR_SELECT.Name = "SCR_DIR_SELECT";
            this.SCR_DIR_SELECT.Size = new System.Drawing.Size(35, 23);
            this.SCR_DIR_SELECT.TabIndex = 12;
            this.SCR_DIR_SELECT.Text = "...";
            this.SCR_DIR_SELECT.UseVisualStyleBackColor = true;
            this.SCR_DIR_SELECT.Click += new System.EventHandler(this.SCR_DIR_SELECT_Click);
            // 
            // DEST_DIR_SELECT
            // 
            this.DEST_DIR_SELECT.Location = new System.Drawing.Point(392, 40);
            this.DEST_DIR_SELECT.Name = "DEST_DIR_SELECT";
            this.DEST_DIR_SELECT.Size = new System.Drawing.Size(35, 23);
            this.DEST_DIR_SELECT.TabIndex = 13;
            this.DEST_DIR_SELECT.Text = "...";
            this.DEST_DIR_SELECT.UseVisualStyleBackColor = true;
            this.DEST_DIR_SELECT.Click += new System.EventHandler(this.DEST_DIR_SELECT_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "客户端导出目录：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "源目录  ：";
            // 
            // RefreshList
            // 
            this.RefreshList.Location = new System.Drawing.Point(25, 362);
            this.RefreshList.Name = "RefreshList";
            this.RefreshList.Size = new System.Drawing.Size(117, 23);
            this.RefreshList.TabIndex = 16;
            this.RefreshList.Text = "刷新列表";
            this.RefreshList.UseVisualStyleBackColor = true;
            this.RefreshList.Click += new System.EventHandler(this.RefreshList_Click);
            // 
            // EXPORT_ALL
            // 
            this.EXPORT_ALL.AutoSize = true;
            this.EXPORT_ALL.Location = new System.Drawing.Point(156, 366);
            this.EXPORT_ALL.Name = "EXPORT_ALL";
            this.EXPORT_ALL.Size = new System.Drawing.Size(72, 16);
            this.EXPORT_ALL.TabIndex = 17;
            this.EXPORT_ALL.Text = "全部导出";
            this.EXPORT_ALL.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "服务端导出目录：";
            // 
            // SVR_DEST_DIR_SELECT
            // 
            this.SVR_DEST_DIR_SELECT.Location = new System.Drawing.Point(392, 68);
            this.SVR_DEST_DIR_SELECT.Name = "SVR_DEST_DIR_SELECT";
            this.SVR_DEST_DIR_SELECT.Size = new System.Drawing.Size(35, 23);
            this.SVR_DEST_DIR_SELECT.TabIndex = 19;
            this.SVR_DEST_DIR_SELECT.Text = "...";
            this.SVR_DEST_DIR_SELECT.UseVisualStyleBackColor = true;
            this.SVR_DEST_DIR_SELECT.Click += new System.EventHandler(this.SVR_DEST_DIR_SELECT_Click);
            // 
            // SVRDST_SRC
            // 
            this.SVRDST_SRC.Enabled = false;
            this.SVRDST_SRC.Location = new System.Drawing.Point(113, 70);
            this.SVRDST_SRC.Name = "SVRDST_SRC";
            this.SVRDST_SRC.Size = new System.Drawing.Size(273, 21);
            this.SVRDST_SRC.TabIndex = 18;
            // 
            // CONVERT_SVR
            // 
            this.CONVERT_SVR.Location = new System.Drawing.Point(329, 362);
            this.CONVERT_SVR.Name = "CONVERT_SVR";
            this.CONVERT_SVR.Size = new System.Drawing.Size(93, 23);
            this.CONVERT_SVR.TabIndex = 21;
            this.CONVERT_SVR.Text = "服务端导出";
            this.CONVERT_SVR.UseVisualStyleBackColor = true;
            this.CONVERT_SVR.Click += new System.EventHandler(this.CONVERT_SVR_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 541);
            this.Controls.Add(this.CONVERT_SVR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SVR_DEST_DIR_SELECT);
            this.Controls.Add(this.SVRDST_SRC);
            this.Controls.Add(this.EXPORT_ALL);
            this.Controls.Add(this.RefreshList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DEST_DIR_SELECT);
            this.Controls.Add(this.SCR_DIR_SELECT);
            this.Controls.Add(this.DEST_SRC);
            this.Controls.Add(this.SRC_DIR);
            this.Controls.Add(this.CONVERT);
            this.Controls.Add(this.SRC_LIST);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.LOG);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 580);
            this.MinimumSize = new System.Drawing.Size(450, 580);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LOG;
        private System.Windows.Forms.ComboBox Type;
        private System.Windows.Forms.ComboBox Code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox SRC_LIST;
        private System.Windows.Forms.Button CONVERT;
        private System.Windows.Forms.TextBox SRC_DIR;
        private System.Windows.Forms.TextBox DEST_SRC;
        private System.Windows.Forms.Button SCR_DIR_SELECT;
        private System.Windows.Forms.Button DEST_DIR_SELECT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button RefreshList;
        private System.Windows.Forms.CheckBox EXPORT_ALL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button SVR_DEST_DIR_SELECT;
        private System.Windows.Forms.TextBox SVRDST_SRC;
        private System.Windows.Forms.Button CONVERT_SVR;
    }
}

