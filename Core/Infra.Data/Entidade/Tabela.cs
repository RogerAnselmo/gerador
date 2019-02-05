using System.ComponentModel.DataAnnotations;

namespace Core.Infra.Data.Entidade
{
    public class Tabela
    {
        [Display(Name = "Tabela:")]
        public string Nome { get; set; }

        [Display(Name = "Coluna")]
        public string Coluna { get; set; }

        [Display(Name = "Tipo de Dados")]
        public string TipoDado { get; set; }

        [Display(Name = "Tamanho")]
        public string Tamanho { get; set; }

        [Display(Name = "Nulo/Não Nulo")]
        public string NaoNulo { get; set; }

        [Display(Name = "Escala Decimal")]
        public int Escala { get; set; }

        [Display(Name = "Comentário")]
        public string Comentario { get; set; }

        [Display(Name = "Chave Primaria")]
        public string ChavePrimaria { get; set; }
    }
}
