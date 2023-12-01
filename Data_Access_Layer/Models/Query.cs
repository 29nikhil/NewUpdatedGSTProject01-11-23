using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Query
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserID { get; set; }
        public string? Question { get; set; }
        public string Answer { get; set; } = "Not Yet Answered";
        public string? AnsweredBy { get; set; } = "Not Answered By CA";
        public DateTime QuestionAskedDate { get; set; }
        public DateTime? AnsweredQuestionDate { get; set; } = DateTime.MinValue;
        public DateTime Modified_date { get; set; }
        public bool IsTestdata { get; set; }

    }
}
