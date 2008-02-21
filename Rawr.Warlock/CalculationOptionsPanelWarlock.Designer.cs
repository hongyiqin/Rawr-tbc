﻿namespace Rawr.Warlock
{
    partial class CalculationOptionsPanelWarlock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculationOptionsPanelWarlock));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTargetLevel = new System.Windows.Forms.ComboBox();
            this.checkBoxEnforceMetagemRequirements = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatency = new System.Windows.Forms.TextBox();
            this.comboBoxFilterSpell = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCastCurse = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPetSelection = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkSacraficed = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxISBUptime = new System.Windows.Forms.TextBox();
            this.comboBoxShadows = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxElements = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkMisery = new System.Windows.Forms.CheckBox();
            this.checkShadowWeaving = new System.Windows.Forms.CheckBox();
            this.checkScorch = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkUnstable = new System.Windows.Forms.CheckBox();
            this.checkSiphonLife = new System.Windows.Forms.CheckBox();
            this.checkCorruption = new System.Windows.Forms.CheckBox();
            this.checkImmolate = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxDuration = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Level: ";
            // 
            // comboBoxTargetLevel
            // 
            this.comboBoxTargetLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetLevel.FormattingEnabled = true;
            this.comboBoxTargetLevel.Items.AddRange(new object[] {
            "70",
            "71",
            "72",
            "73"});
            this.comboBoxTargetLevel.Location = new System.Drawing.Point(97, 3);
            this.comboBoxTargetLevel.Name = "comboBoxTargetLevel";
            this.comboBoxTargetLevel.Size = new System.Drawing.Size(93, 21);
            this.comboBoxTargetLevel.TabIndex = 1;
            this.comboBoxTargetLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetLevel_SelectedIndexChanged);
            // 
            // checkBoxEnforceMetagemRequirements
            // 
            this.checkBoxEnforceMetagemRequirements.AutoSize = true;
            this.checkBoxEnforceMetagemRequirements.Location = new System.Drawing.Point(12, 564);
            this.checkBoxEnforceMetagemRequirements.Name = "checkBoxEnforceMetagemRequirements";
            this.checkBoxEnforceMetagemRequirements.Size = new System.Drawing.Size(178, 17);
            this.checkBoxEnforceMetagemRequirements.TabIndex = 2;
            this.checkBoxEnforceMetagemRequirements.Text = "Enforce Metagem Requirements";
            this.checkBoxEnforceMetagemRequirements.UseVisualStyleBackColor = true;
            this.checkBoxEnforceMetagemRequirements.CheckedChanged += new System.EventHandler(this.checkBoxEnforceMetagemRequirements_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Latency:";
            // 
            // textBoxLatency
            // 
            this.textBoxLatency.Location = new System.Drawing.Point(97, 29);
            this.textBoxLatency.Name = "textBoxLatency";
            this.textBoxLatency.Size = new System.Drawing.Size(93, 20);
            this.textBoxLatency.TabIndex = 7;
            this.textBoxLatency.TextChanged += new System.EventHandler(this.textBoxLatency_TextChanged);
            // 
            // comboBoxFilterSpell
            // 
            this.comboBoxFilterSpell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilterSpell.FormattingEnabled = true;
            this.comboBoxFilterSpell.Items.AddRange(new object[] {
            "Shadowbolt"});
            this.comboBoxFilterSpell.Location = new System.Drawing.Point(91, 14);
            this.comboBoxFilterSpell.Name = "comboBoxFilterSpell";
            this.comboBoxFilterSpell.Size = new System.Drawing.Size(93, 21);
            this.comboBoxFilterSpell.TabIndex = 9;
            this.comboBoxFilterSpell.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilterSpell_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Filler Spell:";
            // 
            // comboBoxCastCurse
            // 
            this.comboBoxCastCurse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCastCurse.FormattingEnabled = true;
            this.comboBoxCastCurse.Items.AddRange(new object[] {
            "CoD",
            "CoA"});
            this.comboBoxCastCurse.Location = new System.Drawing.Point(91, 40);
            this.comboBoxCastCurse.Name = "comboBoxCastCurse";
            this.comboBoxCastCurse.Size = new System.Drawing.Size(93, 21);
            this.comboBoxCastCurse.TabIndex = 11;
            this.comboBoxCastCurse.SelectedIndexChanged += new System.EventHandler(this.comboBoxCastCurse_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cast Curse";
            // 
            // comboBoxPetSelection
            // 
            this.comboBoxPetSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPetSelection.FormattingEnabled = true;
            this.comboBoxPetSelection.Items.AddRange(new object[] {
            "Succubus",
            "Fellhunter",
            "Fellguard",
            "Imp",
            "Voidwalker"});
            this.comboBoxPetSelection.Location = new System.Drawing.Point(91, 20);
            this.comboBoxPetSelection.Name = "comboBoxPetSelection";
            this.comboBoxPetSelection.Size = new System.Drawing.Size(93, 21);
            this.comboBoxPetSelection.TabIndex = 13;
            this.comboBoxPetSelection.SelectedIndexChanged += new System.EventHandler(this.comboBoxPetSelection_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Pet";
            // 
            // checkSacraficed
            // 
            this.checkSacraficed.AutoSize = true;
            this.checkSacraficed.Location = new System.Drawing.Point(107, 47);
            this.checkSacraficed.Name = "checkSacraficed";
            this.checkSacraficed.Size = new System.Drawing.Size(77, 17);
            this.checkSacraficed.TabIndex = 14;
            this.checkSacraficed.Text = "Sacraficed";
            this.checkSacraficed.UseVisualStyleBackColor = true;
            this.checkSacraficed.CheckedChanged += new System.EventHandler(this.checkSacraficed_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxISBUptime);
            this.groupBox1.Controls.Add(this.comboBoxShadows);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxElements);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.checkMisery);
            this.groupBox1.Controls.Add(this.checkShadowWeaving);
            this.groupBox1.Controls.Add(this.checkScorch);
            this.groupBox1.Location = new System.Drawing.Point(6, 365);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 183);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Debuffs";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 145);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 26);
            this.label8.TabIndex = 19;
            this.label8.Text = "Raid ISB \r\nUptime";
            // 
            // textBoxISBUptime
            // 
            this.textBoxISBUptime.Location = new System.Drawing.Point(85, 145);
            this.textBoxISBUptime.Name = "textBoxISBUptime";
            this.textBoxISBUptime.Size = new System.Drawing.Size(93, 20);
            this.textBoxISBUptime.TabIndex = 18;
            this.textBoxISBUptime.TextChanged += new System.EventHandler(this.textBoxISBUptime_TextChanged);
            // 
            // comboBoxShadows
            // 
            this.comboBoxShadows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxShadows.FormattingEnabled = true;
            this.comboBoxShadows.Items.AddRange(new object[] {
            "1.0",
            "1.1",
            "1.11",
            "1.12",
            "1.13"});
            this.comboBoxShadows.Location = new System.Drawing.Point(85, 114);
            this.comboBoxShadows.Name = "comboBoxShadows";
            this.comboBoxShadows.Size = new System.Drawing.Size(93, 21);
            this.comboBoxShadows.TabIndex = 17;
            this.comboBoxShadows.SelectedIndexChanged += new System.EventHandler(this.comboBoxShadows_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "CoS";
            // 
            // comboBoxElements
            // 
            this.comboBoxElements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxElements.FormattingEnabled = true;
            this.comboBoxElements.Items.AddRange(new object[] {
            "1.0",
            "1.1",
            "1.11",
            "1.12",
            "1.13"});
            this.comboBoxElements.Location = new System.Drawing.Point(85, 87);
            this.comboBoxElements.Name = "comboBoxElements";
            this.comboBoxElements.Size = new System.Drawing.Size(93, 21);
            this.comboBoxElements.TabIndex = 15;
            this.comboBoxElements.SelectedIndexChanged += new System.EventHandler(this.comboBoxElements_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "CoE";
            // 
            // checkMisery
            // 
            this.checkMisery.AutoSize = true;
            this.checkMisery.Location = new System.Drawing.Point(6, 65);
            this.checkMisery.Name = "checkMisery";
            this.checkMisery.Size = new System.Drawing.Size(56, 17);
            this.checkMisery.TabIndex = 5;
            this.checkMisery.Text = "Misery";
            this.checkMisery.UseVisualStyleBackColor = true;
            this.checkMisery.CheckedChanged += new System.EventHandler(this.checkMisery_CheckedChanged);
            // 
            // checkShadowWeaving
            // 
            this.checkShadowWeaving.AutoSize = true;
            this.checkShadowWeaving.Location = new System.Drawing.Point(6, 42);
            this.checkShadowWeaving.Name = "checkShadowWeaving";
            this.checkShadowWeaving.Size = new System.Drawing.Size(111, 17);
            this.checkShadowWeaving.TabIndex = 4;
            this.checkShadowWeaving.Text = "Shadow Weaving";
            this.checkShadowWeaving.UseVisualStyleBackColor = true;
            this.checkShadowWeaving.CheckedChanged += new System.EventHandler(this.checkShadowWeaving_CheckedChanged);
            // 
            // checkScorch
            // 
            this.checkScorch.AutoSize = true;
            this.checkScorch.Location = new System.Drawing.Point(6, 19);
            this.checkScorch.Name = "checkScorch";
            this.checkScorch.Size = new System.Drawing.Size(101, 17);
            this.checkScorch.TabIndex = 3;
            this.checkScorch.Text = "Improved Sorch";
            this.checkScorch.UseVisualStyleBackColor = true;
            this.checkScorch.CheckedChanged += new System.EventHandler(this.checkScorch_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkUnstable);
            this.groupBox2.Controls.Add(this.checkSiphonLife);
            this.groupBox2.Controls.Add(this.checkCorruption);
            this.groupBox2.Controls.Add(this.checkImmolate);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBoxFilterSpell);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxCastCurse);
            this.groupBox2.Location = new System.Drawing.Point(6, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 146);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spell Choice";
            // 
            // checkUnstable
            // 
            this.checkUnstable.AutoSize = true;
            this.checkUnstable.Location = new System.Drawing.Point(80, 67);
            this.checkUnstable.Name = "checkUnstable";
            this.checkUnstable.Size = new System.Drawing.Size(111, 17);
            this.checkUnstable.TabIndex = 15;
            this.checkUnstable.Text = "Unstable Affliction";
            this.checkUnstable.UseVisualStyleBackColor = true;
            this.checkUnstable.CheckedChanged += new System.EventHandler(this.checkUnstable_CheckedChanged);
            // 
            // checkSiphonLife
            // 
            this.checkSiphonLife.AutoSize = true;
            this.checkSiphonLife.Location = new System.Drawing.Point(10, 113);
            this.checkSiphonLife.Name = "checkSiphonLife";
            this.checkSiphonLife.Size = new System.Drawing.Size(79, 17);
            this.checkSiphonLife.TabIndex = 14;
            this.checkSiphonLife.Text = "Siphon Life";
            this.checkSiphonLife.UseVisualStyleBackColor = true;
            this.checkSiphonLife.CheckedChanged += new System.EventHandler(this.checkSiphonLife_CheckedChanged);
            // 
            // checkCorruption
            // 
            this.checkCorruption.AutoSize = true;
            this.checkCorruption.Location = new System.Drawing.Point(10, 90);
            this.checkCorruption.Name = "checkCorruption";
            this.checkCorruption.Size = new System.Drawing.Size(74, 17);
            this.checkCorruption.TabIndex = 13;
            this.checkCorruption.Text = "Corruption";
            this.checkCorruption.UseVisualStyleBackColor = true;
            this.checkCorruption.CheckedChanged += new System.EventHandler(this.checkCorruption_CheckedChanged);
            // 
            // checkImmolate
            // 
            this.checkImmolate.AutoSize = true;
            this.checkImmolate.Location = new System.Drawing.Point(10, 67);
            this.checkImmolate.Name = "checkImmolate";
            this.checkImmolate.Size = new System.Drawing.Size(68, 17);
            this.checkImmolate.TabIndex = 12;
            this.checkImmolate.Text = "Immolate";
            this.checkImmolate.UseVisualStyleBackColor = true;
            this.checkImmolate.CheckedChanged += new System.EventHandler(this.checkImmolate_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxPetSelection);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.checkSacraficed);
            this.groupBox3.Location = new System.Drawing.Point(6, 283);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(191, 76);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pet Options";
            // 
            // textBoxDuration
            // 
            this.textBoxDuration.Location = new System.Drawing.Point(97, 55);
            this.textBoxDuration.Name = "textBoxDuration";
            this.textBoxDuration.Size = new System.Drawing.Size(93, 20);
            this.textBoxDuration.TabIndex = 19;
            this.textBoxDuration.TextChanged += new System.EventHandler(this.textBoxDuration_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Duration (s):";
            // 
            // CalculationOptionsPanelWarlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxDuration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxLatency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxEnforceMetagemRequirements);
            this.Controls.Add(this.comboBoxTargetLevel);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalculationOptionsPanelWarlock";
            this.Size = new System.Drawing.Size(208, 597);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxTargetLevel;
        private System.Windows.Forms.CheckBox checkBoxEnforceMetagemRequirements;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatency;
        private System.Windows.Forms.ComboBox comboBoxFilterSpell;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCastCurse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPetSelection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkSacraficed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxShadows;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxElements;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkMisery;
        private System.Windows.Forms.CheckBox checkShadowWeaving;
        private System.Windows.Forms.CheckBox checkScorch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkUnstable;
        private System.Windows.Forms.CheckBox checkSiphonLife;
        private System.Windows.Forms.CheckBox checkCorruption;
        private System.Windows.Forms.CheckBox checkImmolate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxISBUptime;
        private System.Windows.Forms.TextBox textBoxDuration;
        private System.Windows.Forms.Label label9;
	}
}