using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;

namespace LAD
{
    public struct fitted_param
    {
        public Mat fittedImage;
        public double laneWidth;
        public Matrix<double> left_Matrix;
        public Matrix<double> right_Matrix;
        public Mat fittedImageCopy;
    };
    public struct lane_param
    {
        public Mat fittedColorImage;
        public double laneWidth;
        public double center_distance;
        public String offset_direction;
        public double deflection;
        public String deflection_direction;
    }  
   
    public class lad
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]       
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static double Th_low;   // 手动阈值分割低阈值
        static double Th_high;  // 手动分割搞阈值

        static int sp1_x;      // 仿射变换原图中的四个点坐标
        static int sp1_y;
        static int sp2_x;
        static int sp2_y;
        static int sp3_x;
        static int sp3_y;
        static int sp4_x;
        static int sp4_y;

        static int dp1_x;      // 仿射变换目标图像中的四个点坐标
        static int dp1_y;
        static int dp2_x;
        static int dp2_y;
        static int dp3_x;
        static int dp3_y;
        static int dp4_x;
        static int dp4_y;

        static int margin;     // 车道线拟合中方框长度
        static int minpix;      // 方框中最小像素个数
        static int nwindow;     // 滑动窗口数目

        static double xm_per_pix;   // 图像距离和实际距离的比值（x方向）
        static double ym_per_pix;   // 图像距离和实际距离的比值（y方向）

        static string configPath = "./config/config.ini";

        /* 模块0 ：初始化变量
         
        */
        // *************************** 模块0 BEGIN  ***********************************************************//
        public bool init()
        {
            if (File.Exists(configPath))
            {
                StringBuilder tmp1 = new StringBuilder(500);
                GetPrivateProfileString("LAD_segmentation_manualThreshold", "Th_low", "", tmp1, 500, configPath);
                string x1 = tmp1.ToString();
                double y1 = Convert.ToDouble(x1);
                Th_low = y1;

                StringBuilder tmp2 = new StringBuilder(500);
                GetPrivateProfileString("LAD_segmentation_manualThreshold", "Th_high", "", tmp2, 500, configPath);
                string x2 = tmp2.ToString();
                double y2 = Convert.ToDouble(x2);
                Th_high = y2;

                int[] spx = new int[4];
                int[] spy = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    StringBuilder tmpx = new StringBuilder(500);
                    GetPrivateProfileString("LAD_affineTrans", "sp" + Convert.ToString(i + 1) + ".x", "", tmpx, 500, configPath);
                    spx[i] = Convert.ToInt32(tmpx.ToString());

                    StringBuilder tmpy = new StringBuilder(500);
                    GetPrivateProfileString("LAD_affineTrans", "sp" + Convert.ToString(i + 1) + ".y", "", tmpy, 500, configPath);
                    spy[i] = Convert.ToInt32(tmpy.ToString());
                }
                sp1_x = spx[0];
                sp1_y = spy[0];
                sp2_x = spx[1];
                sp2_y = spy[1];
                sp3_x = spx[2];
                sp3_y = spy[2];
                sp4_x = spx[3];
                sp4_y = spy[3];

                int[] dpx = new int[4];
                int[] dpy = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    StringBuilder tmpx = new StringBuilder(500);
                    GetPrivateProfileString("LAD_affineTrans", "dp" + Convert.ToString(i + 1) + ".x", "", tmpx, 500, configPath);
                    dpx[i] = Convert.ToInt32(tmpx.ToString());

                    StringBuilder tmpy = new StringBuilder(500);
                    GetPrivateProfileString("LAD_affineTrans", "dp" + Convert.ToString(i + 1) + ".y", "", tmpy, 500, configPath);
                    dpy[i] = Convert.ToInt32(tmpy.ToString());
                }
                dp1_x = dpx[0];
                dp1_y = dpy[0];
                dp2_x = dpx[1];
                dp2_y = dpy[1];
                dp3_x = dpx[2];
                dp3_y = dpy[2];
                dp4_x = dpx[3];
                dp4_y = dpy[3];

                StringBuilder tmp_margin = new StringBuilder(500);
                GetPrivateProfileString("LAD_laneFit", "margin", "", tmp_margin, 500, configPath);
                margin = Convert.ToInt32(tmp_margin.ToString());

                StringBuilder tmp_minpix = new StringBuilder(500);
                GetPrivateProfileString("LAD_laneFit", "minpix", "", tmp_minpix, 500, configPath);
                minpix = Convert.ToInt32(tmp_minpix.ToString());

                StringBuilder tmp_nwindow = new StringBuilder(500);
                GetPrivateProfileString("LAD_laneFit", "nwindow", "", tmp_nwindow, 500, configPath);
                nwindow = Convert.ToInt32(tmp_nwindow.ToString());

                StringBuilder tmp_xm = new StringBuilder(500);
                GetPrivateProfileString("LAD_getParameter", "xm_per_pix", "", tmp_xm, 500, configPath);
                xm_per_pix = Convert.ToDouble(tmp_xm.ToString());

                StringBuilder tmp_ym = new StringBuilder(500);
                GetPrivateProfileString("LAD_getParameter", "ym_per_pix", "", tmp_ym, 500, configPath);
                ym_per_pix = Convert.ToDouble(tmp_ym.ToString());

                return true;     // 初始化成功
            }
            else
            {
                MessageBox.Show("初始化失败！配置文件不存在！请检查config文件夹下是否存在config.ini", "提醒");
                return false;    // 初始化失败
            }
            
        }
        // *************************** 模块0 END  ***********************************************************//

        /*
        模块1：LAD_getROI
        功能：1）从原图获取相关感兴趣区域，主要ROI为原图下半部分包含车道线信息。
              2）获取图像上半部分区域函数，为后续显示完整图像做准备。
              3）两个图像拼接函数，用来将部分图像拼接起来后做显示。
        输入： srcImage -> 原图
        输出： ROI, upHalf -> 原图下半部分（包含车道线）、原图上半部分（非车道线部分，用于拼接显示）
         */
        // *************************** 模块1 BEGIN  ***********************************************************//
        public Mat LAD_getROI(Mat srcImage)      // 获取图像ROI，取图像下半部分
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);
            int row = image.Rows;
            int col = image.Cols;
            Rectangle rect = new Rectangle(0, row / 2 - 1, col, row / 2);

            Mat dstImage = new Mat(image, rect);
            return dstImage;
        }

        public Mat LAD_getUpHalf(Mat srcImage)       // 获取图像上半部分，为后续拼接做准备
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);
            int row = image.Rows;
            int col = image.Cols;
            Rectangle rect = new Rectangle(0, 0, col, row / 2);

            Mat dstImage = new Mat(image, rect);
            return dstImage;
        }

        public Mat LAD_ImageStitch_1(Mat srcImage)          // 图像上下部分拼接，第一种情况，上半部分为黑色区域
        {
            Mat upHalf = new Mat(srcImage.Size, srcImage.Depth, srcImage.NumberOfChannels);
            if (upHalf.NumberOfChannels == 1)            // 根据输入图像的大小、类型创建相同大小类型的图像，像素设置为0
            {
                upHalf.SetTo(new MCvScalar(0));
            }
            else if (upHalf.NumberOfChannels == 3)
            {
                upHalf.SetTo(new MCvScalar(0, 0, 0));
            }

            Mat dstImage = new Mat();

            dstImage.PushBack(upHalf);             // push_back(): 在原图底部添加新的Mat
            dstImage.PushBack(srcImage);
            
            return dstImage;
        }

        public Mat LAD_ImageStitch_2(Mat image1, Mat image2)  // 图像上下部分拼接，第二种情况
        {
            Mat dstImage = new Mat();
            dstImage.PushBack(image1);
            dstImage.PushBack(image2);

            return dstImage;
        }
        // *************************** 模块1 END  ***********************************************************//

        /*
        模块2：LAD_spaceConversion
        功能：1）图像颜色空间转换和通道图像提取。
             case index:
             0: RGB
             1: RGB2YCbCr
             2: RGB2HSV
             3: RGB2HLS
             4: RGB2YUV
        输入： srcImage -> RGB图像
               index -> 转换方法
               channel -> 输出第channel通道的图像
        输出： Mat -> 颜色空间转换后的第channel通道图像
         */
        // *************************** 模块2 BEGIN  ***********************************************************//

        public Mat LAD_spaceConversion(Mat srcImage, int index, int channel)
        {
            Mat image = new Mat();
            Mat dstImage = new Mat();
            Mat outImage = new Mat();
            srcImage.CopyTo(image);

            switch (index)
            {
                case 0:
                    image.CopyTo(dstImage);
                    break;
                case 1:   //ycbcr
                    CvInvoke.CvtColor(image, dstImage, ColorConversion.Bgr2YCrCb);
                    break;
                case 2:   // hsv
                    CvInvoke.CvtColor(image, dstImage, ColorConversion.Bgr2Hsv);
                    break;
                case 3:  // hls
                    CvInvoke.CvtColor(image, dstImage, ColorConversion.Bgr2Hls);
                    break;
                case 4:  // yuv
                    CvInvoke.CvtColor(image, dstImage, ColorConversion.Bgr2Yuv);
                    break;
                default:
                    break;
            }

            switch (channel)
            {
                case 1:                   
                    dstImage.Split()[0].CopyTo(outImage);
                    break;
                case 2:
                    dstImage.Split()[1].CopyTo(outImage);
                    break;
                case 3:
                    dstImage.Split()[2].CopyTo(outImage);
                    break;
                default:
                    break;
            }

            return outImage;
        }
        // *************************** 模块2 END  ***********************************************************//

        /*
         模块3_1：LAD_segmentation_autoThreshold_1
         功能：1）对单通道图像进行阈值分割。
               2）采用自动阈值分割方法。
               3）采用图像左右分别分割方法。
         输入：srcImage -> 单通道图像（灰度图）
         输出：Mat -> 自动阈值分割后的图像
     */
        // *************************** 模块3_1 BEGIN  ***********************************************************//
        public Mat LAD_segmentation_autoThreshold_1(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            int H = image.Rows;
            int W = image.Cols;

            Mat left = new Mat(image, new Rectangle(0, 0, W / 2, H));
            Mat right = new Mat(image, new Rectangle(W / 2, 0, W / 2, H));

            Mat left_bianary = seg(left);
            Mat right_bianary = seg(right);

            Mat dstImage = new Mat(H, W, 0, 1);
            for (int i = 1; i < W / 2; i++)
            {
                left_bianary.Col(i).CopyTo(dstImage.Col(i));
                right_bianary.Col(i).CopyTo(dstImage.Col(i + W / 2));
            }

            return dstImage;
        }

        private Mat seg(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            int H = image.Rows;
            int W = image.Cols;

            double Mean, Std;
            double[] Th_low = new double[H];
            double[] Th_high = new double[H];
            Mat r;
            MCvScalar m = new MCvScalar();
            MCvScalar s = new MCvScalar();

            for (int h = 0; h < H; h++)
            {
                r = image.Row(h).Clone();
                CvInvoke.MeanStdDev(r, ref m, ref s);
                Mean = m.V0;
                Std = s.V0;
                Th_low[h] = Mean - 3 * Std;
                Th_high[h] = Mean + 3 * Std;
            }

            Image<Gray, byte> I = image.ToImage<Gray, byte>();          
            for (int i = 0; i < H; i++)
            {              
                for (int j = 0; j < W; j++)
                {
                    int a = I.Data[i, j, 0];
                    if (a >= Th_low[i] && a <= Th_high[i])
                        I.Data[i, j, 0] = 0;
                    else
                        I.Data[i, j, 0] = 255;
                }
            }
            Mat dstImage = I.Mat;
            return dstImage;

        }
        // *************************** 模块3_1 END  ***********************************************************//

        /*
        模块3_2：LAD_segmentation_autoThreshold_2
        功能：1）对单通道图像进行阈值分割。
              2）采用自动阈值分割方法。
              3）不采用图像左右分别分割方法（直接对单通道进行分割）
        输入：srcImage -> 单通道图像（灰度图）
        输出：Mat -> 自动阈值分割后的图像
        */
        // *************************** 模块3_2 BEGIN  ***********************************************************//
        public Mat LAD_segmentation_autoThreshold_2(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);
            Mat dstImage = seg(image);
            return dstImage;
        }
        // *************************** 模块3_2 END  ***********************************************************//

        /*
        模块3_3：LAD_segmentation_manualThreshold
        功能：1）对单通道图像进行阈值分割。
              2）采用手动输入阈值分割方法。
        输入：srcImage -> 单通道图像（灰度图）
              Th_low -> 低阈值
              Th_high -> 高阈值
        输出：Mat -> 手动阈值分割后的图像
     */
        // *************************** 模块3_3 BEGIN  ***********************************************************//
        public Mat LAD_segmentation_manualThreshold(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            int H = image.Rows;
            int W = image.Cols;

            Image<Gray, byte> I = image.ToImage<Gray, byte>();
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    double a = I.Data[i, j, 0];
                    if (a >= Th_low && a <= Th_high)
                        I.Data[i, j, 0] = 0;
                    else
                        I.Data[i, j, 0] = 255;
                }
            }
            Mat dstImage = I.Mat;

            return dstImage;
        }
        // *************************** 模块3_3 END  ***********************************************************//

        /*
        模块4：LAD_templateApp
        功能：1）对单通道（二值图）图像运用模板。

        输入：srcImage -> 单通道图像（二值图）
        输出：Mat -> 运用模板过滤后的图像
        */
        // *************************** 模块4 BEGIN  ***********************************************************//
        public Mat LAD_templateApp(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);
            Mat template = CvInvoke.Imread("TemplateImage.jpg");
            CvInvoke.CvtColor(template, template, ColorConversion.Bgr2Gray);

            Image<Gray, byte> image_tmp = image.ToImage<Gray, byte>();
            Image<Gray, byte> template_tmp = template.ToImage<Gray, byte>();

            int H = image.Rows;
            int W = image.Cols;

            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    int data1 = image_tmp.Data[i, j, 0];
                    int data2 = template_tmp.Data[i, j, 0];
                    if (data1 == 255 && data2 == 255)
                        image_tmp.Data[i, j, 0] = 255;
                    else
                        image_tmp.Data[i, j, 0] = 0;
                }
            }
            Mat dstImage = image_tmp.Mat;
            return dstImage;
        }
        // *************************** 模块4 END  ***********************************************************//

        /*
        模块5：LAD_multi2one
        功能：1）过滤图像，使只保留左右各一条直线。

        输入：srcImage -> 单通道图像（二值图）
        输出：Mat -> 只有一条直线的图像
        */
        // *************************** 模块5 BEGIN  ***********************************************************//
        public Mat LAD_multi2one(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            int H = image.Rows;
            int W = image.Cols;
            List<int> r_left = new List<int>();
            List<int> c_left = new List<int>();
            List<int> r_right = new List<int>();
            List<int> c_right = new List<int>();

            Mat outImage = new Mat(image.Size, image.Depth, image.NumberOfChannels);
            outImage.SetTo(new MCvScalar(0));

            Image<Gray, byte> tmp = image.ToImage<Gray, byte>();

            for (int i = 0; i < H; i++)
            {
                for (int j = W / 2; j >= 0; j--)
                {
                    int data = tmp.Data[i, j, 0];
                    if (data == 255)
                    {
                        r_left.Add(i);                     
                        c_left.Add(j);
                        break;
                    }
                }

                for (int h = W / 2; h < W; h++)
                {
                    int data = tmp.Data[i, h, 0];
                    if (data == 255)
                    {
                        r_right.Add(i);
                        c_right.Add(h);
                        break;
                    }
                }
            }

            Image<Gray, byte> tmp2 = outImage.ToImage<Gray, byte>();
            for (int k = 0; k < r_left.Count; k++)
            {
                int rr = r_left[k];
                int cc = c_left[k];
                tmp2.Data[rr, cc, 0] = 255;
            }

            for (int k = 0; k < r_right.Count; k++)
            {
                int rr = r_right[k];
                int cc = c_right[k];
                tmp2.Data[rr, cc, 0] = 255;
            }
            Mat dstImage = tmp2.Mat;

            return dstImage;
        }
        // *************************** 模块5 END  ***********************************************************//

        /*
        模块6：LAD_affineTrans & LAD_reverseAffineTrans
        功能：1）LAD_affineTrans -> 仿射变换
              2）LAD_reverseAffineTrans -> 反仿射变换
        */
        // *************************** 模块6 BEGIN  ***********************************************************//
        public Mat LAD_affineTrans(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            PointF[] src_points = new PointF[4];
            PointF[] dst_points = new PointF[4];
            src_points[0] = new PointF(sp1_x, sp1_y);
            src_points[1] = new PointF(sp2_x, sp2_y);
            src_points[2] = new PointF(sp3_x, sp3_y);
            src_points[3] = new PointF(sp4_x, sp4_y);

            dst_points[0] = new PointF(dp1_x, dp1_y);
            dst_points[1] = new PointF(dp2_x, dp2_y);
            dst_points[2] = new PointF(dp3_x, dp3_y);
            dst_points[3] = new PointF(dp4_x, dp4_y);

            Mat M = CvInvoke.GetPerspectiveTransform(src_points, dst_points);
            Mat dstImage = new Mat();
            CvInvoke.WarpPerspective(image, dstImage, M, new Size(640, 240));
            return dstImage;    
        }

        public Mat LAD_reverseAffineTrans(Mat srcImage)
        {
            Mat image = new Mat();
            srcImage.CopyTo(image);

            PointF[] src_points = new PointF[4];
            PointF[] dst_points = new PointF[4];
            src_points[0] = new PointF(sp1_x, sp1_y);
            src_points[1] = new PointF(sp2_x, sp2_y);
            src_points[2] = new PointF(sp3_x, sp3_y);
            src_points[3] = new PointF(sp4_x, sp4_y);

            dst_points[0] = new PointF(dp1_x, dp1_y);
            dst_points[1] = new PointF(dp2_x, dp2_y);
            dst_points[2] = new PointF(dp3_x, dp3_y);
            dst_points[3] = new PointF(dp4_x, dp4_y);

            Mat M = CvInvoke.GetPerspectiveTransform(dst_points, src_points);
            Mat dstImage = new Mat();
            CvInvoke.WarpPerspective(image, dstImage, M, new Size(640, 240));

            return dstImage;
        }
        // *************************** 模块6 END  ***********************************************************//

        /*
       模块7：LAD_laneFit & curve_fit
       功能：1）LAD_laneFit ->  车道线拟合
             2）curve_fit ->   多项式拟合矩阵计算
       */
        // *************************** 模块7 BEGIN  ***********************************************************//
        public fitted_param LAD_laneFit(Mat srcImage)
        {
            fitted_param f = new fitted_param();          // 结构体，用来返回多个值
            Mat image = new Mat();
            srcImage.CopyTo(image);

            int H = image.Rows;
            int W = image.Cols;
            List<int> numOfperColpixel = new List<int>();          // 储存图像每一列像素之和

            for (int i = 0; i < W; i++)
            {
                int sum = 0;
                Mat x = image.Col(i);
                Image<Gray, byte> tmp = x.ToImage<Gray, byte>();
                for (int j = 0; j < x.Rows; j++)
                {
                    for (int k = 0; k < x.Cols; k++)
                    {
                        sum += tmp.Data[j, k, 0];
                    }
                }
                numOfperColpixel.Add(sum);
            }

            int midpoint = (int)W / 2;         // 图像中心位置
            int arrayLength = W;

            int left_max = numOfperColpixel[0];
            int left_base = 0;
            /*
            for (int i = arrayLength / 2; i >= 0; i--)
            {
                if (numOfperColpixel[i] > 5)
                    left_base = i;
            }
            int right_base = 0;
            for (int i = arrayLength / 2; i < arrayLength; i++)
            {
                if (numOfperColpixel[i] > 5)
                    right_base = i;
            }
            */
            for (int i = 0; i < arrayLength / 2; i++)       // 找出左边车道线基础位置
            {
                if (numOfperColpixel[i] > left_max)
                {
                    left_max = numOfperColpixel[i];
                    left_base = i;
                }
            }

            int right_max = numOfperColpixel[arrayLength / 2];
            int right_base = 0;
            for (int i = arrayLength / 2; i < arrayLength; i++)  // 找出左边车道线基础位置
            {
                if (numOfperColpixel[i] > right_max)
                {
                    right_max = numOfperColpixel[i];
                    right_base = i;
                }
            }
            
            int lane_width = right_base - left_base;     // 车道宽度(pixel)

            List<int> nonzero_x = new List<int>();       // 储存图像非零像素坐标x
            List<int> nonzero_y = new List<int>();       // 储存图像非零像素坐标y

            Image<Gray, byte> image_tmp = image.ToImage<Gray, byte>();
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (image_tmp.Data[i, j, 0] > 0)
                    {
                        nonzero_x.Add(i);
                        nonzero_y.Add(j);
                    }
                }
            }

            int left_currenty = left_base;         // 左边当前位置
            int right_currenty = right_base;       // 右边当前位置
            //int margin = 20;                      // 矩形框长度
            //int minpix = 10;                      // 最小非零像素点个数
            //int nwindow = 9;                      // 滑动窗口数
            int window_height = (int)H / nwindow;           

            int win_x_low, win_x_high, win_yleft_low, win_yleft_high, win_yright_low, win_yright_high;

            List<int> leftx = new List<int>();
            List<int> lefty = new List<int>();
            List<int> rightx = new List<int>();
            List<int> righty = new List<int>();
            //leftx.Add(left_currenty);
            //lefty.Add(H);
            //rightx.Add(right_currenty);
            //righty.Add(H);

            List<int> x_left = new List<int>();
            List<int> y_left = new List<int>();
            List<int> x_right = new List<int>();
            List<int> y_right = new List<int>();

            for (int i = 0; i < nwindow; i++)
            {               
                win_x_low = H - (i + 1) * window_height;
                win_x_high = H - i * window_height;

                win_yleft_low = left_currenty - margin;
                win_yleft_high = left_currenty + margin;

                win_yright_low = right_currenty - margin;
                win_yright_high = right_currenty + margin;

                if (win_x_low < 0)
                    win_x_low = 0;
                if (win_x_high > H)
                    win_x_high = H;
                if (win_yleft_low < 0)
                    win_yleft_low = 0;
                if (win_yleft_high > W / 2)
                    win_yleft_high = W / 2;
                if (win_yright_low < W / 2)
                    win_yright_low = W / 2;
                if (win_yright_high > W)
                    win_yright_high = W;


                Rectangle r_left = new Rectangle(win_yleft_low, win_x_low, 2 * margin, window_height);
                Rectangle r_right = new Rectangle(win_yright_low, win_x_low, 2 * margin, window_height);
                CvInvoke.Rectangle(image, r_left, new MCvScalar(255), 1);
                CvInvoke.Rectangle(image, r_right, new MCvScalar(255), 1);

                for (int j = 0; j < nonzero_x.Count; j++)
                {
                    if ((nonzero_x[j] >= win_x_low) && (nonzero_x[j] <= win_x_high) && (nonzero_y[j] >= win_yleft_low) && (nonzero_y[j] <= win_yleft_high))
                    {
                        x_left.Add(nonzero_x[j]);
                        y_left.Add(nonzero_y[j]);
                    }

                    if ((nonzero_x[j] >= win_x_low) && (nonzero_x[j] <= win_x_high) && (nonzero_y[j] >= win_yright_low) && (nonzero_y[j] <= win_yright_high))
                    {
                        x_right.Add(nonzero_x[j]);
                        y_right.Add(nonzero_y[j]);
                    }
                }

                if (x_left.Count > minpix)
                {
                    int sum_leftx = 0;
                    int sum_lefty = 0;
                    for (int k = 0; k < x_left.Count; k++)
                    {
                        sum_leftx += x_left[k];
                        sum_lefty += y_left[k];
                    }
                    left_currenty = (int)(sum_lefty / x_left.Count);
                    lefty.Add(left_currenty);
                    leftx.Add((int)(sum_leftx / x_left.Count));
                }

                if (x_right.Count > minpix)
                {
                    int sum_rightx = 0;
                    int sum_righty = 0;
                    for (int k = 0; k < x_right.Count; k++)
                    {
                        sum_rightx += x_right[k];
                        sum_righty += y_right[k];
                    }
                    right_currenty = (int)(sum_righty / x_right.Count);
                    righty.Add(right_currenty);
                    rightx.Add((int)(sum_rightx / x_right.Count));
                }
                x_left.Clear();
                y_left.Clear();
                x_right.Clear();
                y_right.Clear();
            }

            Mat fitted_image = new Mat(image.Size, DepthType.Cv8U, 3);
            fitted_image.SetTo(new Bgr(0,0,0).MCvScalar);

            List<Point> left_points = new List<Point>();
            for (int i = 0; i < leftx.Count; i++)
            {
                left_points.Add(new Point(lefty[i], leftx[i]));
            }

            List<Point> right_points = new List<Point>();
            for (int i = 0; i < rightx.Count; i++)
            {
                right_points.Add(new Point(righty[i], rightx[i]));
            }

            Matrix<double> left_Matrix = curve_fit(left_points, 2);     // 计算二次多项式拟合参数
            Point[] left_fitted_points = new Point[H];
            for (int x = 0; x < H; x++)
            {
                int y = (int)(left_Matrix.Data[0, 0] + left_Matrix.Data[1, 0] * x + left_Matrix.Data[2, 0] * Math.Pow(x, 2));
                left_fitted_points[x] = new Point(y, x);
            }
            CvInvoke.Polylines(image, left_fitted_points, false, new MCvScalar(255), 5);        // 画出拟合曲线

            Matrix<double> right_Matrix = curve_fit(right_points, 2);
            Point[] right_fitted_points = new Point[H];
            for (int x = 0; x < H; x++)
            {
                int y = (int)(right_Matrix.Data[0, 0] + right_Matrix.Data[1, 0] * x + right_Matrix.Data[2, 0] * Math.Pow(x, 2));
                right_fitted_points[x] = new Point(y, x);
            }
            CvInvoke.Polylines(image, right_fitted_points, false, new MCvScalar(255), 5);

            f.laneWidth = lane_width;
            f.left_Matrix = left_Matrix;
            f.right_Matrix = right_Matrix;
            f.fittedImage = image;
            f.fittedImageCopy = fitted_image;
            return f;           
        }
        Matrix<double> curve_fit(List<Point> key_point, int n)        // 计算多项式拟合参数矩阵
        {           
            int N = key_point.Count;                    //Number of key points           

            Matrix<double> X = new Matrix<double>(n+1,n+1);         //构造矩阵XX

            for (int i = 0; i < n + 1; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        X.Data[i, j] = X.Data[i, j] + Math.Pow(key_point[k].Y, i + j);
                    }
                }
            }
          
            Matrix<double> Y = new Matrix<double>(n + 1,  1);      //构造矩阵Y

            for (int i = 0; i < n + 1; i++)
            {
                for (int k = 0; k < N; k++)
                {
                    Y.Data[i, 0] = Y.Data[i, 0] + Math.Pow(key_point[k].Y, i) * key_point[k].X;
                }
            }

            Matrix<double> A = new Matrix<double>(n + 1, 1);
            
            CvInvoke.Solve(X, Y, A, DecompMethod.LU);            //求解矩阵A
            return A;
        }
        // *************************** 模块7 END  ***********************************************************//

        /*
          模块8：LAD_getParameter
          功能： 计算车辆行驶状态参数
          */
        // *************************** 模块8 BEGIN  ***********************************************************//
        public lane_param LAD_getParameter(Mat srcImage, Matrix<double> left_Matrix, Matrix<double> right_Matrix, double laneWidth)
        {
            lane_param l = new lane_param();

            int H = srcImage.Rows;
            int W = srcImage.Cols;

            double width_lane = laneWidth * xm_per_pix;
            double car_position = W / 2;

            double left_fit_x = left_Matrix.Data[0, 0] + left_Matrix.Data[1, 0] * H + left_Matrix.Data[2, 0] * Math.Pow(H, 2);
            double right_fit_x = right_Matrix.Data[0, 0] + right_Matrix.Data[1, 0] * H + right_Matrix.Data[2, 0] * Math.Pow(H, 2);       
            double lane_center_position = (left_fit_x + right_fit_x) / 2;
            double center_distance_tmp = (car_position - lane_center_position) * xm_per_pix;
            double center_distance = Math.Abs(center_distance_tmp);

            if (center_distance > 0.005)
                l.offset_direction = "右偏移";
            else if (center_distance < -0.005)
                l.offset_direction = "左偏移";
            else
                l.offset_direction = "无偏移";

            double leftSum = 0;
            double kk1 = 0;
            for (int i = 0; i < H; i++)
            {
                kk1 = left_Matrix.Data[1, 0] + left_Matrix.Data[2, 0] * i;
                leftSum += kk1;
            }
            double k1 = leftSum / H;
            double rightSum = 0;
            double kk2 = 0;
            for (int i = 0; i < H; i++)
            {
                kk2 = right_Matrix.Data[1, 0] + right_Matrix.Data[2, 0] * i;
                rightSum += kk2;
            }
            double k2 = rightSum / H;
            double k = ym_per_pix * (k1 + k2) / 2;
            double theta_tmp = Math.Atan(k) * 180 / Math.PI;
            double theta = Math.Abs(theta_tmp);

            if (theta_tmp > 0.5)
                l.deflection_direction = "右偏转";
            else if (theta_tmp < -0.5)
                l.deflection_direction = "左偏转";
            else
                l.deflection_direction = "无偏转";

            l.deflection = theta;
            l.center_distance = center_distance;
            l.laneWidth = width_lane;
            return l;
        }
        // *************************** 模块8 END  ***********************************************************//

        /*
          模块9：LAD_resShow
          功能： 显示最后效果
        */
        // *************************** 模块9 BEGIN  ***********************************************************//
        public Mat LAD_resShow(Mat srcImage, Mat bianaryImage, Matrix<double>left_Matrix, Matrix<double>right_Matrix)
        {
            Mat image = new Mat();
            bianaryImage.CopyTo(image);

            Mat uphalf = LAD_getUpHalf(srcImage);
            Mat bottomHalf = LAD_getROI(srcImage);

            Mat warpImage = new Mat(bianaryImage.Size, DepthType.Cv8U, 3);
            warpImage.SetTo(new Bgr(0, 0, 0).MCvScalar);

            Mat warpCopy = warpImage.Clone();

            int H = bianaryImage.Rows;
            int W = bianaryImage.Cols;

            Point[] left_fitted_points = new Point[H];
            for (int x = 0; x < H; x++)
            {
                int y = (int)(left_Matrix.Data[0, 0] + left_Matrix.Data[1, 0] * x + left_Matrix.Data[2, 0] * Math.Pow(x, 2));
                left_fitted_points[x] = new Point(y, x);
            }
            CvInvoke.Polylines(warpImage, left_fitted_points, false, new Bgr(255, 0, 0).MCvScalar, 5, LineType.EightConnected);

            Point[] right_fitted_points = new Point[H];
            for (int x = 0; x < H; x++)
            {
                int y = (int)(right_Matrix.Data[0, 0] + right_Matrix.Data[1, 0] * x + right_Matrix.Data[2, 0] * Math.Pow(x, 2));
                right_fitted_points[x] = new Point(y, x);
            }
            CvInvoke.Polylines(warpImage, right_fitted_points, false, new Bgr(255, 0, 0).MCvScalar, 5, LineType.EightConnected);

            Mat rev = LAD_reverseAffineTrans(warpImage);

            Mat laneImage = new Mat();
            CvInvoke.AddWeighted(bottomHalf, 0.5, rev, 0.5, 0, laneImage);

            Mat tmp = new Mat();
            CvInvoke.AddWeighted(uphalf, 0.5, warpCopy, 0.5, 0, tmp);

            Mat dst = LAD_ImageStitch_2(tmp, laneImage);                      
            return dst;
        }

        // *************************** 模块9 END  ***********************************************************//
    }
  

}
