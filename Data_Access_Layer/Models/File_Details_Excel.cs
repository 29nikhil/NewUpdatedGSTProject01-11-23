using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class File_Details_Excel
    {

        [Key]

        public string FileId { get; set; }
        public string? UserId { get; set; }
        public string? UplodedById { get; set; }
        public string? CA_ID { get; set; }
        public string? GSTTye { get; set; }

        public string? Status { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;

        public DateTime? Modified_date { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsTestdata { get; set; }

       [NotMapped]
public string Year
{
    get
    {
        if (Date.HasValue)
        {
            // Format the Date property as "yyyy"
            return Date.Value.ToString("yyyy");
        }
        return string.Empty; // Or any other default value you prefer
    }
}

[NotMapped]
public string Month
{
    get
    {
        if (Date.HasValue)
        {
            // Format the Date property as "MMMM" for the full month name
            return Date.Value.ToString("MMMM");
        }
        return string.Empty; // Or any other default value you prefer
    }
}
        [NotMapped]
        public string TaxPeriod
        {
            get
            {
                if (Date.HasValue)
                {
                    var currentMonth = Date.Value.ToString("MMMM");

                    // Calculate the next month
                    var nextMonth = Date.Value.AddMonths(1).ToString("MMMM");

                    // Concatenate the current month and the next month
                    var result = $"{currentMonth} - {nextMonth}";

                    return result;
                }

                return string.Empty; // Or any other default value you prefer
            }
        }

    }
}
