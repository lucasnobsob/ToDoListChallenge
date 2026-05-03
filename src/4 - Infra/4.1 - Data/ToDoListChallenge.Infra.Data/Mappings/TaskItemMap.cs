using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoListAPI.Domain.Entities;
using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Infra.Data.Mappings
{
    public class TaskItemMap : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnType("varchar(200)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("varchar(1000)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnType("tinyint")
                .IsRequired();

            builder.Property(x => x.DueDate)
                .HasColumnType("date");

            builder.ToTable("TaskItems");
        }
    }
}
