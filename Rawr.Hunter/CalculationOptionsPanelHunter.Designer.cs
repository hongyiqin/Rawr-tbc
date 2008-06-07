﻿namespace Rawr.Hunter
{
    partial class CalculationOptionsPanelHunter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.label1 = new System.Windows.Forms.Label();
			this.cmbTargetLevel = new System.Windows.Forms.ComboBox();
			this.btnTalents = new System.Windows.Forms.Button();
			this.chkEnforceMetaGemRequirements = new System.Windows.Forms.CheckBox();
			this.comboActiveAspect = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboShotRotation = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Target Level:";
			// 
			// cmbTargetLevel
			// 
			this.cmbTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTargetLevel.FormattingEnabled = true;
			this.cmbTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
			this.cmbTargetLevel.Location = new System.Drawing.Point(79, 11);
			this.cmbTargetLevel.Name = "cmbTargetLevel";
			this.cmbTargetLevel.Size = new System.Drawing.Size(121, 21);
			this.cmbTargetLevel.TabIndex = 1;
			this.cmbTargetLevel.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLevel_SelectedIndexChanged);
			// 
			// btnTalents
			// 
			this.btnTalents.Location = new System.Drawing.Point(26, 52);
			this.btnTalents.Name = "btnTalents";
			this.btnTalents.Size = new System.Drawing.Size(167, 23);
			this.btnTalents.TabIndex = 2;
			this.btnTalents.Text = "Talents";
			this.btnTalents.UseVisualStyleBackColor = true;
			this.btnTalents.Click += new System.EventHandler(this.btnTalents_Click);
			// 
			// chkEnforceMetaGemRequirements
			// 
			this.chkEnforceMetaGemRequirements.AutoSize = true;
			this.chkEnforceMetaGemRequirements.Location = new System.Drawing.Point(6, 95);
			this.chkEnforceMetaGemRequirements.Name = "chkEnforceMetaGemRequirements";
			this.chkEnforceMetaGemRequirements.Size = new System.Drawing.Size(178, 17);
			this.chkEnforceMetaGemRequirements.TabIndex = 3;
			this.chkEnforceMetaGemRequirements.Text = "Enforce Metagem Requirements";
			this.chkEnforceMetaGemRequirements.UseVisualStyleBackColor = true;
			this.chkEnforceMetaGemRequirements.CheckedChanged += new System.EventHandler(this.chkEnforceMetaGemRequirements_CheckedChanged);
			// 
			// comboActiveAspect
			// 
			this.comboActiveAspect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboActiveAspect.FormattingEnabled = true;
			this.comboActiveAspect.Location = new System.Drawing.Point(79, 140);
			this.comboActiveAspect.Name = "comboActiveAspect";
			this.comboActiveAspect.Size = new System.Drawing.Size(121, 21);
			this.comboActiveAspect.TabIndex = 5;
			this.comboActiveAspect.SelectedIndexChanged += new System.EventHandler(this.comboActiveAspect_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 143);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Active Aspect:";
			// 
			// comboShotRotation
			// 
			this.comboShotRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShotRotation.FormattingEnabled = true;
			this.comboShotRotation.Location = new System.Drawing.Point(79, 167);
			this.comboShotRotation.Name = "comboShotRotation";
			this.comboShotRotation.Size = new System.Drawing.Size(121, 21);
			this.comboShotRotation.TabIndex = 7;
			this.comboShotRotation.SelectedIndexChanged += new System.EventHandler(this.comboShotRotation_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 170);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Shot Rotation:";
			// 
			// CalculationOptionsPanelHunter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboShotRotation);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboActiveAspect);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chkEnforceMetaGemRequirements);
			this.Controls.Add(this.btnTalents);
			this.Controls.Add(this.cmbTargetLevel);
			this.Controls.Add(this.label1);
			this.Name = "CalculationOptionsPanelHunter";
			this.Size = new System.Drawing.Size(206, 224);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTargetLevel;
        private System.Windows.Forms.Button btnTalents;
        private System.Windows.Forms.CheckBox chkEnforceMetaGemRequirements;
		private System.Windows.Forms.ComboBox comboActiveAspect;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboShotRotation;
		private System.Windows.Forms.Label label3;
    }
}
