using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

public partial class tbperguntum
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [Column(TypeName = "int(11)")]
    public int FORMULARIOID { get; set; }

    [StringLength(255)]
    public string TEXTO { get; set; } = null!;

    [StringLength(255)]
    public string TIPOCAMPO { get; set; } = null!;

    [Column(TypeName = "int(11)")]
    public int ORDEM { get; set; }

    public bool OBRIGATORIA { get; set; }
}
