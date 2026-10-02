#nullable enable

using CalculoIRRF.Presentation.Mvvm;
using System.Windows;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

public enum TipoCampo { Moeda, Numero, Inteiro, Data, Competencia, Horas }

/// <summary>Campo do formulário de uma calculadora; a janela escolhe o controle pelo tipo do campo.</summary>
public abstract class CampoViewModel(string rotulo, string? dica) : ViewModelBase
{
    private bool _visivel = true;

    public string Rotulo { get; } = rotulo;
    public string? Dica { get; } = dica;

    /// <summary>Campos que só valem para uma opção de outro campo ficam ocultos enquanto ela não está escolhida.</summary>
    public bool Visivel { get => _visivel; set => SetProperty(ref _visivel, value); }
}

public sealed class CampoTextoViewModel(string rotulo, TipoCampo tipo, string valorInicial, string? dica = null) : CampoViewModel(rotulo, dica)
{
    private string _valor = valorInicial;

    public TipoCampo Tipo { get; } = tipo;
    public string Valor { get => _valor; set => SetProperty(ref _valor, value); }

    /// <summary>Formato aplicado ao sair do campo; só os valores monetários são reformatados.</summary>
    public string? Formato => Tipo == TipoCampo.Moeda ? "N2" : null;

    public TextAlignment Alinhamento => Tipo is TipoCampo.Moeda or TipoCampo.Numero or TipoCampo.Inteiro or TipoCampo.Horas ? TextAlignment.Right : TextAlignment.Center;
}

public sealed record OpcaoCampo(string Texto, object Valor)
{
    public override string ToString() => Texto;
}

public sealed class CampoOpcaoViewModel : CampoViewModel
{
    private OpcaoCampo _selecionada;

    public CampoOpcaoViewModel(string rotulo, IReadOnlyList<OpcaoCampo> opcoes, string? dica = null) : base(rotulo, dica)
    {
        Opcoes = opcoes;
        _selecionada = opcoes[0];
    }

    public IReadOnlyList<OpcaoCampo> Opcoes { get; }
    public OpcaoCampo Selecionada
    {
        get => _selecionada;
        set
        {
            if (SetProperty(ref _selecionada, value))
                AoAlterar?.Invoke();
        }
    }
    public T Valor<T>() => (T)Selecionada.Valor;

    /// <summary>Chamado a cada mudança de opção, para mostrar ou ocultar os campos que dependem dela.</summary>
    public Action? AoAlterar { get; set; }

    public static CampoOpcaoViewModel SimNao(string rotulo, bool inicial, string? dica = null)
    {
        var campo = new CampoOpcaoViewModel(rotulo, [new("Não", false), new("Sim", true)], dica);
        campo.Selecionada = campo.Opcoes[inicial ? 1 : 0];
        return campo;
    }
}
