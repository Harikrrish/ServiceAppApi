using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceAppApi.Migrations
{
    public partial class Added_Password_Hash_Field_In_PartyRole_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "PartyRole",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "PartyRole");
        }
    }
}
