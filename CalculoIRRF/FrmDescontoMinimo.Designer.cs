namespace CalculoIRRF
{
    partial class FrmDescontoMinimo
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
			BtnExcluir = new System.Windows.Forms.Button();
			BtnAlterar = new System.Windows.Forms.Button();
			BtnGravar = new System.Windows.Forms.Button();
			DgvValorDescontoMinimo = new System.Windows.Forms.DataGridView();
			Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			TxtValor = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			MktCompetencia = new System.Windows.Forms.MaskedTextBox();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)DgvValorDescontoMinimo).BeginInit();
			SuspendLayout();
			// 
			// BtnExcluir
			// 
			BtnExcluir.Enabled = false;
			BtnExcluir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
			BtnExcluir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
			BtnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			BtnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnExcluir.Location = new System.Drawing.Point(203, 81);
			BtnExcluir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnExcluir.Name = "BtnExcluir";
			BtnExcluir.Size = new System.Drawing.Size(88, 27);
			BtnExcluir.TabIndex = 11;
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
			BtnAlterar.Location = new System.Drawing.Point(203, 47);
			BtnAlterar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnAlterar.Name = "BtnAlterar";
			BtnAlterar.Size = new System.Drawing.Size(88, 27);
			BtnAlterar.TabIndex = 12;
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
			BtnGravar.Location = new System.Drawing.Point(203, 14);
			BtnGravar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnGravar.Name = "BtnGravar";
			BtnGravar.Size = new System.Drawing.Size(88, 27);
			BtnGravar.TabIndex = 13;
			BtnGravar.Text = "&Gravar";
			BtnGravar.UseVisualStyleBackColor = true;
			BtnGravar.Click += BtnGravar_Click;
			// 
			// DgvValorDescontoMinimo
			// 
			DgvValorDescontoMinimo.AllowUserToAddRows = false;
			DgvValorDescontoMinimo.AllowUserToDeleteRows = false;
			DgvValorDescontoMinimo.BackgroundColor = System.Drawing.SystemColors.Control;
			DgvValorDescontoMinimo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			DgvValorDescontoMinimo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DgvValorDescontoMinimo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Competencia, Valor });
			DgvValorDescontoMinimo.Location = new System.Drawing.Point(13, 120);
			DgvValorDescontoMinimo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			DgvValorDescontoMinimo.MultiSelect = false;
			DgvValorDescontoMinimo.Name = "DgvValorDescontoMinimo";
			DgvValorDescontoMinimo.ReadOnly = true;
			DgvValorDescontoMinimo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			DgvValorDescontoMinimo.Size = new System.Drawing.Size(278, 173);
			DgvValorDescontoMinimo.TabIndex = 10;
			DgvValorDescontoMinimo.CellDoubleClick += DgvValorDescontoMinimo_CellDoubleClick;
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
			// Valor
			// 
			Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			Valor.DataPropertyName = "Valor";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "N2";
			dataGridViewCellStyle2.NullValue = null;
			Valor.DefaultCellStyle = dataGridViewCellStyle2;
			Valor.HeaderText = "Valor";
			Valor.Name = "Valor";
			Valor.ReadOnly = true;
			// 
			// TxtValor
			// 
			TxtValor.Location = new System.Drawing.Point(13, 74);
			TxtValor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtValor.Name = "TxtValor";
			TxtValor.Size = new System.Drawing.Size(80, 23);
			TxtValor.TabIndex = 9;
			TxtValor.Text = "0,00";
			TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtValor.TextChanged += TxtValor_TextChanged;
			TxtValor.Enter += TxtValor_Enter;
			TxtValor.Leave += TxtValor_Leave;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 55);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(33, 15);
			label2.TabIndex = 8;
			label2.Text = "Valor";
			// 
			// MktCompetencia
			// 
			MktCompetencia.Location = new System.Drawing.Point(13, 29);
			MktCompetencia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			MktCompetencia.Mask = "00/0000";
			MktCompetencia.Name = "MktCompetencia";
			MktCompetencia.Size = new System.Drawing.Size(80, 23);
			MktCompetencia.TabIndex = 7;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(13, 10);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(78, 15);
			label1.TabIndex = 6;
			label1.Text = "Competência";
			// 
			// FrmDescontoMinimo
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(303, 303);
			Controls.Add(BtnExcluir);
			Controls.Add(BtnAlterar);
			Controls.Add(BtnGravar);
			Controls.Add(DgvValorDescontoMinimo);
			Controls.Add(TxtValor);
			Controls.Add(label2);
			Controls.Add(MktCompetencia);
			Controls.Add(label1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FrmDescontoMinimo";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Desconto Minimo";
			Load += FrmDescontoMinimo_Load;
			((System.ComponentModel.ISupportInitialize)DgvValorDescontoMinimo).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnExcluir;
        private System.Windows.Forms.Button BtnAlterar;
        private System.Windows.Forms.Button BtnGravar;
        private System.Windows.Forms.DataGridView DgvValorDescontoMinimo;
        private System.Windows.Forms.TextBox TxtValor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
	}
}