namespace AGV_Mock_Client
{
    partial class Mock_Client_UI
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
            this.camera = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // camera
            // 
            this.camera.Location = new System.Drawing.Point(70, 90);
            this.camera.Name = "camera";
            this.camera.Size = new System.Drawing.Size(107, 59);
            this.camera.TabIndex = 0;
            this.camera.Text = "Camera";
            this.camera.UseVisualStyleBackColor = true;
            this.camera.Click += new System.EventHandler(this.camera_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(70, 219);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 59);
            this.button2.TabIndex = 1;
            this.button2.Text = "Obstacles Detected";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.obstacles_detected_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(70, 342);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 59);
            this.button3.TabIndex = 2;
            this.button3.Text = "Wheels Sensor";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.wheels_sensor_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(362, 90);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(107, 59);
            this.button4.TabIndex = 3;
            this.button4.Text = "Ultrasonic sensor";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ultrasonic_sensor_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(362, 219);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(107, 59);
            this.button5.TabIndex = 4;
            this.button5.Text = "Line Tracker Sensor";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.line_tracker_sensor_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(362, 342);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(107, 59);
            this.button6.TabIndex = 5;
            this.button6.Text = "Resources";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Resources);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBox1.Location = new System.Drawing.Point(36, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(514, 27);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Click on any of the buttons to send a sample document to the database.";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Mock_Client_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 446);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.camera);
            this.Name = "Mock_Client_UI";
            this.Text = "Mock_Client_UI";
            this.Load += new System.EventHandler(this.Mock_Client_UI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button camera;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox1;
    }
}