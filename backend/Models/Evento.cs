using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public partial class Evento
    {
        public Evento()
        {
            PresencaEvento = new HashSet<PresencaEvento>();
        }

        [Key]
        public int IdEventos { get; set; }
        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }
        [StringLength(100)]
        public string Descricao { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataEvento { get; set; }
        public bool? AcessoLivre { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdLocalizacao { get; set; }
        public bool? Ativo { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        [InverseProperty(nameof(Categoria.Evento))]
        public virtual Categoria IdCategoriaNavigation { get; set; }
        [ForeignKey(nameof(IdLocalizacao))]
        [InverseProperty(nameof(Localizacao.Evento))]
        public virtual Localizacao IdLocalizacaoNavigation { get; set; }
        [InverseProperty("IdEventoNavigation")]
        public virtual ICollection<PresencaEvento> PresencaEvento { get; set; }
    }
}
