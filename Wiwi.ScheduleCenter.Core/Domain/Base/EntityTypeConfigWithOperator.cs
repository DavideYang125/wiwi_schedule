using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wiwi.ScheduleCenter.Core.Domain.Base
{
    public class EntityTypeConfigWithOperator<T> : IEntityTypeConfiguration<T>
        where T : EntityWithOperatorBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
            builder.Property(x => x.LastModifyTime).IsRequired().HasComment("最后修改时间");
            builder.Property(x => x.Creater).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50).HasComment("创建人");
            builder.Property(x => x.CreaterId).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50).HasComment("创建人Id");
            builder.Property(x => x.LastModifier).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50).HasComment("最后修改人");
            builder.Property(x => x.LastModifierId).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(50).HasComment("最后修改人Id");
            var genericType = typeof(T);
            builder.ToTable(genericType.Name);
        }
    }
}
