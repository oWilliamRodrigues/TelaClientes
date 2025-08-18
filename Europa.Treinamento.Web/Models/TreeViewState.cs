using System.Diagnostics.CodeAnalysis;

//TODO verificar a necessidade de descer para outra "camada"

namespace Europa.Treinamento.Web.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TreeViewState
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}