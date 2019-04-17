namespace LaneDetection_v201810
{
    partial class mainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resultImageBox = new Emgu.CV.UI.ImageBox();
            this.bianaryImageBox = new Emgu.CV.UI.ImageBox();
            this.affineAndFittedImageBox = new Emgu.CV.UI.ImageBox();
            this.withTempAndSingleImageBox = new Emgu.CV.UI.ImageBox();
            this.colorSpaceImageBox = new Emgu.CV.UI.ImageBox();
            this.srcImageBox = new Emgu.CV.UI.ImageBox();
            this.pictureTestButton = new System.Windows.Forms.Button();
            this.videoTestButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.captureTestButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.laneWidthtextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.deflectionDirectiontextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.deflectiontextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.offsetDirectiontextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.offsettextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.detectionSpeedtextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bianaryImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.affineAndFittedImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.withTempAndSingleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorSpaceImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcImageBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.resultImageBox);
            this.groupBox1.Controls.Add(this.bianaryImageBox);
            this.groupBox1.Controls.Add(this.affineAndFittedImageBox);
            this.groupBox1.Controls.Add(this.withTempAndSingleImageBox);
            this.groupBox1.Controls.Add(this.colorSpaceImageBox);
            this.groupBox1.Controls.Add(this.srcImageBox);
            this.groupBox1.Location = new System.Drawing.Point(17, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(995, 519);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "显示区域";
            // 
            // resultImageBox
            // 
            this.resultImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.resultImageBox.Location = new System.Drawing.Point(663, 266);
            this.resultImageBox.Name = "resultImageBox";
            this.resultImageBox.Size = new System.Drawing.Size(320, 240);
            this.resultImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultImageBox.TabIndex = 4;
            this.resultImageBox.TabStop = false;
            // 
            // bianaryImageBox
            // 
            this.bianaryImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.bianaryImageBox.Location = new System.Drawing.Point(663, 20);
            this.bianaryImageBox.Name = "bianaryImageBox";
            this.bianaryImageBox.Size = new System.Drawing.Size(320, 240);
            this.bianaryImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bianaryImageBox.TabIndex = 3;
            this.bianaryImageBox.TabStop = false;
            // 
            // affineAndFittedImageBox
            // 
            this.affineAndFittedImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.affineAndFittedImageBox.Location = new System.Drawing.Point(337, 266);
            this.affineAndFittedImageBox.Name = "affineAndFittedImageBox";
            this.affineAndFittedImageBox.Size = new System.Drawing.Size(320, 240);
            this.affineAndFittedImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.affineAndFittedImageBox.TabIndex = 2;
            this.affineAndFittedImageBox.TabStop = false;
            // 
            // withTempAndSingleImageBox
            // 
            this.withTempAndSingleImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.withTempAndSingleImageBox.Location = new System.Drawing.Point(11, 266);
            this.withTempAndSingleImageBox.Name = "withTempAndSingleImageBox";
            this.withTempAndSingleImageBox.Size = new System.Drawing.Size(320, 240);
            this.withTempAndSingleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.withTempAndSingleImageBox.TabIndex = 2;
            this.withTempAndSingleImageBox.TabStop = false;
            // 
            // colorSpaceImageBox
            // 
            this.colorSpaceImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.colorSpaceImageBox.Location = new System.Drawing.Point(337, 20);
            this.colorSpaceImageBox.Name = "colorSpaceImageBox";
            this.colorSpaceImageBox.Size = new System.Drawing.Size(320, 240);
            this.colorSpaceImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.colorSpaceImageBox.TabIndex = 2;
            this.colorSpaceImageBox.TabStop = false;
            // 
            // srcImageBox
            // 
            this.srcImageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.srcImageBox.Location = new System.Drawing.Point(11, 20);
            this.srcImageBox.Name = "srcImageBox";
            this.srcImageBox.Size = new System.Drawing.Size(320, 240);
            this.srcImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.srcImageBox.TabIndex = 2;
            this.srcImageBox.TabStop = false;
            // 
            // pictureTestButton
            // 
            this.pictureTestButton.Location = new System.Drawing.Point(89, 57);
            this.pictureTestButton.Name = "pictureTestButton";
            this.pictureTestButton.Size = new System.Drawing.Size(75, 23);
            this.pictureTestButton.TabIndex = 1;
            this.pictureTestButton.Text = "图片测试";
            this.pictureTestButton.UseVisualStyleBackColor = true;
            this.pictureTestButton.Click += new System.EventHandler(this.pictureTestButton_Click);
            // 
            // videoTestButton
            // 
            this.videoTestButton.Location = new System.Drawing.Point(170, 57);
            this.videoTestButton.Name = "videoTestButton";
            this.videoTestButton.Size = new System.Drawing.Size(75, 23);
            this.videoTestButton.TabIndex = 2;
            this.videoTestButton.Text = "视频测试";
            this.videoTestButton.UseVisualStyleBackColor = true;
            this.videoTestButton.Click += new System.EventHandler(this.videoTestButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(332, 57);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 3;
            this.quitButton.Text = "安全退出";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // captureTestButton
            // 
            this.captureTestButton.Location = new System.Drawing.Point(251, 57);
            this.captureTestButton.Name = "captureTestButton";
            this.captureTestButton.Size = new System.Drawing.Size(75, 23);
            this.captureTestButton.TabIndex = 4;
            this.captureTestButton.Text = "摄像头测试";
            this.captureTestButton.UseVisualStyleBackColor = true;
            this.captureTestButton.Click += new System.EventHandler(this.captureTestButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.laneWidthtextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.deflectionDirectiontextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.deflectiontextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.offsetDirectiontextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.offsettextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.detectionSpeedtextBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(510, 537);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(489, 126);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "行驶状态";
            // 
            // laneWidthtextBox
            // 
            this.laneWidthtextBox.Location = new System.Drawing.Point(173, 24);
            this.laneWidthtextBox.Name = "laneWidthtextBox";
            this.laneWidthtextBox.ReadOnly = true;
            this.laneWidthtextBox.Size = new System.Drawing.Size(58, 21);
            this.laneWidthtextBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "检测速度（ms）：";
            // 
            // deflectionDirectiontextBox
            // 
            this.deflectionDirectiontextBox.Location = new System.Drawing.Point(387, 93);
            this.deflectionDirectiontextBox.Name = "deflectionDirectiontextBox";
            this.deflectionDirectiontextBox.ReadOnly = true;
            this.deflectionDirectiontextBox.Size = new System.Drawing.Size(58, 21);
            this.deflectionDirectiontextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "偏转方向：";
            // 
            // deflectiontextBox
            // 
            this.deflectiontextBox.Location = new System.Drawing.Point(173, 93);
            this.deflectiontextBox.Name = "deflectiontextBox";
            this.deflectiontextBox.ReadOnly = true;
            this.deflectiontextBox.Size = new System.Drawing.Size(58, 21);
            this.deflectiontextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "偏转角度（度）：";
            // 
            // offsetDirectiontextBox
            // 
            this.offsetDirectiontextBox.Location = new System.Drawing.Point(387, 58);
            this.offsetDirectiontextBox.Name = "offsetDirectiontextBox";
            this.offsetDirectiontextBox.ReadOnly = true;
            this.offsetDirectiontextBox.Size = new System.Drawing.Size(58, 21);
            this.offsetDirectiontextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "偏离车道线方向：";
            // 
            // offsettextBox
            // 
            this.offsettextBox.Location = new System.Drawing.Point(173, 58);
            this.offsettextBox.Name = "offsettextBox";
            this.offsettextBox.ReadOnly = true;
            this.offsettextBox.Size = new System.Drawing.Size(58, 21);
            this.offsettextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "偏离车道线中心距离（m）：";
            // 
            // detectionSpeedtextBox
            // 
            this.detectionSpeedtextBox.Location = new System.Drawing.Point(387, 24);
            this.detectionSpeedtextBox.Name = "detectionSpeedtextBox";
            this.detectionSpeedtextBox.ReadOnly = true;
            this.detectionSpeedtextBox.Size = new System.Drawing.Size(58, 21);
            this.detectionSpeedtextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "车道宽度（m）：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.quitButton);
            this.groupBox3.Controls.Add(this.captureTestButton);
            this.groupBox3.Controls.Add(this.videoTestButton);
            this.groupBox3.Controls.Add(this.pictureTestButton);
            this.groupBox3.Location = new System.Drawing.Point(17, 537);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 126);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "系统控制";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 675);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "mainForm";
            this.Text = "LaneDetection_Demo_1";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bianaryImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.affineAndFittedImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.withTempAndSingleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorSpaceImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcImageBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Emgu.CV.UI.ImageBox resultImageBox;
        private Emgu.CV.UI.ImageBox bianaryImageBox;
        private Emgu.CV.UI.ImageBox affineAndFittedImageBox;
        private Emgu.CV.UI.ImageBox withTempAndSingleImageBox;
        private Emgu.CV.UI.ImageBox colorSpaceImageBox;
        private Emgu.CV.UI.ImageBox srcImageBox;
        private System.Windows.Forms.Button pictureTestButton;
        private System.Windows.Forms.Button videoTestButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Button captureTestButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox laneWidthtextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox deflectionDirectiontextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox deflectiontextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox offsetDirectiontextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox offsettextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox detectionSpeedtextBox;
        private System.Windows.Forms.Label label1;
    }
}

