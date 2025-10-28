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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            BtnExcluir = new System.Windows.Forms.Button();
            BtnAlterar = new System.Windows.Forms.Button();
            BtnGravar = new System.Windows.Forms.Button();
            DgvTabelaINSS = new System.Windows.Forms.DataGridView();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Faixa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Porcentagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TxtPorcentagem = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            TxtValor = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            TxtFaixa = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            label1 = new System.Windows.Forms.Label();
            LkLblOnline = new System.Windows.Forms.LinkLabel();
            LblLinkSite1 = new System.Windows.Forms.LinkLabel();
            LblLinkSite2 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)DgvTabelaINSS).BeginInit();
            SuspendLayout();
            // 
            // BtnExcluir
            // 
            BtnExcluir.Enabled = false;
            BtnExcluir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
            BtnExcluir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            BtnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            BtnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnExcluir.Location = new System.Drawing.Point(296, 83);
            BtnExcluir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnExcluir.Name = "BtnExcluir";
            BtnExcluir.Size = new System.Drawing.Size(88, 27);
            BtnExcluir.TabIndex = 22;
            BtnExcluir.Text = "&Excluir";
            BtnExcluir.UseVisualStyleBackColor = true;
            BtnExcluir.Click += BtnExcluir_Click;
            // 
            // BtnAlterar
            // 
            BtnAlterar.Enabled = false;
            BtnAlterar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
            BtnAlterar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            BtnAlterar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            BtnAlterar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnAlterar.Location = new System.Drawing.Point(296, 50);
            BtnAlterar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnAlterar.Name = "BtnAlterar";
            BtnAlterar.Size = new System.Drawing.Size(88, 27);
            BtnAlterar.TabIndex = 21;
            BtnAlterar.Text = "&Alterar";
            BtnAlterar.UseVisualStyleBackColor = true;
            BtnAlterar.Click += BtnAlterar_Click;
            // 
            // BtnGravar
            // 
            BtnGravar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
            BtnGravar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            BtnGravar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            BtnGravar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnGravar.Location = new System.Drawing.Point(296, 16);
            BtnGravar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnGravar.Name = "BtnGravar";
            BtnGravar.Size = new System.Drawing.Size(88, 27);
            BtnGravar.TabIndex = 20;
            BtnGravar.Text = "&Gravar";
            BtnGravar.UseVisualStyleBackColor = true;
            BtnGravar.Click += BtnGravar_Click;
            // 
            // DgvTabelaINSS
            // 
            DgvTabelaINSS.AllowUserToAddRows = false;
            DgvTabelaINSS.AllowUserToDeleteRows = false;
            DgvTabelaINSS.BackgroundColor = System.Drawing.SystemColors.Control;
            DgvTabelaINSS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            DgvTabelaINSS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvTabelaINSS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Competencia, Faixa, Valor, Porcentagem });
            DgvTabelaINSS.Location = new System.Drawing.Point(15, 117);
            DgvTabelaINSS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DgvTabelaINSS.MultiSelect = false;
            DgvTabelaINSS.Name = "DgvTabelaINSS";
            DgvTabelaINSS.ReadOnly = true;
            DgvTabelaINSS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            DgvTabelaINSS.Size = new System.Drawing.Size(369, 173);
            DgvTabelaINSS.TabIndex = 19;
            DgvTabelaINSS.CellDoubleClick += DgvTabelaINSS_CellDoubleClick;
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "MM/yyyy";
            dataGridViewCellStyle9.NullValue = null;
            Competencia.DefaultCellStyle = dataGridViewCellStyle9;
            Competencia.HeaderText = "Competência";
            Competencia.Name = "Competencia";
            Competencia.ReadOnly = true;
            Competencia.Width = 103;
            // 
            // Faixa
            // 
            Faixa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            Faixa.DataPropertyName = "Faixa";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            Faixa.DefaultCellStyle = dataGridViewCellStyle10;
            Faixa.HeaderText = "Faixa";
            Faixa.Name = "Faixa";
            Faixa.ReadOnly = true;
            Faixa.Width = 58;
            // 
            // Valor
            // 
            Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            Valor.DefaultCellStyle = dataGridViewCellStyle11;
            Valor.HeaderText = "Valor";
            Valor.Name = "Valor";
            Valor.ReadOnly = true;
            // 
            // Porcentagem
            // 
            Porcentagem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            Porcentagem.DataPropertyName = "Porcentagem";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            dataGridViewCellStyle12.NullValue = null;
            Porcentagem.DefaultCellStyle = dataGridViewCellStyle12;
            Porcentagem.HeaderText = "%";
            Porcentagem.Name = "Porcentagem";
            Porcentagem.ReadOnly = true;
            Porcentagem.Width = 42;
            // 
            // TxtPorcentagem
            // 
            TxtPorcentagem.Location = new System.Drawing.Point(147, 76);
            TxtPorcentagem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtPorcentagem.Name = "TxtPorcentagem";
            TxtPorcentagem.Size = new System.Drawing.Size(40, 23);
            TxtPorcentagem.TabIndex = 17;
            TxtPorcentagem.Text = "0,00";
            TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtPorcentagem.TextChanged += TxtPorcentagem_TextChanged;
            TxtPorcentagem.Enter += TxtPorcentagem_Enter;
            TxtPorcentagem.Leave += TxtPorcentagem_Leave;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(144, 58);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(17, 15);
            label4.TabIndex = 12;
            label4.Text = "%";
            // 
            // TxtValor
            // 
            TxtValor.Location = new System.Drawing.Point(56, 76);
            TxtValor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtValor.Name = "TxtValor";
            TxtValor.Size = new System.Drawing.Size(83, 23);
            TxtValor.TabIndex = 13;
            TxtValor.Text = "0,00";
            TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtValor.TextChanged += TxtValor_TextChanged;
            TxtValor.Enter += TxtValor_Enter;
            TxtValor.Leave += TxtValor_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(52, 58);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(33, 15);
            label3.TabIndex = 14;
            label3.Text = "Valor";
            // 
            // TxtFaixa
            // 
            TxtFaixa.Location = new System.Drawing.Point(15, 76);
            TxtFaixa.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtFaixa.Name = "TxtFaixa";
            TxtFaixa.Size = new System.Drawing.Size(33, 23);
            TxtFaixa.TabIndex = 11;
            TxtFaixa.Text = "1";
            TxtFaixa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtFaixa.TextChanged += TxtFaixa_TextChanged;
            TxtFaixa.Enter += TxtFaixa_Enter;
            TxtFaixa.Leave += TxtFaixa_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 58);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(33, 15);
            label2.TabIndex = 16;
            label2.Text = "Faixa";
            // 
            // MktCompetencia
            // 
            MktCompetencia.Location = new System.Drawing.Point(15, 31);
            MktCompetencia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MktCompetencia.Mask = "00/0000";
            MktCompetencia.Name = "MktCompetencia";
            MktCompetencia.Size = new System.Drawing.Size(80, 23);
            MktCompetencia.TabIndex = 9;
            MktCompetencia.Leave += MktCompetencia_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(15, 13);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(78, 15);
            label1.TabIndex = 10;
            label1.Text = "Competência";
            // 
            // LkLblOnline
            // 
            LkLblOnline.AutoSize = true;
            LkLblOnline.Location = new System.Drawing.Point(151, 13);
            LkLblOnline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LkLblOnline.Name = "LkLblOnline";
            LkLblOnline.Size = new System.Drawing.Size(128, 15);
            LkLblOnline.TabIndex = 23;
            LkLblOnline.TabStop = true;
            LkLblOnline.Text = "Atualizar Tabela Online";
            LkLblOnline.LinkClicked += LkLblOnline_LinkClicked;
            // 
            // LblLinkSite1
            // 
            LblLinkSite1.AutoSize = true;
            LblLinkSite1.Location = new System.Drawing.Point(12, 293);
            LblLinkSite1.Name = "LblLinkSite1";
            LblLinkSite1.Size = new System.Drawing.Size(266, 15);
            LblLinkSite1.TabIndex = 24;
            LblLinkSite1.TabStop = true;
            LblLinkSite1.Text = "Site: https://www.contabeis.com.br/tabelas/inss/";
            LblLinkSite1.LinkClicked += LblLinkSite_LinkClicked;
            // 
            // LblLinkSite2
            // 
            LblLinkSite2.AutoSize = true;
            LblLinkSite2.Location = new System.Drawing.Point(12, 310);
            LblLinkSite2.Name = "LblLinkSite2";
            LblLinkSite2.Size = new System.Drawing.Size(279, 15);
            LblLinkSite2.TabIndex = 25;
            LblLinkSite2.TabStop = true;
            LblLinkSite2.Text = "Site: https://www.debit.com.br/tabelas/tabelas-inss";
            LblLinkSite2.LinkClicked += LblLinkSite2_LinkClicked;
            // 
            // FrmTabelaINSS
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(394, 346);
            Controls.Add(LblLinkSite2);
            Controls.Add(LblLinkSite1);
            Controls.Add(LkLblOnline);
            Controls.Add(BtnExcluir);
            Controls.Add(BtnAlterar);
            Controls.Add(BtnGravar);
            Controls.Add(DgvTabelaINSS);
            Controls.Add(TxtPorcentagem);
            Controls.Add(label4);
            Controls.Add(TxtValor);
            Controls.Add(label3);
            Controls.Add(TxtFaixa);
            Controls.Add(label2);
            Controls.Add(MktCompetencia);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrmTabelaINSS";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Tabela INSS";
            Load += FrmTabelaINSS_Load;
            ((System.ComponentModel.ISupportInitialize)DgvTabelaINSS).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.LinkLabel LkLblOnline;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
		private System.Windows.Forms.DataGridViewTextBoxColumn Faixa;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
		private System.Windows.Forms.DataGridViewTextBoxColumn Porcentagem;
		private System.Windows.Forms.LinkLabel LblLinkSite1;
        private System.Windows.Forms.LinkLabel LblLinkSite2;
    }
}