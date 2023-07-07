using System.ComponentModel.DataAnnotations;


//Request
namespace FilmesApi.Data.Dto
{
    public class CreateFilmeDto
    {
        [Required(ErrorMessage = "Informe o titulo do filme")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Informe o genero do filme")]
        [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "Informe a duracao do filme")]
        public int Duracao { get; set; }

    }
}
