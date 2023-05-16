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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle61 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle62 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle63 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle64 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle65 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtFaixa = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtValor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtPorcentagem = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDeducao = new System.Windows.Forms.TextBox();
            this.DgvTabelaIRRF = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Faixa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Porcentagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Deducao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnGravar = new System.Windows.Forms.Button();
            this.BtnAlterar = new System.Windows.Forms.Button();
            this.BtnExcluir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTabelaIRRF)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Competência";
            // 
            // MktCompetencia
            // 
            this.MktCompetencia.Location = new System.Drawing.Point(12, 26);
            this.MktCompetencia.Mask = "00/0000";
            this.MktCompetencia.Name = "MktCompetencia";
            this.MktCompetencia.Size = new System.Drawing.Size(69, 20);
            this.MktCompetencia.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Faixa";
            // 
            // TxtFaixa
            // 
            this.TxtFaixa.Location = new System.Drawing.Point(12, 65);
            this.TxtFaixa.Name = "TxtFaixa";
            this.TxtFaixa.Size = new System.Drawing.Size(29, 20);
            this.TxtFaixa.TabIndex = 1;
            this.TxtFaixa.Text = "1";
            this.TxtFaixa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtFaixa.TextChanged += new System.EventHandler(this.TxtFaixa_TextChanged);
            this.TxtFaixa.Enter += new System.EventHandler(this.TxtFaixa_Enter);
            this.TxtFaixa.Leave += new System.EventHandler(this.TxtFaixa_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Valor";
            // 
            // TxtValor
            // 
            this.TxtValor.Location = new System.Drawing.Point(47, 65);
            this.TxtValor.Name = "TxtValor";
            this.TxtValor.Size = new System.Drawing.Size(72, 20);
            this.TxtValor.TabIndex = 2;
            this.TxtValor.Text = "0,00";
            this.TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtValor.TextChanged += new System.EventHandler(this.TxtValor_TextChanged);
            this.TxtValor.Enter += new System.EventHandler(this.TxtValor_Enter);
            this.TxtValor.Leave += new System.EventHandler(this.TxtValor_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "%";
            // 
            // TxtPorcentagem
            // 
            this.TxtPorcentagem.Location = new System.Drawing.Point(125, 65);
            this.TxtPorcentagem.Name = "TxtPorcentagem";
            this.TxtPorcentagem.Size = new System.Drawing.Size(35, 20);
            this.TxtPorcentagem.TabIndex = 3;
            this.TxtPorcentagem.Text = "0,00";
            this.TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtPorcentagem.TextChanged += new System.EventHandler(this.TxtPorcentagem_TextChanged);
            this.TxtPorcentagem.Enter += new System.EventHandler(this.TxtPorcentagem_Enter);
            this.TxtPorcentagem.Leave += new System.EventHandler(this.TxtPorcentagem_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(163, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Dedução";
            // 
            // TxtDeducao
            // 
            this.TxtDeducao.Location = new System.Drawing.Point(166, 65);
            this.TxtDeducao.Name = "TxtDeducao";
            this.TxtDeducao.Size = new System.Drawing.Size(59, 20);
            this.TxtDeducao.TabIndex = 4;
            this.TxtDeducao.Text = "0,00";
            this.TxtDeducao.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtDeducao.TextChanged += new System.EventHandler(this.TxtDeducao_TextChanged);
            this.TxtDeducao.Enter += new System.EventHandler(this.TxtDeducao_Enter);
            this.TxtDeducao.Leave += new System.EventHandler(this.TxtDeducao_Leave);
            // 
            // DgvTabelaIRRF
            // 
            this.DgvTabelaIRRF.AllowUserToAddRows = false;
            this.DgvTabelaIRRF.AllowUserToDeleteRows = false;
            this.DgvTabelaIRRF.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvTabelaIRRF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DgvTabelaIRRF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTabelaIRRF.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Competencia,
            this.Faixa,
            this.Valor,
            this.Porcentagem,
            this.Deducao});
            this.DgvTabelaIRRF.Location = new System.Drawing.Point(12, 100);
            this.DgvTabelaIRRF.MultiSelect = false;
            this.DgvTabelaIRRF.Name = "DgvTabelaIRRF";
            this.DgvTabelaIRRF.ReadOnly = true;
            this.DgvTabelaIRRF.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvTabelaIRRF.Size = new System.Drawing.Size(326, 150);
            this.DgvTabelaIRRF.TabIndex = 5;
            this.DgvTabelaIRRF.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTabelaIRRF_CellDoubleClick);
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
            this.Competencia.DataPropertyName = "Competencia";
            dataGridViewCellStyle61.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle61.Format = "MM/yyyy";
            dataGridViewCellStyle61.NullValue = null;
            this.Competencia.DefaultCellStyle = dataGridViewCellStyle61;
            this.Competencia.HeaderText = "Competencia";
            this.Competencia.Name = "Competencia";
            this.Competencia.ReadOnly = true;
            this.Competencia.Visible = false;
            // 
            // Faixa
            // 
            this.Faixa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Faixa.DataPropertyName = "Faixa";
            dataGridViewCellStyle62.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle62.Format = "N0";
            dataGridViewCellStyle62.NullValue = null;
            this.Faixa.DefaultCellStyle = dataGridViewCellStyle62;
            this.Faixa.HeaderText = "Faixa";
            this.Faixa.Name = "Faixa";
            this.Faixa.ReadOnly = true;
            this.Faixa.Width = 57;
            // 
            // Valor
            // 
            this.Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle63.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle63.Format = "N2";
            dataGridViewCellStyle63.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle63;
            this.Valor.HeaderText = "Valor";
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Width = 56;
            // 
            // Porcentagem
            // 
            this.Porcentagem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Porcentagem.DataPropertyName = "Porcentagem";
            dataGridViewCellStyle64.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle64.Format = "N2";
            dataGridViewCellStyle64.NullValue = null;
            this.Porcentagem.DefaultCellStyle = dataGridViewCellStyle64;
            this.Porcentagem.HeaderText = "%";
            this.Porcentagem.Name = "Porcentagem";
            this.Porcentagem.ReadOnly = true;
            this.Porcentagem.Width = 40;
            // 
            // Deducao
            // 
            this.Deducao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Deducao.DataPropertyName = "Deducao";
            dataGridViewCellStyle65.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle65.Format = "N2";
            dataGridViewCellStyle65.NullValue = null;
            this.Deducao.DefaultCellStyle = dataGridViewCellStyle65;
            this.Deducao.HeaderText = "Dedução";
            this.Deducao.Name = "Deducao";
            this.Deducao.ReadOnly = true;
            this.Deducao.Width = 76;
            // 
            // BtnGravar
            // 
            this.BtnGravar.Location = new System.Drawing.Point(263, 13);
            this.BtnGravar.Name = "BtnGravar";
            this.BtnGravar.Size = new System.Drawing.Size(75, 23);
            this.BtnGravar.TabIndex = 6;
            this.BtnGravar.Text = "&Gravar";
            this.BtnGravar.UseVisualStyleBackColor = true;
            this.BtnGravar.Click += new System.EventHandler(this.BtnGravar_Click);
            // 
            // BtnAlterar
            // 
            this.BtnAlterar.Location = new System.Drawing.Point(263, 42);
            this.BtnAlterar.Name = "BtnAlterar";
            this.BtnAlterar.Size = new System.Drawing.Size(75, 23);
            this.BtnAlterar.TabIndex = 7;
            this.BtnAlterar.Text = "&Alterar";
            this.BtnAlterar.UseVisualStyleBackColor = true;
            this.BtnAlterar.Click += new System.EventHandler(this.BtnAlterar_Click);
            // 
            // BtnExcluir
            // 
            this.BtnExcluir.Location = new System.Drawing.Point(263, 71);
            this.BtnExcluir.Name = "BtnExcluir";
            this.BtnExcluir.Size = new System.Drawing.Size(75, 23);
            this.BtnExcluir.TabIndex = 8;
            this.BtnExcluir.Text = "&Excluir";
            this.BtnExcluir.UseVisualStyleBackColor = true;
            this.BtnExcluir.Click += new System.EventHandler(this.BtnExcluir_Click);
            // 
            // FrmTabelaIRRF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 261);
            this.Controls.Add(this.BtnExcluir);
            this.Controls.Add(this.BtnAlterar);
            this.Controls.Add(this.BtnGravar);
            this.Controls.Add(this.DgvTabelaIRRF);
            this.Controls.Add(this.TxtPorcentagem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtValor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtDeducao);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxtFaixa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MktCompetencia);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmTabelaIRRF";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tabela IRRF";
            this.Load += new System.EventHandler(this.FrmTabelaIRRF_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvTabelaIRRF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Faixa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Porcentagem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deducao;
    }
}