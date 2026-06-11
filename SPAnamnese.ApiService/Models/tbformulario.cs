using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbformulario")]
public partial class tbformulario
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [StringLength(255)]
    public string NOME { get; set; } = null!;

    [StringLength(255)]
    public string VERSAO { get; set; } = null!;
}
