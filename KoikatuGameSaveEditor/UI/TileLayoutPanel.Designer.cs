using System.ComponentModel;

namespace KGSE.UI {
    partial class TileLayoutPanel {

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
            this.scroll = new System.Windows.Forms.VScrollBar();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // scroll
            // 
            this.scroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scroll.LargeChange = 101;
            this.scroll.Location = new System.Drawing.Point(434, 0);
            this.scroll.Name = "scroll";
            this.scroll.Size = new System.Drawing.Size(20, 300);
            this.scroll.SmallChange = 50;
            this.scroll.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(430, 300);
            this.panel.TabIndex = 1;
            // 
            // TileLayoutPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.scroll);
            this.Name = "TileLayoutPanel";
            this.Size = new System.Drawing.Size(450, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar scroll;
        private System.Windows.Forms.Panel panel;
    }
}

