using jh_payment_database.Entity;
using Microsoft.EntityFrameworkCore;

namespace jh_payment_database.DatabaseContext
{
    public class JHDataAccessContext : DbContext
    {
        public JHDataAccessContext(DbContextOptions<JHDataAccessContext> options) : base(options) { }

        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionInformation> TransactionInformations => Set<TransactionInformation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.PaymentId);

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<User>()
                .HasKey(t => t.UserId);

            // enable foreign keys (EF ensures referential shapes in migrations)
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.CreatedAt);

            modelBuilder.Entity<TransactionInformation>()
                .HasIndex(p => p.TransactionId);
        }
    }
}
