using Microsoft.AspNetCore.Mvc;
using backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

//para adicionar a arvore de objetos adicionamos uma nova biblioteca json
//dotnet add package Microsoft.AspNetCore.MVC.NewtonSoftJson

namespace backend.Controllers{
    //definimos nossa rota do controller e dizemos que é 1 controle de API
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase  
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/evento
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            //var eventos = await _contexto.Evento.ToListAsync();
            var eventos = await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();

            if (eventos == null){
                return NotFound();
            }
            return eventos;
        }

        //GET: api/evento/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            //var evento = await _contexto.Evento.FindAsync(id);
            var evento = await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.IdEventos == id);
            //tbm poderia ser utilizado lambda 
            //var evento = await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.IdEventos == id);

            if (evento == null){
                return NotFound();
            }
            return evento;
        }

        //POST api/Evento
        [HttpPost]
        public async Task<ActionResult<Evento>> Post(Evento evento)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(evento);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return evento;
        }

        //PUT api/evento/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, Evento evento)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != evento.IdEventos){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(evento).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var evento_valido = await _contexto.Evento.FindAsync(id);

                if(evento_valido == null){
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
        public async Task<ActionResult<Evento>> Delete(int id)
        {
            var evento = await _contexto.Evento.FindAsync(id);

            if(evento == null){
                return NotFound();
            }

            _contexto.Evento.Remove(evento);
            await _contexto.SaveChangesAsync();

            return evento;
        }

    }
}