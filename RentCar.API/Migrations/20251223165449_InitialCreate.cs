using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCar.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsCar",
                columns: table => new
                {
                    Car_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    License_plate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number_of_car_seats = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price_per_day = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCar", x => x.Car_id);
                });

            migrationBuilder.CreateTable(
                name: "MsCustomer",
                columns: table => new
                {
                    Customer_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Driver_license_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCustomer", x => x.Customer_id);
                });

            migrationBuilder.CreateTable(
                name: "MsEmployee",
                columns: table => new
                {
                    Employee_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Email = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Phone_number = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEmployee", x => x.Employee_id);
                });

            migrationBuilder.CreateTable(
                name: "MsCarImages",
                columns: table => new
                {
                    Image_car_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Car_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Image_link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCarImages", x => x.Image_car_id);
                    table.ForeignKey(
                        name: "FK_MsCarImages_MsCar_Car_id",
                        column: x => x.Car_id,
                        principalTable: "MsCar",
                        principalColumn: "Car_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrRental",
                columns: table => new
                {
                    Rental_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Customer_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Car_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Rental_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Return_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Payment_status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrRental", x => x.Rental_id);
                    table.ForeignKey(
                        name: "FK_TrRental_MsCar_Car_id",
                        column: x => x.Car_id,
                        principalTable: "MsCar",
                        principalColumn: "Car_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrRental_MsCustomer_Customer_id",
                        column: x => x.Customer_id,
                        principalTable: "MsCustomer",
                        principalColumn: "Customer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrMaintenance",
                columns: table => new
                {
                    Maintenance_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Car_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Employee_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Maintenance_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrMaintenance", x => x.Maintenance_id);
                    table.ForeignKey(
                        name: "FK_TrMaintenance_MsCar_Car_id",
                        column: x => x.Car_id,
                        principalTable: "MsCar",
                        principalColumn: "Car_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrMaintenance_MsEmployee_Employee_id",
                        column: x => x.Employee_id,
                        principalTable: "MsEmployee",
                        principalColumn: "Employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LtPayment",
                columns: table => new
                {
                    Payment_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Rental_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Payment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Payment_method = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtPayment", x => x.Payment_id);
                    table.ForeignKey(
                        name: "FK_LtPayment_TrRental_Rental_id",
                        column: x => x.Rental_id,
                        principalTable: "TrRental",
                        principalColumn: "Rental_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LtPayment_Rental_id",
                table: "LtPayment",
                column: "Rental_id");

            migrationBuilder.CreateIndex(
                name: "IX_MsCar_Status",
                table: "MsCar",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MsCarImages_Car_id",
                table: "MsCarImages",
                column: "Car_id");

            migrationBuilder.CreateIndex(
                name: "IX_MsCustomer_Email",
                table: "MsCustomer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrMaintenance_Car_id",
                table: "TrMaintenance",
                column: "Car_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrMaintenance_Employee_id",
                table: "TrMaintenance",
                column: "Employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrRental_Car_id",
                table: "TrRental",
                column: "Car_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrRental_Customer_id",
                table: "TrRental",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrRental_Payment_status",
                table: "TrRental",
                column: "Payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_TrRental_Rental_date",
                table: "TrRental",
                column: "Rental_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LtPayment");

            migrationBuilder.DropTable(
                name: "MsCarImages");

            migrationBuilder.DropTable(
                name: "TrMaintenance");

            migrationBuilder.DropTable(
                name: "TrRental");

            migrationBuilder.DropTable(
                name: "MsEmployee");

            migrationBuilder.DropTable(
                name: "MsCar");

            migrationBuilder.DropTable(
                name: "MsCustomer");
        }
    }
}
