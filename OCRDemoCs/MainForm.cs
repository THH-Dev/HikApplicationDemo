using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;

namespace OCRDemoCs
{
    public partial class MainForm : Form
    {
        private string strSolutionPath = null;//方案路径
        private VmProcedure procedure = null;//流程
        private bool isSolutionLoad = false;//true表示方案加载成功，false表示方案加载失败
        private bool isContinuRun = false;//true表示连续运行，false表示停止连续运行
        private string logPath = Application.StartupPath + "/Log/Message";//日志路径
        private MainViewControl mainViewControl;
        private RenderControl renderControl;
        private Timer LoadSolutionIndicateTimer = new Timer();


        public MainForm()
        {
            InitializeComponent();
            renderControl = new RenderControl();
            mainViewControl = new MainViewControl();
            mainViewControl.Dock = DockStyle.Fill;
            renderControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(mainViewControl);
            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;
            //VM回调事件
            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;//注册流程状态回调
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
                    isContinuRun = true;
                    buttonContiRun.Text = "停止运行";

                    //禁用按钮
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "开始连续运行！";
                    LogUpdate(strMessage);
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
                    isContinuRun = false;
                    buttonContiRun.Text = "连续运行";

                    //启用按钮
                    buttonSelectSolu.Enabled = true;
                    buttonRunOnce.Enabled = true;
                    buttonLoadSolu.Enabled = true;
                    buttonSaveSolu.Enabled = true;
                    comboProcedure.Enabled = true;

                    strMessage = "运行结束！";
                    LogUpdate(strMessage);
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
            if (!isSolutionLoad)
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
            if (isSolutionLoad)
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// 流程执行回调
        /// </summary>
        /// <param name="workStatusInfo"></param>
        private void VmSolution_OnWorkStatusEvent(VM.PlatformSDKCS.ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            string strMessage = null;
            try
            {
                if (workStatusInfo.nWorkStatus == 0 && workStatusInfo.nProcessID == 10000)//流程空闲且为流程1
                {
                    List<VmDynamicIODefine.IoNameInfo> ioNameInfos = procedure.ModuResult.GetAllOutputNameInfo();
                    if (ioNameInfos.Count != 0)//判断流程结果个数是否为0
                    {
                        if (ioNameInfos[0].TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)//判断流程第一个结果是否为字符串类型
                        {
                            //获取流程结果
                            string strResult = procedure.ModuResult.GetOutputString(ioNameInfos[0].Name).astStringVal[0].strValue;
                            if (strResult != null)
                            {
                                UpdateResult(strResult);
                                strMessage = "流程运行耗时：" + procedure.ProcessTime.ToString() + "ms";
                                LogUpdate(strMessage);
                            }
                            else
                            {
                                strMessage = "获取结果失败：结果为空!";
                                LogUpdate(strMessage);
                            }
                        }
                    }
                    else
                    {
                        strMessage = "获取结果失败：流程结果个数为0!";
                        LogUpdate(strMessage);
                    }
                }
            }
            catch (VmException ex)
            {
                strMessage = "获取结果失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "获取结果失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 更新结果
        /// </summary>
        /// <param name="strResult"></param>
        private void UpdateResult(string strResult)
        {
            this.BeginInvoke(new Action(() =>
            {
                string[] vs = strResult.Split(';');
                if (vs[0] == "1")
                {
                    labelResult.Text = "OK";
                    labelResult.BackColor = Color.FromArgb(0, 192, 0);
                }
                else
                {
                    labelResult.Text = "NG";
                    labelResult.BackColor = Color.Red;
                }
                string result = "结果：码信息：" + vs[2] + ";  字符个数：" + vs[1] + ";  置信度：" + vs[3];
                listBoxResult.Items.Add(result);
                listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
            }));
        }

        /// <summary>
        /// 加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
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
                if (isSolutionLoad == true)
                {
                    isSolutionLoad = false;
                }
                VmSolution.Load(strSolutionPath);//加载
                isSolutionLoad = true;
                strMessage = "方案加载成功.";
                LogUpdate(strMessage);

                if (comboProcedure.Items.Count != 0)
                {
                    comboProcedure.Items.Clear();//清空下拉控件中的内容
                }

                ProcessInfoList processInfoList = VmSolution.Instance.GetAllProcedureList();//获取所有流程
                for (int i = 0; i < processInfoList.nNum; i++)
                {
                    comboProcedure.Items.Add(processInfoList.astProcessInfo[i].strProcessName);
                }
                if (comboProcedure.Items.Count > 0)
                {
                    comboProcedure.SelectedIndex = 0;//默认选择第一个流程
                    procedure = VmSolution.Instance[processInfoList.astProcessInfo[0].strProcessName] as VmProcedure;
                    if (procedure == null)
                    {
                        strMessage = "流程为空，请检查方案!";
                        LogUpdate(strMessage);
                        return;
                    }
                    //绑定渲染源
                    renderControl.vmRenderControl1.ModuleSource = procedure;
                }
                else
                {
                    strMessage = "流程个数为零.请检查方案!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "方案加载失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "方案加载失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
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
        /// 选择流程时配置渲染信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxAllProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                procedure = VmSolution.Instance[comboProcedure.SelectedItem.ToString()] as VmProcedure;
                renderControl.vmRenderControl1.ModuleSource = procedure;
                strMessage = "选择[" + comboProcedure.SelectedItem.ToString() + "]成功.";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "选择流程失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "选择流程失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                VmSolution.Save();
                strMessage = "保存方案成功.";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "保存方案失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "保存方案失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != strSolutionPath && true == isSolutionLoad)
            {
                if (MessageBox.Show("是否保存方案", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (procedure != null || isContinuRun == true)
                        {
                            procedure.ContinuousRunEnable = false;
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        string strMsg = "方案保存失败！";
                        LogUpdate(strMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(renderControl);
            buttonRender.BackColor = Color.Orange;
        }

        /// <summary>
        /// 选择方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                strSolutionPath = openFileDialog.FileName;
                isSolutionLoad = false;
                LoadSolutionIndicateTimer.Enabled = true;
                strMessage = "选择方案成功.";
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 单次运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                if (isSolutionLoad == true && null != procedure)
                {
                    procedure.Run();
                }
                else
                {
                    strMessage = "所选流程不存在!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "单次运行失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "单次运行失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 连续运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                if (isSolutionLoad == true && null != procedure)
                {
                    procedure.ContinuousRunEnable = procedure.ContinuousRunEnable ^ true;
                    isContinuRun = isContinuRun ^ true;
                }
                else
                {
                    strMessage = "所选流程不存在!";
                    LogUpdate(strMessage);
                }
            }
            catch (VmException ex)
            {
                strMessage = "连续运行失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "连续运行失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 图像显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRender_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(renderControl);
                buttonRender.BackColor = Color.Orange;
                buttonConfig.BackColor = Color.Gray;
                strMessage = "切换图像显示界面成功.";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "切换图像显示界面失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "切换图像显示界面失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }
        }

        /// <summary>
        /// 参数配置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfig_Click(object sender, EventArgs e)
        {
            string strMessage = null;
            try
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(mainViewControl);
                buttonConfig.BackColor = Color.Orange;
                buttonRender.BackColor = Color.Gray;
                strMessage = "切换参数配置界面成功.";
                LogUpdate(strMessage);
            }
            catch (VmException ex)
            {
                strMessage = "切换参数配置界面失败，错误码：0x" + Convert.ToString(ex.errorCode, 16);
                LogUpdate(strMessage);
            }
            catch (Exception ex)
            {
                strMessage = "切换参数配置界面失败：" + Convert.ToString(ex.Message);
                LogUpdate(strMessage);
            }

        }

        /// <summary>
        /// 消息显示
        /// </summary>
        /// <param name="str"></param>
        private void LogUpdate(string str)
        {
            string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
            //如果记录超过1万条，应当清空再添加记录，以防记录的条目巨大引起界面卡顿和闪烁
            if (listViewLog.Items.Count > 10000)
                listViewLog.Items.Clear();

            listViewLog.BeginInvoke(new Action(() =>
            {
                listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, str }));
            }));

            SaveLog(str);
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="str"></param>
        private void SaveLog(string str)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(logPath))//如果日志目录不存在就创建
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    string filename = logPath + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
                    StreamWriter mySw = File.AppendText(filename);
                    mySw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss::ffff\t") + str);
                    mySw.Close();
                }
                catch
                {
                    return;
                }
            });
        }
        
        /// <summary>
        /// 清空消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logClear_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
        }
    }
}
