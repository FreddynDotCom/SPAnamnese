namespace SPAnamnese.ApiService.Utils
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Globalization;

    /// <summary>
    /// Classe responsável por LIMPAR / SANITIZAR dados de entrada.
    /// Remove caracteres de máscara, normaliza espaços, etc.
    /// Para validação dos dados, use <see cref="ValidacoesBack"/>.
    /// </summary>
    public static class LimpezaDados
    {
        // ─────────────────────────────────────────────
        //  CPF / CNPJ
        // ─────────────────────────────────────────────

        /// <summary>
        /// Remove pontos, traços e barras de CPF ou CNPJ.
        /// Entrada:  "123.456.789-09"  →  Saída: "12345678909"
        /// Entrada:  "11.222.333/0001-81" → Saída: "11222333000181"
        /// </summary>
        public static string LimparCpfCnpj(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[.\-/]", "").Trim();

        // ─────────────────────────────────────────────
        //  TELEFONE / CELULAR
        // ─────────────────────────────────────────────

        /// <summary>
        /// Remove parênteses, espaços, traços e o +55 de telefones.
        /// Entrada:  "(11) 91234-5678"  →  Saída: "11912345678"
        /// Entrada:  "+55 (11) 3456-7890" → Saída: "1134567890"
        /// </summary>
        public static string LimparTelefone(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return string.Empty;
            string limpo = Regex.Replace(valor, @"[^\d]", "");

            // Remove o código do país 55 se o número ficar muito longo
            if (limpo.Length == 13 && limpo.StartsWith("55"))
                limpo = limpo.Substring(2);
            else if (limpo.Length == 12 && limpo.StartsWith("55"))
                limpo = limpo.Substring(2);

            return limpo;
        }

        // ─────────────────────────────────────────────
        //  EMAIL
        // ─────────────────────────────────────────────

        /// <summary>
        /// Normaliza o e-mail: remove espaços e converte para minúsculas.
        /// Entrada:  "  Usuario@Email.COM  "  →  Saída: "usuario@email.com"
        /// </summary>
        public static string LimparEmail(string valor) =>
            (valor ?? string.Empty).Trim().ToLowerInvariant();

        // ─────────────────────────────────────────────
        //  CEP
        // ─────────────────────────────────────────────

        /// <summary>
        /// Remove o hífen do CEP.
        /// Entrada:  "01001-000"  →  Saída: "01001000"
        /// </summary>
        public static string LimparCep(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[^\d]", "").Trim();

        // ─────────────────────────────────────────────
        //  TEXTO GERAL
        // ─────────────────────────────────────────────

        /// <summary>Remove espaços extras (início, fim e múltiplos internos).</summary>
        public static string LimparEspacos(string valor) =>
            Regex.Replace((valor ?? string.Empty).Trim(), @"\s+", " ");

        /// <summary>Remove todos os caracteres não numéricos da string.</summary>
        public static string ApenasNumeros(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[^\d]", "");

        /// <summary>Remove todos os caracteres não alfabéticos (mantém letras e espaços).</summary>
        public static string ApenasLetras(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[^a-zA-ZÀ-ÿ\s]", "").Trim();

        /// <summary>Remove caracteres especiais, mantendo letras, números e espaços.</summary>
        public static string RemoverEspeciais(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[^a-zA-ZÀ-ÿ0-9\s]", "").Trim();

        /// <summary>
        /// Remove acentos/diacríticos de uma string.
        /// Entrada:  "Ação"  →  Saída: "Acao"
        /// </summary>
        public static string RemoverAcentos(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return string.Empty;
            string normalizado = valor.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalizado)
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>Capitaliza a primeira letra de cada palavra (Title Case).</summary>
        public static string CapitalizarNome(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return string.Empty;
            return CultureInfo.GetCultureInfo("pt-BR").TextInfo
                .ToTitleCase(valor.Trim().ToLower());
        }

        // ─────────────────────────────────────────────
        //  CARTÃO DE CRÉDITO
        // ─────────────────────────────────────────────

        /// <summary>Remove espaços e hifens do número do cartão.</summary>
        public static string LimparCartao(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[\s\-]", "");

        // ─────────────────────────────────────────────
        //  HTML / SQL INJECTION (sanitização básica)
        // ─────────────────────────────────────────────

        /// <summary>
        /// Escapa caracteres HTML perigosos para evitar XSS básico.
        /// Não substitui uso de bibliotecas de sanitização em produção.
        /// </summary>
        public static string EscaparHtml(string valor) =>
            (valor ?? string.Empty)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#x27;");

        /// <summary>
        /// Remove caracteres comuns de SQL Injection de forma básica.
        /// ATENÇÃO: prefira sempre parâmetros parametrizados no ADO.NET/EF.
        /// </summary>
        public static string SanitizarSql(string valor) =>
            Regex.Replace(valor ?? string.Empty, @"[';""\\]", "");

        // ─────────────────────────────────────────────
        //  UTILITÁRIO: Limpar + Validar em uma chamada
        // ─────────────────────────────────────────────

        /// <summary>
        /// Limpa e valida CPF em uma única chamada.
        /// Retorna o CPF limpo via <paramref name="cpfLimpo"/> e true se válido.
        /// </summary>
        public static bool LimparEValidarCpf(string? cpf, out string cpfLimpo)
        {
            cpfLimpo = LimparCpfCnpj(cpf ?? string.Empty);
            return ValidacoesBack.IsCpfValido(cpfLimpo);
        }

        /// <summary>
        /// Limpa e valida CNPJ em uma única chamada.
        /// Retorna o CNPJ limpo via <paramref name="cnpjLimpo"/> e true se válido.
        /// </summary>
        public static bool LimparEValidarCnpj(string? cnpj, out string cnpjLimpo)
        {
            cnpjLimpo = LimparCpfCnpj(cnpj ?? string.Empty);
            return ValidacoesBack.IsCnpjValido(cnpjLimpo);
        }

        /// <summary>
        /// Limpa e valida telefone em uma única chamada.
        /// Retorna o telefone limpo via <paramref name="telefoneLimpo"/> e true se válido.
        /// </summary>
        public static bool LimparEValidarTelefone(string? telefone, out string telefoneLimpo)
        {
            telefoneLimpo = LimparTelefone(telefone ?? string.Empty);
            return ValidacoesBack.IsTelefoneValido(telefoneLimpo);
        }

        /// <summary>
        /// Limpa e valida e-mail em uma única chamada.
        /// Retorna o e-mail normalizado via <paramref name="emailLimpo"/> e true se válido.
        /// </summary>
        public static bool LimparEValidarEmail(string? email, out string emailLimpo)
        {
            emailLimpo = LimparEmail(email ?? string.Empty);
            return ValidacoesBack.IsEmailValido(emailLimpo);
        }

        /// <summary>
        /// Limpa e valida CEP em uma única chamada.
        /// Retorna o CEP limpo via <paramref name="cepLimpo"/> e true se válido.
        /// </summary>
        public static bool LimparEValidarCep(string? cep, out string cepLimpo)
        {
            cepLimpo = LimparCep(cep ?? string.Empty);
            return ValidacoesBack.IsCepValido(cepLimpo);
        }
    }
}
