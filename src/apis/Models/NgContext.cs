﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;

namespace apis.Models
{
    public class NgContext : IdentityDbContext<Models.User, IdentityRole<Int32>, Int32>
    {
        IConfigurationRoot _configuration;

        public NgContext(IConfigurationRoot configuration)
        {
            _configuration = configuration;

            Database.Migrate();
        }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Recette> Recettes { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Commentaire> Commentaires { get; set; }

        public DbSet<IngredientRecette> IngredientsRecettes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String connString = _configuration.Get<String>("Data:NgConnectionString");

            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IngredientRecette>()
                .HasKey(t => new { t.IngredientId, t.RecetteId });

            builder.Entity<Recette>()
                .Ignore(r => r.Ingredients)
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.CreatorId)
                .HasPrincipalKey(u => u.Id);

            builder.Entity<IngredientRecette>()
                .HasOne(ir => ir.Recette)
                .WithMany(r => r.IngredientsRecettes)
                .HasForeignKey(ir => ir.RecetteId)
                .HasPrincipalKey(r => r.Id);

            builder.Entity<IngredientRecette>()
                .HasOne(ir => ir.Ingredient)
                .WithMany(i => i.IngredientsRecettes)
                .HasForeignKey(ir => ir.IngredientId)
                .HasPrincipalKey(i => i.Id);

            builder.Entity<Commentaire>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder.Entity<Commentaire>()
                .HasOne(c => c.Recette)
                .WithMany(r => r.Commentaires)
                .HasForeignKey(c => c.RecetteId);

            builder.Entity<User>()
                .Ignore(u => u.Password);

            builder.Entity<Recette>()
                .Ignore(u => u.Calories);

            base.OnModelCreating(builder);
        }
    }
}
