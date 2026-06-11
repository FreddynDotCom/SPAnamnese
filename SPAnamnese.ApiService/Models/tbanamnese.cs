using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbanamnese")]
[Index("PacienteId", Name = "idx_paciente")]
[Index("ProfissionalId", Name = "idx_profissional")]
[MySqlCollation("utf8mb4_general_ci")]
public partial class tbanamnese
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int PacienteId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ProfissionalId { get; set; }

    [Column(TypeName = "text")]
    public string? QueixaPrincipal { get; set; }

    public DateOnly? HmaInicio { get; set; }

    [Column(TypeName = "text")]
    public string? HmaEvolucao { get; set; }

    [Column(TypeName = "text")]
    public string? HmaLocalizacao { get; set; }

    [Column(TypeName = "text")]
    public string? HmaSintomasAssociados { get; set; }

    [Column(TypeName = "text")]
    public string? HmaMelhoraPiora { get; set; }

    [Column(TypeName = "text")]
    public string? HmaTratamentosFeitos { get; set; }

    [Column(TypeName = "text")]
    public string? HmaImpactoRotina { get; set; }

    [Column(TypeName = "text")]
    public string? AntDoencasPrevias { get; set; }

    [Column(TypeName = "text")]
    public string? AntCirurgiasInternacoes { get; set; }

    [Column(TypeName = "text")]
    public string? AntAlergias { get; set; }

    [Column(TypeName = "text")]
    public string? AntMedicamentos { get; set; }

    [Column(TypeName = "text")]
    public string? AntVacinas { get; set; }

    [Column(TypeName = "text")]
    public string? AntGestacaoPartos { get; set; }

    [Column(TypeName = "text")]
    public string? FamDoencasCronicas { get; set; }

    [Column(TypeName = "text")]
    public string? FamCardiopatiasAvc { get; set; }

    [Column(TypeName = "text")]
    public string? FamCancer { get; set; }

    [Column(TypeName = "text")]
    public string? FamTranstornosMentais { get; set; }

    [Column(TypeName = "text")]
    public string? FamDoencasHereditarias { get; set; }

    [Column(TypeName = "text")]
    public string? HabAlimentacaoSonoHidratacao { get; set; }

    [Column(TypeName = "text")]
    public string? HabAtividadeFisica { get; set; }

    [Column(TypeName = "text")]
    public string? HabSubstancias { get; set; }

    [Column(TypeName = "text")]
    public string? HabTrabalhoPostura { get; set; }

    [Column(TypeName = "text")]
    public string? HabEstressePsicossocial { get; set; }

    [Column(TypeName = "text")]
    public string? RevRespiratorio { get; set; }

    [Column(TypeName = "text")]
    public string? RevCardiovascular { get; set; }

    [Column(TypeName = "text")]
    public string? RevGastrointestinal { get; set; }

    [Column(TypeName = "text")]
    public string? RevNeurologico { get; set; }

    [Column(TypeName = "text")]
    public string? RevGeniturinario { get; set; }

    [Column(TypeName = "text")]
    public string? RevOsteoarticular { get; set; }

    [Column(TypeName = "text")]
    public string? RevDermatologico { get; set; }

    [Column(TypeName = "text")]
    public string? RevOutros { get; set; }

    [Column(TypeName = "text")]
    public string? ExpectativaPaciente { get; set; }

    [Column(TypeName = "text")]
    public string? PlanoOrientacoesIniciais { get; set; }

    [Column(TypeName = "text")]
    public string? PlanoProximosPassos { get; set; }

    public DateOnly? PlanoDataRetorno { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime DataCriacao { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? DataAtualizacao { get; set; }
}
