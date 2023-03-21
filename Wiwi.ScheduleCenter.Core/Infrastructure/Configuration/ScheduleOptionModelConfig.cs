//using Wiwi.ScheduleCenter.Core.Domain;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Wiwi.ScheduleCenter.Core.Infrastructure.Configuration
//{
//    public class ScheduleOptionModelConfig : IEntityTypeConfiguration<ScheduleOptionModel>
//    {
//        public void Configure(EntityTypeBuilder<ScheduleOptionModel> builder)
//        {
//            builder.HasKey(x => x.ScheduleId);
//            builder.Property(x => x.ScheduleId).IsRequired().HasMaxLength(36).HasComment("主键");
//            builder.Property(x => x.RequestUrl).IsRequired().HasMaxLength(200);
//            builder.Property(x => x.Body);
//            builder.ToTable("sc_schedule_option");
//        }
//    }
//}
