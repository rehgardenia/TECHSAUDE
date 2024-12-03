using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechSaude.Server.Models;

namespace TechSaude.Server.Data
{
    public class AppDbContext :IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<HistoricoMedico> HistoricosMedicos { get; set; }

        // HISTORICO MEDICO
        public DbSet<Alergia> Alergias { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }
        public DbSet<Doenca> Doencas { get; set; }

        public DbSet<Documento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuração do índice único
            modelBuilder.Entity<Paciente>()
             .HasIndex(p => p.CNS)
             .IsUnique();
            
            modelBuilder.Entity<Medico>()
             .HasIndex(m=> m.CRM)
             .IsUnique();

            modelBuilder.Entity<Medico>()
                .HasKey(m=> m.Id);

            // Configuração do relacionamento entre Consulta e Paciente
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração do relacionamento entre Consulta e Medico
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Medico)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configura os relacionamentos entre HistoricoMedico e Paciente
            modelBuilder.Entity<HistoricoMedico>()
               .HasOne(hm => hm.Paciente)
               .WithOne(p => p.HistoricosMedicos)
               .HasForeignKey<HistoricoMedico>(hm => hm.PacienteId)
               .OnDelete(DeleteBehavior.Cascade);

            // Configura a chave primária composta e relacionamento para Alergia
            modelBuilder.Entity<Alergia>()
                .HasKey(a =>  a.AlergiaId );
            modelBuilder.Entity<Alergia>()
                .HasOne(a => a.Historico)
                .WithMany(h => h.Alergias)
                .HasForeignKey(a => a.HistoricoId);

            // Configura a chave primária composta e relacionamento para Exame
            modelBuilder.Entity<Documento>()
                .HasKey(e =>  e.DocumentoId );

            modelBuilder.Entity<Documento>()
                .HasOne(e => e.Historico)
                .WithMany(h => h.Documentos)
                .HasForeignKey(e => e.HistoricoId);
            
           // Configura a chave primária composta e relacionamento para Doenca
            modelBuilder.Entity<Doenca>()
                .HasKey(d => d.DoencaId );
            modelBuilder.Entity<Doenca>()
                .HasOne(d => d.Historico)
                .WithMany(h => h.Doencas)
                .HasForeignKey(d => d.HistoricoId);


            // Configura a chave primária composta e relacionamento para Vacina
            modelBuilder.Entity<Vacina>()
                .HasKey(v => v.VacinaId );
            modelBuilder.Entity<Vacina>()
                .HasOne(v => v.Historico)
                .WithMany(h => h.Vacinas)
                .HasForeignKey(v => v.HistoricoId);


            base.OnModelCreating(modelBuilder);
        }
    }
 
}
