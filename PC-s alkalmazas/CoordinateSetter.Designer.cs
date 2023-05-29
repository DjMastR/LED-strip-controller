namespace Client_program
{
    partial class CoordinateSetter
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
            XCoord = new TextBox();
            YCoord = new TextBox();
            Reorder_button = new Button();
            Place_button = new Button();
            SuspendLayout();
            // 
            // XCoord
            // 
            XCoord.Location = new Point(12, 12);
            XCoord.Name = "XCoord";
            XCoord.Size = new Size(64, 23);
            XCoord.TabIndex = 0;
            // 
            // YCoord
            // 
            YCoord.Location = new Point(82, 12);
            YCoord.Name = "YCoord";
            YCoord.Size = new Size(64, 23);
            YCoord.TabIndex = 1;
            // 
            // Reorder_button
            // 
            Reorder_button.Location = new Point(216, 12);
            Reorder_button.Name = "Reorder_button";
            Reorder_button.Size = new Size(58, 23);
            Reorder_button.TabIndex = 2;
            Reorder_button.Text = "Reorder";
            Reorder_button.UseVisualStyleBackColor = true;
            Reorder_button.Click += Reorder_button_Click;
            // 
            // Place_button
            // 
            Place_button.Location = new Point(152, 12);
            Place_button.Name = "Place_button";
            Place_button.Size = new Size(58, 23);
            Place_button.TabIndex = 3;
            Place_button.Text = "Place";
            Place_button.UseVisualStyleBackColor = true;
            Place_button.Click += Place_button_Click;
            // 
            // CoordinateSetter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(280, 41);
            Controls.Add(Place_button);
            Controls.Add(Reorder_button);
            Controls.Add(YCoord);
            Controls.Add(XCoord);
            Name = "CoordinateSetter";
            Text = "CoordinateSetter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox XCoord;
        private TextBox YCoord;
        private Button Reorder_button;
        private Button Place_button;
    }
}