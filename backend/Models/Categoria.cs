﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Evento = new HashSet<Evento>();
        }

        [Key]
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(2255)]
        public string Titulo { get; set; }
        public bool? Ativo { get; set; }

        [InverseProperty("IdCategoriaNavigation")]
        public virtual ICollection<Evento> Evento { get; set; }
    }
}
