using JogoMegaSena.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JogoMegaSena.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObterTodosOsJogosController : PrincipalController
    {
        public ObterTodosOsJogosController()
        {
            _registroJogoCaminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", "jogosMega.json");
        }

        private readonly string _registroJogoCaminhoArquivo;

        #region Métodos arquivo
        private List<RegistroJogo> LerJogosDoArquivo()
        {
            if (!System.IO.File.Exists(_registroJogoCaminhoArquivo))
            {
                return new List<RegistroJogo>();
            }

            string json = System.IO.File.ReadAllText(_registroJogoCaminhoArquivo);
            if (string.IsNullOrEmpty(json))
            {
                return new List<RegistroJogo>();
            }
            return JsonConvert.DeserializeObject<List<RegistroJogo>>(json);
        }
        #endregion

        [HttpGet]
        public IActionResult Get()
        {
            List<RegistroJogo> jogos = LerJogosDoArquivo();
            return Ok(jogos);
        }
    }
}
