using System.ComponentModel;

namespace KGSE.UI {
    partial class MaleCharacterControl {

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.photo = new System.Windows.Forms.PictureBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblNickname = new System.Windows.Forms.Label();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.lblIntellect = new System.Windows.Forms.Label();
            this.txtIntellect = new System.Windows.Forms.TextBox();
            this.lblStrength = new System.Windows.Forms.Label();
            this.txtStrength = new System.Windows.Forms.TextBox();
            this.lblH = new System.Windows.Forms.Label();
            this.txtH = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.photo)).BeginInit();
            this.SuspendLayout();
            // 
            // photo
            // 
            this.photo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.photo.Location = new System.Drawing.Point(2, 2);
            this.photo.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.photo.Name = "photo";
            this.photo.Size = new System.Drawing.Size(140, 200);
            this.photo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.photo.TabIndex = 0;
            this.photo.TabStop = false;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(262, 2);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(125, 23);
            this.txtLastName.TabIndex = 1;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(262, 29);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(125, 23);
            this.txtFirstName.TabIndex = 2;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(152, 5);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(83, 17);
            this.lblLastName.TabIndex = 3;
            this.lblLastName.Text = "Family Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(152, 32);
            this.lblFirstName.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(79, 17);
            this.lblFirstName.TabIndex = 4;
            this.lblFirstName.Text = "Given Name";
            // 
            // lblNickname
            // 
            this.lblNickname.AutoSize = true;
            this.lblNickname.Location = new System.Drawing.Point(152, 59);
            this.lblNickname.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(66, 17);
            this.lblNickname.TabIndex = 6;
            this.lblNickname.Text = "Nickname";
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(262, 56);
            this.txtNickname.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(125, 23);
            this.txtNickname.TabIndex = 5;
            // 
            // lblIntellect
            // 
            this.lblIntellect.AutoSize = true;
            this.lblIntellect.Location = new System.Drawing.Point(152, 86);
            this.lblIntellect.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblIntellect.Name = "lblIntellect";
            this.lblIntellect.Size = new System.Drawing.Size(53, 17);
            this.lblIntellect.TabIndex = 8;
            this.lblIntellect.Text = "Intellect";
            // 
            // txtIntellect
            // 
            this.txtIntellect.Location = new System.Drawing.Point(262, 83);
            this.txtIntellect.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtIntellect.Name = "txtIntellect";
            this.txtIntellect.Size = new System.Drawing.Size(125, 23);
            this.txtIntellect.TabIndex = 7;
            // 
            // lblStrength
            // 
            this.lblStrength.AutoSize = true;
            this.lblStrength.Location = new System.Drawing.Point(152, 113);
            this.lblStrength.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblStrength.Name = "lblStrength";
            this.lblStrength.Size = new System.Drawing.Size(57, 17);
            this.lblStrength.TabIndex = 10;
            this.lblStrength.Text = "Strength";
            // 
            // txtStrength
            // 
            this.txtStrength.Location = new System.Drawing.Point(262, 110);
            this.txtStrength.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtStrength.Name = "txtStrength";
            this.txtStrength.Size = new System.Drawing.Size(125, 23);
            this.txtStrength.TabIndex = 9;
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Location = new System.Drawing.Point(152, 140);
            this.lblH.Margin = new System.Windows.Forms.Padding(41, 0, 41, 0);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(17, 17);
            this.lblH.TabIndex = 12;
            this.lblH.Text = "H";
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(262, 137);
            this.txtH.Margin = new System.Windows.Forms.Padding(41, 17, 41, 17);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(125, 23);
            this.txtH.TabIndex = 11;
            // 
            // MaleCharacterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblH);
            this.Controls.Add(this.txtH);
            this.Controls.Add(this.lblStrength);
            this.Controls.Add(this.txtStrength);
            this.Controls.Add(this.lblIntellect);
            this.Controls.Add(this.txtIntellect);
            this.Controls.Add(this.lblNickname);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.photo);
            this.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(41, 23, 41, 23);
            this.Name = "MaleCharacterControl";
            this.Size = new System.Drawing.Size(630, 330);
            ((System.ComponentModel.ISupportInitialize)(this.photo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.Label lblStrength;
        private System.Windows.Forms.Label lblIntellect;
        private System.Windows.Forms.Label lblNickname;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.TextBox txtStrength;
        private System.Windows.Forms.TextBox txtIntellect;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.TextBox txtLastName;

        private System.Windows.Forms.PictureBox photo;

    }
}

