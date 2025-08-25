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
            label1 = new System.Windows.Forms.Label();
            TxtValorBruto = new System.Windows.Forms.TextBox();
            TxtPorcentagem = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            TxtOutrosDescontos = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            BtnCalcular = new System.Windows.Forms.Button();
            RtxDescricao = new System.Windows.Forms.RichTextBox();
            BtnDetalhar = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 10);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 15);
            label1.TabIndex = 0;
            label1.Text = "Valor Bruto";
            // 
            // TxtValorBruto
            // 
            TxtValorBruto.Location = new System.Drawing.Point(14, 29);
            TxtValorBruto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtValorBruto.Name = "TxtValorBruto";
            TxtValorBruto.Size = new System.Drawing.Size(96, 23);
            TxtValorBruto.TabIndex = 0;
            TxtValorBruto.Text = "0,00";
            TxtValorBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtValorBruto.TextChanged += TxtValorBruto_TextChanged;
            TxtValorBruto.Enter += TxtValorBruto_Enter;
            TxtValorBruto.Leave += TxtValorBruto_Leave;
            // 
            // TxtPorcentagem
            // 
            TxtPorcentagem.Location = new System.Drawing.Point(118, 29);
            TxtPorcentagem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtPorcentagem.Name = "TxtPorcentagem";
            TxtPorcentagem.Size = new System.Drawing.Size(78, 23);
            TxtPorcentagem.TabIndex = 1;
            TxtPorcentagem.Text = "30,00";
            TxtPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtPorcentagem.TextChanged += TxtPorcentagem_TextChanged;
            TxtPorcentagem.Enter += TxtPorcentagem_Enter;
            TxtPorcentagem.Leave += TxtPorcentagem_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(114, 10);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 15);
            label2.TabIndex = 3;
            label2.Text = "Porcentagem";
            // 
            // TxtOutrosDescontos
            // 
            TxtOutrosDescontos.Location = new System.Drawing.Point(203, 29);
            TxtOutrosDescontos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TxtOutrosDescontos.Name = "TxtOutrosDescontos";
            TxtOutrosDescontos.Size = new System.Drawing.Size(103, 23);
            TxtOutrosDescontos.TabIndex = 2;
            TxtOutrosDescontos.Text = "0,00";
            TxtOutrosDescontos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            TxtOutrosDescontos.TextChanged += TxtOutrosDescontos_TextChanged;
            TxtOutrosDescontos.Enter += TxtOutrosDescontos_Enter;
            TxtOutrosDescontos.Leave += TxtOutrosDescontos_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(200, 10);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(101, 15);
            label3.TabIndex = 3;
            label3.Text = "Outros Descontos";
            // 
            // BtnCalcular
            // 
            BtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnCalcular.Location = new System.Drawing.Point(314, 29);
            BtnCalcular.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnCalcular.Name = "BtnCalcular";
            BtnCalcular.Size = new System.Drawing.Size(121, 27);
            BtnCalcular.TabIndex = 3;
            BtnCalcular.Text = "Calcular";
            BtnCalcular.UseVisualStyleBackColor = true;
            BtnCalcular.Click += BtnCalcular_Click;
            // 
            // RtxDescricao
            // 
            RtxDescricao.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            RtxDescricao.Location = new System.Drawing.Point(14, 70);
            RtxDescricao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RtxDescricao.Name = "RtxDescricao";
            RtxDescricao.ReadOnly = true;
            RtxDescricao.Size = new System.Drawing.Size(1112, 641);
            RtxDescricao.TabIndex = 5;
            RtxDescricao.Text = "";
            // 
            // BtnDetalhar
            // 
            BtnDetalhar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnDetalhar.Location = new System.Drawing.Point(442, 29);
            BtnDetalhar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnDetalhar.Name = "BtnDetalhar";
            BtnDetalhar.Size = new System.Drawing.Size(88, 27);
            BtnDetalhar.TabIndex = 4;
            BtnDetalhar.Text = "Detalhar";
            BtnDetalhar.UseVisualStyleBackColor = true;
            BtnDetalhar.Click += BtnDetalhar_Click;
            // 
            // FrmPensao
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1141, 726);
            Controls.Add(BtnDetalhar);
            Controls.Add(RtxDescricao);
            Controls.Add(BtnCalcular);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(TxtOutrosDescontos);
            Controls.Add(TxtPorcentagem);
            Controls.Add(TxtValorBruto);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPensao";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Pensão";
            ResumeLayout(false);
            PerformLayout();

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