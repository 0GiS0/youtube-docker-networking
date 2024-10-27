using Microsoft.EntityFrameworkCore;

public class HeroesContext : DbContext
{
    public DbSet<Hero> Heroes { get; set; }

    public HeroesContext(DbContextOptions<HeroesContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Hero>().HasData(
            new Hero(1, "Batman", "Bruce Wayne", "A wealthy American playboy, philanthropist, and owner of Wayne Enterprises."),
            new Hero(2, "Superman", "Clark Kent", "Born as Kal-El on the dying planet Krypton, his parents Jor-El and Lara sent him to Earth."),
            new Hero(3, "Wonder Woman", "Diana Prince", "The Amazonian demigoddess daughter of Queen Hippolyta and Zeus, king of the Olympians."),
            new Hero(4, "Flash", "Barry Allen", "A Central City assistant police forensic investigator."),
            new Hero(5, "Green Lantern", "Hal Jordan", "A test pilot, Hal Jordan was chosen to join the Green Lantern Corps."),
            new Hero(6, "Aquaman", "Arthur Curry", "The son of Atlanna, queen of Atlantis, and Tom Curry, a lighthouse keeper.")
        );
    }
}