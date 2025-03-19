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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            label1 = new System.Windows.Forms.Label();
            TxtValorBruto = new System.Windows.Forms.TextBox();
            TxtDescontoInss = new System.Windows.Forms.Label();
            TxtBaseInss = new System.Windows.Forms.TextBox();
            TxtQtdDependente = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            BtnCalcular = new System.Windows.Forms.Button();
            BtnTabelaIRRF = new System.Windows.Forms.Button();
            BtnTabelaValSimplificado = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            RTxtResultado = new System.Windows.Forms.RichTextBox();
            BtnDependente = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            label4 = new System.Windows.Forms.Label();
            BtnTabelaINSS = new System.Windows.Forms.Button();
            BtnDescMinimo = new System.Windows.Forms.Button();
            BtnPensao = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(100, 10);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(78, 15);
            label1.TabIndex = 0;
            label1.Text = "Valor Bruto IR";
            // 
            // TxtValorBruto
            // 
            TxtValorBruto.Location = new System.Drawing.Point(100, 29);
            TxtValorBruto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtValorBruto.Name = "TxtValorBruto";
            TxtValorBruto.Size = new System.Drawing.Size(103, 23);
            TxtValorBruto.TabIndex = 1;
            TxtValorBruto.Text = "0,00";
            TxtValorBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtValorBruto.TextChanged += TxtValorBruto_TextChanged;
            TxtValorBruto.Enter += TxtValorBruto_Enter;
            TxtValorBruto.Leave += TxtValorBruto_Leave;
            // 
            // TxtDescontoInss
            // 
            TxtDescontoInss.AutoSize = true;
            TxtDescontoInss.Location = new System.Drawing.Point(211, 10);
            TxtDescontoInss.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TxtDescontoInss.Name = "TxtDescontoInss";
            TxtDescontoInss.Size = new System.Drawing.Size(58, 15);
            TxtDescontoInss.TabIndex = 2;
            TxtDescontoInss.Text = "Base INSS";
            // 
            // TxtBaseInss
            // 
            TxtBaseInss.Location = new System.Drawing.Point(211, 29);
            TxtBaseInss.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtBaseInss.Name = "TxtBaseInss";
            TxtBaseInss.Size = new System.Drawing.Size(103, 23);
            TxtBaseInss.TabIndex = 2;
            TxtBaseInss.Text = "0,00";
            TxtBaseInss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtBaseInss.TextChanged += TxtDescInss_TextChanged;
            TxtBaseInss.Enter += TxtDescInss_Enter;
            TxtBaseInss.Leave += TxtDescInss_Leave;
            // 
            // TxtQtdDependente
            // 
            TxtQtdDependente.Location = new System.Drawing.Point(322, 29);
            TxtQtdDependente.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtQtdDependente.Name = "TxtQtdDependente";
            TxtQtdDependente.Size = new System.Drawing.Size(103, 23);
            TxtQtdDependente.TabIndex = 3;
            TxtQtdDependente.Text = "0";
            TxtQtdDependente.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtQtdDependente.TextChanged += TxtQtdDependente_TextChanged;
            TxtQtdDependente.Enter += TxtQtdDependente_Enter;
            TxtQtdDependente.Leave += TxtQtdDependente_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(322, 10);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(97, 15);
            label3.TabIndex = 2;
            label3.Text = "Qtd. Dependente";
            // 
            // BtnCalcular
            // 
            BtnCalcular.FlatAppearance.BorderColor = System.Drawing.Color.DarkGreen;
            BtnCalcular.FlatAppearance.BorderSize = 2;
            BtnCalcular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            BtnCalcular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGreen;
            BtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnCalcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            BtnCalcular.Location = new System.Drawing.Point(13, 69);
            BtnCalcular.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnCalcular.Name = "BtnCalcular";
            BtnCalcular.Size = new System.Drawing.Size(302, 38);
            BtnCalcular.TabIndex = 4;
            BtnCalcular.Text = "&Calcular";
            BtnCalcular.UseVisualStyleBackColor = true;
            BtnCalcular.Click += BtnCalcular_Click;
            // 
            // BtnTabelaIRRF
            // 
            BtnTabelaIRRF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            BtnTabelaIRRF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            BtnTabelaIRRF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            BtnTabelaIRRF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            BtnTabelaIRRF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnTabelaIRRF.Location = new System.Drawing.Point(584, 14);
            BtnTabelaIRRF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnTabelaIRRF.Name = "BtnTabelaIRRF";
            BtnTabelaIRRF.Size = new System.Drawing.Size(105, 27);
            BtnTabelaIRRF.TabIndex = 6;
            BtnTabelaIRRF.Text = "IRRF";
            BtnTabelaIRRF.UseVisualStyleBackColor = true;
            BtnTabelaIRRF.Click += BtnTabelaIRRF_Click;
            // 
            // BtnTabelaValSimplificado
            // 
            BtnTabelaValSimplificado.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            BtnTabelaValSimplificado.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            BtnTabelaValSimplificado.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            BtnTabelaValSimplificado.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            BtnTabelaValSimplificado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnTabelaValSimplificado.Location = new System.Drawing.Point(471, 81);
            BtnTabelaValSimplificado.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnTabelaValSimplificado.Name = "BtnTabelaValSimplificado";
            BtnTabelaValSimplificado.Size = new System.Drawing.Size(218, 27);
            BtnTabelaValSimplificado.TabIndex = 9;
            BtnTabelaValSimplificado.Text = "Valor Simplificado";
            BtnTabelaValSimplificado.UseVisualStyleBackColor = true;
            BtnTabelaValSimplificado.Click += BtnTabelaValSimplificado_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(RTxtResultado);
            groupBox1.Location = new System.Drawing.Point(13, 114);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(677, 493);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Calculo do IR";
            // 
            // RTxtResultado
            // 
            RTxtResultado.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            RTxtResultado.Location = new System.Drawing.Point(7, 14);
            RTxtResultado.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RTxtResultado.Name = "RTxtResultado";
            RTxtResultado.ReadOnly = true;
            RTxtResultado.Size = new System.Drawing.Size(662, 471);
            RTxtResultado.TabIndex = 0;
            RTxtResultado.Text = "";
            // 
            // BtnDependente
            // 
            BtnDependente.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            BtnDependente.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            BtnDependente.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            BtnDependente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            BtnDependente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnDependente.Location = new System.Drawing.Point(584, 47);
            BtnDependente.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnDependente.Name = "BtnDependente";
            BtnDependente.Size = new System.Drawing.Size(105, 27);
            BtnDependente.TabIndex = 8;
            BtnDependente.Text = "Dependente";
            BtnDependente.UseVisualStyleBackColor = true;
            BtnDependente.Click += BtnDependente_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(13, 10);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 15);
            label2.TabIndex = 7;
            label2.Text = "Competência";
            // 
            // MktCompetencia
            // 
            MktCompetencia.Location = new System.Drawing.Point(13, 29);
            MktCompetencia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MktCompetencia.Mask = "00/0000";
            MktCompetencia.Name = "MktCompetencia";
            MktCompetencia.Size = new System.Drawing.Size(80, 23);
            MktCompetencia.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(2, 610);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(190, 15);
            label4.TabIndex = 9;
            label4.Text = "Calculadora de IR - Maycon Wisley";
            // 
            // BtnTabelaINSS
            // 
            BtnTabelaINSS.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            BtnTabelaINSS.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            BtnTabelaINSS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            BtnTabelaINSS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            BtnTabelaINSS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnTabelaINSS.Location = new System.Drawing.Point(471, 14);
            BtnTabelaINSS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnTabelaINSS.Name = "BtnTabelaINSS";
            BtnTabelaINSS.Size = new System.Drawing.Size(105, 27);
            BtnTabelaINSS.TabIndex = 5;
            BtnTabelaINSS.Text = "INSS";
            BtnTabelaINSS.UseVisualStyleBackColor = true;
            BtnTabelaINSS.Click += BtnTabelaINSS_Click;
            // 
            // BtnDescMinimo
            // 
            BtnDescMinimo.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            BtnDescMinimo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            BtnDescMinimo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            BtnDescMinimo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            BtnDescMinimo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnDescMinimo.Location = new System.Drawing.Point(471, 47);
            BtnDescMinimo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnDescMinimo.Name = "BtnDescMinimo";
            BtnDescMinimo.Size = new System.Drawing.Size(105, 27);
            BtnDescMinimo.TabIndex = 7;
            BtnDescMinimo.Text = "Desc. Minimo";
            BtnDescMinimo.UseVisualStyleBackColor = true;
            BtnDescMinimo.Click += BtnDescMinimo_Click;
            // 
            // BtnPensao
            // 
            BtnPensao.Enabled = false;
            BtnPensao.FlatAppearance.BorderColor = System.Drawing.Color.DarkGreen;
            BtnPensao.FlatAppearance.BorderSize = 2;
            BtnPensao.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            BtnPensao.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGreen;
            BtnPensao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnPensao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            BtnPensao.Location = new System.Drawing.Point(322, 69);
            BtnPensao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnPensao.Name = "BtnPensao";
            BtnPensao.Size = new System.Drawing.Size(104, 38);
            BtnPensao.TabIndex = 11;
            BtnPensao.Text = "&Pensão";
            BtnPensao.UseVisualStyleBackColor = true;
            BtnPensao.Click += BtnPensao_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(704, 636);
            Controls.Add(BtnPensao);
            Controls.Add(label4);
            Controls.Add(MktCompetencia);
            Controls.Add(label2);
            Controls.Add(BtnDescMinimo);
            Controls.Add(BtnDependente);
            Controls.Add(groupBox1);
            Controls.Add(BtnTabelaValSimplificado);
            Controls.Add(BtnTabelaINSS);
            Controls.Add(BtnTabelaIRRF);
            Controls.Add(BtnCalcular);
            Controls.Add(label3);
            Controls.Add(TxtQtdDependente);
            Controls.Add(TxtDescontoInss);
            Controls.Add(TxtBaseInss);
            Controls.Add(TxtValorBruto);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Calculadora Novo IR";
            Load += FrmPrincipal_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtValorBruto;
        private System.Windows.Forms.Label TxtDescontoInss;
        private System.Windows.Forms.TextBox TxtBaseInss;
        private System.Windows.Forms.TextBox TxtQtdDependente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnCalcular;
        private System.Windows.Forms.Button BtnTabelaIRRF;
        private System.Windows.Forms.Button BtnTabelaValSimplificado;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox RTxtResultado;
        private System.Windows.Forms.Button BtnDependente;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnTabelaINSS;
        private System.Windows.Forms.Button BtnDescMinimo;
        private System.Windows.Forms.Button BtnPensao;
    }
}

