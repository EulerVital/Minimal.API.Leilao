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
    public class RegraItens : RegraBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RegraItens(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Item, se nulo basta excutar o metodo Creator
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Cria o item de acordo com o leilão passado
        /// </summary>
        /// <param name="leilao">Leilão que deve ser fornecido para criar o item</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Ser for passado valor nulo, será lançada a exeção</exception>
        public RegraItens Creator(Models.Leilao leilao, bool isSaveChange = true)
        {
            if (leilao == null)
                throw new ArgumentException("leiao", "Parâmetro não pode ser nulo");

            Item = new Item(Util.RetornaNomeStringAleatorio("Item ", 4), Util.RetornaNomeStringAleatorio("Descrição: ", 30), leilao);

            _dbContext.Items.Add(Item);

            if (isSaveChange)
                _dbContext.SaveChanges();

            return this;
        }

        /// <summary>
        /// Adiciona um usuário ao arremate
        /// </summary>
        /// <param name="usuario">Usuário que deve ser fornecido</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public RegraItens AddUsuarioArremate(Usuario usuario)
        {
            if(Item == null)
                throw new Exception("Item não pode ser nulo, tem que executar o 'Creator'.");

            if (usuario == null)
                throw new ArgumentException("usuario", "Parâmetro não pode ser nulo.");

            Item.Usuarios.Add(usuario);
            usuario.Itens.Add(Item);

            var itemUser = new ItemUsuario(usuario, Item);

            _dbContext.ItemUsuarios.Add(itemUser);
            itemUser.Id = _dbContext.SaveChanges();

            return this;
        }

        /// <summary>
        /// Obtém um item na propriedade "Item" com base no id fornecido
        /// </summary>
        /// <param name="idItem">Id a ser fornecido</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RegraItens GetItemId(int idItem)
        {
            if(!(idItem > 0))
            {
                Mensagem = "Item não encontrado";
                throw new Exception("Parâmetro deve ser maior que 0");
            }

            Item = _dbContext.Items.FirstOrDefault(c => c.Id == idItem);

            if(Item == null)
                Mensagem = "Item não encontrado";

            return this;
        }
    }
}
