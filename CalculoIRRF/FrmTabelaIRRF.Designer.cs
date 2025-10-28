namespace CalculoIRRF
{
    partial class FrmTabelaIRRF
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			label1 = new System.Windows.Forms.Label();
			MktCompetencia = new System.Windows.Forms.MaskedTextBox();
			label2 = new System.Windows.Forms.Label();
			TxtFaixa = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			TxtValor = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			TxtPorcentagem = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			TxtDeducao = new System.Windows.Forms.TextBox();
			DgvTabelaIRRF = new System.Windows.Forms.DataGridView();
			BtnGravar = new System.Windows.Forms.Button();
			BtnAlterar = new System.Windows.Forms.Button();
			BtnExcluir = new System.Windows.Forms.Button();
			LblLinkOnline = new System.Windows.Forms.LinkLabel();
			LblLinkSite = new System.Windows.Forms.LinkLabel();
			Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Faixa = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Porcentagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Deducao = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)DgvTabelaIRRF).BeginInit();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(14, 12);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(78, 15);
			label1.TabIndex = 0;
			label1.Text = "Competência";
			// 
			// MktCompetencia
			// 
			MktCompetencia.Location = new System.Drawing.Point(14, 30);
			MktCompetencia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			MktCompetencia.Mask = "00/0000";
			MktCompetencia.Name = "MktCompetencia";
			MktCompetencia.Size = new System.Drawing.Size(80, 23);
			MktCompetencia.TabIndex = 0;
			MktCompetencia.Leave += MktCompetencia_Leave;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(10, 57);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(34, 15);
			label2.TabIndex = 2;
			label2.Text = "Faixa";
			// 
			// TxtFaixa
			// 
			TxtFaixa.Location = new System.Drawing.Point(14, 75);
			TxtFaixa.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtFaixa.Name = "TxtFaixa";
			TxtFaixa.Size = new System.Drawing.Size(33, 23);
			TxtFaixa.TabIndex = 1;
			TxtFaixa.Text = "1";
			TxtFaixa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtFaixa.TextChanged += TxtFaixa_TextChanged;
			TxtFaixa.Enter += TxtFaixa_Enter;
			TxtFaixa.Leave += TxtFaixa_Leave;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(51, 57);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(33, 15);
			label3.TabIndex = 2;
			label3.Text = "Valor";
			// 
			// TxtValor
			// 
			TxtValor.Location = new System.Drawing.Point(55, 75);
			TxtValor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtValor.Name = "TxtValor";
			TxtValor.Size = new System.Drawing.Size(83, 23);
			TxtValor.TabIndex = 2;
			TxtValor.Text = "0,00";
			TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtValor.TextChanged += TxtValor_TextChanged;
			TxtValor.Enter += TxtValor_Enter;
			TxtValor.Leave += TxtValor_Leave;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(142, 57);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(17, 15);
			label4.TabIndex = 2;
			label4.Text = "%";
			// 
			// TxtPorcentagem
			// 
			TxtPorcentagem.Location = new System.Drawing.Point(146, 75);
			TxtPorcentagem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtPorcentagem.Name = "TxtPorcentagem";
			TxtPorcentagem.Size = new System.Drawing.Size(40, 23);
			TxtPorcentagem.TabIndex = 3;
			TxtPorcentagem.Text = "0,00";
			TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtPorcentagem.TextChanged += TxtPorcentagem_TextChanged;
			TxtPorcentagem.Enter += TxtPorcentagem_Enter;
			TxtPorcentagem.Leave += TxtPorcentagem_Leave;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(190, 57);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(54, 15);
			label5.TabIndex = 2;
			label5.Text = "Dedução";
			// 
			// TxtDeducao
			// 
			TxtDeducao.Location = new System.Drawing.Point(194, 75);
			TxtDeducao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtDeducao.Name = "TxtDeducao";
			TxtDeducao.Size = new System.Drawing.Size(68, 23);
			TxtDeducao.TabIndex = 4;
			TxtDeducao.Text = "0,00";
			TxtDeducao.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtDeducao.TextChanged += TxtDeducao_TextChanged;
			TxtDeducao.Enter += TxtDeducao_Enter;
			TxtDeducao.Leave += TxtDeducao_Leave;
			// 
			// DgvTabelaIRRF
			// 
			DgvTabelaIRRF.AllowUserToAddRows = false;
			DgvTabelaIRRF.AllowUserToDeleteRows = false;
			DgvTabelaIRRF.BackgroundColor = System.Drawing.SystemColors.Control;
			DgvTabelaIRRF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			DgvTabelaIRRF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DgvTabelaIRRF.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Competencia, Faixa, Valor, Porcentagem, Deducao });
			DgvTabelaIRRF.Location = new System.Drawing.Point(14, 115);
			DgvTabelaIRRF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			DgvTabelaIRRF.MultiSelect = false;
			DgvTabelaIRRF.Name = "DgvTabelaIRRF";
			DgvTabelaIRRF.ReadOnly = true;
			DgvTabelaIRRF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			DgvTabelaIRRF.Size = new System.Drawing.Size(463, 173);
			DgvTabelaIRRF.TabIndex = 5;
			DgvTabelaIRRF.CellDoubleClick += DgvTabelaIRRF_CellDoubleClick;
			// 
			// BtnGravar
			// 
			BtnGravar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
			BtnGravar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
			BtnGravar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			BtnGravar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnGravar.Location = new System.Drawing.Point(390, 15);
			BtnGravar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnGravar.Name = "BtnGravar";
			BtnGravar.Size = new System.Drawing.Size(88, 27);
			BtnGravar.TabIndex = 6;
			BtnGravar.Text = "&Gravar";
			BtnGravar.UseVisualStyleBackColor = true;
			BtnGravar.Click += BtnGravar_Click;
			// 
			// BtnAlterar
			// 
			BtnAlterar.Enabled = false;
			BtnAlterar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
			BtnAlterar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
			BtnAlterar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			BtnAlterar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnAlterar.Location = new System.Drawing.Point(390, 48);
			BtnAlterar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnAlterar.Name = "BtnAlterar";
			BtnAlterar.Size = new System.Drawing.Size(88, 27);
			BtnAlterar.TabIndex = 7;
			BtnAlterar.Text = "&Alterar";
			BtnAlterar.UseVisualStyleBackColor = true;
			BtnAlterar.Click += BtnAlterar_Click;
			// 
			// BtnExcluir
			// 
			BtnExcluir.Enabled = false;
			BtnExcluir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
			BtnExcluir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
			BtnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			BtnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnExcluir.Location = new System.Drawing.Point(390, 82);
			BtnExcluir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnExcluir.Name = "BtnExcluir";
			BtnExcluir.Size = new System.Drawing.Size(88, 27);
			BtnExcluir.TabIndex = 8;
			BtnExcluir.Text = "&Excluir";
			BtnExcluir.UseVisualStyleBackColor = true;
			BtnExcluir.Click += BtnExcluir_Click;
			// 
			// LblLinkOnline
			// 
			LblLinkOnline.AutoSize = true;
			LblLinkOnline.Location = new System.Drawing.Point(254, 15);
			LblLinkOnline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			LblLinkOnline.Name = "LblLinkOnline";
			LblLinkOnline.Size = new System.Drawing.Size(116, 15);
			LblLinkOnline.TabIndex = 9;
			LblLinkOnline.TabStop = true;
			LblLinkOnline.Text = "Buscar Tabela Online";
			LblLinkOnline.LinkClicked += LblLinkOnline_LinkClicked;
			// 
			// LblLinkSite
			// 
			LblLinkSite.AutoSize = true;
			LblLinkSite.Location = new System.Drawing.Point(14, 291);
			LblLinkSite.Name = "LblLinkSite";
			LblLinkSite.Size = new System.Drawing.Size(325, 15);
			LblLinkSite.TabIndex = 10;
			LblLinkSite.TabStop = true;
			LblLinkSite.Text = "Site: https://www.contabeis.com.br/tabelas/imposto-renda/";
			LblLinkSite.LinkClicked += LblLinkSite_LinkClicked;
			// 
			// Id
			// 
			Id.DataPropertyName = "Id";
			Id.HeaderText = "Id";
			Id.Name = "Id";
			Id.ReadOnly = true;
			Id.Visible = false;
			// 
			// Competencia
			// 
			Competencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			Competencia.DataPropertyName = "Competencia";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle1.Format = "MM/yyyy";
			dataGridViewCellStyle1.NullValue = null;
			Competencia.DefaultCellStyle = dataGridViewCellStyle1;
			Competencia.HeaderText = "Competência";
			Competencia.Name = "Competencia";
			Competencia.ReadOnly = true;
			Competencia.Width = 103;
			// 
			// Faixa
			// 
			Faixa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			Faixa.DataPropertyName = "Faixa";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "N0";
			dataGridViewCellStyle2.NullValue = null;
			Faixa.DefaultCellStyle = dataGridViewCellStyle2;
			Faixa.HeaderText = "Faixa";
			Faixa.Name = "Faixa";
			Faixa.ReadOnly = true;
			Faixa.Width = 59;
			// 
			// Valor
			// 
			Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			Valor.DataPropertyName = "Valor";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "N2";
			dataGridViewCellStyle3.NullValue = null;
			Valor.DefaultCellStyle = dataGridViewCellStyle3;
			Valor.HeaderText = "Valor";
			Valor.Name = "Valor";
			Valor.ReadOnly = true;
			// 
			// Porcentagem
			// 
			Porcentagem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			Porcentagem.DataPropertyName = "Porcentagem";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "N2";
			dataGridViewCellStyle4.NullValue = null;
			Porcentagem.DefaultCellStyle = dataGridViewCellStyle4;
			Porcentagem.HeaderText = "%";
			Porcentagem.Name = "Porcentagem";
			Porcentagem.ReadOnly = true;
			Porcentagem.Width = 42;
			// 
			// Deducao
			// 
			Deducao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			Deducao.DataPropertyName = "Deducao";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "N2";
			dataGridViewCellStyle5.NullValue = null;
			Deducao.DefaultCellStyle = dataGridViewCellStyle5;
			Deducao.HeaderText = "Dedução";
			Deducao.Name = "Deducao";
			Deducao.ReadOnly = true;
			Deducao.Width = 79;
			// 
			// FrmTabelaIRRF
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(491, 315);
			Controls.Add(LblLinkSite);
			Controls.Add(LblLinkOnline);
			Controls.Add(BtnExcluir);
			Controls.Add(BtnAlterar);
			Controls.Add(BtnGravar);
			Controls.Add(DgvTabelaIRRF);
			Controls.Add(TxtPorcentagem);
			Controls.Add(label4);
			Controls.Add(TxtValor);
			Controls.Add(label3);
			Controls.Add(TxtDeducao);
			Controls.Add(label5);
			Controls.Add(TxtFaixa);
			Controls.Add(label2);
			Controls.Add(MktCompetencia);
			Controls.Add(label1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FrmTabelaIRRF";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Tabela IRRF";
			Load += FrmTabelaIRRF_Load;
			((System.ComponentModel.ISupportInitialize)DgvTabelaIRRF).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtFaixa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtValor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtPorcentagem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtDeducao;
        private System.Windows.Forms.DataGridView DgvTabelaIRRF;
        private System.Windows.Forms.Button BtnGravar;
        private System.Windows.Forms.Button BtnAlterar;
        private System.Windows.Forms.Button BtnExcluir;
        private System.Windows.Forms.LinkLabel LblLinkOnline;
		private System.Windows.Forms.LinkLabel LblLinkSite;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
		private System.Windows.Forms.DataGridViewTextBoxColumn Faixa;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
		private System.Windows.Forms.DataGridViewTextBoxColumn Porcentagem;
		private System.Windows.Forms.DataGridViewTextBoxColumn Deducao;
	}
}