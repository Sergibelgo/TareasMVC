using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutorial2TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                            IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id='a2ece3ab-7654-419d-98d3-5bbdf43a8aa3')
                            BEGIN
                            INSERT INTO AspNetRoles(Id,[Name],[NormalizedName])
                            VALUES ('a2ece3ab-7654-419d-98d3-5bbdf43a8aa3','admin','ADMIN')
                            END
                            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                DELETE AspNetUserRoles WHERE Id='a2ece3ab-7654-419d-98d3-5bbdf43a8aa3'
                                    ");
        }
    }
}
