using Browser_based_chat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Browser_based_chat.Areas.Identity.Data.Configurations
{
    public class RoomConfiguration: IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasIndex(x => x.ID);

            builder.Property(x => x.Description).HasMaxLength(255);
        }
    }
}
