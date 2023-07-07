using FilmesApi.Data;
using FilmesApi.Data.Dto;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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


        /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
            if (filme == null) return NotFound(new
            {
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

        [HttpGet]
        [Route("duracao/desc")]
        public IActionResult orderDuracaoDesc([FromQuery] int skip = 0, [FromQuery] int take = 100)
        {
            var filmesOrdenados = this.context.Filmes.OrderByDescending(filme => filme.Duracao);
            return Ok(filmesOrdenados.Skip(skip).Take(take));
        }


        [HttpGet]
        [Route("duracao/asc")]
        public IActionResult orderDuracaoAsc([FromQuery] int skip = 0, [FromQuery] int take = 100)
        {
            var filmesOrdenados = this.context.Filmes.OrderBy(filme => filme.Duracao);
            return Ok(filmesOrdenados.Skip(skip).Take(take));
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateFilmeEspecifico(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = this.context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound(new
            {
                Message = "Error",
                Sucess = false
            });//404

            var attFilme = new UpdateFilmeDto
            {
                Titulo = filme.Titulo,
                Genero = filme.Genero,
                Duracao = filme.Duracao
            };

            patch.ApplyTo(attFilme, ModelState);
            if (!TryValidateModel(attFilme))
            {
                return ValidationProblem(ModelState);
            }

            filme.Titulo = attFilme.Titulo;
            filme.Genero = attFilme.Genero;
            filme.Duracao = attFilme.Duracao;
            this.context.SaveChanges();

            return Ok();//200

        }

    }
   }
