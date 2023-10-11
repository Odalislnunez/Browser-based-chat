using Browser_based_chat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Browser_based_chat.Areas.Identity.Data.Configurations
{
    public class RoomChatConfiguration: IEntityTypeConfiguration<RoomChat>
    {
        public void Configure (EntityTypeBuilder<RoomChat> builder)
        {
            builder.HasIndex(x => new { x.roomID, x.userID});

            builder.Property(x => x.message).HasMaxLength(500);

            builder.Property(x => x.message).IsRequired();

            builder.HasOne(x => x.user)
                .WithMany(x => x.roomChats)
                .HasForeignKey(x => x.userID)
                .HasPrincipalKey(x => x.Id);

            builder.HasOne(x => x.room)
                .WithMany(x => x.roomChats)
                .HasForeignKey(x => x.roomID)
                .HasPrincipalKey(x => x.ID);
        }
    }
}
