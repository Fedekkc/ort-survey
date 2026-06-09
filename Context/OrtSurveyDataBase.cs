using Microsoft.EntityFrameworkCore;
using OrtSurvey.Models;
using Microsoft.Extensions.Options;

namespace OrtSurvey.Context
{
    public class OrtSurveyDataBase : DbContext
    {
        public OrtSurveyDataBase(DbContextOptions<OrtSurveyDataBase> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.id_usuario);
                entity.Property(e => e.nombre).HasColumnType("varchar(80)").IsRequired();
                entity.Property(e => e.correo).HasColumnType("varchar(120)").IsRequired();
                entity.HasIndex(e => e.correo).IsUnique();
                entity.Property(e => e.password_hash).HasColumnType("varchar(255)").IsRequired();
                entity.Property(e => e.genero).HasColumnType("varchar(20)");
                entity.Property(e => e.fecha_creacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Encuesta>(entity => {
                entity.HasKey(e => e.id_encuesta);
                entity.Property(e => e.titulo).HasColumnType("varchar(120)").IsRequired();
                entity.Property(e => e.descripcion).HasColumnType("varchar(500)").IsRequired(false);
                entity.Property(e => e.es_publica).HasColumnType("bit").HasDefaultValue(true);
                entity.Property(e => e.fecha_creacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.fecha_cierre).HasColumnType("datetime").IsRequired(false);
                entity.Property(e => e.estado).HasColumnType("varchar(20)");
                entity.HasOne(e => e.Usuario).WithMany(u => u.Encuestas).HasForeignKey(e => e.id_usuario).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Pregunta>(entity => {
                entity.HasKey(e => e.id_pregunta);
                entity.Property(e => e.texto).HasColumnType("varchar(300)").IsRequired();
                entity.HasOne(e => e.Encuesta).WithMany(e => e.Preguntas).HasForeignKey(e => e.id_encuesta).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Opcion>(entity => {
                entity.HasKey(e => e.id_opcion);
                entity.Property(e => e.texto).HasColumnType("varchar(300)").IsRequired();
                entity.HasOne(e => e.Pregunta).WithMany(p => p.Opciones).HasForeignKey(e => e.id_pregunta).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Respuesta>(entity => {
                entity.HasKey(e => e.id_respuesta);
                entity.Property(e => e.valor_texto).HasColumnType("varchar(500)").IsRequired(false);
                entity.Property(e => e.submission_id).HasColumnType("uniqueidentifier");
                entity.Property(e => e.ip_respondedor).HasColumnType("varchar(45)");
                entity.Property(e => e.fecha_respuesta).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.id_usuario).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Pregunta).WithMany(p => p.Respuestas).HasForeignKey(e => e.id_pregunta).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Opcion).WithMany().HasForeignKey(e => e.id_opcion).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Reporte>(entity => {
                entity.HasKey(e => e.id_reporte);
                entity.Property(e => e.tipo_reporte).HasColumnType("varchar(20)");
                entity.Property(e => e.fecha_generacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.correo_destino).HasColumnType("varchar(120)").IsRequired(false);
                entity.HasOne(e => e.Encuesta).WithMany().HasForeignKey(e => e.id_encuesta).OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
