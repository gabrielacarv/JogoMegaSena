﻿using JogoMegaSena.Model.Request;
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
    public class JogoController : PrincipalController
    {
        public JogoController()
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

        [HttpGet("ObterTodosOsJogos")]
        public IActionResult Get()
        {
            List<RegistroJogo> jogos = LerJogosDoArquivo();
            return Ok(jogos);
        }

        [HttpPost("RegistrarJogo")]
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

            if (!ValidarNumerosDiferentes(Numeros))
            {
                return ApiBadRequestResponse(ModelState, "Dados Inválidos, números repetidos");
            }

            bool ValidarNumerosDiferentes(List<int> numeros)
            {
                HashSet<int> numerosUnicos = new HashSet<int>();

                foreach (int numero in numeros)
                {
                    if (!numerosUnicos.Add(numero))
                    {
                        return false;
                    }
                }

                return true;
            }

            jogos.Add(novoJogo);
            EscreverJogoNoArquivo(jogos);

            return ApiResponse(novoJogo, "Jogo registrado com sucesso!");
        }
    }
}