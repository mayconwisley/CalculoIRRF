namespace CalculoIRRF
{
    partial class FrmDeducaoSimplificada
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
			label1 = new System.Windows.Forms.Label();
			MktCompetencia = new System.Windows.Forms.MaskedTextBox();
			label2 = new System.Windows.Forms.Label();
			TxtValor = new System.Windows.Forms.TextBox();
			DgvValorSimplificado = new System.Windows.Forms.DataGridView();
			Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			BtnGravar = new System.Windows.Forms.Button();
			BtnAlterar = new System.Windows.Forms.Button();
			BtnExcluir = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)DgvValorSimplificado).BeginInit();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(13, 12);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(78, 15);
			label1.TabIndex = 0;
			label1.Text = "Competência";
			// 
			// MktCompetencia
			// 
			MktCompetencia.Location = new System.Drawing.Point(13, 30);
			MktCompetencia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			MktCompetencia.Mask = "00/0000";
			MktCompetencia.Name = "MktCompetencia";
			MktCompetencia.Size = new System.Drawing.Size(80, 23);
			MktCompetencia.TabIndex = 1;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 57);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(33, 15);
			label2.TabIndex = 2;
			label2.Text = "Valor";
			// 
			// TxtValor
			// 
			TxtValor.Location = new System.Drawing.Point(13, 75);
			TxtValor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			TxtValor.Name = "TxtValor";
			TxtValor.Size = new System.Drawing.Size(80, 23);
			TxtValor.TabIndex = 3;
			TxtValor.Text = "0,00";
			TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			TxtValor.TextChanged += TxtValor_TextChanged;
			TxtValor.Enter += TxtValor_Enter;
			TxtValor.Leave += TxtValor_Leave;
			// 
			// DgvValorSimplificado
			// 
			DgvValorSimplificado.AllowUserToAddRows = false;
			DgvValorSimplificado.AllowUserToDeleteRows = false;
			DgvValorSimplificado.BackgroundColor = System.Drawing.SystemColors.Control;
			DgvValorSimplificado.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			DgvValorSimplificado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DgvValorSimplificado.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Competencia, Valor });
			DgvValorSimplificado.Location = new System.Drawing.Point(13, 121);
			DgvValorSimplificado.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			DgvValorSimplificado.MultiSelect = false;
			DgvValorSimplificado.Name = "DgvValorSimplificado";
			DgvValorSimplificado.ReadOnly = true;
			DgvValorSimplificado.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			DgvValorSimplificado.Size = new System.Drawing.Size(278, 173);
			DgvValorSimplificado.TabIndex = 4;
			DgvValorSimplificado.CellDoubleClick += DgvValorSimplificado_CellDoubleClick;
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
			// BtnGravar
			// 
			BtnGravar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
			BtnGravar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
			BtnGravar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			BtnGravar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			BtnGravar.Location = new System.Drawing.Point(203, 15);
			BtnGravar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnGravar.Name = "BtnGravar";
			BtnGravar.Size = new System.Drawing.Size(88, 27);
			BtnGravar.TabIndex = 5;
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
			BtnAlterar.Location = new System.Drawing.Point(203, 48);
			BtnAlterar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnAlterar.Name = "BtnAlterar";
			BtnAlterar.Size = new System.Drawing.Size(88, 27);
			BtnAlterar.TabIndex = 5;
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
			BtnExcluir.Location = new System.Drawing.Point(203, 82);
			BtnExcluir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			BtnExcluir.Name = "BtnExcluir";
			BtnExcluir.Size = new System.Drawing.Size(88, 27);
			BtnExcluir.TabIndex = 5;
			BtnExcluir.Text = "&Excluir";
			BtnExcluir.UseVisualStyleBackColor = true;
			BtnExcluir.Click += BtnExcluir_Click;
			// 
			// FrmDeducaoSimplificada
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(304, 306);
			Controls.Add(BtnExcluir);
			Controls.Add(BtnAlterar);
			Controls.Add(BtnGravar);
			Controls.Add(DgvValorSimplificado);
			Controls.Add(TxtValor);
			Controls.Add(label2);
			Controls.Add(MktCompetencia);
			Controls.Add(label1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FrmDeducaoSimplificada";
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Dedução Simplificada";
			Load += FrmDeducaoSimplificada_Load;
			((System.ComponentModel.ISupportInitialize)DgvValorSimplificado).EndInit();
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtValor;
        private System.Windows.Forms.DataGridView DgvValorSimplificado;
        private System.Windows.Forms.Button BtnGravar;
        private System.Windows.Forms.Button BtnAlterar;
        private System.Windows.Forms.Button BtnExcluir;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
		private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
	}
}