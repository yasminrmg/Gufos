using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public partial class PresencaEvento
    {
        [Key]
        public int IdPresenca { get; set; }
        public int? IdEvento { get; set; }
        public int? IdUsuario { get; set; }
        [Required]
        [StringLength(255)]
        public string PresencaStatus { get; set; }

        [ForeignKey(nameof(IdEvento))]
        [InverseProperty(nameof(Evento.PresencaEvento))]
        public virtual Evento IdEventoNavigation { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        [InverseProperty(nameof(Usuario.PresencaEvento))]
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}