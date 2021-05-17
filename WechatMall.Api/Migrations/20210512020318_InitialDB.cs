using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WechatMall.Api.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    OrderbyId = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(maxLength: 255, nullable: false),
                    IsShown = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.UniqueConstraint("AK_Categories_CategoryID", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 64, nullable: false),
                    Value = table.Column<string>(maxLength: 4096, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CouponName = table.Column<string>(maxLength: 40, nullable: false),
                    ProductIDs = table.Column<string>(maxLength: 255, nullable: false),
                    CouponType = table.Column<int>(nullable: false),
                    Condition = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    AllowLimit = table.Column<int>(nullable: false),
                    CouponCount = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingFares",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false),
                    Rules = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingFares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<Guid>(nullable: false),
                    OpenID = table.Column<string>(maxLength: 64, nullable: false),
                    UnionID = table.Column<string>(maxLength: 64, nullable: true),
                    SessionKey = table.Column<string>(maxLength: 64, nullable: true),
                    NickName = table.Column<string>(maxLength: 64, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Language = table.Column<string>(maxLength: 10, nullable: false),
                    City = table.Column<string>(maxLength: 64, nullable: false),
                    Province = table.Column<string>(maxLength: 64, nullable: false),
                    Country = table.Column<string>(maxLength: 64, nullable: false),
                    AvatarUrl = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_UserID", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<string>(maxLength: 10, nullable: false),
                    CategoryID = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    StockCount = table.Column<int>(nullable: false),
                    SoldCount = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    ShippingAddress = table.Column<string>(maxLength: 10, nullable: true),
                    ShippingFareID = table.Column<int>(nullable: false),
                    Detail = table.Column<string>(nullable: true),
                    Recommend = table.Column<int>(nullable: false, defaultValue: 0),
                    OrderbyId = table.Column<int>(nullable: false),
                    OnSale = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_ProductID", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ShippingFares_ShippingFareID",
                        column: x => x.ShippingFareID,
                        principalTable: "ShippingFares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coupon_Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    CouponID = table.Column<int>(nullable: false),
                    RecievedCount = table.Column<int>(nullable: false),
                    RemainedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon_Users", x => new { x.CouponID, x.UserID });
                    table.ForeignKey(
                        name: "FK_Coupon_Users_Coupons_CouponID",
                        column: x => x.CouponID,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coupon_Users_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShippingAddrs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<Guid>(nullable: false),
                    Province = table.Column<string>(maxLength: 20, nullable: false),
                    ProvinceID = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 20, nullable: false),
                    CityID = table.Column<int>(nullable: false),
                    County = table.Column<string>(maxLength: 20, nullable: false),
                    CountyID = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: false),
                    ReceiverName = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: false),
                    PostCode = table.Column<string>(maxLength: 6, nullable: false),
                    OrderById = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAddrs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(nullable: false),
                    ProductID = table.Column<string>(maxLength: 10, nullable: true),
                    ImagePath = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    PhysicalPath = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    OrderbyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrderID = table.Column<string>(maxLength: 20, nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    DeliverTime = table.Column<DateTime>(nullable: true),
                    ShippingAddrId = table.Column<int>(nullable: false),
                    TrackingNumber = table.Column<string>(maxLength: 20, nullable: true),
                    CouponAmount = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    PaidPrice = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true),
                    ShippingFare = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    PayTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.UniqueConstraint("AK_Orders_OrderID", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_ShippingAddrs_Id",
                        column: x => x.Id,
                        principalTable: "ShippingAddrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<string>(maxLength: 15, nullable: false),
                    ProductID = table.Column<string>(maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryID",
                table: "Categories",
                column: "CategoryID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configs_Key",
                table: "Configs",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_Users_UserID",
                table: "Coupon_Users",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductID",
                table: "OrderItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderID",
                table: "Orders",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_Guid",
                table: "ProductImages",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductID",
                table: "Products",
                column: "ProductID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShippingFareID",
                table: "Products",
                column: "ShippingFareID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddrs_UserID",
                table: "ShippingAddrs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OpenID",
                table: "Users",
                column: "OpenID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserID",
                table: "Users",
                column: "UserID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Coupon_Users");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShippingAddrs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ShippingFares");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
