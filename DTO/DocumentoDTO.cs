using CadastroDocumentos.Enumerators;

namespace CadastroDocumentos.DTO
{
    public class DocumentoDTO
    {
        public int DocumentoID { get; set; }
        public string Nome { get; set; } = default!;
        public string? Descricao { get; set; }
        public EStatusDocumento Status { get; set; }
        public string? Base64Documento { get; set; }
    }
}
