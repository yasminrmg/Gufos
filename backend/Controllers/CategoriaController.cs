using Microsoft.AspNetCore.Mvc;
using backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers{
    //definimos nossa rota do controller e dizemos que é 1 controle de API
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase  
    {
        GufosBDContext _contexto = new GufosBDContext();

        //GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _contexto.Categoria.ToListAsync();

            if (categorias == null){
                return NotFound();
            }
            return categorias;
        }

        //GET: api/categoria/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco
            var categoria = await _contexto.Categoria.FindAsync(id);

            if (categoria == null){
                return NotFound();
            }
            return categoria;
        }

        //POST api/Categoria
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try{
                // tratamos contra ataques de sql injection
                await _contexto.AddAsync(categoria);
                //salvamos efetivament o nosso objeto no banco
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                throw;
            }

            return categoria;
        }

        //PUT api/categoria/3
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, Categoria categoria)
        {
            //se o id do objeto nao existir
            //ele retorna erro 400
            if(id != categoria.IdCategoria){
                return BadRequest();
            }

            //comparamos os atributos que foram modificados atraves do EF
            _contexto.Entry(categoria).State = EntityState.Modified;

            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                //verificamos se o bojeto inserido realmente existe no banco
                var categoria_valido = await _contexto.Categoria.FindAsync(id);

                if(categoria_valido == null){
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
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = await _contexto.Categoria.FindAsync(id);

            if(categoria == null){
                return NotFound();
            }

            _contexto.Categoria.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return categoria;
        }

    }
}