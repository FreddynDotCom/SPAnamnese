using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbanexo")]
public partial class tbanexo
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [Column(TypeName = "int(11)")]
    public int ATENDIMENTOID { get; set; }

    [StringLength(255)]
    public string NOMEARQUIVO { get; set; } = null!;

    [StringLength(255)]
    public string CAMINHO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DATACADASTRO { get; set; }
}
