using Microsoft.AspNetCore.Mvc;
using backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers{
    //definimos nossa rota do controller e dizemos que é 1 controle de API
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase  
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/localizacao
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get()
        {
            var localizacoes = await _contexto.Localizacao.ToListAsync();

            if (localizacoes == null){
                return NotFound();
            }
            return localizacoes;
        }

        //GET: api/localizacao/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Localizacao>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            var localizacao = await _contexto.Localizacao.FindAsync(id);

            if (localizacao == null){
                return NotFound();
            }
            return localizacao;
        }

        //POST api/Localizacao
        [HttpPost]
        public async Task<ActionResult<Localizacao>> Post(Localizacao localizacao)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(localizacao);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return localizacao;
        }

        //PUT api/localizacao/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, Localizacao localizacao)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != localizacao.IdLocalizacao){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(localizacao).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var localizacao_valido = await _contexto.Localizacao.FindAsync(id);

                if(localizacao_valido == null){
                    return NotFound();
                }else{
                    throw;
                }
            }

            // no content = retorna 204, sem nada
            return NoContent();
        }

        //DELETE api/categboria/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Localizacao>> Delete(int id)
        {
            var localizacao = await _contexto.Localizacao.FindAsync(id);

            if(localizacao == null){
                return NotFound();
            }

            _contexto.Localizacao.Remove(localizacao);
            await _contexto.SaveChangesAsync();

            return localizacao;
        }

    }
}