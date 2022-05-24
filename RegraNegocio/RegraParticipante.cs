using Leilao.Repository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegraNegocio
{
    public class RegraParticipante : RegraBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RegraParticipante(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Participante RetornaParticipanteLeilao(int idLeilao)
        {
            if (!(idLeilao > 0))
                throw new Exception("idLeiloa deve ser maior que 0.");

            var participante = new Participante();

            participante.QtdUsuarios = _dbContext.ItemUsuarios.Count(c => c.Item.LeilaoId == idLeilao);

            if (participante.QtdUsuarios > 0)
                participante.UltimoParticipante = _dbContext.ItemUsuarios.OrderByDescending(c => c.Id).Select(s => s.Usuario).FirstOrDefault() ?? new Usuario();

            return participante;            
        }

        
    }
}
