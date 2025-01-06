namespace CalculoIRRF
{
    partial class FrmTabelaINSS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnExcluir = new System.Windows.Forms.Button();
            this.BtnAlterar = new System.Windows.Forms.Button();
            this.BtnGravar = new System.Windows.Forms.Button();
            this.DgvTabelaINSS = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Faixa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Porcentagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxtPorcentagem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtValor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtFaixa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LkLblOnline = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTabelaINSS)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExcluir
            // 
            this.BtnExcluir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnExcluir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.BtnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.BtnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExcluir.Location = new System.Drawing.Point(254, 72);
            this.BtnExcluir.Name = "BtnExcluir";
            this.BtnExcluir.Size = new System.Drawing.Size(75, 23);
            this.BtnExcluir.TabIndex = 22;
            this.BtnExcluir.Text = "&Excluir";
            this.BtnExcluir.UseVisualStyleBackColor = true;
            this.BtnExcluir.Click += new System.EventHandler(this.BtnExcluir_Click);
            // 
            // BtnAlterar
            // 
            this.BtnAlterar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnAlterar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.BtnAlterar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.BtnAlterar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAlterar.Location = new System.Drawing.Point(254, 43);
            this.BtnAlterar.Name = "BtnAlterar";
            this.BtnAlterar.Size = new System.Drawing.Size(75, 23);
            this.BtnAlterar.TabIndex = 21;
            this.BtnAlterar.Text = "&Alterar";
            this.BtnAlterar.UseVisualStyleBackColor = true;
            this.BtnAlterar.Click += new System.EventHandler(this.BtnAlterar_Click);
            // 
            // BtnGravar
            // 
            this.BtnGravar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnGravar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.BtnGravar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.BtnGravar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGravar.Location = new System.Drawing.Point(254, 14);
            this.BtnGravar.Name = "BtnGravar";
            this.BtnGravar.Size = new System.Drawing.Size(75, 23);
            this.BtnGravar.TabIndex = 20;
            this.BtnGravar.Text = "&Gravar";
            this.BtnGravar.UseVisualStyleBackColor = true;
            this.BtnGravar.Click += new System.EventHandler(this.BtnGravar_Click);
            // 
            // DgvTabelaINSS
            // 
            this.DgvTabelaINSS.AllowUserToAddRows = false;
            this.DgvTabelaINSS.AllowUserToDeleteRows = false;
            this.DgvTabelaINSS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvTabelaINSS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DgvTabelaINSS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTabelaINSS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Competencia,
            this.Faixa,
            this.Valor,
            this.Porcentagem});
            this.DgvTabelaINSS.Location = new System.Drawing.Point(13, 101);
            this.DgvTabelaINSS.MultiSelect = false;
            this.DgvTabelaINSS.Name = "DgvTabelaINSS";
            this.DgvTabelaINSS.ReadOnly = true;
            this.DgvTabelaINSS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvTabelaINSS.Size = new System.Drawing.Size(316, 150);
            this.DgvTabelaINSS.TabIndex = 19;
            this.DgvTabelaINSS.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTabelaINSS_CellDoubleClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // Competencia
            // 
            this.Competencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Competencia.DataPropertyName = "Competencia";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "MM/yyyy";
            dataGridViewCellStyle5.NullValue = null;
            this.Competencia.DefaultCellStyle = dataGridViewCellStyle5;
            this.Competencia.HeaderText = "Competência";
            this.Competencia.Name = "Competencia";
            this.Competencia.ReadOnly = true;
            this.Competencia.Width = 94;
            // 
            // Faixa
            // 
            this.Faixa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Faixa.DataPropertyName = "Faixa";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.Faixa.DefaultCellStyle = dataGridViewCellStyle6;
            this.Faixa.HeaderText = "Faixa";
            this.Faixa.Name = "Faixa";
            this.Faixa.ReadOnly = true;
            this.Faixa.Width = 57;
            // 
            // Valor
            // 
            this.Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle7;
            this.Valor.HeaderText = "Valor";
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Width = 56;
            // 
            // Porcentagem
            // 
            this.Porcentagem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Porcentagem.DataPropertyName = "Porcentagem";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = null;
            this.Porcentagem.DefaultCellStyle = dataGridViewCellStyle8;
            this.Porcentagem.HeaderText = "%";
            this.Porcentagem.Name = "Porcentagem";
            this.Porcentagem.ReadOnly = true;
            this.Porcentagem.Width = 40;
            // 
            // TxtPorcentagem
            // 
            this.TxtPorcentagem.Location = new System.Drawing.Point(126, 66);
            this.TxtPorcentagem.Name = "TxtPorcentagem";
            this.TxtPorcentagem.Size = new System.Drawing.Size(35, 20);
            this.TxtPorcentagem.TabIndex = 17;
            this.TxtPorcentagem.Text = "0,00";
            this.TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtPorcentagem.TextChanged += new System.EventHandler(this.TxtPorcentagem_TextChanged);
            this.TxtPorcentagem.Enter += new System.EventHandler(this.TxtPorcentagem_Enter);
            this.TxtPorcentagem.Leave += new System.EventHandler(this.TxtPorcentagem_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(123, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "%";
            // 
            // TxtValor
            // 
            this.TxtValor.Location = new System.Drawing.Point(48, 66);
            this.TxtValor.Name = "TxtValor";
            this.TxtValor.Size = new System.Drawing.Size(72, 20);
            this.TxtValor.TabIndex = 13;
            this.TxtValor.Text = "0,00";
            this.TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtValor.TextChanged += new System.EventHandler(this.TxtValor_TextChanged);
            this.TxtValor.Enter += new System.EventHandler(this.TxtValor_Enter);
            this.TxtValor.Leave += new System.EventHandler(this.TxtValor_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Valor";
            // 
            // TxtFaixa
            // 
            this.TxtFaixa.Location = new System.Drawing.Point(13, 66);
            this.TxtFaixa.Name = "TxtFaixa";
            this.TxtFaixa.Size = new System.Drawing.Size(29, 20);
            this.TxtFaixa.TabIndex = 11;
            this.TxtFaixa.Text = "1";
            this.TxtFaixa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtFaixa.TextChanged += new System.EventHandler(this.TxtFaixa_TextChanged);
            this.TxtFaixa.Enter += new System.EventHandler(this.TxtFaixa_Enter);
            this.TxtFaixa.Leave += new System.EventHandler(this.TxtFaixa_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Faixa";
            // 
            // MktCompetencia
            // 
            this.MktCompetencia.Location = new System.Drawing.Point(13, 27);
            this.MktCompetencia.Mask = "00/0000";
            this.MktCompetencia.Name = "MktCompetencia";
            this.MktCompetencia.Size = new System.Drawing.Size(69, 20);
            this.MktCompetencia.TabIndex = 9;
            this.MktCompetencia.Leave += new System.EventHandler(this.MktCompetencia_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Competência";
            // 
            // LkLblOnline
            // 
            this.LkLblOnline.AutoSize = true;
            this.LkLblOnline.Location = new System.Drawing.Point(176, 19);
            this.LkLblOnline.Name = "LkLblOnline";
            this.LkLblOnline.Size = new System.Drawing.Size(37, 13);
            this.LkLblOnline.TabIndex = 23;
            this.LkLblOnline.TabStop = true;
            this.LkLblOnline.Text = "Online";
            this.LkLblOnline.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LkLblOnline_LinkClicked);
            // 
            // FrmTabelaINSS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 262);
            this.Controls.Add(this.LkLblOnline);
            this.Controls.Add(this.BtnExcluir);
            this.Controls.Add(this.BtnAlterar);
            this.Controls.Add(this.BtnGravar);
            this.Controls.Add(this.DgvTabelaINSS);
            this.Controls.Add(this.TxtPorcentagem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtValor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtFaixa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MktCompetencia);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmTabelaINSS";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tabela INSS";
            this.Load += new System.EventHandler(this.FrmTabelaINSS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvTabelaINSS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExcluir;
        private System.Windows.Forms.Button BtnAlterar;
        private System.Windows.Forms.Button BtnGravar;
        private System.Windows.Forms.DataGridView DgvTabelaINSS;
        private System.Windows.Forms.TextBox TxtPorcentagem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtValor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtFaixa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Faixa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Porcentagem;
        private System.Windows.Forms.LinkLabel LkLblOnline;
    }
}