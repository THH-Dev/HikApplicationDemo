using ImageSourceModuleCs;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace LocateDemoCs
{
    public partial class MainForm : Form
    {
        const string image_path = @"C:\Program Files\VisionMaster4.2.0\Development\V4.x\Samples\C#\ApplicationDemo\LocateDemoCs\Data\Image.bmp";
        const string solution_path = @"C:\Program Files\VisionMaster4.2.0\Development\V4.x\Samples\C#\ApplicationDemo\LocateDemoCs\Data\Tuanna-Test.sol";

        /// <summary>
        /// 渲染窗口用到的用户控件
        /// </summary>
        private RenderControl renderControl;

        /// <summary>
        /// 参数配置用到的用户控件
        /// </summary>
        private MainViewControl mainViewControl;

        /// <summary>
        /// 当前方案路径
        /// </summary>
        private string currentSolutionPath = solution_path;

        /// <summary>
        /// 流程列表
        /// </summary>
        private List<VmProcedure> processList;


        /// <summary>
        /// 方案已加载标志
        /// </summary>
        private bool SolutionIsLoaded = false;
         
        private Timer LoadSolutionIndicateTimer = new Timer();


        public MainForm()
        {
            InitializeComponent();
            renderControl = new RenderControl();
            mainViewControl = new MainViewControl();
            renderControl.Dock = DockStyle.Fill;
            mainViewControl.Dock = DockStyle.Fill;
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;

            // 回调事件
            VmSolution.OnProcessStatusStartEvent += VmSolution_OnProcessStatusStartEvent;   // 开始连续执行状态回调
            VmSolution.OnProcessStatusStopEvent += VmSolution_OnProcessStatusStopEvent; // 结束连续执行状态回调
        }

        /// <summary>
        /// 开始连续执行状态
        /// </summary>
        /// <param name="statusInfo">开始执行状态</param>
        private void VmSolution_OnProcessStatusStartEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStatus == 0)
                {
                    string strMessage = null;
                    buttonContiRun.Text = "ContiRun";

                    //禁用按钮
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "开始连续运行！";
                    AppendLog(strMessage);
                }
            }));
        }

        /// <summary>
        /// 结束连续执行状态回调
        /// </summary>
        /// <param name="statusInfo">结束执行状态</param>
        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStopAction == 1)
                {
                    string strMessage = null;
                    buttonContiRun.Text = "连续运行";

                    //启用按钮
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;

                    strMessage = "运行结束！";
                    AppendLog(strMessage);
                }
            }));
        }

        /// <summary>
        /// 加载方案按钮闪烁提示，提示选择完了方案接下来就要加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSolutionIndicateTimer_Tick(object sender, EventArgs e)
        {
            if (!SolutionIsLoaded)
            {
                if (buttonLoadSolu.BackColor == Color.DimGray)
                {
                    buttonLoadSolu.BackColor = Color.Orange;
                }
                else
                {
                    buttonLoadSolu.BackColor = Color.DimGray;
                }
            }
            if (SolutionIsLoaded)
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();           
            renderPanel.Controls.Add(mainViewControl);
            renderPanel.Controls.Add(renderControl);
            renderPanel.Controls.Remove(mainViewControl);
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="message"></param>
        private void AppendLog(string message)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
                //如果记录超过1万条，应当清空再添加记录，以防记录的条目巨大引起界面卡顿和闪烁
                if (listViewLog.Items.Count > 10000)
                    listViewLog.Items.Clear();
                //判断是否是当前界面线程调用此方法
                if (listViewLog.InvokeRequired)
                {
                    listViewLog.BeginInvoke(new Action(() =>
                    {
                        listViewLog.Items.Insert(0,new ListViewItem(new string[] {timeStamp, message}));
                    }));
                }
                else
                {
                    listViewLog.Items.Insert(0,new ListViewItem(new string[] {timeStamp, message}));
                }

                //如果日志文件夹不存在就创建它
                if (!Directory.Exists("./log"))
                {
                    Directory.CreateDirectory("./log");
                }
                //写入日志到文件
                using (FileStream fs = new FileStream("./log/LocateDemoCs.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(timeStamp + ":" + message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateResultListBox(string result)
        {
            //如果记录超过1万条，应当清空再添加记录，以防记录的条目巨大引起界面卡顿和闪烁
            if (listBoxResult.Items.Count > 10000)
                listBoxResult.Items.Clear();          
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
            //判断是否是当前界面线程调用此方法
            if (listBoxResult.InvokeRequired)
            {
                listBoxResult.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Insert(0,timeStamp + result);
                }));
            }
            else
            {
                listBoxResult.Items.Insert(0, timeStamp + result);
            }
        }

        /// <summary>
        /// 更新状态标签
        /// </summary>
        /// <param name="result"></param>
        private void UpdateLableState(string result)
        {
            try
            {
                string[] strArray = result.Split(',');
                //判断是否是当前界面线程调用此方法
                if (labelResultState.InvokeRequired)
                {
                    labelResultState.BeginInvoke(new Action(() =>
                    {
                        if (strArray[0] == "1")
                        {
                            labelResultState.BackColor = Color.FromArgb(0, 192, 0);
                            labelResultState.Text = "OK";
                        }
                        else
                        {
                            labelResultState.BackColor = Color.Red;
                            labelResultState.Text = "NG";
                        }
                    }));
                }
                else
                {
                    if (strArray[0] == "1")
                    {
                        labelResultState.BackColor = Color.DarkGreen;
                        labelResultState.Text = "OK";
                    }
                    else
                    {
                        labelResultState.BackColor = Color.Red;
                        labelResultState.Text = "NG";
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }           
        }

        /// <summary>
        /// 显示主渲染窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRender_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
            buttonRender.BackColor = Color.Orange;
            buttonConfig.BackColor = Color.Gray;
        }

        /// <summary>
        /// 显示调试主界面，由此界面进入参数调试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(mainViewControl);
            buttonRender.BackColor = Color.Gray;
            buttonConfig.BackColor = Color.Orange;
        }

        /// <summary>
        /// 加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "方案文件(*.sol)|*.sol";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //选择了方案，就会认定当前没有加载方案，等待加载方案按钮按下
                    SolutionIsLoaded = false;
                    currentSolutionPath = ofd.FileName;
                    LoadSolutionIndicateTimer.Enabled = true;
                    AppendLog("当前方案路径："+currentSolutionPath);
                    MessageBox.Show("当前方案路径：" + currentSolutionPath + "\n请点击加载方案按钮以开始加载方案",
                        "提示",MessageBoxButtons.OK,MessageBoxIcon.Information);                                        
                }
                else
                {
                    currentSolutionPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            LoadSolutionIndicateTimer.Enabled = false;
            buttonLoadSolu.BackColor = Color.Orange;
            buttonLoadSolu.Enabled = false;

            // 禁用其余按钮，防止误操作
            buttonSelectSolu.Enabled = false;
            buttonRunOnce.Enabled = false;
            buttonContiRun.Enabled = false;
            buttonSaveSolu.Enabled = false;
            comboProcedure.Enabled = false;
            buttonRender.Enabled = false;
            buttonConfig.Enabled = false;
            try
            {
                if (currentSolutionPath != String.Empty)
                {
                    VmSolution.Load(currentSolutionPath);
                    processList = GetCurrentSolProcedureList();
                    UpdateProcessComboBox(processList);
                    RegisterProcedureWorkEndCallback(processList);
                    SolutionIsLoaded = true;
                    renderControl.ModuleSource = processList[0];
                    AppendLog("方案加载已完成");
                    MessageBox.Show("方案加载已完成", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            catch (Exception ex)
            {
                listViewLog.Items.Add(new ListViewItem(new string[]
                    {DateTime.Now.ToString("YY-MM-DD HH:mm:ss-fff"), ex.Message}));
            }
            finally
            {
                buttonLoadSolu.BackColor = Color.DimGray;
                buttonLoadSolu.Enabled = true;

                // 重新启用其余按钮
                buttonSelectSolu.Enabled = true;
                buttonRunOnce.Enabled = true;
                buttonContiRun.Enabled = true;
                buttonSaveSolu.Enabled = true;
                comboProcedure.Enabled = true;
                buttonRender.Enabled = true;
                buttonConfig.Enabled = true;
            }
        }

        /// <summary>
        /// 获取当前方案的流程列表
        /// </summary>
        private List<VmProcedure> GetCurrentSolProcedureList()
        {
            List<VmProcedure> procedureList = new List<VmProcedure>();
            string processName = "";
            var processInfoList = VmSolution.Instance.GetAllProcedureList();
            for (int i = 0; i < processInfoList.nNum; i++)
            {
                processName = processInfoList.astProcessInfo[i].strProcessName;
                procedureList.Add((VmProcedure)VmSolution.Instance[processName]);          
            }
            return procedureList;
        }

        /// <summary>
        /// 更新流程列表下拉控件
        /// </summary>
        /// <param name="lst"></param>
        private void UpdateProcessComboBox(List<VmProcedure> lst)
        {
            comboProcedure.Items.Clear();
            foreach (var vmProcedure in lst)
            {
                comboProcedure.Items.Add(vmProcedure.Name);
            }
            if (comboProcedure.Items.Count > 0)
            {
                comboProcedure.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded)
                {
                    VmSolution.Save();
                    AppendLog("方案保存成功");
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
                    
        }

        /// <summary>
        /// 运行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded && comboProcedure.SelectedIndex != -1)
                {
                    string processName = comboProcedure.SelectedItem.ToString();
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
                    ImageSourceModuleTool imageSourceModuleTool = (ImageSourceModuleTool)VmSolution.Instance["Flow1.Image Source1"];


                    if(chbxIsLoadPath.Checked)
                    {
                        imageSourceModuleTool.SetImagePath(image_path);
                    }   
                    else
                    {
                        var CapturedImage = Cv2.ImRead(image_path, ImreadModes.Color);
                        var data_byte = ConvertMatToByte(CapturedImage);
                        //ImageBaseData imageBaseData = new ImageBaseData(data_byte, (uint)data_byte.Length, CapturedImage.Width, CapturedImage.Height, (int)ImagePixelFormat.IMAGE_PIXEL_FORMAT_RGB24);
                        ImageBaseData imageBaseData = ConvertMatToImageBaseData(CapturedImage);
                        imageSourceModuleTool.SetImageData(imageBaseData);
                    }    


                    if (procedure != null)
                    {
                        AppendLog("方案开始执行");
                        procedure.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 连续运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (SolutionIsLoaded && comboProcedure.SelectedIndex != -1)
                {
                    string processName = comboProcedure.SelectedItem.ToString();
                    VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
                    if (procedure != null)
                    {
                        procedure.ContinuousRunEnable = procedure.ContinuousRunEnable ^ true;
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 注册流程工作结束回调
        /// </summary>
        /// <param name="lst"></param>
        public void RegisterProcedureWorkEndCallback(List<VmProcedure> lst)
        {
            try
            {
                foreach (var vmProcedure in processList)
                {
                    vmProcedure.OnWorkEndStatusCallBack += VmProcedure_OnWorkEndStatusCallBack;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 流程结束回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VmProcedure_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            try
            {
                VmProcedure procedure = sender as VmProcedure;
                if (procedure != null)
                {
                    var outputList = procedure.ModuResult.GetAllOutputNameInfo();
                    bool outputConfigIsWrong = true;
                    //获取流程输出配置中为out的输出项
                    foreach (var ioNameInfo in outputList)
                    {
                        if (ioNameInfo.Name == "out" &&
                            ioNameInfo.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                        {
                            var result = procedure.ModuResult.GetOutputString(ioNameInfo.Name);
                            string resultStrValue = result.astStringVal[0].strValue;
                            AppendLog("流程结果:"+resultStrValue);
                            UpdateResultListBox("流程结果:" + resultStrValue);
                            UpdateLableState(resultStrValue);
                            outputConfigIsWrong = false;
                        }
                    }
                    //流程输出配置有误，提示输出配置错误
                    if (outputConfigIsWrong)
                    {
                        AppendLog("流程输出配置中不存在名为out的输出或者out输出类型非字符串");
                    }
                    AppendLog("流程耗时" + procedure.ProcessTime);
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }                       
        }

        /// <summary>
        /// 根据下拉框决定渲染窗口的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                renderControl.ModuleSource = (VmProcedure)VmSolution.Instance[comboProcedure.SelectedItem.ToString()];
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 清除日志消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemClearMsg_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }

        /// <summary>
        /// 主窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != currentSolutionPath && true == SolutionIsLoaded)
            {
                if (MessageBox.Show("是否保存方案", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (currentSolutionPath != String.Empty)
                        {
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog(ex.Message);
                    }
                }
            }
        }

        //Convert mat
        private static byte[] ConvertMatToByte(Mat mat)
        {
            byte[] buffer = null;

            if (mat == null)
            {
                throw new Exception("Image input emtry");
            }

            if (mat.Type() != MatType.CV_8UC1 && mat.Type() != MatType.CV_8UC3)
            {
                throw new Exception("Pixel format not supp");
            }

            uint imgWidth = (uint)mat.Size().Width;
            uint imgHeight = (uint)mat.Size().Height;
            uint channalNum = (uint)mat.Channels();
            uint imgsize = imgWidth * imgHeight * channalNum;

            try
            {
                if (mat.Type() == MatType.CV_8UC1)
                {
                    IntPtr ptrMat = mat.Ptr(0);
                    buffer = new byte[imgsize];
                    Marshal.Copy(ptrMat, buffer, 0, buffer.Length);
                }
                else if (mat.Type() == MatType.CV_8UC3)
                {
                    IntPtr ptrMat = mat.Ptr(0);
                    buffer = new byte[imgsize];
                    Marshal.Copy(ptrMat, buffer, 0, buffer.Length);
                    //for (int i = 0; i < buffer.Length - 2; i += 3)
                    //{
                    //    byte temp = buffer[i];
                    //    buffer[i] = buffer[i + 2];
                    //    buffer[i + 2] = temp;
                    //}

                }
            }
            catch (VmException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }

        // Convert mat to base
        private static ImageBaseData ConvertMatToImageBaseData(Mat mat)
        {
            ImageBaseData imageBaseData = null;

            if (mat == null)
            {
                throw new Exception("Image input emtry");
            }

            if (mat.Type() != MatType.CV_8UC1 && mat.Type() != MatType.CV_8UC3)
            {
                throw new Exception("Pixel format not supp");
            }

            uint imgWidth = (uint)mat.Size().Width;
            uint imgHeight = (uint)mat.Size().Height;
            uint channalNum = (uint)mat.Channels();
            uint imgsize = imgWidth * imgHeight * channalNum;

            try
            {
                if (mat.Type() == MatType.CV_8UC1)
                {
                    IntPtr ptrMat = mat.Ptr(0);
                    byte[] buffer = new byte[imgsize];
                    Marshal.Copy(ptrMat, buffer, 0, buffer.Length);
                    imageBaseData = new ImageBaseData(buffer, imgsize, (int)imgWidth, (int)imgHeight, (int)ImagePixelFormat.IMAGE_PIXEL_FORMAT_MONO8);
                }
                else if (mat.Type() == MatType.CV_8UC3)
                {
                    IntPtr ptrMat = mat.Ptr(0);
                    byte[] buffer = new byte[imgsize];
                    Marshal.Copy(ptrMat, buffer, 0, buffer.Length);
                    //for (int i = 0; i < buffer.Length - 2; i += 3)
                    //{
                    //    byte temp = buffer[i];
                    //    buffer[i] = buffer[i + 2];
                    //    buffer[i + 2] = temp;
                    //}
                    imageBaseData = new ImageBaseData(buffer, imgsize, (int)imgWidth, (int)imgHeight, (int)ImagePixelFormat.IMAGE_PIXEL_FORMAT_RGB24);

                }
            }
            catch (VmException ex)
            {
                imageBaseData = null;
                throw ex;
            }
            catch (Exception ex)
            {
                imageBaseData = null;
                throw ex;
            }
            return imageBaseData;
        }

    }
}
