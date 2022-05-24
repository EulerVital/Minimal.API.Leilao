using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Participante
    {
        public Usuario UltimoParticipante { get; set; } = new Usuario();
        public int QtdUsuarios { get; set; }
    }
}
