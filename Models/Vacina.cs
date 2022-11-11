using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioVacinas.Models
{
    public class Vacina
    {
        public int VacinaId { get; set; }
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; }
        [ForeignKey("Virus")]
        public int FkVirus { get; set; }
        public virtual Virus? Virus { get; set; }


    }
}