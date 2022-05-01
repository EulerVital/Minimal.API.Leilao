using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemUsuario
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public Item Item { get; set; }
        public int UsuarioId { get; set; }
        public int ItemId { get; set; }

        public ItemUsuario(Usuario usuario, Item item)
        {
            Usuario = usuario;
            Item = item;
            UsuarioId = usuario.Id;
            ItemId = item.Id;
        }

        public ItemUsuario()
        {

        }
    }
}
