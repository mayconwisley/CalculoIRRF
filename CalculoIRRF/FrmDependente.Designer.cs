namespace CalculoIRRF
{
    partial class FrmDependente
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
            this.BtnExcluir = new System.Windows.Forms.Button();
            this.BtnAlterar = new System.Windows.Forms.Button();
            this.BtnGravar = new System.Windows.Forms.Button();
            this.DgvValorDependente = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Competencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxtValor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MktCompetencia = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvValorDependente)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExcluir
            // 
            this.BtnExcluir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnExcluir.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.BtnExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.BtnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExcluir.Location = new System.Drawing.Point(173, 71);
            this.BtnExcluir.Name = "BtnExcluir";
            this.BtnExcluir.Size = new System.Drawing.Size(75, 23);
            this.BtnExcluir.TabIndex = 11;
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
            this.BtnAlterar.Location = new System.Drawing.Point(173, 42);
            this.BtnAlterar.Name = "BtnAlterar";
            this.BtnAlterar.Size = new System.Drawing.Size(75, 23);
            this.BtnAlterar.TabIndex = 12;
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
            this.BtnGravar.Location = new System.Drawing.Point(173, 13);
            this.BtnGravar.Name = "BtnGravar";
            this.BtnGravar.Size = new System.Drawing.Size(75, 23);
            this.BtnGravar.TabIndex = 13;
            this.BtnGravar.Text = "&Gravar";
            this.BtnGravar.UseVisualStyleBackColor = true;
            this.BtnGravar.Click += new System.EventHandler(this.BtnGravar_Click);
            // 
            // DgvValorDependente
            // 
            this.DgvValorDependente.AllowUserToAddRows = false;
            this.DgvValorDependente.AllowUserToDeleteRows = false;
            this.DgvValorDependente.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvValorDependente.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DgvValorDependente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvValorDependente.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Competencia,
            this.Valor});
            this.DgvValorDependente.Location = new System.Drawing.Point(10, 105);
            this.DgvValorDependente.MultiSelect = false;
            this.DgvValorDependente.Name = "DgvValorDependente";
            this.DgvValorDependente.ReadOnly = true;
            this.DgvValorDependente.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvValorDependente.Size = new System.Drawing.Size(238, 150);
            this.DgvValorDependente.TabIndex = 10;
            this.DgvValorDependente.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvValorSimplificado_CellDoubleClick);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "MM/yyyy";
            dataGridViewCellStyle1.NullValue = null;
            this.Competencia.DefaultCellStyle = dataGridViewCellStyle1;
            this.Competencia.HeaderText = "Competência";
            this.Competencia.Name = "Competencia";
            this.Competencia.ReadOnly = true;
            this.Competencia.Width = 94;
            // 
            // Valor
            // 
            this.Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle2;
            this.Valor.HeaderText = "Valor";
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Width = 56;
            // 
            // TxtValor
            // 
            this.TxtValor.Location = new System.Drawing.Point(10, 65);
            this.TxtValor.Name = "TxtValor";
            this.TxtValor.Size = new System.Drawing.Size(69, 20);
            this.TxtValor.TabIndex = 9;
            this.TxtValor.Text = "0,00";
            this.TxtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtValor.TextChanged += new System.EventHandler(this.TxtValor_TextChanged);
            this.TxtValor.Enter += new System.EventHandler(this.TxtValor_Enter);
            this.TxtValor.Leave += new System.EventHandler(this.TxtValor_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Valor";
            // 
            // MktCompetencia
            // 
            this.MktCompetencia.Location = new System.Drawing.Point(10, 26);
            this.MktCompetencia.Mask = "00/0000";
            this.MktCompetencia.Name = "MktCompetencia";
            this.MktCompetencia.Size = new System.Drawing.Size(69, 20);
            this.MktCompetencia.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Competência";
            // 
            // FrmDependente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 264);
            this.Controls.Add(this.BtnExcluir);
            this.Controls.Add(this.BtnAlterar);
            this.Controls.Add(this.BtnGravar);
            this.Controls.Add(this.DgvValorDependente);
            this.Controls.Add(this.TxtValor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MktCompetencia);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDependente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dependente";
            this.Load += new System.EventHandler(this.FrmDependente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvValorDependente)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExcluir;
        private System.Windows.Forms.Button BtnAlterar;
        private System.Windows.Forms.Button BtnGravar;
        private System.Windows.Forms.DataGridView DgvValorDependente;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Competencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.TextBox TxtValor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox MktCompetencia;
        private System.Windows.Forms.Label label1;
    }
}