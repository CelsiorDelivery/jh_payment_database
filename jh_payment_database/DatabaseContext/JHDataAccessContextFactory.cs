using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace jh_payment_database.DatabaseContext
{
    public class JHDataAccessContextFactory : IDesignTimeDbContextFactory<JHDataAccessContext>
    {
        public JHDataAccessContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<JHDataAccessContext>();
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "jh_poc_payments.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
            options.UseSqlite($"Data Source={dbPath}");
            return new JHDataAccessContext(options.Options);
        }
    }
}
