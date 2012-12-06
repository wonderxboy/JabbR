using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JabbR.Infrastructure;

namespace JabbR.Models
{
    public class ChatUser
    {
        [Key]
        public int Key { get; set; }

        [StringLength(36)]
        public string Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        // MD5 email hash for gravatar
        [StringLength(32)]
        public string Hash { get; set; }
        [StringLength(256)]
        public string Salt { get; set; }
        [StringLength(256)]
        public string HashedPassword { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime? LastNudged { get; set; }
        public int Status { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        [StringLength(200)]
        public string AfkNote { get; set; }

        public bool IsAfk { get; set; }

        [StringLength(2)]
        public string Flag { get; set; }

        [StringLength(1000)]
        public string Identity { get; set; }
        [StringLength(9)]
        public string EmployeeId { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }

        // List of clients that are currently connected for this user
        public virtual ICollection<ChatClient> ConnectedClients { get; set; }
        public virtual ICollection<ChatRoom> OwnedRooms { get; set; }
        public virtual ICollection<ChatRoom> Rooms { get; set; }

        // Private rooms this user is allowed to go into
        public virtual ICollection<ChatRoom> AllowedRooms { get; set; }

        public ChatUser()
        {
            ConnectedClients = new SafeCollection<ChatClient>();
            OwnedRooms = new SafeCollection<ChatRoom>();
            Rooms = new SafeCollection<ChatRoom>();
            AllowedRooms = new SafeCollection<ChatRoom>();
        }
    }
}