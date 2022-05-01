namespace Models
{
    public class Usuario
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do usuário participando do leilão
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Itens ao qual está parcipando no momento
        /// </summary>
        public List<Item> Itens { get; set; }

        public Usuario(string nome, List<Item> itens)
        {
            Nome = nome;
            Itens = itens;
        }

        public Usuario(string nome)
        {
            Nome = nome;
            Itens = new List<Item>();
        }

        public Usuario()
        {

        }
    }
}