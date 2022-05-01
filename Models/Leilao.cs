namespace Models
{
    public class Leilao
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do leilão
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data em que foi criado o leilão
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Itens do leilão
        /// </summary>
        public virtual List<Item> Items { get; set; }

        public Leilao(string nome, List<Item> items)
        {
            Nome = nome;
            Items = items;
            DataCriacao = DateTime.Now;
        }

        public Leilao(string nome)
        {
            Nome = nome;
            DataCriacao = DateTime.Now;
            Items = new List<Item>();
        }

        public Leilao()
        {
            Items = new List<Item>();
        }
    }
}