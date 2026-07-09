
namespace SPAnamnese.Client.DTOs
{
    public class AnamneseDTO
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }

        public int? ProfissionalId { get; set; }

        public string? QueixaPrincipal { get; set; }

        public DateTime? HmaInicio { get; set; }

        public string? HmaEvolucao { get; set; }

        public string? HmaLocalizacao { get; set; }

        public string? HmaSintomasAssociados { get; set; }

        public string? HmaMelhoraPiora { get; set; }

        public string? HmaTratamentosFeitos { get; set; }

        public string? HmaImpactoRotina { get; set; }

        public string? AntDoencasPrevias { get; set; }

        public string? AntCirurgiasInternacoes { get; set; }

        public string? AntAlergias { get; set; }

        public string? AntMedicamentos { get; set; }

        public string? AntVacinas { get; set; }

        public string? AntGestacaoPartos { get; set; }

        public string? FamDoencasCronicas { get; set; }

        public string? FamCardiopatiasAvc { get; set; }

        public string? FamCancer { get; set; }

        public string? FamTranstornosMentais { get; set; }

        public string? FamDoencasHereditarias { get; set; }

        public string? HabAlimentacaoSonoHidratacao { get; set; }

        public string? HabAtividadeFisica { get; set; }

        public string? HabSubstancias { get; set; }

        public string? HabTrabalhoPostura { get; set; }

        public string? HabEstressePsicossocial { get; set; }

        public string? RevRespiratorio { get; set; }

        public string? RevCardiovascular { get; set; }

        public string? RevGastrointestinal { get; set; }

        public string? RevNeurologico { get; set; }

        public string? RevGeniturinario { get; set; }

        public string? RevOsteoarticular { get; set; }

        public string? RevDermatologico { get; set; }

        public string? RevOutros { get; set; }

        public string? ExpectativaPaciente { get; set; }

        public string? PlanoOrientacoesIniciais { get; set; }

        public string? PlanoProximosPassos { get; set; }

        public DateTime? PlanoDataRetorno { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public PacienteSistemaDTO Paciente { get; set; } = new PacienteSistemaDTO();
    }
}
