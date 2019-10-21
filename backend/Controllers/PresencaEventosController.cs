using Microsoft.AspNetCore.Mvc;
using backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers
{
    //definimos nossa rota do controller e dizemos que é 1 controle de API
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaEventosController : ControllerBase
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/presenca
        [HttpGet]
        public async Task<ActionResult<List<PresencaEvento>>> Get()
        {
            var presencas = await _contexto.PresencaEvento.ToListAsync();

            if (presencas == null){
                return NotFound();
            }
            return presencas;
        }

        //GET: api/presenca/2
        [HttpGet("{id}")]
        public async Task<ActionResult<PresencaEvento>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            var presenca = await _contexto.PresencaEvento.FindAsync(id);

            if (presenca == null){
                return NotFound();
            }
            return presenca;
        }

        //POST api/presenca
        [HttpPost]
        public async Task<ActionResult<PresencaEvento>> Post(PresencaEvento presenca)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(presenca);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return presenca;
        }

        //PUT api/presenca/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, PresencaEvento presenca)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != presenca.IdPresenca){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(presenca).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var presenca_valido = await _contexto.PresencaEvento.FindAsync(id);

                if(presenca_valido == null){
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
        public async Task<ActionResult<PresencaEvento>> Delete(int id)
        {
            var presenca = await _contexto.PresencaEvento.FindAsync(id);

            if(presenca == null){
                return NotFound();
            }

            _contexto.PresencaEvento.Remove(presenca);
            await _contexto.SaveChangesAsync();

            return presenca;
        }
    }
}