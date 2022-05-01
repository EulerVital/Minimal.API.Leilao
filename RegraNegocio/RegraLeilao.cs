using Leilao.Repository;
using Models;
using RegraNegocio.Utils;

namespace RegraNegocio
{
    public class RegraLeilao : RegraBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RegraLeilao(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Models.Leilao LeilaoCriado { get; set; }

        /// <summary>
        /// Cria o leilão com opção de já adicionar 1 item
        /// </summary>
        /// <param name="nome">Nome do leilão que deve ser fonrcido.</param>
        /// <param name="isAddItem">True para add o item e false não add 1 item, default é true.</param>
        /// <returns></returns>
        public RegraLeilao Creator(string nome, bool isAddItem = true)
        {
            if (string.IsNullOrEmpty(nome))
            {
                Mensagem = "Parâmetro nome nao pode ser nulo.";
                throw new ArgumentException("nome", "Parâmetro nao pode ser nulo.");
            }

            LeilaoCriado = new Models.Leilao(nome);
            
            _dbContext.Add(LeilaoCriado);

            if (isAddItem)
                AddItens();

            _dbContext.SaveChanges();

            return this;
        }


        /// <summary>
        /// Adiciona o item ao leilão
        /// </summary>
        /// <param name="idLeilao">Id do leilão ao ser adicionado.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Não aceita que LeilaoCriado seja nuloe, execute o Creator primeiro.</exception>
        public RegraLeilao AddItens(int? idLeilao = null)
        {

            if(idLeilao > 0)
                LeilaoCriado = _dbContext.Leilaos.FirstOrDefault(c => c.Id == idLeilao) ?? new Models.Leilao();

            if (LeilaoCriado == null)
            {
                Mensagem = "Erro, não foi possível adicionar um item";
                throw new Exception("Não é possível adicionar itens, é necessário executar o 'Creator' primeiro.");
            }

            var criaItens = new RegraItens(_dbContext);
            criaItens.Creator(LeilaoCriado, !(idLeilao == null));

            return this;
        }

        /// <summary>
        /// Obtém um leilão cadastrado na propriedade LeilaoCriado, com base no id fornecido, ainda possui a opção de trazer os itens
        /// </summary>
        /// <param name="idLeilao">Id do leilão que deve ser fornecido</param>
        /// <param name="trazerItens">true para trazer os itens e false para não trazer, por default é true.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RegraLeilao GetLeilaoId(int idLeilao, bool trazerItens = true)
        {
            if(!(idLeilao > 0))
            {
                Mensagem = "Leilão não encontrado";
                throw new Exception("O parâmetro idLeilao deve ser maior que 0");
            }

            LeilaoCriado = _dbContext.Leilaos.FirstOrDefault(c => c.Id == idLeilao);

            if (LeilaoCriado == null) 
            {
                Mensagem = "Leilão não encontrado";
                return this;
            }

            if (trazerItens)
            {
                LeilaoCriado.Items = _dbContext.Items.Where(c => c.Leilao.Id == idLeilao).ToList();
            }

            return this;
        }

        /// <summary>
        /// Simplemente retorna todos os leilões cadastrados
        /// </summary>
        /// <returns></returns>
        public List<Models.Leilao> GetListLeilao()
        {
            return _dbContext.Leilaos.ToList();
        }


    }
}