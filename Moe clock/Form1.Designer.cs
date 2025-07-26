namespace Moe_clock
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            themeToolStripMenuItem = new ToolStripMenuItem();
            gelbooruToolStripMenuItem = new ToolStripMenuItem();
            gelbooruhToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { themeToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(112, 48);
            // 
            // themeToolStripMenuItem
            // 
            themeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gelbooruToolStripMenuItem, gelbooruhToolStripMenuItem });
            themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            themeToolStripMenuItem.Size = new Size(111, 22);
            themeToolStripMenuItem.Text = "Theme";
            // 
            // gelbooruToolStripMenuItem
            // 
            gelbooruToolStripMenuItem.Name = "gelbooruToolStripMenuItem";
            gelbooruToolStripMenuItem.Size = new Size(135, 22);
            gelbooruToolStripMenuItem.Text = "Gelbooru";
            // 
            // gelbooruhToolStripMenuItem
            // 
            gelbooruhToolStripMenuItem.Name = "gelbooruhToolStripMenuItem";
            gelbooruhToolStripMenuItem.Size = new Size(135, 22);
            gelbooruhToolStripMenuItem.Text = "Gelbooru-h";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(111, 22);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "Form1";
            Text = "Form1";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem themeToolStripMenuItem;
        private ToolStripMenuItem gelbooruhToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem gelbooruToolStripMenuItem;
    }
}
