namespace CalculoIRRF
{
    partial class FrmPrincipal
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtBase = new System.Windows.Forms.TextBox();
            this.TxtDescontoInss = new System.Windows.Forms.Label();
            this.TxtDescInss = new System.Windows.Forms.TextBox();
            this.TxtQtdDependente = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnCalcular = new System.Windows.Forms.Button();
            this.BtnTabelaIRRF = new System.Windows.Forms.Button();
            this.BtnTabelaValSimplificado = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RTxtResultado = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base";
            // 
            // TxtBase
            // 
            this.TxtBase.Location = new System.Drawing.Point(12, 25);
            this.TxtBase.Name = "TxtBase";
            this.TxtBase.Size = new System.Drawing.Size(89, 20);
            this.TxtBase.TabIndex = 1;
            this.TxtBase.Text = "0,00";
            this.TxtBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TxtDescontoInss
            // 
            this.TxtDescontoInss.AutoSize = true;
            this.TxtDescontoInss.Location = new System.Drawing.Point(202, 9);
            this.TxtDescontoInss.Name = "TxtDescontoInss";
            this.TxtDescontoInss.Size = new System.Drawing.Size(63, 13);
            this.TxtDescontoInss.TabIndex = 2;
            this.TxtDescontoInss.Text = "Desc. INSS";
            // 
            // TxtDescInss
            // 
            this.TxtDescInss.Location = new System.Drawing.Point(202, 25);
            this.TxtDescInss.Name = "TxtDescInss";
            this.TxtDescInss.Size = new System.Drawing.Size(89, 20);
            this.TxtDescInss.TabIndex = 1;
            this.TxtDescInss.Text = "0,00";
            this.TxtDescInss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TxtQtdDependente
            // 
            this.TxtQtdDependente.Location = new System.Drawing.Point(107, 25);
            this.TxtQtdDependente.Name = "TxtQtdDependente";
            this.TxtQtdDependente.Size = new System.Drawing.Size(89, 20);
            this.TxtQtdDependente.TabIndex = 1;
            this.TxtQtdDependente.Text = "0";
            this.TxtQtdDependente.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Qtd. Dependente";
            // 
            // BtnCalcular
            // 
            this.BtnCalcular.Location = new System.Drawing.Point(12, 51);
            this.BtnCalcular.Name = "BtnCalcular";
            this.BtnCalcular.Size = new System.Drawing.Size(279, 33);
            this.BtnCalcular.TabIndex = 3;
            this.BtnCalcular.Text = "&Calcular";
            this.BtnCalcular.UseVisualStyleBackColor = true;
            // 
            // BtnTabelaIRRF
            // 
            this.BtnTabelaIRRF.Location = new System.Drawing.Point(345, 32);
            this.BtnTabelaIRRF.Name = "BtnTabelaIRRF";
            this.BtnTabelaIRRF.Size = new System.Drawing.Size(112, 23);
            this.BtnTabelaIRRF.TabIndex = 4;
            this.BtnTabelaIRRF.Text = "IRRF";
            this.BtnTabelaIRRF.UseVisualStyleBackColor = true;
            this.BtnTabelaIRRF.Click += new System.EventHandler(this.BtnTabelaIRRF_Click);
            // 
            // BtnTabelaValSimplificado
            // 
            this.BtnTabelaValSimplificado.Location = new System.Drawing.Point(345, 61);
            this.BtnTabelaValSimplificado.Name = "BtnTabelaValSimplificado";
            this.BtnTabelaValSimplificado.Size = new System.Drawing.Size(112, 23);
            this.BtnTabelaValSimplificado.TabIndex = 4;
            this.BtnTabelaValSimplificado.Text = "Valor Simplificado";
            this.BtnTabelaValSimplificado.UseVisualStyleBackColor = true;
            this.BtnTabelaValSimplificado.Click += new System.EventHandler(this.BtnTabelaValSimplificado_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RTxtResultado);
            this.groupBox1.Location = new System.Drawing.Point(12, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 349);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calculo do IRRF";
            // 
            // RTxtResultado
            // 
            this.RTxtResultado.Location = new System.Drawing.Point(6, 19);
            this.RTxtResultado.Name = "RTxtResultado";
            this.RTxtResultado.Size = new System.Drawing.Size(433, 324);
            this.RTxtResultado.TabIndex = 0;
            this.RTxtResultado.Text = "";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 472);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnTabelaValSimplificado);
            this.Controls.Add(this.BtnTabelaIRRF);
            this.Controls.Add(this.BtnCalcular);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtQtdDependente);
            this.Controls.Add(this.TxtDescontoInss);
            this.Controls.Add(this.TxtDescInss);
            this.Controls.Add(this.TxtBase);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculo IRRF";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBase;
        private System.Windows.Forms.Label TxtDescontoInss;
        private System.Windows.Forms.TextBox TxtDescInss;
        private System.Windows.Forms.TextBox TxtQtdDependente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnCalcular;
        private System.Windows.Forms.Button BtnTabelaIRRF;
        private System.Windows.Forms.Button BtnTabelaValSimplificado;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox RTxtResultado;
    }
}

