using CowsAndChicken.DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CowsAndChicken.Infrastructure
{
    public class CowsAndChickenContext : DbContext, ICowsAndChickenContext
    {
        public CowsAndChickenContext(DbContextOptions<CowsAndChickenContext> options): base(options)
        {
            
        }

        public virtual DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasKey(p => p.Id);
            modelBuilder.Entity<Player>().Metadata.FindNavigation(nameof(Player.Games)).SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Player>().HasMany(p => p.Games).WithOne().IsRequired();
            modelBuilder.Entity<Player>().Ignore(p => p.YearOfBirth);

            modelBuilder.Entity<Game>().HasKey(g => g.Id);
            modelBuilder.Entity<Game>().Property<int[]>("_numberToGuess").HasColumnName("NumberToGuess").HasConversion(v => string.Join("", v), v => Array.ConvertAll(v.ToArray(), x => int.Parse(x.ToString())));
            modelBuilder.Entity<Game>().Metadata.FindNavigation(nameof(Game.Turns)).SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Game>().HasMany(g => g.Turns).WithOne().IsRequired();

            modelBuilder.Entity<Turn>().Property<int>("Id");
            modelBuilder.Entity<Turn>().HasKey("Id");
            modelBuilder.Entity<Turn>().OwnsOne(t => t.Outcome);
            modelBuilder.Entity<Turn>().Property(t => t.Guess).HasConversion(v => string.Join("", v), v => Array.ConvertAll(v.ToArray(), x => int.Parse(x.ToString())));
        }
    }
}
