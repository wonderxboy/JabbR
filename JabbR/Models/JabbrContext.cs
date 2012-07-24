using System.Data.Entity;
using JabbR.Models.Mapping;

namespace JabbR.Models
{
    public class JabbrContext : DbContext
    {
        static JabbrContext()
        {
            Database.SetInitializer<JabbrContext>(new CreateDatabaseIfNotExists<JabbrContext>());
        }

        public JabbrContext()
            : base("Jabbr")
        {
        }

        public DbSet<ChatClient> Clients { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<ChatRoom> Rooms { get; set; }
        public DbSet<ChatUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChatClientMap());

            modelBuilder.Configurations.Add(new ChatMessageMap());

            modelBuilder.Configurations.Add(new ChatRoomMap());

            modelBuilder.Configurations.Add(new ChatUserMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}