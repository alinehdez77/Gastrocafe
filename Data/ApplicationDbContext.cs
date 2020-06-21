using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanRafael.Models;
using SanRafael.Models.InsumoModels;

namespace SanRafael.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*builder.Entity<UnidadesInsumos>().HasKey(x => new { x.IdUnidad, x.IdInsumo });
            builder.Entity<UnidadesInsumos>()
                .HasOne(x => x.Unidad)
                .WithMany(x => x.Insumos)
                .HasForeignKey(pc => pc.IdInsumo);

            builder.Entity<UnidadesInsumos>()
                .HasOne(pc => pc.Insumo)
                .WithMany(c => c.UnidadesDelInsumo)
                .HasForeignKey(pc => pc.IdUnidad);
            */
            builder.Entity<InsumosRecetas>().HasKey(x => new { x.IdInsumo, x.IdReceta, x.IdUnidad });

            builder.Entity<InsumosRecetas>()
                .HasOne(x => x.Insumo)
                .WithMany(x => x.Recetas)
                .HasForeignKey(pc => pc.IdInsumo);
                
            builder.Entity<InsumosRecetas>()
                .HasOne(pc => pc.Receta)
                .WithMany(c => c.InsumosReceta)
                .HasForeignKey(pc => pc.IdReceta);
                
            builder.Entity<RecetaAReceta>().HasKey(x => new { x.IdRecetaHijo, x.IdRecetaPadre });
            builder.Entity<RecetaAReceta>()
                .HasOne(x => x.RecetaHijo)
                .WithMany(x => x.RecetasPadres)
                .HasForeignKey(x => x.IdRecetaHijo)
                .OnDelete(DeleteBehavior.Restrict);
            ;

            builder.Entity<RecetaAReceta>()
                .HasOne(x => x.RecetaPadre)
                .WithMany(x => x.RecetasIntegradoras)
                .HasForeignKey(x => x.IdRecetaPadre);

            //Agregado por: Alan Yoset Garcia Cruz
            builder.Entity<InsumosMarcas>().HasKey(x => new { x.InsumoId, x.MarcaId });
            builder.Entity<InsumosPresentaciones>().HasKey(x => new { x.InsumoId, x.PresentacionId });
             builder.Entity<InsumosPresentaciones>()
               .HasOne(pc => pc.Insumo)
               .WithOne()
               .OnDelete(DeleteBehavior.Restrict);
            
            //Fin de la modificación



        }

        public DbSet<AreaProduccion> AreaProduccion { get; set; }
        public DbSet<InsumosRecetas> InsumosRecetas { get; set; }
        public DbSet<Receta> Receta { get; set; }
        public DbSet<RecetaAReceta> RecetaAReceta { get; set; }

        //Agregado por: Alan Yoset Garcia Cruz
        public DbSet<InsumosMarcas> InsumosMarcas { get; set; }
        public DbSet<InsumosPresentaciones> InsumosPresentaciones { get; set; }
        //Fin de la modificación                

        #region Insumo
        public DbSet<Insumo> Insumo { get; set; }
        public DbSet<Unidad> Unidad { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<InsumoPrecioHistorial> InsumoPrecioHistorial { get; set; }
        public DbSet<Presentacion> Presentacion { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<SanRafael.Models.Costo> Costo { get; set; }
        #endregion
    }
}
