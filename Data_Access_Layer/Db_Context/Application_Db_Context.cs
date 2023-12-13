using Data_Access_Layer.Models;
using Data_Access_Layer.Store_Procedures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Db_Context
{
    public class Application_Db_Context : IdentityDbContext
    {
        public Application_Db_Context()
        {
        }

        public Application_Db_Context(DbContextOptions options) : base(options)
        {
         

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=NAROLA-50\\SQLEXPRESS2022;Database=The_GST_31;Trusted_Connection=true ;Encrypt=false;TrustServerCertificate=true;");
                // Use the appropriate database provider (e.g., UseSqlServer, UseMySQL, UseInMemory, etc.)
            }
        }


        public DbSet<Application_User> appUser { get; set; }

        public DbSet<UserOtherDetails> UserDetails { get; set; }
        public DbSet<ExcelSheetData> ExcelSheet { get; set; }
        public DbSet<AllocatedTask> AllocatedTasks { get; set; }
        public DbSet<GSTBillsData> GSTBills { get; set; }
        public DbSet<ReturnFiles> ReturnFiles { get; set; }
        public DbSet<File_Details_Excel> File_Details_Excel { get; set; }
        public DbSet<FilesRecords> FilesRecords { get; set; }
        public DbSet<TaskAllocated> TaskAllocated { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<MessageChat> MessageChat { get; set; }
        public DbSet<LoginLogs> LoginLogs { get; set; }
        public DbSet<DeleteUserLogs> DeleteUserLogs { get; set; }
        public DbSet<UserRegisterLogs> userResistorLogs { get; set; }
        public DbSet<Query> queries { get; set; }



    }
}
