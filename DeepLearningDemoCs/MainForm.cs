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
using Microsoft.WindowsAPICodePack.Dialogs;
using VM.Core;
using VM.PlatformSDKCS;

namespace DeepLearningDemoCs
{
    public partial class MainForm : Form
    {

        private RenderControl renderControl;
        private MainViewControl mainViewControl;
        public bool mSolutionIsLoad = false;  //true 代表方案加载成功，false 代表方案关闭
        public VmProcedure vmProcedure = null;//流程对象
        public ProcessInfoList vmProcessInfoList = new ProcessInfoList();//流程列表
        public string vmSolutionPath = null;//方案路径
        private string logPath = Application.StartupPath + "/Log/Message";//日志路径
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
            renderPanel.Controls.Add(mainViewControl);
            LoadSolutionIndicateTimer.Interval = 300;
            LoadSolutionIndicateTimer.Tick += LoadSolutionIndicateTimer_Tick;
            //VM回调事件
            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;//工作状态
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
                    buttonContiRun.Text = "停止运行";

                    //禁用按钮
                    buttonSelectSolu.Enabled = false;
                    buttonRunOnce.Enabled = false;
                    buttonLoadSolu.Enabled = false;
                    buttonSaveSolu.Enabled = false;
                    comboProcedure.Enabled = false;

                    strMessage = "开始连续运行！";
                    LogFunction(strMessage);
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
                    LogFunction(strMessage);
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
            if (!mSolutionIsLoad)
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
            if (mSolutionIsLoad)
            {
                buttonLoadSolu.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            renderPanel.Controls.Clear();
            renderPanel.Controls.Add(renderControl);
        }

        /// <summary>
        /// 流程执行回调
        /// </summary>
        /// <param name="workStatusInfo"></param>
        private void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            if (workStatusInfo.nWorkStatus == 0)//0为执行完毕，1为正在执行
            {
                try
                {
                    switch (workStatusInfo.nProcessID)
                    {
                        case 10000:
                            if (vmProcessInfoList.nNum==0) return;
                            VmProcedure vmProcedure = (VmProcedure)VmSolution.Instance[vmProcessInfoList.astProcessInfo[0].strProcessName];
                            if (vmProcedure == null) return;
                            List<VmDynamicIODefine.IoNameInfo> ioNameInfos = vmProcedure.ModuResult.GetAllOutputNameInfo();
                            foreach (var item in ioNameInfos)
                            {
                                if (item.Name == "out"&&item.TypeName!= IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                                {
                                    Task.Run(() =>
                                    {
                                        UpdateResult("结果类型不一致！");

                                    });
                                    return;
                                }
                            }
                            var vmResult = vmProcedure.ModuResult.GetOutputString("out").astStringVal[0].strValue;
                            //开线程处理结果数据
                            Task.Run(() =>
                            {
                                UpdateResult(vmResult);
                                LogFunction("执行成功，流程耗时:" + vmProcedure.ProcessTime.ToString() + "ms");
                            });
                            break;
                        default:
                            break;
                    }
                }
                catch (VmException ex)
                {
                    LogFunction("回调出错:"+ Convert.ToString(ex.errorCode, 16));
                    return;
                }
                catch (Exception ex)
                {
                    LogFunction("回调出错:" + ex.ToString());
                    return;
                }
            }
        }

        /// <summary>
        /// 更新结果数据
        /// </summary>
        /// <param name="result"></param>
        public void UpdateResult(string result)
        {
            try
            {
                string[] str = result.Split(',');
                if (str[0] == "OK")
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        labelResult.Text = "OK";
                        labelResult.BackColor = Color.FromArgb(255, 0, 192, 0);
                    }));
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        labelResult.Text = "NG";
                        labelResult.BackColor = Color.FromArgb(255, 255, 0, 0);
                    }));
                }
                this.BeginInvoke(new Action(() =>
                {
                    listBoxResult.Items.Add("结果:" + result.ToString());
                    listBoxResult.TopIndex = listBoxResult.Items.Count - 1;
                }));
            }
            catch (Exception ex)
            {
                LogFunction("更新结果出错:" + ex.ToString());
                return;
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
        /// 选择方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSolu_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "VM Sol File|*.sol*";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                vmSolutionPath = openFileDialog.FileName;
                mSolutionIsLoad = false;
                LoadSolutionIndicateTimer.Enabled = true;
                LogFunction(vmSolutionPath);
            }
        }

        /// <summary>
        /// 加载方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadSolu_Click(object sender, EventArgs e)
        {
            string strMsg = null;
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
                if (vmSolutionPath != null && File.Exists(vmSolutionPath))
                {
                    VmSolution.Load(vmSolutionPath);
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();//获取所有流程列
                    vmProcedure = VmSolution.Instance[vmProcessInfoList.astProcessInfo[0].strProcessName] as VmProcedure;
                    comboProcedure.Items.Clear();
                    for (int item = 0; item < vmProcessInfoList.nNum; item++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[item].strProcessName);
                    }
                    if (comboProcedure.Items.Count > 0)
                    {
                        comboProcedure.SelectedIndex = 0;
                        comboProcedure.Text = comboProcedure.SelectedItem.ToString();
                    }
                    renderControl.vmRenderControl1.ModuleSource = vmProcedure;
                    mSolutionIsLoad = true;
                    strMsg = "加载方案成功！";
                    LogFunction(strMsg);
                }
                else
                {
                    strMsg = "方案为空！";
                    LogFunction(strMsg);
                }
            }
            catch (VmException ex)
            {
                strMsg = "加载方案出错. Error Code: " + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "加载方案出错. Error Code: " + ex.ToString();
                LogFunction(strMsg);
                return;
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
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveSolu_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            if (mSolutionIsLoad == true)
            {
                try
                {
                    VmSolution.Save();
                }
                catch (VmException ex)
                {
                    strMsg = "保存方案出错. Error Code: " + Convert.ToString(ex.errorCode, 16);
                    LogFunction(strMsg);
                    return;
                }
                strMsg = "保存方案成功！";
                LogFunction(strMsg);
            }
            else
            {
                strMsg = "请先加载方案！";
                LogFunction(strMsg);
            }
        }

        /// <summary>
        /// 流程下拉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboProcedure_DropDown(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (mSolutionIsLoad)
                {
                    comboProcedure.Items.Clear();
                    vmProcessInfoList = VmSolution.Instance.GetAllProcedureList();//获取所有流程列
                    for (int item = 0; item < vmProcessInfoList.nNum; item++)
                    {
                        comboProcedure.Items.Add(vmProcessInfoList.astProcessInfo[item].strProcessName);
                    }
                }
                else
                {
                    strMsg ="请先加载方案！";
                    LogFunction(strMsg);
                }
            }
            catch (VmException ex)
            {
                strMsg = "获取流程列表失败. Error Code: " + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "获取流程列表失败. Error Code: " + ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunOnce_Click(object sender, EventArgs e)
        {
            string strMsg = null;
            try
            {
                if (comboProcedure.Text != "")
                {
                    vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                    if (null == vmProcedure) return;
                    renderControl.vmRenderControl1.ModuleSource = vmProcedure;
                    vmProcedure.Run();
                }
                else
                {
                    strMsg = "请先选择流程! ";
                    LogFunction(strMsg);
                    return;
                }
            }
            catch (VmException ex)
            {
                strMsg = "执行失败. Error Code: " + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "执行失败. Error Code: " + ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// 连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonContiRun_Click(object sender, EventArgs e)
        {
            string strMsg = null;

            try
            {
                if (comboProcedure.Text != "")
                {
                    vmProcedure = (VmProcedure)VmSolution.Instance[comboProcedure.Text];
                    if (null == vmProcedure)
                    {
                        strMsg = comboProcedure.Text + " 流程不存在！";
                        LogFunction(strMsg);
                        return;
                    }
                    vmProcedure.ContinuousRunEnable = vmProcedure.ContinuousRunEnable^true;
                }
                else
                {
                    strMsg = "请先选择流程！ ";
                    LogFunction(strMsg);
                    return;
                }
            }
            catch (VmException ex)
            {
                strMsg = "连续执行失败. Error Code: " + Convert.ToString(ex.errorCode, 16);
                LogFunction(strMsg);
                return;
            }
            catch (Exception ex)
            {
                strMsg = "连续执行失败. Error Code: " +ex.ToString();
                LogFunction(strMsg);
                return;
            }
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="strMsg"></param>
        public void LogFunction(string strMsg)
        {
            this.BeginInvoke(new Action(() =>
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add("");
                listViewItem.SubItems[0].Text = DateTime.Now.ToString();
                listViewItem.SubItems[1].Text = strMsg;
                listViewLog.Items.Insert(0, listViewItem);
            }));
            SaveLog(strMsg);
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewLog.Items.Clear();
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
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != vmSolutionPath && true == mSolutionIsLoad)
            {
                if (MessageBox.Show("是否保存方案", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (vmProcedure != null)
                        {
                            VmSolution.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        string strMsg = "方案保存失败！";
                        LogFunction(strMsg);
                    }
                }
            }
        }
    }
}
