using Microsoft.EntityFrameworkCore.Migrations;

namespace Controller.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Protocol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelID);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceName = table.Column<string>(nullable: true),
                    SetChannel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
