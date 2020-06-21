using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SanRafael.Migrations
{
    public partial class CrearTablas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaProduccion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaProduccion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Costo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(nullable: false),
                    Deshabilitado = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    Monto = table.Column<decimal>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 64, nullable: false),
                    Periodicidad = table.Column<string>(nullable: false),
                    Tipo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    MarcaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marca", x => x.MarcaId);
                });

            migrationBuilder.CreateTable(
                name: "Unidad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Grupo = table.Column<int>(nullable: false),
                    Nombre = table.Column<int>(nullable: false),
                    Simbolo = table.Column<string>(nullable: true),
                    UnidadBase = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Activo = table.Column<bool>(nullable: false),
                    AreaProduccionId = table.Column<int>(nullable: false),
                    Clasificacion = table.Column<string>(nullable: true),
                    CostoOperacion = table.Column<decimal>(nullable: false),
                    CostoOtrosConUtilidad = table.Column<decimal>(nullable: false),
                    CostoUnitario = table.Column<decimal>(nullable: false),
                    IngresoProducto = table.Column<decimal>(nullable: false),
                    MetodoPreparacion = table.Column<string>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    Porciones = table.Column<int>(nullable: false),
                    PrecioDefinidoPorUsuario = table.Column<decimal>(nullable: false),
                    PrecioSugerido = table.Column<decimal>(nullable: false),
                    PrecioVentaConIva = table.Column<decimal>(nullable: false),
                    RecetasVendidas = table.Column<int>(nullable: false),
                    TipoReceta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receta_AreaProduccion_AreaProduccionId",
                        column: x => x.AreaProduccionId,
                        principalTable: "AreaProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insumo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cantidad = table.Column<int>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: false),
                    Deshabilitado = table.Column<bool>(nullable: false),
                    FechaDeRegistro = table.Column<DateTime>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 64, nullable: false),
                    Precio = table.Column<float>(nullable: false),
                    RutaImagen = table.Column<string>(nullable: true),
                    StockMinimo = table.Column<int>(nullable: false),
                    Tienda = table.Column<string>(maxLength: 64, nullable: false),
                    UnidadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insumo_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Insumo_Unidad_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presentacion",
                columns: table => new
                {
                    idPresentacion = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UnidadId = table.Column<int>(nullable: false),
                    cantidadUnidades = table.Column<int>(nullable: false),
                    fechaCaducidad = table.Column<bool>(nullable: false),
                    nombre = table.Column<string>(nullable: false),
                    precioPresentacion = table.Column<double>(nullable: false),
                    precioUnitario = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentacion", x => x.idPresentacion);
                    table.ForeignKey(
                        name: "FK_Presentacion_Unidad_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecetaAReceta",
                columns: table => new
                {
                    IdRecetaHijo = table.Column<int>(nullable: false),
                    IdRecetaPadre = table.Column<int>(nullable: false),
                    Porciones = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecetaAReceta", x => new { x.IdRecetaHijo, x.IdRecetaPadre });
                    table.ForeignKey(
                        name: "FK_RecetaAReceta_Receta_IdRecetaHijo",
                        column: x => x.IdRecetaHijo,
                        principalTable: "Receta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecetaAReceta_Receta_IdRecetaPadre",
                        column: x => x.IdRecetaPadre,
                        principalTable: "Receta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsumoPrecioHistorial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(nullable: false),
                    InsumoId = table.Column<int>(nullable: false),
                    Precio = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumoPrecioHistorial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumoPrecioHistorial_Insumo_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsumosMarcas",
                columns: table => new
                {
                    InsumoId = table.Column<int>(nullable: false),
                    MarcaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosMarcas", x => new { x.InsumoId, x.MarcaId });
                    table.ForeignKey(
                        name: "FK_InsumosMarcas_Insumo_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsumosMarcas_Marca_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marca",
                        principalColumn: "MarcaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsumosRecetas",
                columns: table => new
                {
                    IdInsumo = table.Column<int>(nullable: false),
                    IdReceta = table.Column<int>(nullable: false),
                    IdUnidad = table.Column<int>(nullable: false),
                    PesoNeto = table.Column<double>(nullable: false),
                    UnidadId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosRecetas", x => new { x.IdInsumo, x.IdReceta, x.IdUnidad });
                    table.ForeignKey(
                        name: "FK_InsumosRecetas_Insumo_IdInsumo",
                        column: x => x.IdInsumo,
                        principalTable: "Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsumosRecetas_Receta_IdReceta",
                        column: x => x.IdReceta,
                        principalTable: "Receta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsumosRecetas_Unidad_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsumosPresentaciones",
                columns: table => new
                {
                    InsumoId = table.Column<int>(nullable: false),
                    PresentacionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosPresentaciones", x => new { x.InsumoId, x.PresentacionId });
                    table.ForeignKey(
                        name: "FK_InsumosPresentaciones_Insumo_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Insumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsumosPresentaciones_Presentacion_PresentacionId",
                        column: x => x.PresentacionId,
                        principalTable: "Presentacion",
                        principalColumn: "idPresentacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Insumo_CategoriaId",
                table: "Insumo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Insumo_UnidadId",
                table: "Insumo",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumoPrecioHistorial_InsumoId",
                table: "InsumoPrecioHistorial",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosMarcas_MarcaId",
                table: "InsumosMarcas",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosPresentaciones_InsumoId",
                table: "InsumosPresentaciones",
                column: "InsumoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsumosPresentaciones_PresentacionId",
                table: "InsumosPresentaciones",
                column: "PresentacionId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosRecetas_IdReceta",
                table: "InsumosRecetas",
                column: "IdReceta");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosRecetas_UnidadId",
                table: "InsumosRecetas",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Presentacion_UnidadId",
                table: "Presentacion",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Receta_AreaProduccionId",
                table: "Receta",
                column: "AreaProduccionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecetaAReceta_IdRecetaPadre",
                table: "RecetaAReceta",
                column: "IdRecetaPadre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Costo");

            migrationBuilder.DropTable(
                name: "InsumoPrecioHistorial");

            migrationBuilder.DropTable(
                name: "InsumosMarcas");

            migrationBuilder.DropTable(
                name: "InsumosPresentaciones");

            migrationBuilder.DropTable(
                name: "InsumosRecetas");

            migrationBuilder.DropTable(
                name: "RecetaAReceta");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropTable(
                name: "Presentacion");

            migrationBuilder.DropTable(
                name: "Insumo");

            migrationBuilder.DropTable(
                name: "Receta");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Unidad");

            migrationBuilder.DropTable(
                name: "AreaProduccion");
        }
    }
}
