﻿namespace smbx_npc_editor
{
    partial class SpinnerControlValue
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
            this.label = new System.Windows.Forms.Label();
            this.valueSpinner = new System.Windows.Forms.NumericUpDown();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.valueSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label.Location = new System.Drawing.Point(1, 1);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(79, 27);
            this.label.TabIndex = 18;
            this.label.Text = "Height:";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // valueSpinner
            // 
            this.valueSpinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.valueSpinner.Enabled = false;
            this.valueSpinner.Location = new System.Drawing.Point(86, 6);
            this.valueSpinner.Name = "valueSpinner";
            this.valueSpinner.Size = new System.Drawing.Size(109, 20);
            this.valueSpinner.TabIndex = 17;
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(203, 9);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(15, 14);
            this.enabledCheckBox.TabIndex = 16;
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.enabledCheckBox_CheckedChanged);
            // 
            // SpinnerControlValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label);
            this.Controls.Add(this.valueSpinner);
            this.Controls.Add(this.enabledCheckBox);
            this.Name = "SpinnerControlValue";
            this.Size = new System.Drawing.Size(228, 37);
            this.Load += new System.EventHandler(this.SpinnerControlValue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valueSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox enabledCheckBox;
        public System.Windows.Forms.NumericUpDown valueSpinner;
        public System.Windows.Forms.Label label;

    }
}
