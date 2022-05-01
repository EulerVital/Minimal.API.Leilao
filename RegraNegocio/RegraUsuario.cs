using Leilao.Repository;
using Models;
using RegraNegocio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegraNegocio
{
    public class RegraUsuario : RegraBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RegraUsuario(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Cria usuário
        /// </summary>
        /// <param name="nome">Se fornecido, coloca como nome do usuário</param>
        /// <param name="item">Se fornecido, adiciona o item</param>
        /// <returns></returns>
        public RegraUsuario Creator(string? nome = null, Item? item = null)
        {
            Usuario = string.IsNullOrEmpty(nome) ?  new Usuario(Util.RetornaNomeStringAleatorio("Usuário ", 5)) : new Usuario(nome);

            if (item != null)
                Usuario.Itens.Add(item);

            _dbContext.Usuarios.Add(Usuario);
            Usuario.Id = _dbContext.SaveChanges();

            if (item != null)
            {
                var itemUsuario = new ItemUsuario(Usuario, item);

                _dbContext.ItemUsuarios.Add(itemUsuario);
                itemUsuario.Id = _dbContext.SaveChanges();
            }

            return this;
        }

        /// <summary>
        /// Usuário participa do item leiloado
        /// </summary>
        /// <param name="idUser">Id do usuário</param>
        /// <param name="idItem">Id do item</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RegraUsuario EntrarNoItem(int idUser, int idItem)
        {
            if (!(idItem > 0))
                throw new Exception("Parâmetro idItem não pode ser meno que 0.");

            if (!(idUser > 0))
                throw new Exception("Parâmetro idUser precisa ser maior que 0");

            Usuario = _dbContext.Usuarios.FirstOrDefault(c => c.Id == idUser);

            if (Usuario == null)
            {
                Mensagem = "Usuário não encontrado";
                return this;
            }

            var item = _dbContext.Items.FirstOrDefault(i => i.Id == idItem);

            if (item == null)
            {
                Mensagem = "Item não encontrado";
                return this;
            }

            var itemUser = new ItemUsuario(Usuario, item);

            if(_dbContext.ItemUsuarios.Where(w=>w.ItemId ==  idItem && w.UsuarioId == idUser).FirstOrDefault() != null)
            {
                Mensagem = "Já está participando do item leiloado";
            }

            _dbContext.ItemUsuarios.Add(itemUser);
            itemUser.Id = _dbContext.SaveChanges();

            Usuario.Itens = Usuario.Itens ?? new List<Item>();
            Usuario.Itens.Add(item);

            return this;
        }

        public RegraUsuario GetUsuarioId(int idUsuario, bool isTrazerItens = false)
        {
            if(!(idUsuario > 0))
            {
                Mensagem = "Usuário não encontrado";
                throw new Exception("Parâmetro não pode ser menor que 0");
            }

            Usuario = _dbContext.Usuarios.FirstOrDefault(c => c.Id == idUsuario);

            if(Usuario == null)
            {
                Mensagem = "Usuário não encontrado";
                return this;
            }

            if (isTrazerItens)
                Usuario.Itens = _dbContext.ItemUsuarios.Where(c => c.UsuarioId == idUsuario).Select(s => s.Item).ToList();

            return this;
        }
    }
}
