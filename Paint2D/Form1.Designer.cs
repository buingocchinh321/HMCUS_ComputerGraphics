
namespace Paint2D
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
            this.components = new System.ComponentModel.Container();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.bt_Line = new System.Windows.Forms.Button();
            this.bt_Circle = new System.Windows.Forms.Button();
            this.bt_Rectangle = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.bt_Color = new System.Windows.Forms.Button();
            this.bt_Square = new System.Windows.Forms.Button();
            this.bt_Ellipse = new System.Windows.Forms.Button();
            this.bt_Pen = new System.Windows.Forms.Button();
            this.bt_Hex = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.bt_Clear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.bt_poly = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Location = new System.Drawing.Point(13, 13);
            this.openGLControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(755, 495);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseClick);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // bt_Line
            // 
            this.bt_Line.Location = new System.Drawing.Point(950, 41);
            this.bt_Line.Name = "bt_Line";
            this.bt_Line.Size = new System.Drawing.Size(85, 23);
            this.bt_Line.TabIndex = 1;
            this.bt_Line.Text = "Line";
            this.bt_Line.UseVisualStyleBackColor = true;
            this.bt_Line.Click += new System.EventHandler(this.bt_Line_Click);
            // 
            // bt_Circle
            // 
            this.bt_Circle.Location = new System.Drawing.Point(1065, 41);
            this.bt_Circle.Name = "bt_Circle";
            this.bt_Circle.Size = new System.Drawing.Size(85, 23);
            this.bt_Circle.TabIndex = 4;
            this.bt_Circle.Text = "Circle";
            this.bt_Circle.UseVisualStyleBackColor = true;
            this.bt_Circle.Click += new System.EventHandler(this.bt_Circle_Click);
            // 
            // bt_Rectangle
            // 
            this.bt_Rectangle.Location = new System.Drawing.Point(950, 84);
            this.bt_Rectangle.Name = "bt_Rectangle";
            this.bt_Rectangle.Size = new System.Drawing.Size(85, 23);
            this.bt_Rectangle.TabIndex = 5;
            this.bt_Rectangle.Text = "Rectangle";
            this.bt_Rectangle.UseVisualStyleBackColor = true;
            this.bt_Rectangle.Click += new System.EventHandler(this.bt_Rectangle_Click);
            // 
            // bt_Color
            // 
            this.bt_Color.Location = new System.Drawing.Point(801, 41);
            this.bt_Color.Name = "bt_Color";
            this.bt_Color.Size = new System.Drawing.Size(85, 23);
            this.bt_Color.TabIndex = 6;
            this.bt_Color.Text = "Color";
            this.bt_Color.UseVisualStyleBackColor = true;
            this.bt_Color.Click += new System.EventHandler(this.bt_Color_Click);
            // 
            // bt_Square
            // 
            this.bt_Square.Location = new System.Drawing.Point(1065, 84);
            this.bt_Square.Name = "bt_Square";
            this.bt_Square.Size = new System.Drawing.Size(85, 23);
            this.bt_Square.TabIndex = 7;
            this.bt_Square.Text = "Square";
            this.bt_Square.UseVisualStyleBackColor = true;
            this.bt_Square.Click += new System.EventHandler(this.Square_Click);
            // 
            // bt_Ellipse
            // 
            this.bt_Ellipse.Location = new System.Drawing.Point(950, 127);
            this.bt_Ellipse.Name = "bt_Ellipse";
            this.bt_Ellipse.Size = new System.Drawing.Size(85, 23);
            this.bt_Ellipse.TabIndex = 8;
            this.bt_Ellipse.Text = "Ellipse";
            this.bt_Ellipse.UseVisualStyleBackColor = true;
            this.bt_Ellipse.Click += new System.EventHandler(this.bt_Ellipse_Click);
            // 
            // bt_Pen
            // 
            this.bt_Pen.Location = new System.Drawing.Point(1065, 127);
            this.bt_Pen.Name = "bt_Pen";
            this.bt_Pen.Size = new System.Drawing.Size(85, 23);
            this.bt_Pen.TabIndex = 9;
            this.bt_Pen.Text = "Pentagon";
            this.bt_Pen.UseVisualStyleBackColor = true;
            this.bt_Pen.Click += new System.EventHandler(this.bt_Pen_Click);
            // 
            // bt_Hex
            // 
            this.bt_Hex.Location = new System.Drawing.Point(1065, 178);
            this.bt_Hex.Name = "bt_Hex";
            this.bt_Hex.Size = new System.Drawing.Size(85, 23);
            this.bt_Hex.TabIndex = 10;
            this.bt_Hex.Text = "Hexagon";
            this.bt_Hex.UseVisualStyleBackColor = true;
            this.bt_Hex.Click += new System.EventHandler(this.bt_Hex_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Location = new System.Drawing.Point(857, 128);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 22);
            this.numericUpDown1.TabIndex = 11;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bt_Clear
            // 
            this.bt_Clear.Location = new System.Drawing.Point(799, 84);
            this.bt_Clear.Name = "bt_Clear";
            this.bt_Clear.Size = new System.Drawing.Size(85, 23);
            this.bt_Clear.TabIndex = 12;
            this.bt_Clear.Text = "Clear";
            this.bt_Clear.UseVisualStyleBackColor = true;
            this.bt_Clear.Click += new System.EventHandler(this.bt_Clear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(801, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "Size";
            // 
            // timeBox
            // 
            this.timeBox.Location = new System.Drawing.Point(843, 178);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(100, 22);
            this.timeBox.TabIndex = 14;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(775, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "TimeBox";
            // 
            // bt_poly
            // 
            this.bt_poly.Location = new System.Drawing.Point(950, 177);
            this.bt_poly.Name = "bt_poly";
            this.bt_poly.Size = new System.Drawing.Size(85, 23);
            this.bt_poly.TabIndex = 16;
            this.bt_poly.Text = "Polygons";
            this.bt_poly.UseVisualStyleBackColor = true;
            this.bt_poly.Click += new System.EventHandler(this.bt_poly_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Scanline",
            "Floodfill"});
            this.comboBox1.Location = new System.Drawing.Point(799, 223);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.moveToolStripMenuItem,
            this.rotateToolStripMenuItem,
            this.zoomToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 128);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.clearToolStripMenuItem.Text = "Clear";
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.moveToolStripMenuItem.Text = "Move";
            // 
            // rotateToolStripMenuItem
            // 
            this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
            this.rotateToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.rotateToolStripMenuItem.Text = "Rotate";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1199, 521);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bt_poly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Clear);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.bt_Hex);
            this.Controls.Add(this.bt_Pen);
            this.Controls.Add(this.bt_Ellipse);
            this.Controls.Add(this.bt_Square);
            this.Controls.Add(this.bt_Color);
            this.Controls.Add(this.bt_Rectangle);
            this.Controls.Add(this.bt_Circle);
            this.Controls.Add(this.bt_Line);
            this.Controls.Add(this.openGLControl);
            this.Name = "Form1";
            this.Text = "DHMT_Lab1_19127109";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Button bt_Line;
        private System.Windows.Forms.Button bt_Circle;
        private System.Windows.Forms.Button bt_Rectangle;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button bt_Color;
        private System.Windows.Forms.Button bt_Square;
        private System.Windows.Forms.Button bt_Ellipse;
        private System.Windows.Forms.Button bt_Pen;
        private System.Windows.Forms.Button bt_Hex;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button bt_Clear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_poly;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
    }
}

