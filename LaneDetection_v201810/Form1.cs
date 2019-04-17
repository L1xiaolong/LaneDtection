using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;
using Emgu.CV;
using LAD;

namespace LaneDetection_v201810
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }
        private VideoCapture cap = null;
        int fps;
        public Mat frame;
        lad Lad = new lad();

        private void quitButton_Click(object sender, EventArgs e)      // 安全退出
        {
            Application.Exit();
        }
        private void pictureTestButton_Click(object sender, EventArgs e)        // 图片测试
        {
            bool flag = Lad.init();
            if (flag)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();          // 弹出文件夹选择图片
                openFileDialog.Filter = "JPG文件|*.jpg|BMP文件|*.bmp|PNG文件|*.png|所有文件(*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DateTime nowtime1 = System.DateTime.Now;

                    string fName = openFileDialog.FileName;

                    frame = CvInvoke.Imread(fName);             // 原图
                    process(frame);

                    DateTime nowtime2 = System.DateTime.Now;                       // 计算程序运行速度
                    TimeSpan ts = nowtime2.Subtract(nowtime1);
                    double time = ts.TotalMilliseconds;
                    detectionSpeedtextBox.Text = time.ToString("0.0");
                }
            }
            
        }
        private void videoTestButton_Click(object sender, EventArgs e)
        {
            bool flag = Lad.init();
            if(flag)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "AVI文件|*.avi|MP4文件|*.mp4|所有文件|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Application.Idle += Application_Idle;
                    cap = new VideoCapture(openFileDialog.FileName);
                    fps = (int)cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                }
            }          
        }
        private void Application_Idle(object sender, EventArgs e)
        {
            Mat frame = cap.QueryFrame();
            if (frame != null)
            {
                System.Threading.Thread.Sleep((int)(1000.0 / fps - 5));

                DateTime nowtime1 = System.DateTime.Now;

                process(frame);

                DateTime nowtime2 = System.DateTime.Now;                       // 计算程序运行速度
                TimeSpan ts = nowtime2.Subtract(nowtime1);
                double time = ts.TotalMilliseconds;
                detectionSpeedtextBox.Text = time.ToString("0.0");

                GC.Collect();               // 强制回收垃圾，可降低程序的内存占用
            }
        }

        private void captureTestButton_Click(object sender, EventArgs e)
        {
            bool flag = Lad.init();
            if (flag)
            {
                cap = new VideoCapture();
                cap.ImageGrabbed += capture_ImageGrabbed;
                cap.Start();
            }           
        }

        private void capture_ImageGrabbed(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            cap.Retrieve(frame);

            DateTime nowtime1 = System.DateTime.Now;

            process(frame);

            DateTime nowtime2 = System.DateTime.Now;                       // 计算程序运行速度
            TimeSpan ts = nowtime2.Subtract(nowtime1);
            double time = ts.TotalMilliseconds;
            detectionSpeedtextBox.Text = time.ToString("0.0");

            frame.Dispose();
        }
        public void process(Mat frame)         // 核心代码，处理模块
        {
            srcImageBox.Image = frame;             // 第一个控件中显示原图

            Mat LaneROI = Lad.LAD_getROI(frame);                 // 调用LAD.dll中LAD_getROI函数，获取图像下半部分

            Mat YcrCb_channel_3 = Lad.LAD_spaceConversion(LaneROI, 1, 3);     // 转到YcrCb颜色空间
            Mat colorSpaceShow = Lad.LAD_ImageStitch_1(YcrCb_channel_3);     // 拼接后显示到控件               
            colorSpaceImageBox.Image = colorSpaceShow;

            Mat bianaryImage = Lad.LAD_segmentation_autoThreshold_1(YcrCb_channel_3); // ROI区域的二值化图像(自动阈值，左右分别分割)
            Mat bianaryShow = Lad.LAD_ImageStitch_1(bianaryImage);
            bianaryImageBox.Image = bianaryShow;

            //Mat templateAppImage = Lad.LAD_templateApp(bianaryImage);  // 应用模板后的二值图像
            Mat multiImage = Lad.LAD_multi2one(bianaryImage);      // 只保留一条直线
            Mat multiShow = Lad.LAD_ImageStitch_1(multiImage);         // 拼接后显示
            withTempAndSingleImageBox.Image = multiShow;

            Mat affineTranImage = Lad.LAD_affineTrans(multiImage);   // 仿射变换图像
            Mat affineShow = Lad.LAD_ImageStitch_1(affineTranImage);
            fitted_param fitted = Lad.LAD_laneFit(affineTranImage);

            Mat laneFittedImage = fitted.fittedImage;                  // 拟合后图像
            double laneWidthInImage = fitted.laneWidth;                // 图像中车道宽度(pixel)
            Matrix<double> left_Matrix = fitted.left_Matrix;           // 左边车道拟合参数
            Matrix<double> right_Matrix = fitted.right_Matrix;         // 右边车道拟合参数
            Mat fittedImageCopy = fitted.fittedImageCopy;

            Mat laneFittedImageShow = Lad.LAD_ImageStitch_1(laneFittedImage);    // 拼接后显示
            affineAndFittedImageBox.Image = laneFittedImageShow;

            lane_param param = Lad.LAD_getParameter(fittedImageCopy, left_Matrix, right_Matrix, laneWidthInImage);  // 参数计算

            laneWidthtextBox.Text = param.laneWidth.ToString("0.000");      // 将参数显示在控件中
            offsettextBox.Text = param.center_distance.ToString("0.000");
            offsetDirectiontextBox.Text = param.offset_direction;
            deflectiontextBox.Text = param.deflection.ToString("0.00");
            deflectionDirectiontextBox.Text = param.deflection_direction;

            Mat resImage = Lad.LAD_resShow(frame, affineTranImage, left_Matrix, right_Matrix);
            resultImageBox.Image = resImage;
        }





    }
}
