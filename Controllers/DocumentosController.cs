using CadastroDocumentos.Data;
using CadastroDocumentos.DTO;
using CadastroDocumentos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroDocumentos.Controllers
{
    public class DocumentosController : Controller
    {
        private readonly CadastroDocumentosContext _context;

        public DocumentosController(CadastroDocumentosContext context)
        {
            _context = context;
        }

        // GET: Documentos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Documento.ToListAsync());
        }

        // GET: Documentos/Details/5
        public async Task<IActionResult> Exibir(int? id)
        {
            if (id == null || _context.Documento == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .FirstOrDefaultAsync(m => m.DocumentoID == id);
            if (documento == null)
            {
                return NotFound();
            }

            var documentoDTO = new DocumentoDTO
            {
                DocumentoID = documento.DocumentoID,
                Nome = documento.Nome,
                Descricao = documento.Descricao,
                Status = documento.Status
            };

            return View(documentoDTO);
        }

        // GET: Documentos/Create
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Documentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salvar([Bind("DocumentoID,Nome,Descricao,Status,Base64Documento")] DocumentoDTO documentoDTO)
        {
            if (ModelState.IsValid)
            {
                var documento = new Documento
                {
                    Nome = documentoDTO.Nome,
                    Descricao = documentoDTO.Descricao,
                    DocumentoID = 0,
                    Status = documentoDTO.Status
                };
                _context.Add(documento);
                await _context.SaveChangesAsync();

                if (!SalvarDocumentoLocal(documentoDTO.Base64Documento, documento.Nome))
                    return Problem("Erro ao salvar arquivo local");
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(documentoDTO);
        }

        private bool SalvarDocumentoLocal(string? base64Documento, string? nomeDocumento)
        {
            try
            {
                if (!string.IsNullOrEmpty(base64Documento))
                {
                    var path = @"C:\CadastroDocumentos";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    var somenteBase64 = base64Documento.Split(',')[1];
                    var extension = base64Documento.Split(';')[0].Split('/')[1];
                    byte[] fileBytes = Convert.FromBase64String(somenteBase64);
                    System.IO.File.WriteAllBytes($"{path}\\{nomeDocumento}", fileBytes);
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

            return true;
        }

        // GET: Documentos/Edit/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || _context.Documento == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            var documentoDTO = new DocumentoDTO
            {
                DocumentoID = documento.DocumentoID,
                Nome = documento.Nome,
                Descricao = documento.Descricao,
                Status = documento.Status
            };

            return View(documentoDTO);
        }

        // POST: Documentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("DocumentoID,Nome,Descricao,Status")] Documento documento)
        {
            if (id != documento.DocumentoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentoExists(documento.DocumentoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var documentoDTO = new DocumentoDTO
            {
                DocumentoID = documento.DocumentoID,
                Nome = documento.Nome,
                Descricao = documento.Descricao,
                Status = documento.Status
            };

            return View(documentoDTO);
        }

        // GET: Documentos/Baixar/5
        public async Task<IActionResult> Baixar(int? id)
        {
            if (id == null || _context.Documento == null)
            {
                return NotFound();
            }
            var documento = await _context.Documento.FindAsync(id);

            string path = $"C:\\CadastroDocumentos\\{documento?.Nome}";
            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return Problem(@"Arquivo não localizado no diretório: C:\CadastroDocumentos");
        }

        // GET: Documentos/Delete/5
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.Documento == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .FirstOrDefaultAsync(m => m.DocumentoID == id);
            if (documento == null)
            {
                return NotFound();
            }

            var documentoDTO = new DocumentoDTO
            {
                DocumentoID = documento.DocumentoID,
                Nome = documento.Nome,
                Descricao = documento.Descricao,
                Status = documento.Status
            };

            return View(documentoDTO);
        }

        // POST: Documentos/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExcluir(int id)
        {
            if (_context.Documento == null)
            {
                return Problem("Entity set 'CadastroDocumentosContext.Documento'  is null.");
            }
            var documento = await _context.Documento.FindAsync(id);
            if (documento != null)
            {
                _context.Documento.Remove(documento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documento.Any(e => e.DocumentoID == id);
        }
    }
}
