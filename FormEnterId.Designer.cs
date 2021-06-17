namespace Rawr
{
    partial class FormEnterId
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textItemId = new System.Windows.Forms.TextBox();
            this.lblTextAddItemID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(163, 70);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(82, 70);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textItemId
            // 
            this.textItemId.Location = new System.Drawing.Point(15, 44);
            this.textItemId.Name = "textItemId";
            this.textItemId.Size = new System.Drawing.Size(297, 20);
            this.textItemId.TabIndex = 3;
            // 
            // lblTextAddItemID
            // 
            this.lblTextAddItemID.AutoSize = true;
            this.lblTextAddItemID.Location = new System.Drawing.Point(12, 9);
            this.lblTextAddItemID.Name = "lblTextAddItemID";
            this.lblTextAddItemID.Size = new System.Drawing.Size(302, 26);
            this.lblTextAddItemID.TabIndex = 4;
            this.lblTextAddItemID.Text = "Automatic item loading is not working.\r\nPress Ok and you\'ll be prompted to manual" +
    "ly create a new item";
            // 
            // FormEnterId
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 105);
            this.ControlBox = false;
            this.Controls.Add(this.lblTextAddItemID);
            this.Controls.Add(this.textItemId);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormEnterId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add item";
            this.Load += new System.EventHandler(this.FormEnterId_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textItemId;
        private System.Windows.Forms.Label lblTextAddItemID;
    }
}