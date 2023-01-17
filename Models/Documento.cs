using CadastroDocumentos.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace CadastroDocumentos.Models
{
    public class Documento
    {
        [Key]
        public int DocumentoID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        public EStatusDocumento Status { get; set; }
    }
}
