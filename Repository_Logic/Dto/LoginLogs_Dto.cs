using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
   public class LoginLogs_Dto
    {
        public string Id { get; set; }
        public string? UserID { get; set; }

        public string? Message { get; set; }

        public string? CurrentStatus { get; set; }

        public string Date { get; set; }

    }
}
