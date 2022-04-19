using Postgrest.Attributes;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avaloniachat.Models
{
    public class Messages : SupabaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("user_id")]
        public int UserId { get; set; } = 0;

        [Column("text")]
        public string Text { get; set; } = string.Empty;
    }

}
