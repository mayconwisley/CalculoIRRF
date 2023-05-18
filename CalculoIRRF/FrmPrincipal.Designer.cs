﻿namespace CalculoIRRF
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtValorBruto = new System.Windows.Forms.TextBox();
            this.TxtDescontoInss = new System.Windows.Forms.Label();
            this.TxtDescInss = new System.Windows.Forms.TextBox();
            this.TxtQtdDependente = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnCalcular = new System.Windows.Forms.Button();
            this.BtnTabelaIRRF = new System.Windows.Forms.Button();
            this.BtnTabelaValSimplificado = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RTxtResultado = new System.Windows.Forms.RichTextBox();
            this.BtnDependente = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Valor Bruto";
            // 
            // TxtValorBruto
            // 
            this.TxtValorBruto.Location = new System.Drawing.Point(86, 25);
            this.TxtValorBruto.Name = "TxtValorBruto";
            this.TxtValorBruto.Size = new System.Drawing.Size(89, 20);
            this.TxtValorBruto.TabIndex = 1;
            this.TxtValorBruto.Text = "0,00";
            this.TxtValorBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtValorBruto.TextChanged += new System.EventHandler(this.TxtValorBruto_TextChanged);
            this.TxtValorBruto.Enter += new System.EventHandler(this.TxtValorBruto_Enter);
            this.TxtValorBruto.Leave += new System.EventHandler(this.TxtValorBruto_Leave);
            // 
            // TxtDescontoInss
            // 
            this.TxtDescontoInss.AutoSize = true;
            this.TxtDescontoInss.Location = new System.Drawing.Point(276, 9);
            this.TxtDescontoInss.Name = "TxtDescontoInss";
            this.TxtDescontoInss.Size = new System.Drawing.Size(63, 13);
            this.TxtDescontoInss.TabIndex = 2;
            this.TxtDescontoInss.Text = "Desc. INSS";
            // 
            // TxtDescInss
            // 
            this.TxtDescInss.Location = new System.Drawing.Point(276, 25);
            this.TxtDescInss.Name = "TxtDescInss";
            this.TxtDescInss.Size = new System.Drawing.Size(89, 20);
            this.TxtDescInss.TabIndex = 3;
            this.TxtDescInss.Text = "0,00";
            this.TxtDescInss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtDescInss.TextChanged += new System.EventHandler(this.TxtDescInss_TextChanged);
            this.TxtDescInss.Enter += new System.EventHandler(this.TxtDescInss_Enter);
            this.TxtDescInss.Leave += new System.EventHandler(this.TxtDescInss_Leave);
            // 
            // TxtQtdDependente
            // 
            this.TxtQtdDependente.Location = new System.Drawing.Point(181, 25);
            this.TxtQtdDependente.Name = "TxtQtdDependente";
            this.TxtQtdDependente.Size = new System.Drawing.Size(89, 20);
            this.TxtQtdDependente.TabIndex = 2;
            this.TxtQtdDependente.Text = "0";
            this.TxtQtdDependente.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtQtdDependente.TextChanged += new System.EventHandler(this.TxtQtdDependente_TextChanged);
            this.TxtQtdDependente.Enter += new System.EventHandler(this.TxtQtdDependente_Enter);
            this.TxtQtdDependente.Leave += new System.EventHandler(this.TxtQtdDependente_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Qtd. Dependente";
            // 
            // BtnCalcular
            // 
            this.BtnCalcular.FlatAppearance.BorderColor = System.Drawing.Color.DarkGreen;
            this.BtnCalcular.FlatAppearance.BorderSize = 2;
            this.BtnCalcular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnCalcular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
            this.BtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCalcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCalcular.Location = new System.Drawing.Point(11, 60);
            this.BtnCalcular.Name = "BtnCalcular";
            this.BtnCalcular.Size = new System.Drawing.Size(354, 33);
            this.BtnCalcular.TabIndex = 4;
            this.BtnCalcular.Text = "&Calcular";
            this.BtnCalcular.UseVisualStyleBackColor = true;
            this.BtnCalcular.Click += new System.EventHandler(this.BtnCalcular_Click);
            // 
            // BtnTabelaIRRF
            // 
            this.BtnTabelaIRRF.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.BtnTabelaIRRF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            this.BtnTabelaIRRF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.BtnTabelaIRRF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTabelaIRRF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTabelaIRRF.Location = new System.Drawing.Point(479, 12);
            this.BtnTabelaIRRF.Name = "BtnTabelaIRRF";
            this.BtnTabelaIRRF.Size = new System.Drawing.Size(112, 23);
            this.BtnTabelaIRRF.TabIndex = 5;
            this.BtnTabelaIRRF.Text = "IRRF";
            this.BtnTabelaIRRF.UseVisualStyleBackColor = true;
            this.BtnTabelaIRRF.Click += new System.EventHandler(this.BtnTabelaIRRF_Click);
            // 
            // BtnTabelaValSimplificado
            // 
            this.BtnTabelaValSimplificado.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.BtnTabelaValSimplificado.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            this.BtnTabelaValSimplificado.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.BtnTabelaValSimplificado.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTabelaValSimplificado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTabelaValSimplificado.Location = new System.Drawing.Point(479, 70);
            this.BtnTabelaValSimplificado.Name = "BtnTabelaValSimplificado";
            this.BtnTabelaValSimplificado.Size = new System.Drawing.Size(112, 23);
            this.BtnTabelaValSimplificado.TabIndex = 7;
            this.BtnTabelaValSimplificado.Text = "Valor Simplificado";
            this.BtnTabelaValSimplificado.UseVisualStyleBackColor = true;
            this.BtnTabelaValSimplificado.Click += new System.EventHandler(this.BtnTabelaValSimplificado_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RTxtResultado);
            this.groupBox1.Location = new System.Drawing.Point(11, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 349);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calculo do IRRF";
            // 
            // RTxtResultado
            // 
            this.RTxtResultado.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTxtResultado.Location = new System.Drawing.Point(6, 12);
            this.RTxtResultado.Name = "RTxtResultado";
            this.RTxtResultado.ReadOnly = true;
            this.RTxtResultado.Size = new System.Drawing.Size(568, 324);
            this.RTxtResultado.TabIndex = 0;
            this.RTxtResultado.Text = "";
            // 
            // BtnDependente
            // 
            this.BtnDependente.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.BtnDependente.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gray;
            this.BtnDependente.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.BtnDependente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnDependente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDependente.Location = new System.Drawing.Point(479, 41);
            this.BtnDependente.Name = "BtnDependente";
            this.BtnDependente.Size = new System.Drawing.Size(112, 23);
            this.BtnDependente.TabIndex = 6;
            this.BtnDependente.Text = "Dependente";
            this.BtnDependente.UseVisualStyleBackColor = true;
            this.BtnDependente.Click += new System.EventHandler(this.BtnDependente_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Competência";
            // 
            // MktCompetencia
            // 
            this.MktCompetencia.Location = new System.Drawing.Point(11, 25);
            this.MktCompetencia.Mask = "00/0000";
            this.MktCompetencia.Name = "MktCompetencia";
            this.MktCompetencia.Size = new System.Drawing.Size(69, 20);
            this.MktCompetencia.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 469);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Calculadora de IR - Maycon Wisley";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 491);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MktCompetencia);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnDependente);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnTabelaValSimplificado);
            this.Controls.Add(this.BtnTabelaIRRF);
            this.Controls.Add(this.BtnCalcular);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtQtdDependente);
            this.Controls.Add(this.TxtDescontoInss);
            this.Controls.Add(this.TxtDescInss);
            this.Controls.Add(this.TxtValorBruto);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculo IRRF";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtValorBruto;
        private System.Windows.Forms.Label TxtDescontoInss;
        private System.Windows.Forms.TextBox TxtDescInss;
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
    }
}

