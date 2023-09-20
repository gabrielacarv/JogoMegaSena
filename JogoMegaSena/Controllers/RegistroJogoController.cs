using JogoMegaSena.Model.Request;
using JogoMegaSena.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JogoMegaSena.Model.Validation;

namespace JogoMegaSena.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroJogoController : PrincipalController
    {
        public RegistroJogoController()
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

        private void EscreverJogoNoArquivo(List<RegistroJogo> registroJogos)
        {
            string json = JsonConvert.SerializeObject(registroJogos);
            System.IO.File.WriteAllText(_registroJogoCaminhoArquivo, json);
        }
        #endregion

        [HttpPost]
        public IActionResult Post([FromBody] NovoRegistroJogoViewModel jogo)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequestResponse(ModelState, "Dados Inválidos");
            }

            List<RegistroJogo> jogos = LerJogosDoArquivo();

            RegistroJogo novoJogo = new RegistroJogo()
            {
                Nome = jogo.Nome,
                Cpf = jogo.Cpf,
                PrimeiroNro = jogo.PrimeiroNro,
                SegundoNro = jogo.SegundoNro,
                TerceiroNro = jogo.TerceiroNro,
                QuartoNro = jogo.QuartoNro,
                QuintoNro = jogo.QuintoNro,
                SextoNro = jogo.SextoNro,
                DataJogo = DateTime.Now
            };

            List<int> Numeros = new List<int>() { jogo.PrimeiroNro, jogo.SegundoNro, jogo.TerceiroNro, jogo.QuartoNro, jogo.QuintoNro, jogo.SextoNro};

            //Numeros.Add(jogo);
            if (!ValidarNumerosDiferentes(Numeros))
            {
                return ApiBadRequestResponse(ModelState, "Dados Inválidos, números repetidos");
            }

            bool ValidarNumerosDiferentes(List<int> numeros)
            {
                // Use um HashSet para rastrear números únicos.
                HashSet<int> numerosUnicos = new HashSet<int>();

                foreach (int numero in numeros)
                {
                    // Se tentarmos adicionar um número que já existe no HashSet, não será adicionado novamente.
                    if (!numerosUnicos.Add(numero))
                    {
                        return false; // Encontramos um número repetido, a validação falha.
                    }
                }

                // Se chegarmos até aqui, todos os números são diferentes.
                return true;
            }

            jogos.Add(novoJogo);
            EscreverJogoNoArquivo(jogos);

            return ApiResponse(novoJogo, "Jogo registrado com sucesso!");
        }
    }
}
