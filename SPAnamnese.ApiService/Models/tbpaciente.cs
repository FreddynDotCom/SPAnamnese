using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbpaciente")]
public partial class tbpaciente
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [StringLength(255)]
    public string NOME { get; set; } = null!;

    [StringLength(11)]
    public string CPF { get; set; } = null!;

    public DateTime DATANASCIMENTO { get; set; }

    [StringLength(20)]
    public string TELEFONE { get; set; } = null!;

    [StringLength(15)]
    public string SEXO { get; set; } = null!;

    [StringLength(255)]
    public string RESPONSAVELLEGAL { get; set; } = null!;

    [StringLength(255)]
    public string EMAIL { get; set; } = null!;

    [StringLength(255)]
    public string ENDERECO { get; set; } = null!;
}
