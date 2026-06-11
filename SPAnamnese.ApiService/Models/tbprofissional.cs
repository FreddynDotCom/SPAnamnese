using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

[Table("tbprofissional")]
public partial class tbprofissional
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [StringLength(255)]
    public string NOME { get; set; } = null!;

    [StringLength(255)]
    public string EMAIL { get; set; } = null!;

    [StringLength(255)]
    public string PERFIL { get; set; } = null!;
}
