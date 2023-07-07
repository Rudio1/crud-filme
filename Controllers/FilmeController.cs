using FilmesApi.Data;
using FilmesApi.Data.Dto;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext context;

        public FilmeController(FilmeContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = new Filme
            {
                Titulo = filmeDto.Titulo,
                Genero = filmeDto.Genero,
                Duracao = filmeDto.Duracao,

            };

            this.context.Filmes.Add(filme);
            this.context.SaveChanges();

            return CreatedAtAction(nameof
                (getFilmesById), new { Id = filme.Id },
                filme); //201 Created
        }

        [HttpGet]
        public IEnumerable<Filme> getFilmes([FromQuery] int skip = 0, [FromQuery] int take = 100) //paginate
        {
            return this.context.Filmes.Skip(skip).Take(take);
        }

        [HttpGet("{id}")]
        public IActionResult getFilmesById(int id)
        {   
            var filme = this.context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound(new
            {
                Message = "Error",
                Sucess = false
            }); //404

            return Ok(filme); //200
        }

        [HttpPut("{id}")]

        public IActionResult UpdateFilme(int id, [FromBody] UpdateFilmeDto filmedto)
        {
            var filme = this.context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound(new { 
                Message = "Error",
                Sucess = false
            });//404

            filme.Titulo = filmedto.Titulo;
            filme.Genero = filmedto.Genero;
            filme.Duracao = filmedto.Duracao;

            this.context.SaveChanges();
            return Ok(new
            {
                Message = $"Filme de id {id}, atualizado com sucesso",
                Sucess = true
            }); //200

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFilme(int id)
        {
            var filme = this.context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound(new
            {
                Message = "Error",
                Sucess = false
            });//404

            this.context.Filmes.Remove(filme);
            this.context.SaveChanges();

            return Ok(new
            {
                Message = $"Filme de id {id} excluído com sucesso",
                Sucess = true
            });//200

        }
    }
}
