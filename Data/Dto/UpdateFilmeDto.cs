using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dto
{
    public class UpdateFilmeDto
    {
        [Required(ErrorMessage = "Informe o titulo do filme")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Informe o genero do filme")]
        [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
        public string Genero { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe a duração do filme")]
        public int Duracao { get; set; }
    }
}