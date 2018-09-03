using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaltyEmu.DatabasePlugin.Configuration;

namespace SaltyEmu.DatabasePlugin.Context
{
    public class NosSharpContextFactory : IDesignTimeDbContextFactory<NosSharpContext>
    {
        public NosSharpContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NosSharpContext> optionsBuilder = new DbContextOptionsBuilder<NosSharpContext>();
            optionsBuilder.UseSqlServer(new DatabaseConfiguration().ToString());
            return new NosSharpContext(optionsBuilder.Options);
        }
    }
}