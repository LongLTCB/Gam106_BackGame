using Microsoft.EntityFrameworkCore;
using WebApplication1.Models; // ??m b?o namespace này ch?a l?p Player

namespace WebApplication1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // =======================================================
    // V? TRÍ C?N THÊM DbSet<Player>
    // =======================================================
    public DbSet<User> Users { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Player> Players { get; set; } // <--- ?Ã THÊM VÀO ?ÂY
    // =======================================================

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.regionId);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.roleId);
            entity.Property(e => e.Name).IsRequired();
        });

        // C?u hình cho User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.userId);
            entity.HasIndex(e => e.username).IsUnique();
            entity.Property(e => e.username).IsRequired();
            entity.Property(e => e.linkAvatar);
            entity.Property(e => e.otp);

            entity.HasOne(e => e.region)
                    .WithMany(r => r.Users)
                    .HasForeignKey("regionId")
                    .IsRequired();

            entity.HasOne(e => e.role)
                    .WithMany(r => r.Users)
                    .HasForeignKey("roleId")
                    .IsRequired();
        });

        // =======================================================
        // C?u hình cho Player (Tùy ch?n: ch? thêm n?u b?n c?n)
        // N?u không, EF Core s? t? ??ng ánh x? theo quy ??c
        // Ví d?:
        /*
        modelBuilder.Entity<Player>(entity =>
        {
             entity.HasKey(e => e.PlayerId); 
             entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });
        */
        // =======================================================


        // Optional: Seed some data
        modelBuilder.Entity<Region>().HasData(
            new Region(1, "Region1"),
            new Region(2, "Region2")
        );
        modelBuilder.Entity<Role>().HasData(
            new Role(1, "Admin"),
            new Role(2, "User")
        );
        modelBuilder.Entity<User>().HasData(
            new { userId = 1, username = "user1", linkAvatar = "avatar1.png", otp = 123456, regionId = 1, roleId = 1 },
            new { userId = 2, username = "user2", linkAvatar = "avatar2.png", otp = 654321, regionId = 2, roleId = 2 }
        );
    }
}