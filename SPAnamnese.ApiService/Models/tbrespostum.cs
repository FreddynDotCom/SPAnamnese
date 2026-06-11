using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPAnamnese.ApiService.Models;

public partial class tbrespostum
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int ID { get; set; }

    [Column(TypeName = "int(11)")]
    public int ATENDIMENTOID { get; set; }

    [Column(TypeName = "int(11)")]
    public int PERGUNTAID { get; set; }

    [Column(TypeName = "text")]
    public string VALOR { get; set; } = null!;
}
