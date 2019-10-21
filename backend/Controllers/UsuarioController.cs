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
    public class UsuarioController : ControllerBase
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuarios = await _contexto.Usuario.ToListAsync();

            if (usuarios == null){
                return NotFound();
            }
            return usuarios;
        }

        //GET: api/usuario/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            var usuario = await _contexto.Usuario.FindAsync(id);

            if (usuario == null){
                return NotFound();
            }
            return usuario;
        }

        //POST api/usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(usuario);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return usuario;
        }

        //PUT api/usuario/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, Usuario usuario)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != usuario.IdUsuario){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(usuario).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var usuario_valido = await _contexto.Usuario.FindAsync(id);

                if(usuario_valido == null){
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
        public async Task<ActionResult<Usuario>> Delete(int id)
        {
            var usuario = await _contexto.Usuario.FindAsync(id);

            if(usuario == null){
                return NotFound();
            }

            _contexto.Usuario.Remove(usuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }
    }
}