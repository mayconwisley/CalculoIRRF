﻿using CalculoIRRF.Model;
using CalculoIRRF.Modelo.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmDependente : Form
    {
        public FrmDependente()
        {
            InitializeComponent();
        }
        int idDependente = 0;
        private void ListarTabelaDependente()
        {
            try
            {
                Modelo.Dependente.Cadastro dependente = new Modelo.Dependente.Cadastro();
                DgvValorDependente.DataSource = dependente.ListarTodos();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimparCampos()
        {
            TxtValor.Text = "0,00";
            TxtValor.Focus();
        }
        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Modelo.Dependente.Cadastro dependente = new Modelo.Dependente.Cadastro();

                Dependente dados = new Dependente
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim())
                };

                dependente.Gravar(dados);
                ListarTabelaDependente();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                Modelo.Dependente.Cadastro dependente = new Modelo.Dependente.Cadastro();
                Dependente dados = new Dependente
                {
                    Id = idDependente,
                    Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim())
                };
                dependente.Alterar(dados);
                ListarTabelaDependente();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                Modelo.Dependente.Cadastro dependente = new Modelo.Dependente.Cadastro();
                dependente.Excluir(idDependente);
                ListarTabelaDependente();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DgvValorSimplificado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idDependente = int.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtValor.Text = decimal.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FrmDependente_Load(object sender, EventArgs e)
        {
            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
            ListarTabelaDependente();
        }
        private void TxtValor_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValor.Text = validar.ValidarValor(TxtValor.Text);
            TxtValor.Select(TxtValor.Text.Length, 0);
        }
        private void TxtValor_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValor.Text = validar.Zero(TxtValor.Text);
            TxtValor.Text = validar.Formatar(TxtValor.Text);
        }
        private void TxtValor_Enter(object sender, EventArgs e)
        {
            if (TxtValor.Text == "0,00")
            {
                TxtValor.Text = "";
            }
        }
    }
}
