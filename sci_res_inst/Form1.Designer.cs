namespace sci_res_inst
{
    partial class Main_page
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_1
            // 
            this.lb_1.AutoSize = true;
            this.lb_1.Location = new System.Drawing.Point(418, 137);
            this.lb_1.Name = "lb_1";
            this.lb_1.Size = new System.Drawing.Size(29, 13);
            this.lb_1.TabIndex = 0;
            this.lb_1.Text = "тест";
            this.lb_1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(500, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "тест";
            // 
            // Main_page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 601);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lb_1);
            this.Name = "Main_page";
            this.Text = "Научно исследовательский институт \"Научный Хаб\"";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_1;
        private System.Windows.Forms.Label label1;
    }
}

