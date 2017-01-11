namespace SnakeClient
{
  partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.connectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.scoreBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colorButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.spectateComboBox = new System.Windows.Forms.ComboBox();
            this.drawingPanel1 = new DrawingPanel.DrawingPanel();
            this.WelcomePanel = new System.Windows.Forms.Panel();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.serverAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.drawingPanel1.SuspendLayout();
            this.WelcomePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.connectButton.Location = new System.Drawing.Point(366, 12);
            this.connectButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(140, 27);
            this.connectButton.TabIndex = 6;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(67, 15);
            this.nameBox.Margin = new System.Windows.Forms.Padding(4);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(132, 22);
            this.nameBox.TabIndex = 8;
            this.nameBox.Text = "Player 1";
            // 
            // scoreBox
            // 
            this.scoreBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.scoreBox.Enabled = false;
            this.scoreBox.FormattingEnabled = true;
            this.scoreBox.ItemHeight = 16;
            this.scoreBox.Location = new System.Drawing.Point(598, 0);
            this.scoreBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scoreBox.Name = "scoreBox";
            this.scoreBox.Size = new System.Drawing.Size(159, 356);
            this.scoreBox.TabIndex = 10;
            this.scoreBox.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.colorButton);
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Controls.Add(this.nameBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(598, 46);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // colorButton
            // 
            this.colorButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.colorButton.Location = new System.Drawing.Point(221, 12);
            this.colorButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(138, 27);
            this.colorButton.TabIndex = 9;
            this.colorButton.Text = "Color";
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // spectateComboBox
            // 
            this.spectateComboBox.FormattingEnabled = true;
            this.spectateComboBox.Location = new System.Drawing.Point(512, 13);
            this.spectateComboBox.Name = "spectateComboBox";
            this.spectateComboBox.Size = new System.Drawing.Size(121, 24);
            this.spectateComboBox.TabIndex = 10;
            this.spectateComboBox.Visible = false;
            this.spectateComboBox.DropDown += new System.EventHandler(this.spectateComboBox_DropDown);
            this.spectateComboBox.SelectedIndexChanged += new System.EventHandler(this.spectateComboBox_SelectionChanged);
            this.spectateComboBox.DropDownClosed += new System.EventHandler(this.spectateComboBox_SelectionChanged);
            // 
            // drawingPanel1
            // 
            this.drawingPanel1.AutoSize = true;
            this.drawingPanel1.BackgroundImage = global::ChatClient.Properties.Resources.grass03_0;
            this.drawingPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drawingPanel1.Controls.Add(this.WelcomePanel);
            this.drawingPanel1.Location = new System.Drawing.Point(8, 53);
            this.drawingPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.drawingPanel1.Name = "drawingPanel1";
            this.drawingPanel1.Size = new System.Drawing.Size(754, 296);
            this.drawingPanel1.TabIndex = 9;
            // 
            // WelcomePanel
            // 
            this.WelcomePanel.Controls.Add(this.richTextBox2);
            this.WelcomePanel.Controls.Add(this.richTextBox1);
            this.WelcomePanel.Controls.Add(this.serverAddress);
            this.WelcomePanel.Controls.Add(this.label1);
            this.WelcomePanel.Controls.Add(this.pictureBox1);
            this.WelcomePanel.Location = new System.Drawing.Point(3, 2);
            this.WelcomePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.WelcomePanel.Name = "WelcomePanel";
            this.WelcomePanel.Size = new System.Drawing.Size(744, 286);
            this.WelcomePanel.TabIndex = 0;
            // 
            // richTextBox2
            // 
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Location = new System.Drawing.Point(19, 190);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(205, 96);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "Instructions:\n     Move North: ↑ -or- w\n     Move South: ↓ -or- s\n     Move East:" +
    "  → -or- d\n     Move West: ←-or- a\n";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(173, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(557, 169);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // serverAddress
            // 
            this.serverAddress.Location = new System.Drawing.Point(325, 234);
            this.serverAddress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.serverAddress.Name = "serverAddress";
            this.serverAddress.Size = new System.Drawing.Size(149, 22);
            this.serverAddress.TabIndex = 3;
            this.serverAddress.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ChatClient.Properties.Resources.SnakeGif;
            this.pictureBox1.Location = new System.Drawing.Point(19, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(149, 169);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(757, 356);
            this.Controls.Add(this.spectateComboBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.scoreBox);
            this.Controls.Add(this.drawingPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(298, 77);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.drawingPanel1.ResumeLayout(false);
            this.WelcomePanel.ResumeLayout(false);
            this.WelcomePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox serverAddress;
    private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameBox;
        private DrawingPanel.DrawingPanel drawingPanel1;
        private System.Windows.Forms.ListBox scoreBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel WelcomePanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox spectateComboBox;
    }
}

