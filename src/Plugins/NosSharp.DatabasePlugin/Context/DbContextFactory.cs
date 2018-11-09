using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaltyEmu.DatabasePlugin.Configuration;

namespace SaltyEmu.DatabasePlugin.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<SaltyDbContext>
    {
        public SaltyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SaltyDbContext> optionsBuilder = new DbContextOptionsBuilder<SaltyDbContext>();
            optionsBuilder.UseSqlServer(new DatabaseConfiguration().ToString());
            return new SaltyDbContext(optionsBuilder.Options);
        }
    }
}