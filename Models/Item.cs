using System.Text.Json.Serialization;

namespace Models
{
    public class Item
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do item leiloado
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Descrição completa do item
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Data em que o item foi criado e colocado em leilão
        /// </summary>
        public DateTime DataCriacao { get; set; }
        /// <summary>
        /// Número de arremates do item
        /// </summary>
        public int NumeroArremates { get { return Usuarios == null ? 0 : Usuarios.Count; } }

        /// <summary>
        /// Leilão ao qual o item pertence
        /// </summary>
        [JsonIgnore]
        public Leilao Leilao { get; set; }

        public int LeilaoId { get; set; }

        /// <summary>
        /// Usuários que deram arremates nos itens
        /// </summary>
        public List<Usuario> Usuarios { get; set; }

        public Item(string nome, string descricao, Leilao leilao)
        {
            Nome = nome;
            Descricao = descricao;
            Leilao = leilao;
            LeilaoId = leilao.Id;
            DataCriacao = DateTime.Now;
            Usuarios = new List<Usuario>();
        }

        public Item(string nome, Leilao leilao)
        {
            Nome = nome;
            Leilao = leilao;
            LeilaoId = leilao.Id;
            DataCriacao = DateTime.Now;
            Usuarios = new List<Usuario>();
        }

        public Item()
        {

        }
    }
}