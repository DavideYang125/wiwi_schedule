using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Wiwi.ScheduleCenter.Core.Domain;

namespace Wiwi.ScheduleCenter.Core.Infrastructure
{
    public class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext()
        {
        }

        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.Load("Wiwi.ScheduleCenter.Core");
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ScheduleModel> Schedules { get; set; }
    }
}
