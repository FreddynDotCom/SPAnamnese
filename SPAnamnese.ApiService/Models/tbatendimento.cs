using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbatendimento")]
public partial class tbatendimento
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [Column(TypeName = "int(11)")]
    public int PACIENTEID { get; set; }

    [Column(TypeName = "int(11)")]
    public int PROFISSIONALID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DATAATENDIMENTO { get; set; }

    [Column(TypeName = "text")]
    public string OBSERVACOES { get; set; } = null!;
}
