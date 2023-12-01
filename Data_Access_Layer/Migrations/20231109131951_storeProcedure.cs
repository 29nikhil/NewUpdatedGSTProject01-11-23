using Data_Access_Layer.Store_Procedures;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class storeProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                Store_ProceduresClass storeProcedure = new Store_ProceduresClass();

                migrationBuilder.Sql(storeProcedure.EditUserProcedure);
                migrationBuilder.Sql(storeProcedure.EditFellowshipProcedure);
                migrationBuilder.Sql(storeProcedure.DeleteUserProcedure);
                migrationBuilder.Sql(storeProcedure.EmailStatus);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You might want to throw the exception again to mark the migration as failed
                throw;
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add rollback logic if needed
        }

    }
}
