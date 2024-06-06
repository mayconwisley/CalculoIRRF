namespace CalculoIRRF
{
    partial class FrmPensao
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
            this.TxtValorBruto = new System.Windows.Forms.TextBox();
            this.TxtPorcentagem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtOutrosDescontos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnCalcular = new System.Windows.Forms.Button();
            this.RtxDescricao = new System.Windows.Forms.RichTextBox();
            this.BtnDetalhar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Valor Bruto";
            // 
            // TxtValorBruto
            // 
            this.TxtValorBruto.Location = new System.Drawing.Point(12, 25);
            this.TxtValorBruto.Name = "TxtValorBruto";
            this.TxtValorBruto.Size = new System.Drawing.Size(83, 20);
            this.TxtValorBruto.TabIndex = 0;
            this.TxtValorBruto.Text = "0,00";
            this.TxtValorBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtValorBruto.TextChanged += new System.EventHandler(this.TxtValorBruto_TextChanged);
            this.TxtValorBruto.Enter += new System.EventHandler(this.TxtValorBruto_Enter);
            this.TxtValorBruto.Leave += new System.EventHandler(this.TxtValorBruto_Leave);
            // 
            // TxtPorcentagem
            // 
            this.TxtPorcentagem.Location = new System.Drawing.Point(101, 25);
            this.TxtPorcentagem.Name = "TxtPorcentagem";
            this.TxtPorcentagem.Size = new System.Drawing.Size(67, 20);
            this.TxtPorcentagem.TabIndex = 1;
            this.TxtPorcentagem.Text = "30,00";
            this.TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtPorcentagem.TextChanged += new System.EventHandler(this.TxtPorcentagem_TextChanged);
            this.TxtPorcentagem.Enter += new System.EventHandler(this.TxtPorcentagem_Enter);
            this.TxtPorcentagem.Leave += new System.EventHandler(this.TxtPorcentagem_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Porcentagem";
            // 
            // TxtOutrosDescontos
            // 
            this.TxtOutrosDescontos.Location = new System.Drawing.Point(174, 25);
            this.TxtOutrosDescontos.Name = "TxtOutrosDescontos";
            this.TxtOutrosDescontos.Size = new System.Drawing.Size(89, 20);
            this.TxtOutrosDescontos.TabIndex = 2;
            this.TxtOutrosDescontos.Text = "0,00";
            this.TxtOutrosDescontos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtOutrosDescontos.TextChanged += new System.EventHandler(this.TxtOutrosDescontos_TextChanged);
            this.TxtOutrosDescontos.Enter += new System.EventHandler(this.TxtOutrosDescontos_Enter);
            this.TxtOutrosDescontos.Leave += new System.EventHandler(this.TxtOutrosDescontos_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Outros Descontos";
            // 
            // BtnCalcular
            // 
            this.BtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCalcular.Location = new System.Drawing.Point(269, 25);
            this.BtnCalcular.Name = "BtnCalcular";
            this.BtnCalcular.Size = new System.Drawing.Size(104, 23);
            this.BtnCalcular.TabIndex = 3;
            this.BtnCalcular.Text = "Calcular";
            this.BtnCalcular.UseVisualStyleBackColor = true;
            this.BtnCalcular.Click += new System.EventHandler(this.BtnCalcular_Click);
            // 
            // RtxDescricao
            // 
            this.RtxDescricao.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RtxDescricao.Location = new System.Drawing.Point(12, 61);
            this.RtxDescricao.Name = "RtxDescricao";
            this.RtxDescricao.ReadOnly = true;
            this.RtxDescricao.Size = new System.Drawing.Size(954, 556);
            this.RtxDescricao.TabIndex = 5;
            this.RtxDescricao.Text = "";
            // 
            // BtnDetalhar
            // 
            this.BtnDetalhar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDetalhar.Location = new System.Drawing.Point(379, 25);
            this.BtnDetalhar.Name = "BtnDetalhar";
            this.BtnDetalhar.Size = new System.Drawing.Size(75, 23);
            this.BtnDetalhar.TabIndex = 4;
            this.BtnDetalhar.Text = "Detalhar";
            this.BtnDetalhar.UseVisualStyleBackColor = true;
            this.BtnDetalhar.Click += new System.EventHandler(this.BtnDetalhar_Click);
            // 
            // FrmPensao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 629);
            this.Controls.Add(this.BtnDetalhar);
            this.Controls.Add(this.RtxDescricao);
            this.Controls.Add(this.BtnCalcular);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtOutrosDescontos);
            this.Controls.Add(this.TxtPorcentagem);
            this.Controls.Add(this.TxtValorBruto);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPensao";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pensão";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtValorBruto;
        private System.Windows.Forms.TextBox TxtPorcentagem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtOutrosDescontos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnCalcular;
        private System.Windows.Forms.RichTextBox RtxDescricao;
        private System.Windows.Forms.Button BtnDetalhar;
    }
}