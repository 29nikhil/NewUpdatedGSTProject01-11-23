using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class MessageChat
    {

        // Models/Message.cs
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string Id { get; set; }
            public string? SenderId { get; set; }
            public string? ReceiverId { get; set; }
            public string? Text { get; set; }
            public MessageStatus? Status { get; set; }
            public DateTime SentAt { get; set; }
            public DateTime? ReadAt { get; set; }
            public DateTime? CreatedDate { get; set; } = DateTime.Now;
            public DateTime? Modified_date { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsTestdata { get; set; }
    }

        public enum MessageStatus
        {
            Sent,
            Delivered,
            Read
        }

    
}
