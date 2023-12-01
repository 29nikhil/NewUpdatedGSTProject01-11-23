using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class Query_Dto
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string? UserName { get; set; }
        public string? Question { get; set; }
        public string Answer { get; set; }
        public string? AnsweredBy { get; set; }
        public DateTime QuestionAskedDate { get; set; }
        public string QuestionDate { get; set; }
        public DateTime? AnsweredQuestionDate { get; set; }
        public string? AnswerDate { get; set; }
        public string? Email { get; set; }
    }
}
