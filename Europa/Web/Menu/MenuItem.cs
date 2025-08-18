using System.Collections.Generic;

namespace Europa.Web.Menu
{
    public class MenuItem
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string EnderecoAcesso { get; set; }
        public IList<MenuItem> Filhos { get; set; } = new List<MenuItem>();

        public MenuItem()
        {

        }

        public MenuItem(string nomeModulo)
        {
            Nome = nomeModulo;
        }

        public MenuItem(string codigo, string nome, string enderecoAcesso)
        {
            Codigo = codigo;
            Nome = nome;
            EnderecoAcesso = enderecoAcesso;
        }

        public bool IsModulo()
        {
            return Filhos != null && Filhos.Count > 0;
        }

        public bool IsUnidadeFuncional()
        {
            return !IsModulo();
        }

        public void AddFilho(MenuItem menuItem)
        {
            Filhos.Add(menuItem);
        }
    }
}
