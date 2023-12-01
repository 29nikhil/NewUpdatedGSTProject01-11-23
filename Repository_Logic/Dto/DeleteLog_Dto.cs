using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class DeleteLog_Dto
    {
        public string? Id { get; set; }
        public string? UserID { get; set; }
        public string? DeletedById { get; set; }
        public string? UserName { get; set; }
        public string? DeletedByName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string?  Date { get; set; }
    }
}
