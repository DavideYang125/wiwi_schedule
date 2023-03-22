using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wiwi.ScheduleCenter.Core.Domain;
using Wiwi.ScheduleCenter.Core.Domain.Base;

namespace Wiwi.ScheduleCenter.Core.Infrastructure.Configuration
{
    public class ScheduleModelConfig : EntityTypeConfigWithOperator<ScheduleModel>
    {
        public override void Configure(EntityTypeBuilder<ScheduleModel> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.ScheduleId);
            builder.Property(x => x.ScheduleId).IsRequired().HasMaxLength(36).HasComment("主键");
            builder.Property(x => x.ShcheduleName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Remark);
            builder.Property(x => x.CronExpression).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(5);
            builder.Property(x => x.LastRunTime);
            builder.Property(x => x.NextRunTime);
            builder.Property(x => x.ShcheduleName).IsRequired();
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.Deleted).IsRequired();
            builder.Property(x => x.RequestUrl).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Body);
        }
    }
}
