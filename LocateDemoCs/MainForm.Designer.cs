﻿namespace LocateDemoCs
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.renderPanel = new System.Windows.Forms.Panel();
            this.groupBoxSolution = new System.Windows.Forms.GroupBox();
            this.buttonSaveSolu = new System.Windows.Forms.Button();
            this.buttonLoadSolu = new System.Windows.Forms.Button();
            this.buttonSelectSolu = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.timeStampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.infoHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemClearMsg = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRunOnce = new System.Windows.Forms.Button();
            this.buttonContiRun = new System.Windows.Forms.Button();
            this.groupBoxProcedure = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProcedure = new System.Windows.Forms.ComboBox();
            this.buttonRender = new System.Windows.Forms.Button();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.labelResultState = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.chbxIsLoadPath = new System.Windows.Forms.CheckBox();
            this.groupBoxSolution.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBoxProcedure.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.groupBoxResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderPanel.Location = new System.Drawing.Point(12, 8);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(868, 648);
            this.renderPanel.TabIndex = 0;
            // 
            // groupBoxSolution
            // 
            this.groupBoxSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSolution.Controls.Add(this.buttonSaveSolu);
            this.groupBoxSolution.Controls.Add(this.buttonLoadSolu);
            this.groupBoxSolution.Controls.Add(this.buttonSelectSolu);
            this.groupBoxSolution.ForeColor = System.Drawing.Color.White;
            this.groupBoxSolution.Location = new System.Drawing.Point(886, 41);
            this.groupBoxSolution.Name = "groupBoxSolution";
            this.groupBoxSolution.Size = new System.Drawing.Size(379, 94);
            this.groupBoxSolution.TabIndex = 2;
            this.groupBoxSolution.TabStop = false;
            this.groupBoxSolution.Text = "BoxSolution";
            // 
            // buttonSaveSolu
            // 
            this.buttonSaveSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonSaveSolu.FlatAppearance.BorderSize = 0;
            this.buttonSaveSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSaveSolu.Location = new System.Drawing.Point(263, 31);
            this.buttonSaveSolu.Name = "buttonSaveSolu";
            this.buttonSaveSolu.Size = new System.Drawing.Size(110, 44);
            this.buttonSaveSolu.TabIndex = 2;
            this.buttonSaveSolu.Text = "Save Solu";
            this.buttonSaveSolu.UseVisualStyleBackColor = false;
            this.buttonSaveSolu.Click += new System.EventHandler(this.buttonSaveSolu_Click);
            // 
            // buttonLoadSolu
            // 
            this.buttonLoadSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonLoadSolu.FlatAppearance.BorderSize = 0;
            this.buttonLoadSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadSolu.ForeColor = System.Drawing.Color.White;
            this.buttonLoadSolu.Location = new System.Drawing.Point(135, 31);
            this.buttonLoadSolu.Name = "buttonLoadSolu";
            this.buttonLoadSolu.Size = new System.Drawing.Size(110, 44);
            this.buttonLoadSolu.TabIndex = 1;
            this.buttonLoadSolu.Text = "Load Solu";
            this.buttonLoadSolu.UseVisualStyleBackColor = false;
            this.buttonLoadSolu.Click += new System.EventHandler(this.buttonLoadSolu_Click);
            // 
            // buttonSelectSolu
            // 
            this.buttonSelectSolu.BackColor = System.Drawing.Color.DimGray;
            this.buttonSelectSolu.FlatAppearance.BorderSize = 0;
            this.buttonSelectSolu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectSolu.ForeColor = System.Drawing.Color.White;
            this.buttonSelectSolu.Location = new System.Drawing.Point(6, 31);
            this.buttonSelectSolu.Name = "buttonSelectSolu";
            this.buttonSelectSolu.Size = new System.Drawing.Size(110, 44);
            this.buttonSelectSolu.TabIndex = 0;
            this.buttonSelectSolu.Text = "Select Solu";
            this.buttonSelectSolu.UseVisualStyleBackColor = false;
            this.buttonSelectSolu.Click += new System.EventHandler(this.buttonSelectSolu_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listViewLog);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(884, 252);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 560);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeStampHeader,
            this.infoHeader});
            this.listViewLog.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewLog.ForeColor = System.Drawing.Color.White;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Location = new System.Drawing.Point(12, 23);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(366, 531);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // timeStampHeader
            // 
            this.timeStampHeader.Text = "TimeStamp";
            this.timeStampHeader.Width = 98;
            // 
            // infoHeader
            // 
            this.infoHeader.Text = "Info";
            this.infoHeader.Width = 120;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemClearMsg});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // ToolStripMenuItemClearMsg
            // 
            this.ToolStripMenuItemClearMsg.Name = "ToolStripMenuItemClearMsg";
            this.ToolStripMenuItemClearMsg.Size = new System.Drawing.Size(98, 22);
            this.ToolStripMenuItemClearMsg.Text = "清除";
            this.ToolStripMenuItemClearMsg.Click += new System.EventHandler(this.ToolStripMenuItemClearMsg_Click);
            // 
            // buttonRunOnce
            // 
            this.buttonRunOnce.BackColor = System.Drawing.Color.DimGray;
            this.buttonRunOnce.FlatAppearance.BorderSize = 0;
            this.buttonRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunOnce.ForeColor = System.Drawing.Color.White;
            this.buttonRunOnce.Location = new System.Drawing.Point(135, 34);
            this.buttonRunOnce.Name = "buttonRunOnce";
            this.buttonRunOnce.Size = new System.Drawing.Size(110, 46);
            this.buttonRunOnce.TabIndex = 0;
            this.buttonRunOnce.Text = "Run Once";
            this.buttonRunOnce.UseVisualStyleBackColor = false;
            this.buttonRunOnce.Click += new System.EventHandler(this.buttonRunOnce_Click);
            // 
            // buttonContiRun
            // 
            this.buttonContiRun.BackColor = System.Drawing.Color.DimGray;
            this.buttonContiRun.FlatAppearance.BorderSize = 0;
            this.buttonContiRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContiRun.ForeColor = System.Drawing.Color.White;
            this.buttonContiRun.Location = new System.Drawing.Point(262, 34);
            this.buttonContiRun.Name = "buttonContiRun";
            this.buttonContiRun.Size = new System.Drawing.Size(110, 46);
            this.buttonContiRun.TabIndex = 1;
            this.buttonContiRun.Text = "ContiRun";
            this.buttonContiRun.UseVisualStyleBackColor = false;
            this.buttonContiRun.Click += new System.EventHandler(this.buttonContiRun_Click);
            // 
            // groupBoxProcedure
            // 
            this.groupBoxProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProcedure.Controls.Add(this.label1);
            this.groupBoxProcedure.Controls.Add(this.buttonContiRun);
            this.groupBoxProcedure.Controls.Add(this.comboProcedure);
            this.groupBoxProcedure.Controls.Add(this.buttonRunOnce);
            this.groupBoxProcedure.ForeColor = System.Drawing.Color.White;
            this.groupBoxProcedure.Location = new System.Drawing.Point(886, 142);
            this.groupBoxProcedure.Name = "groupBoxProcedure";
            this.groupBoxProcedure.Size = new System.Drawing.Size(379, 104);
            this.groupBoxProcedure.TabIndex = 4;
            this.groupBoxProcedure.TabStop = false;
            this.groupBoxProcedure.Text = "Procedure";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Procedure";
            // 
            // comboProcedure
            // 
            this.comboProcedure.BackColor = System.Drawing.Color.Gray;
            this.comboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboProcedure.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboProcedure.ForeColor = System.Drawing.Color.White;
            this.comboProcedure.FormattingEnabled = true;
            this.comboProcedure.Location = new System.Drawing.Point(6, 53);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Size = new System.Drawing.Size(121, 22);
            this.comboProcedure.TabIndex = 0;
            this.comboProcedure.SelectedIndexChanged += new System.EventHandler(this.comboProcedure_SelectedIndexChanged);
            // 
            // buttonRender
            // 
            this.buttonRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRender.BackColor = System.Drawing.Color.DimGray;
            this.buttonRender.FlatAppearance.BorderSize = 0;
            this.buttonRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRender.ForeColor = System.Drawing.Color.White;
            this.buttonRender.Location = new System.Drawing.Point(888, 8);
            this.buttonRender.Name = "buttonRender";
            this.buttonRender.Size = new System.Drawing.Size(82, 27);
            this.buttonRender.TabIndex = 5;
            this.buttonRender.Text = "Render";
            this.buttonRender.UseVisualStyleBackColor = false;
            this.buttonRender.Click += new System.EventHandler(this.buttonRender_Click);
            // 
            // buttonConfig
            // 
            this.buttonConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfig.BackColor = System.Drawing.Color.DimGray;
            this.buttonConfig.FlatAppearance.BorderSize = 0;
            this.buttonConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfig.ForeColor = System.Drawing.Color.White;
            this.buttonConfig.Location = new System.Drawing.Point(971, 8);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(85, 27);
            this.buttonConfig.TabIndex = 6;
            this.buttonConfig.Text = "Config";
            this.buttonConfig.UseVisualStyleBackColor = false;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // labelResultState
            // 
            this.labelResultState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResultState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelResultState.Font = new System.Drawing.Font("SimSun", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResultState.ForeColor = System.Drawing.Color.White;
            this.labelResultState.Location = new System.Drawing.Point(735, 34);
            this.labelResultState.Name = "labelResultState";
            this.labelResultState.Size = new System.Drawing.Size(100, 92);
            this.labelResultState.TabIndex = 7;
            this.labelResultState.Text = "OK";
            this.labelResultState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resultPanel
            // 
            this.resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.resultPanel.Controls.Add(this.groupBoxResult);
            this.resultPanel.Location = new System.Drawing.Point(12, 659);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(868, 154);
            this.resultPanel.TabIndex = 8;
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxResult.Controls.Add(this.listBoxResult);
            this.groupBoxResult.Controls.Add(this.labelResultState);
            this.groupBoxResult.ForeColor = System.Drawing.Color.White;
            this.groupBoxResult.Location = new System.Drawing.Point(4, 3);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(861, 151);
            this.groupBoxResult.TabIndex = 9;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "结果";
            // 
            // listBoxResult
            // 
            this.listBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxResult.ForeColor = System.Drawing.Color.White;
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.Location = new System.Drawing.Point(6, 26);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(706, 104);
            this.listBoxResult.TabIndex = 8;
            // 
            // chbxIsLoadPath
            // 
            this.chbxIsLoadPath.AutoSize = true;
            this.chbxIsLoadPath.ForeColor = System.Drawing.Color.Coral;
            this.chbxIsLoadPath.Location = new System.Drawing.Point(1129, 18);
            this.chbxIsLoadPath.Name = "chbxIsLoadPath";
            this.chbxIsLoadPath.Size = new System.Drawing.Size(133, 17);
            this.chbxIsLoadPath.TabIndex = 9;
            this.chbxIsLoadPath.Text = "Load Image From Path";
            this.chbxIsLoadPath.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1278, 826);
            this.Controls.Add(this.chbxIsLoadPath);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.buttonRender);
            this.Controls.Add(this.groupBoxProcedure);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxSolution);
            this.Controls.Add(this.renderPanel);
            this.Name = "MainForm";
            this.Text = "LocateDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxSolution.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBoxProcedure.ResumeLayout(false);
            this.groupBoxProcedure.PerformLayout();
            this.resultPanel.ResumeLayout(false);
            this.groupBoxResult.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel renderPanel;
        private System.Windows.Forms.GroupBox groupBoxSolution;
        private System.Windows.Forms.Button buttonContiRun;
        private System.Windows.Forms.Button buttonSaveSolu;
        private System.Windows.Forms.Button buttonLoadSolu;
        private System.Windows.Forms.Button buttonRunOnce;
        private System.Windows.Forms.Button buttonSelectSolu;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.GroupBox groupBoxProcedure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboProcedure;
        private System.Windows.Forms.Button buttonRender;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Label labelResultState;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.ColumnHeader timeStampHeader;
        private System.Windows.Forms.ColumnHeader infoHeader;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClearMsg;
        private System.Windows.Forms.CheckBox chbxIsLoadPath;
    }
}

