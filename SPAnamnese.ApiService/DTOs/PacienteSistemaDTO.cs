namespace SPAnamnese.ApiService.DTOs
{
    /// <summary>
    /// DTO de Paciente com informações privadas do sistema. Utilizado para operações internas e administrativas.
    /// DTO Completa.
    /// </summary>
    public class PacienteSistemaDTO
    {
        public int ID { get; set; }
        public string NOME { get; set; } = null;
        public string CPF { get; set; } = null;
        public DateTime DATANASCIMENTO { get; set; }
        public string TELEFONE { get; set; } = null;
        public string EMAIL { get; set; } = null;
        public string ENDERECO { get; set; } = null;
        public string RESPONSAVELLEGAL { get; set; } = null;
        public string SEXO { get; set; } = null;

        //Adicional
        public bool TEMANAMNESE { get; set; }
    }
}
