#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;
using CalculoIRRF.Domain.Trabalhista;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Verbas rescisórias conforme o motivo do desligamento. O aviso prévio indenizado integra o tempo de serviço
/// (CLT, art. 487, § 1º) e gera avos de 13º e de férias. Aviso indenizado, férias indenizadas e multa do FGTS
/// não têm INSS nem IRRF; o saldo de salário e o 13º têm, cada um calculado à parte.
/// </summary>
public sealed class SimularRescisaoUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularRescisaoRequest>
{
    public async Task<DemonstrativoDto> ExecutarAsync(SimularRescisaoRequest r, CancellationToken cancellationToken)
    {
        Validar(r);
        var motivo = r.Motivo;
        var justaCausa = motivo == MotivoRescisao.DispensaPorJustaCausa;
        var aviso = motivo is MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.Acordo or MotivoRescisao.PedidoDeDemissao ? r.Aviso : CumprimentoAvisoPrevio.TrabalhadoOuDispensado;
        if (motivo == MotivoRescisao.PedidoDeDemissao && aviso == CumprimentoAvisoPrevio.Indenizado)
            throw new ArgumentException("No pedido de demissão não há aviso prévio indenizado: o empregado cumpre o aviso, é dispensado dele pelo empregador ou tem o valor descontado se não cumprir.");
        if (motivo != MotivoRescisao.PedidoDeDemissao && aviso == CumprimentoAvisoPrevio.NaoCumpridoPeloEmpregado)
            throw new ArgumentException("O desconto do aviso prévio não cumprido só se aplica ao pedido de demissão.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, new DateOnly(r.Desligamento.Year, r.Desligamento.Month, 1), cancellationToken);
        var remuneracao = r.Salario + r.Medias;
        var ano = r.Desligamento.Year;
        var anosCompletos = RegrasTrabalhistas.AnosCompletos(r.Admissao, r.Desligamento);

        var diasSaldo = DiasDeSaldo(r.Admissao, r.Desligamento);
        var saldo = Arredondar(r.Salario / 30m * diasSaldo);

        // No acordo, o aviso indenizado é pago pela metade (CLT, art. 484-A, I, a).
        var diasAvisoProporcional = RegrasTrabalhistas.DiasDeAvisoPrevio(r.Admissao, r.Desligamento);
        var diasIndenizados = aviso == CumprimentoAvisoPrevio.Indenizado ? (motivo == MotivoRescisao.Acordo ? diasAvisoProporcional / 2m : diasAvisoProporcional) : 0m;
        var avisoIndenizado = Arredondar(remuneracao / 30m * diasIndenizados);
        var descontoAviso = aviso == CumprimentoAvisoPrevio.NaoCumpridoPeloEmpregado ? Arredondar(remuneracao) : 0m;
        var projetada = r.Desligamento.AddDays((int)Math.Floor(diasIndenizados));

        int avos13 = 0, avos13Aviso = 0;
        if (!justaCausa)
        {
            avos13 = RegrasTrabalhistas.AvosDecimoTerceiro(r.Admissao, r.Desligamento, ano);
            var avosProjetados = projetada.Year == ano
                ? RegrasTrabalhistas.AvosDecimoTerceiro(r.Admissao, projetada, ano)
                : RegrasTrabalhistas.AvosDecimoTerceiro(r.Admissao, new DateOnly(ano, 12, 31), ano) + RegrasTrabalhistas.AvosDecimoTerceiro(r.Admissao, projetada, projetada.Year);
            avos13Aviso = avosProjetados - avos13;
        }
        var decimo = Arredondar(remuneracao / 12m * avos13);
        var decimoAviso = Arredondar(remuneracao / 12m * avos13Aviso);

        var feriasVencidas = Arredondar(remuneracao * r.PeriodosFeriasVencidas);
        var tercoVencidas = Arredondar(feriasVencidas / 3m);
        var inicioPeriodo = RegrasTrabalhistas.InicioPeriodoAquisitivo(r.Admissao, r.Desligamento);
        var diasDireito = RegrasTrabalhistas.DiasDeFeriasPorFaltas(r.FaltasPeriodoAtual);
        int avosFerias = 0, avosFeriasAviso = 0;
        if (!justaCausa)
        {
            avosFerias = RegrasTrabalhistas.AvosFerias(inicioPeriodo, r.Desligamento);
            avosFeriasAviso = RegrasTrabalhistas.AvosFerias(inicioPeriodo, projetada) - avosFerias;
        }
        var feriasProporcionais = Arredondar(remuneracao / 30m * diasDireito / 12m * avosFerias);
        var feriasAviso = Arredondar(remuneracao / 30m * diasDireito / 12m * avosFeriasAviso);
        var tercoProporcionais = Arredondar((feriasProporcionais + feriasAviso) / 3m);

        var depositoFgts = Arredondar((saldo + avisoIndenizado + decimo + decimoAviso) * .08m);
        var percentualMulta = motivo switch { MotivoRescisao.DispensaSemJustaCausa => 40m, MotivoRescisao.Acordo => 20m, _ => 0m };
        var percentualSaque = motivo switch { MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.TerminoDeContratoPorPrazo => 100m, MotivoRescisao.Acordo => 80m, _ => 0m };
        var usaSaldoFgts = percentualMulta > 0m || percentualSaque > 0m;
        var mesesContrato = RegrasTrabalhistas.AvosFerias(r.Admissao, r.Desligamento);
        var saldoEstimado = r.SaldoFgts == 0m;
        var saldoFgts = saldoEstimado ? Arredondar(remuneracao * .08m * mesesContrato * 13m / 12m) : r.SaldoFgts;
        var multa = Arredondar((saldoFgts + depositoFgts) * percentualMulta / 100m);
        var saque = Arredondar((saldoFgts + depositoFgts) * percentualSaque / 100m) + multa;

        var inssSaldo = tabelas.CalcularInss(saldo);
        var irrfSaldo = tabelas.CalcularIrrf(saldo, inssSaldo.Valor, r.Dependentes);
        var total13 = decimo + decimoAviso;
        var inss13 = tabelas.CalcularInss(total13);
        var irrf13 = tabelas.CalcularIrrf(total13, inss13.Valor, r.Dependentes);

        var proventos = new List<VerbaDto> { new("Saldo de salário", Formato.Dias(diasSaldo), saldo) };
        if (avisoIndenizado > 0m) proventos.Add(new("Aviso prévio indenizado", Dias(diasIndenizados), avisoIndenizado));
        if (decimo > 0m) proventos.Add(new("13º salário proporcional", Formato.Avos(avos13), decimo));
        if (decimoAviso > 0m) proventos.Add(new("13º sobre o aviso prévio indenizado", Formato.Avos(avos13Aviso), decimoAviso));
        if (feriasVencidas > 0m)
        {
            proventos.Add(new("Férias vencidas", r.PeriodosFeriasVencidas == 1 ? "1 período" : $"{r.PeriodosFeriasVencidas} períodos", feriasVencidas));
            proventos.Add(new("1/3 sobre férias vencidas", "", tercoVencidas));
        }
        if (feriasProporcionais > 0m) proventos.Add(new("Férias proporcionais", Formato.Avos(avosFerias), feriasProporcionais));
        if (feriasAviso > 0m) proventos.Add(new("Férias sobre o aviso prévio indenizado", Formato.Avos(avosFeriasAviso), feriasAviso));
        if (tercoProporcionais > 0m) proventos.Add(new("1/3 sobre férias proporcionais", "", tercoProporcionais));

        var descontos = new List<VerbaDto>();
        if (saldo > 0m)
        {
            descontos.Add(new("INSS sobre o saldo de salário", "", inssSaldo.Valor));
            descontos.Add(new(MemoriaTributaria.DescricaoIrrf("IRRF sobre o saldo de salário", irrfSaldo), MemoriaTributaria.ReferenciaIrrf(irrfSaldo), irrfSaldo.Imposto));
        }
        if (total13 > 0m)
        {
            descontos.Add(new("INSS sobre o 13º", "", inss13.Valor));
            descontos.Add(new(MemoriaTributaria.DescricaoIrrf("IRRF sobre o 13º", irrf13), MemoriaTributaria.ReferenciaIrrf(irrf13), irrf13.Imposto));
        }
        if (descontoAviso > 0m) descontos.Add(new("Aviso prévio não cumprido", "30 dias", descontoAviso));
        if (r.AdiantamentoDecimoTerceiro > 0m) descontos.Add(new("Adiantamento do 13º já pago", "", r.AdiantamentoDecimoTerceiro));
        var liquido = proventos.Sum(verba => verba.Valor) - descontos.Sum(verba => verba.Valor);

        var informativos = new List<VerbaDto> { new("Depósito do FGTS do mês da rescisão", "8%", depositoFgts) };
        if (usaSaldoFgts) informativos.Add(new(saldoEstimado ? "Saldo do FGTS (estimado)" : "Saldo do FGTS (informado)", "", saldoFgts));
        if (multa > 0m) informativos.Add(new("Multa rescisória do FGTS", Formato.PercentualCurto(percentualMulta), multa));
        if (percentualSaque > 0m) informativos.Add(new("FGTS disponível para saque", Formato.PercentualCurto(percentualSaque) + (multa > 0m ? " + multa" : ""), saque));

        var memoria = new List<GrupoMemoriaDto>
        {
            MemoriaContrato(r, aviso, anosCompletos, diasAvisoProporcional, diasIndenizados, avisoIndenizado, remuneracao, projetada, descontoAviso),
            new("Saldo de salário", $"Saldo: {Formato.Moeda(saldo)}",
            [
                new("Dias trabalhados no mês", $"{Formato.Dias(diasSaldo)} até {Formato.Data(r.Desligamento)}, no mês comercial de 30 dias"),
                new("Saldo de salário", $"{Formato.Moeda(r.Salario)} ÷ 30 x {Formato.Dias(diasSaldo)} = {Formato.Moeda(saldo)}")
            ])
        };
        if (!justaCausa)
            memoria.Add(new("13º salário", $"13º: {Formato.Moeda(total13)}",
            [
                new("Avos até o desligamento", $"{Formato.Avos(avos13)}: meses de {ano} com 15 dias ou mais de trabalho"),
                new("Avos pela projeção do aviso", avos13Aviso > 0 ? $"{Formato.Avos(avos13Aviso)} a mais, com o contrato projetado até {Formato.Data(projetada)}" : "Nenhum avo a mais"),
                new("13º proporcional", $"{FormulaRemuneracao(r)} ÷ 12 x {avos13 + avos13Aviso} avos = {Formato.Moeda(total13)}")
            ]));
        memoria.Add(MemoriaFerias(r, justaCausa, inicioPeriodo, diasDireito, avosFerias, avosFeriasAviso, feriasProporcionais + feriasAviso, tercoProporcionais, feriasVencidas, tercoVencidas));
        memoria.Add(MemoriaFgts(depositoFgts, saldo, avisoIndenizado, total13, usaSaldoFgts, saldoEstimado, saldoFgts, remuneracao, mesesContrato, percentualMulta, multa, percentualSaque, saque));
        if (saldo > 0m)
        {
            memoria.Add(MemoriaTributaria.Inss("INSS sobre o saldo de salário", inssSaldo, "saldo de salário"));
            memoria.Add(MemoriaTributaria.Irrf("IRRF sobre o saldo de salário", irrfSaldo, "saldo de salário"));
        }
        if (total13 > 0m)
        {
            memoria.Add(MemoriaTributaria.Inss("INSS sobre o 13º", inss13, "13º proporcional"));
            memoria.Add(MemoriaTributaria.Irrf("IRRF sobre o 13º", irrf13, "13º proporcional"));
        }

        var observacoes = new List<string>
        {
            DescreverMotivo(motivo),
            "Aviso prévio indenizado, férias indenizadas e multa do FGTS não têm INSS nem IRRF; o 13º tem INSS e IRRF calculados à parte do saldo de salário.",
            "O pagamento deve ser feito em até 10 dias após o fim do contrato (CLT, art. 477, § 6º).",
            "Não inclui férias vencidas em dobro, horas extras e adicionais do mês, descontos de benefícios nem verbas previstas em convenção coletiva."
        };
        if (usaSaldoFgts && saldoEstimado)
            observacoes.Insert(1, "O saldo do FGTS foi estimado com o salário atual; informe o saldo do extrato do FGTS para obter a multa e o saque exatos.");
        if (liquido < 0m)
            observacoes.Insert(0, "Os descontos superam os proventos. Na rescisão, a compensação de descontos é limitada a uma remuneração mensal (CLT, art. 477, § 5º).");

        return new DemonstrativoDto(
            "Rescisão do contrato de trabalho",
            $"{NomeMotivo(motivo)} • Desligamento em {Formato.Data(r.Desligamento)}",
            [
                new("Líquido da rescisão", Formato.Moeda(liquido), "Pago em até 10 dias"),
                new("Aviso prévio", DescreverAviso(motivo, aviso, diasAvisoProporcional, diasIndenizados), aviso == CumprimentoAvisoPrevio.Indenizado ? $"Indenizado: {Formato.Moeda(avisoIndenizado)}" : "Sem valor a pagar"),
                new("Multa do FGTS", percentualMulta > 0m ? Formato.Moeda(multa) : "Não se aplica", percentualMulta > 0m ? $"{Formato.PercentualCurto(percentualMulta)} sobre o saldo do FGTS" : "Somente na dispensa sem justa causa e no acordo"),
                new("FGTS para saque", percentualSaque > 0m ? Formato.Moeda(saque) : "Sem saque", percentualSaque > 0m ? (saldoEstimado ? "Com saldo estimado" : "Com o saldo informado") : "O FGTS fica na conta")
            ],
            proventos,
            descontos,
            informativos,
            memoria,
            observacoes,
            RotuloResultado: "Líquido da rescisão");
    }

    private static void Validar(SimularRescisaoRequest r)
    {
        if (r.Desligamento < r.Admissao)
            throw new ArgumentException("A data de desligamento deve ser igual ou posterior à data de admissão.");
        if (r.Salario < 0m || r.Medias < 0m || r.SaldoFgts < 0m || r.AdiantamentoDecimoTerceiro < 0m || r.Dependentes < 0 || r.FaltasPeriodoAtual < 0)
            throw new ArgumentException("Os valores, as faltas e a quantidade de dependentes não podem ser negativos.");
        if (r.PeriodosFeriasVencidas is < 0 or > 2)
            throw new ArgumentException("Informe de 0 a 2 períodos de férias vencidas.");
    }

    /// <summary>Dias de trabalho no mês do desligamento, no mês comercial de 30 dias.</summary>
    private static int DiasDeSaldo(DateOnly admissao, DateOnly desligamento)
    {
        var mesmoMes = admissao.Year == desligamento.Year && admissao.Month == desligamento.Month;
        var ultimoDiaDoMes = desligamento.Day == DateTime.DaysInMonth(desligamento.Year, desligamento.Month);
        if (mesmoMes)
            return Math.Min(30, (ultimoDiaDoMes && admissao.Day == 1 ? 30 : desligamento.Day - admissao.Day + 1));
        return ultimoDiaDoMes ? 30 : Math.Min(30, desligamento.Day);
    }

    private static GrupoMemoriaDto MemoriaContrato(SimularRescisaoRequest r, CumprimentoAvisoPrevio aviso, int anosCompletos, int diasAvisoProporcional, decimal diasIndenizados, decimal avisoIndenizado, decimal remuneracao, DateOnly projetada, decimal descontoAviso)
    {
        var formulas = new List<FormulaDto>
        {
            new("Contrato", $"De {Formato.Data(r.Admissao)} a {Formato.Data(r.Desligamento)}: {anosCompletos} ano(s) completo(s) de serviço")
        };
        if (r.Motivo is MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.Acordo)
            formulas.Add(new("Aviso prévio proporcional", $"30 dias + 3 x {anosCompletos} ano(s) = {Formato.Dias(diasAvisoProporcional)}{(diasAvisoProporcional == 90 ? " (limite de 90 dias)" : "")} (Lei 12.506/2011)"));
        if (aviso == CumprimentoAvisoPrevio.Indenizado)
        {
            if (r.Motivo == MotivoRescisao.Acordo)
                formulas.Add(new("Metade do aviso no acordo", $"{Formato.Dias(diasAvisoProporcional)} ÷ 2 = {Dias(diasIndenizados)} (CLT, art. 484-A)"));
            formulas.Add(new("Aviso prévio indenizado", $"{FormulaRemuneracao(r)} ÷ 30 x {Dias(diasIndenizados)} = {Formato.Moeda(avisoIndenizado)}"));
            formulas.Add(new("Projeção do contrato", $"O aviso indenizado conta como tempo de serviço: o contrato se estende até {Formato.Data(projetada)} para o 13º e as férias."));
        }
        else if (descontoAviso > 0m)
            formulas.Add(new("Aviso prévio não cumprido", $"{FormulaRemuneracao(r)} de 30 dias = {Formato.Moeda(descontoAviso)}, descontados (CLT, art. 487, § 2º)"));
        else if (r.Motivo is MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.Acordo or MotivoRescisao.PedidoDeDemissao)
            formulas.Add(new("Aviso prévio", "Trabalhado ou dispensado: os dias trabalhados já estão no saldo de salário."));
        else
            formulas.Add(new("Aviso prévio", "Não há aviso prévio neste motivo de desligamento."));
        return new GrupoMemoriaDto("Contrato e aviso prévio", aviso == CumprimentoAvisoPrevio.Indenizado ? $"Aviso: {Formato.Moeda(avisoIndenizado)}" : "Aviso sem valor", formulas);
    }

    private static GrupoMemoriaDto MemoriaFerias(SimularRescisaoRequest r, bool justaCausa, DateOnly inicioPeriodo, int diasDireito, int avosFerias, int avosFeriasAviso, decimal proporcionais, decimal tercoProporcionais, decimal vencidas, decimal tercoVencidas)
    {
        var formulas = new List<FormulaDto>();
        if (vencidas > 0m)
        {
            formulas.Add(new("Férias vencidas", $"{FormulaRemuneracao(r)} x {r.PeriodosFeriasVencidas} período(s) = {Formato.Moeda(vencidas)}"));
            formulas.Add(new("1/3 sobre férias vencidas", $"{Formato.Moeda(vencidas)} ÷ 3 = {Formato.Moeda(tercoVencidas)}"));
        }
        if (justaCausa)
            formulas.Add(new("Férias proporcionais", "Não são devidas na dispensa por justa causa (Súmula 171 do TST)."));
        else
        {
            formulas.Add(new("Período aquisitivo em curso", $"Desde {Formato.Data(inicioPeriodo)}: {Formato.Avos(avosFerias)} até o desligamento{(avosFeriasAviso > 0 ? $" e {Formato.Avos(avosFeriasAviso)} a mais pela projeção do aviso" : "")}"));
            formulas.Add(new("Dias de direito", $"{r.FaltasPeriodoAtual} falta(s) injustificada(s): {Formato.Dias(diasDireito)} por período completo (CLT, art. 130)"));
            formulas.Add(new("Férias proporcionais", $"{FormulaRemuneracao(r)} ÷ 30 x {Formato.Dias(diasDireito)} ÷ 12 x {avosFerias + avosFeriasAviso} avos = {Formato.Moeda(proporcionais)}"));
            formulas.Add(new("1/3 sobre férias proporcionais", $"{Formato.Moeda(proporcionais)} ÷ 3 = {Formato.Moeda(tercoProporcionais)}"));
        }
        return new GrupoMemoriaDto("Férias", $"Férias + 1/3: {Formato.Moeda(proporcionais + tercoProporcionais + vencidas + tercoVencidas)}", formulas);
    }

    private static GrupoMemoriaDto MemoriaFgts(decimal deposito, decimal saldo, decimal avisoIndenizado, decimal total13, bool usaSaldoFgts, bool saldoEstimado, decimal saldoFgts, decimal remuneracao, int mesesContrato, decimal percentualMulta, decimal multa, decimal percentualSaque, decimal saque)
    {
        var formulas = new List<FormulaDto>
        {
            new("Depósito do mês", $"({Formato.Moeda(saldo)} (saldo) + {Formato.Moeda(avisoIndenizado)} (aviso) + {Formato.Moeda(total13)} (13º)) x 8% = {Formato.Moeda(deposito)}")
        };
        if (usaSaldoFgts)
            formulas.Add(new("Saldo do FGTS", saldoEstimado
                ? $"Estimado: {Formato.Moeda(remuneracao)} x 8% x {mesesContrato} meses de contrato x 13 ÷ 12 (com o 13º) = {Formato.Moeda(saldoFgts)}"
                : $"Informado: {Formato.Moeda(saldoFgts)}"));
        if (percentualMulta > 0m)
            formulas.Add(new("Multa rescisória", $"({Formato.Moeda(saldoFgts)} + {Formato.Moeda(deposito)}) x {Formato.PercentualCurto(percentualMulta)} = {Formato.Moeda(multa)}"));
        formulas.Add(new("Saque", percentualSaque > 0m
            ? $"({Formato.Moeda(saldoFgts)} + {Formato.Moeda(deposito)}) x {Formato.PercentualCurto(percentualSaque)}{(multa > 0m ? $" + {Formato.Moeda(multa)} (multa)" : "")} = {Formato.Moeda(saque)}"
            : "Neste motivo de desligamento, o FGTS não pode ser sacado."));
        return new GrupoMemoriaDto("FGTS", percentualSaque > 0m ? $"Saque: {Formato.Moeda(saque)}" : $"Depósito: {Formato.Moeda(deposito)}", formulas);
    }

    private static string FormulaRemuneracao(SimularRescisaoRequest r) =>
        r.Medias > 0m ? $"({Formato.Moeda(r.Salario)} + {Formato.Moeda(r.Medias)} de médias)" : Formato.Moeda(r.Salario);

    private static string DescreverAviso(MotivoRescisao motivo, CumprimentoAvisoPrevio aviso, int diasProporcional, decimal diasIndenizados) => aviso switch
    {
        CumprimentoAvisoPrevio.Indenizado => Dias(diasIndenizados),
        CumprimentoAvisoPrevio.NaoCumpridoPeloEmpregado => "Descontado",
        _ when motivo is MotivoRescisao.DispensaPorJustaCausa or MotivoRescisao.TerminoDeContratoPorPrazo => "Não se aplica",
        _ when motivo == MotivoRescisao.PedidoDeDemissao => "30 dias",
        _ => Formato.Dias(diasProporcional)
    };

    private static string NomeMotivo(MotivoRescisao motivo) => motivo switch
    {
        MotivoRescisao.DispensaSemJustaCausa => "Dispensa sem justa causa",
        MotivoRescisao.PedidoDeDemissao => "Pedido de demissão",
        MotivoRescisao.Acordo => "Acordo entre as partes",
        MotivoRescisao.DispensaPorJustaCausa => "Dispensa por justa causa",
        _ => "Término de contrato por prazo determinado"
    };

    private static string DescreverMotivo(MotivoRescisao motivo) => motivo switch
    {
        MotivoRescisao.DispensaSemJustaCausa => "Dispensa sem justa causa: o trabalhador recebe todas as verbas, a multa de 40% do FGTS e pode sacar o FGTS.",
        MotivoRescisao.PedidoDeDemissao => "Pedido de demissão: não há multa do FGTS nem saque; o 13º e as férias proporcionais são devidos (Súmula 261 do TST).",
        MotivoRescisao.Acordo => "Acordo (CLT, art. 484-A): metade do aviso indenizado, multa de 20% do FGTS e saque de 80% do saldo; as demais verbas são integrais.",
        MotivoRescisao.DispensaPorJustaCausa => "Justa causa: são devidos apenas o saldo de salário e as férias vencidas com 1/3.",
        _ => "Término do contrato por prazo determinado no prazo: sem aviso prévio e sem multa do FGTS, com saque do FGTS."
    };

    private static string Dias(decimal dias) => dias == 1m ? "1 dia" : $"{Formato.Numero(dias)} dias";

    private static decimal Arredondar(decimal valor) => CalculadoraTributacao.Arredondar(valor);
}
