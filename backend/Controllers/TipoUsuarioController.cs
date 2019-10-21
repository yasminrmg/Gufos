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
    public class TipoUsuarioController : ControllerBase
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/tipoUsuario
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get()
        {
            var tiposUsuarios = await _contexto.TipoUsuario.ToListAsync();

            if (tiposUsuarios == null){
                return NotFound();
            }
            return tiposUsuarios;
        }

        //GET: api/tipoUsuario/2
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuario>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            var tipoUsuario = await _contexto.TipoUsuario.FindAsync(id);

            if (tipoUsuario == null){
                return NotFound();
            }
            return tipoUsuario;
        }

        //POST api/tipoUsuario
        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> Post(TipoUsuario tipoUsuario)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(tipoUsuario);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return tipoUsuario;
        }

        //PUT api/tipoUsuario/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, TipoUsuario tipoUsuario)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != tipoUsuario.IdTipo){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(tipoUsuario).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var tipoUsuario_valido = await _contexto.TipoUsuario.FindAsync(id);

                if(tipoUsuario_valido == null){
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
        public async Task<ActionResult<TipoUsuario>> Delete(int id)
        {
            var tipoUsuario = await _contexto.TipoUsuario.FindAsync(id);

            if(tipoUsuario == null){
                return NotFound();
            }

            _contexto.TipoUsuario.Remove(tipoUsuario);
            await _contexto.SaveChangesAsync();

            return tipoUsuario;
        }
    }
}