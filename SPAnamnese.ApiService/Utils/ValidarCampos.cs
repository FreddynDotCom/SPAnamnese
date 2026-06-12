namespace SPAnamnese.ApiService.Utils
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Classe responsável por VALIDAR campos de entrada.
    /// Não altera os dados — apenas retorna true/false e mensagens de erro.
    /// Para limpeza dos dados, use <see cref="LimpezaDados"/>.
    /// </summary>
    public static class ValidacoesBack
    {
        // ─────────────────────────────────────────────
        //  NULO / VAZIO
        // ─────────────────────────────────────────────

        /// <summary>Verifica se uma string é nula ou vazia (inclui só espaços).</summary>
        public static bool IsNuloOuVazio(string? valor) =>
            string.IsNullOrWhiteSpace(valor);

        /// <summary>Verifica se um objeto genérico é nulo.</summary>
        public static bool IsNulo(object? valor) => valor is null;

        // ─────────────────────────────────────────────
        //  CPF
        // ─────────────────────────────────────────────

        /// <summary>
        /// Valida um CPF (aceita com ou sem máscara: 000.000.000-00 ou 00000000000).
        /// </summary>
        public static bool IsCpfValido(string? cpf)
        {
            if (IsNuloOuVazio(cpf)) return false;

            cpf = LimpezaDados.LimparCpfCnpj(cpf!);

            if (cpf.Length != 11) return false;
            if (!Regex.IsMatch(cpf, @"^\d{11}$")) return false;

            // Rejeita sequências inválidas conhecidas (ex: 111.111.111-11)
            if (new string(cpf[0], 11) == cpf) return false;

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf[9] == digito1.ToString()[0] && cpf[10] == digito2.ToString()[0];
        }

        // ─────────────────────────────────────────────
        //  CNPJ
        // ─────────────────────────────────────────────

        /// <summary>
        /// Valida um CNPJ (aceita com ou sem máscara: 00.000.000/0000-00 ou 14 dígitos).
        /// </summary>
        public static bool IsCnpjValido(string? cnpj)
        {
            if (IsNuloOuVazio(cnpj)) return false;

            cnpj = LimpezaDados.LimparCpfCnpj(cnpj!);

            if (cnpj.Length != 14) return false;
            if (!Regex.IsMatch(cnpj, @"^\d{14}$")) return false;
            if (new string(cnpj[0], 14) == cnpj) return false;

            int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * mult1[i];

            int resto = soma % 11;
            int d1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * mult2[i];

            resto = soma % 11;
            int d2 = resto < 2 ? 0 : 11 - resto;

            return cnpj[12] == d1.ToString()[0] && cnpj[13] == d2.ToString()[0];
        }

        // ─────────────────────────────────────────────
        //  CPF OU CNPJ (automático pelo tamanho)
        // ─────────────────────────────────────────────

        /// <summary>Detecta automaticamente se é CPF ou CNPJ e valida.</summary>
        public static bool IsCpfOuCnpjValido(string? valor)
        {
            if (IsNuloOuVazio(valor)) return false;
            string limpo = LimpezaDados.LimparCpfCnpj(valor!);
            return limpo.Length == 11 ? IsCpfValido(limpo) : IsCnpjValido(limpo);
        }

        // ─────────────────────────────────────────────
        //  TELEFONE / CELULAR
        // ─────────────────────────────────────────────

        /// <summary>
        /// Valida telefone brasileiro (fixo ou celular), com ou sem máscara.
        /// Formatos aceitos: (11) 91234-5678 / (11) 1234-5678 / 11912345678
        /// </summary>
        public static bool IsTelefoneValido(string? telefone)
        {
            if (IsNuloOuVazio(telefone)) return false;

            string limpo = LimpezaDados.LimparTelefone(telefone!);

            // Com DDD: 10 dígitos (fixo) ou 11 dígitos (celular)
            if (limpo.Length is not (10 or 11)) return false;

            // Celular começa com 9 após o DDD
            if (limpo.Length == 11 && limpo[2] != '9') return false;

            return Regex.IsMatch(limpo, @"^\d+$");
        }

        /// <summary>Valida apenas celular (11 dígitos com 9 após o DDD).</summary>
        public static bool IsCelularValido(string? telefone)
        {
            if (IsNuloOuVazio(telefone)) return false;
            string limpo = LimpezaDados.LimparTelefone(telefone!);
            return limpo.Length == 11 && limpo[2] == '9' && Regex.IsMatch(limpo, @"^\d{11}$");
        }

        // ─────────────────────────────────────────────
        //  EMAIL
        // ─────────────────────────────────────────────

        /// <summary>Valida formato de e-mail.</summary>
        public static bool IsEmailValido(string? email)
        {
            if (IsNuloOuVazio(email)) return false;
            return Regex.IsMatch(email!.Trim(),
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        // ─────────────────────────────────────────────
        //  SENHA
        // ─────────────────────────────────────────────

        /// <summary>
        /// Valida força de senha com critérios configuráveis.
        /// Por padrão: mínimo 8 caracteres, 1 maiúscula, 1 minúscula, 1 número, 1 especial.
        /// </summary>
        public static bool IsSenhaValida(
            string? senha,
            int tamanhoMinimo = 8,
            bool exigirMaiuscula = true,
            bool exigirMinuscula = true,
            bool exigirNumero = true,
            bool exigirEspecial = true)
        {
            if (IsNuloOuVazio(senha)) return false;
            if (senha!.Length < tamanhoMinimo) return false;
            if (exigirMaiuscula && !Regex.IsMatch(senha, @"[A-Z]")) return false;
            if (exigirMinuscula && !Regex.IsMatch(senha, @"[a-z]")) return false;
            if (exigirNumero && !Regex.IsMatch(senha, @"\d")) return false;
            if (exigirEspecial && !Regex.IsMatch(senha, @"[^a-zA-Z0-9]")) return false;
            return true;
        }

        /// <summary>Retorna uma descrição dos problemas da senha (útil para feedback ao usuário).</summary>
        public static string[] MensagensErroDeSenha(string? senha, int tamanhoMinimo = 8)
        {
            var erros = new System.Collections.Generic.List<string>();
            if (IsNuloOuVazio(senha)) { erros.Add("Senha não pode ser vazia."); return erros.ToArray(); }
            if (senha!.Length < tamanhoMinimo) erros.Add($"Mínimo de {tamanhoMinimo} caracteres.");
            if (!Regex.IsMatch(senha, @"[A-Z]")) erros.Add("Deve conter ao menos uma letra maiúscula.");
            if (!Regex.IsMatch(senha, @"[a-z]")) erros.Add("Deve conter ao menos uma letra minúscula.");
            if (!Regex.IsMatch(senha, @"\d")) erros.Add("Deve conter ao menos um número.");
            if (!Regex.IsMatch(senha, @"[^a-zA-Z0-9]")) erros.Add("Deve conter ao menos um caractere especial.");
            return erros.ToArray();
        }

        // ─────────────────────────────────────────────
        //  CEP
        // ─────────────────────────────────────────────

        /// <summary>Valida CEP brasileiro (com ou sem hífen: 01001-000 ou 01001000).</summary>
        public static bool IsCepValido(string? cep)
        {
            if (IsNuloOuVazio(cep)) return false;
            string limpo = LimpezaDados.LimparCep(cep!);
            return limpo.Length == 8 && Regex.IsMatch(limpo, @"^\d{8}$");
        }

        // ─────────────────────────────────────────────
        //  NUMÉRICOS GENÉRICOS
        // ─────────────────────────────────────────────

        /// <summary>Verifica se a string representa apenas dígitos.</summary>
        public static bool IsApenasNumeros(string? valor) =>
            !IsNuloOuVazio(valor) && Regex.IsMatch(valor!, @"^\d+$");

        /// <summary>Verifica se um int está dentro de um intervalo (inclusivo).</summary>
        public static bool IsIntNoIntervalo(int valor, int min, int max) =>
            valor >= min && valor <= max;

        /// <summary>Verifica se um long está dentro de um intervalo (inclusivo).</summary>
        public static bool IsLongNoIntervalo(long valor, long min, long max) =>
            valor >= min && valor <= max;

        /// <summary>Verifica se um decimal está dentro de um intervalo (inclusivo).</summary>
        public static bool IsDecimalNoIntervalo(decimal valor, decimal min, decimal max) =>
            valor >= min && valor <= max;

        // ─────────────────────────────────────────────
        //  TEXTO GENÉRICO
        // ─────────────────────────────────────────────

        /// <summary>Verifica se o texto possui ao menos <paramref name="minimo"/> caracteres.</summary>
        public static bool IsTamanhoMinimo(string? valor, int minimo) =>
            !IsNuloOuVazio(valor) && valor!.Trim().Length >= minimo;

        /// <summary>Verifica se o texto não excede <paramref name="maximo"/> caracteres.</summary>
        public static bool IsTamanhoMaximo(string? valor, int maximo) =>
            !IsNuloOuVazio(valor) && valor!.Trim().Length <= maximo;

        /// <summary>Verifica se o texto está dentro de um intervalo de tamanho (inclusive).</summary>
        public static bool IsTamanhoValido(string? valor, int minimo, int maximo)
        {
            if (IsNuloOuVazio(valor)) return false;
            int len = valor!.Trim().Length;
            return len >= minimo && len <= maximo;
        }

        // ─────────────────────────────────────────────
        //  DATA
        // ─────────────────────────────────────────────

        /// <summary>Verifica se a string representa uma data válida no formato dd/MM/yyyy.</summary>
        public static bool IsDataValida(string? data) =>
            !IsNuloOuVazio(data) &&
            DateTime.TryParseExact(data!.Trim(), "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _);

        /// <summary>Verifica se a data é maior que hoje (datas futuras).</summary>
        public static bool IsDataFutura(DateTime data) => data.Date > DateTime.Today;

        /// <summary>Verifica se a data é menor que hoje (datas passadas).</summary>
        public static bool IsDataPassada(DateTime data) => data.Date < DateTime.Today;

        /// <summary>Verifica se uma idade mínima é atingida com base na data de nascimento.</summary>
        public static bool IsIdadeMinima(DateTime nascimento, int idadeMinima)
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade >= idadeMinima;
        }

        // ─────────────────────────────────────────────
        //  URL
        // ─────────────────────────────────────────────

        /// <summary>Valida se a string é uma URL http/https bem formada.</summary>
        public static bool IsUrlValida(string? url) =>
            !IsNuloOuVazio(url) &&
            Uri.TryCreate(url!.Trim(), UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

        // ─────────────────────────────────────────────
        //  CARTÃO DE CRÉDITO (Luhn)
        // ─────────────────────────────────────────────

        /// <summary>Valida número de cartão de crédito usando o algoritmo de Luhn.</summary>
        public static bool IsCartaoValido(string? numero)
        {
            if (IsNuloOuVazio(numero)) return false;
            string limpo = Regex.Replace(numero!, @"\D", "");
            if (limpo.Length < 13 || limpo.Length > 19) return false;

            int soma = 0;
            bool dobrar = false;
            for (int i = limpo.Length - 1; i >= 0; i--)
            {
                int d = int.Parse(limpo[i].ToString());
                if (dobrar) { d *= 2; if (d > 9) d -= 9; }
                soma += d;
                dobrar = !dobrar;
            }
            return soma % 10 == 0;
        }
    }
}
