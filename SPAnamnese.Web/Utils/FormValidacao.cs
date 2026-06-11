namespace SPAnamnese.Web.Utils
{
    public static class FormValidacao
    {
        public static void ValidarCpf(string cpf, Dictionary<string, string> erros)
        {
            var cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
            if (string.IsNullOrWhiteSpace(cpfLimpo))
                erros["cpf"] = "O CPF é obrigatório.";
            else if (cpfLimpo.Length != 11 || !cpfLimpo.All(char.IsDigit))
                erros["cpf"] = "CPF inválido.";
        }

        public static void ValidarCampoPadraoObrigatorio(string? valor, string chave, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(valor))
                erros[chave] = "Esse campo é obrigatório.";
        }

        public static void ValidarSenha(string senha, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(senha))
                erros["senha"] = "A senha é obrigatória.";
        }

        public static void ValidarNome(string nome, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(nome))
                erros["nome"] = "O nome é obrigatório.";
            else if (nome.Trim().Length < 3)
                erros["nome"] = "Nome muito curto.";
        }

        public static void ValidarEmail(string email, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(email))
                erros["email"] = "O e-mail é obrigatório.";
            else if (!email.Contains('@') || !email.Contains('.'))
                erros["email"] = "E-mail inválido.";
        }

        public static void ValidarTelefone(string telefone, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                erros["telefone"] = "O telefone é obrigatório.";
        }

        public static void ValidarEndereco(string endereco, Dictionary<string, string> erros)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                erros["endereco"] = "O endereço é obrigatório.";
        }

        public static void ValidarDataNascimento(DateTime? data, Dictionary<string, string> erros)
        {
            if (data == null)
                erros["dataNascimento"] = "A data de nascimento é obrigatória.";
            else if (data > DateTime.Today)
                erros["dataNascimento"] = "Data de nascimento inválida.";
        }
    }
}