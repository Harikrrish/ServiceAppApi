using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceAppApi.Migrations
{
    public partial class Added_New_Role_And_Changed_AddressId_As_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyRole_Address_AddressId",
                table: "PartyRole");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "PartyRole",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyRole_Address_AddressId",
                table: "PartyRole",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                BEGIN
                    -- Insert Roles --

                    IF NOT EXISTS(SELECT * FROM dbo.Role WHERE RoleName = 'Admin')
                     BEGIN
                        Insert Into dbo.Role(RoleName)
                        Values('Admin')
                     END 

                    IF NOT EXISTS(SELECT * FROM dbo.Role WHERE RoleName = 'Customer')
                     BEGIN
                        Insert Into dbo.Role(RoleName)
                        Values('Customer')
                     END
                END
                GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyRole_Address_AddressId",
                table: "PartyRole");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "PartyRole",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyRole_Address_AddressId",
                table: "PartyRole",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
