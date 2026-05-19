using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace OrtSurvey.Context
{
    public class  OrtSurveyDataBase : dbContext
    {
        public
      OrtSurveyDataBase(DbContext<OrtSurveyDataBase> options)
       : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<RespuestaEncuesta> RespuestasEncuestas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=OrtSurvey;Trusted_Connection=True;");
        }
    }

}
}
