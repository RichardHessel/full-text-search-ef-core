using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fulltext.Context;
using fulltext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fulltext.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> _logger;
        private readonly DbSet<Produto> _db;
        public ProdutosController(ILogger<ProdutosController> logger, CatalogoContext ctx)
        {
            _logger = logger;
            _db = ctx.Set<Produto>();
        }



        /// <summary>
        ///  Retorna os produtos que possuírem registrado na coluna "descrição" o termo especificado na consulta
        ///  com variações da língua portuguesa utilizando o termo "INFLECTIONAL" ou registros com a parte inicial do texto
        ///  idêntica ao que foi especificado no termo.
        /// </summary>
        /// <param name="termo">termo utilizado para filtragem</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string termo)
        {
            if (string.IsNullOrEmpty(termo))
                return BadRequest();

            var produtos =  
                _db.Where(p =>
                    Funcoes.ContainsIn("(Descricao, Observacao)",
                    $"FORMSOF(INFLECTIONAL, {termo}) OR \"{termo}*\""))
                    .ToList();

            return Ok(produtos);
        }

    }
}
