IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AreaProduccion] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NULL,
    CONSTRAINT [PK_AreaProduccion] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Categoria] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_Categoria] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Unidad] (
    [Id] int NOT NULL IDENTITY,
    [Grupo] int NOT NULL,
    [Nombre] int NOT NULL,
    [Simbolo] nvarchar(max) NULL,
    [UnidadBase] int NOT NULL,
    CONSTRAINT [PK_Unidad] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Receta] (
    [Id] int NOT NULL IDENTITY,
    [Activo] bit NOT NULL,
    [AreaProduccionId] int NOT NULL,
    [Clasificacion] nvarchar(max) NULL,
    [CostoOtrosConUtilidad] decimal(18, 2) NOT NULL,
    [CostoUnitario] decimal(18, 2) NOT NULL,
    [MetodoPreparacion] nvarchar(max) NULL,
    [Nombre] nvarchar(max) NULL,
    [Porciones] int NOT NULL,
    [PrecioDefinidoPorUsuario] decimal(18, 2) NOT NULL,
    [PrecioSugerido] decimal(18, 2) NOT NULL,
    [PrecioVentaConIva] decimal(18, 2) NOT NULL,
    [TipoReceta] nvarchar(max) NULL,
    CONSTRAINT [PK_Receta] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Receta_AreaProduccion_AreaProduccionId] FOREIGN KEY ([AreaProduccionId]) REFERENCES [AreaProduccion] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Insumo] (
    [Id] int NOT NULL IDENTITY,
    [CategoriaId] int NOT NULL,
    [Deshabilitado] bit NOT NULL,
    [FechaDeRegistro] datetime2 NOT NULL,
    [Marca] nvarchar(64) NOT NULL,
    [Merma] real NOT NULL,
    [Nombre] nvarchar(64) NOT NULL,
    [Precio] real NOT NULL,
    [PrecioAjustado] real NOT NULL,
    [Rendimiento] real NOT NULL,
    [Tienda] nvarchar(64) NOT NULL,
    [UnidadId] int NOT NULL,
    CONSTRAINT [PK_Insumo] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Insumo_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Insumo_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [RecetaAReceta] (
    [IdRecetaHijo] int NOT NULL,
    [IdRecetaPadre] int NOT NULL,
    [Porciones] int NOT NULL,
    CONSTRAINT [PK_RecetaAReceta] PRIMARY KEY ([IdRecetaHijo], [IdRecetaPadre]),
    CONSTRAINT [FK_RecetaAReceta_Receta_IdRecetaHijo] FOREIGN KEY ([IdRecetaHijo]) REFERENCES [Receta] ([Id]),
    CONSTRAINT [FK_RecetaAReceta_Receta_IdRecetaPadre] FOREIGN KEY ([IdRecetaPadre]) REFERENCES [Receta] ([Id])
);

GO

CREATE TABLE [InsumosRecetas] (
    [IdInsumo] int NOT NULL,
    [IdReceta] int NOT NULL,
    [IdUnidad] int NOT NULL,
    [PesoNeto] float NOT NULL,
    [UnidadId] int NULL,
    CONSTRAINT [PK_InsumosRecetas] PRIMARY KEY ([IdInsumo], [IdReceta], [IdUnidad]),
    CONSTRAINT [FK_InsumosRecetas_Insumo_IdReceta] FOREIGN KEY ([IdReceta]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InsumosRecetas_Receta_IdReceta] FOREIGN KEY ([IdReceta]) REFERENCES [Receta] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InsumosRecetas_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE INDEX [IX_Insumo_CategoriaId] ON [Insumo] ([CategoriaId]);

GO

CREATE INDEX [IX_Insumo_UnidadId] ON [Insumo] ([UnidadId]);

GO

CREATE INDEX [IX_InsumosRecetas_IdReceta] ON [InsumosRecetas] ([IdReceta]);

GO

CREATE INDEX [IX_InsumosRecetas_UnidadId] ON [InsumosRecetas] ([UnidadId]);

GO

CREATE INDEX [IX_Receta_AreaProduccionId] ON [Receta] ([AreaProduccionId]);

GO

CREATE INDEX [IX_RecetaAReceta_IdRecetaPadre] ON [RecetaAReceta] ([IdRecetaPadre]);

GO

CREATE TABLE [Unidades] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NULL,
    CONSTRAINT [PK_Unidades] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [UnidadesInsumos] (
    [IdUnidad] int NOT NULL,
    [IdInsumo] int NOT NULL,
    [InsumoId] int NULL,
    [UnidadId] int NULL,
    CONSTRAINT [PK_UnidadesInsumos] PRIMARY KEY ([IdUnidad], [IdInsumo]),
    CONSTRAINT [FK_UnidadesInsumos_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UnidadesInsumos_Unidades_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidades] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_InsumosRecetas_InsumoId] ON [InsumosRecetas] ([IdInsumo]);

GO

CREATE INDEX [IX_InsumosRecetas_RecetaId] ON [InsumosRecetas] ([IdReceta]);

GO

CREATE INDEX [IX_UnidadesInsumos_InsumoId] ON [UnidadesInsumos] ([InsumoId]);

GO

CREATE INDEX [IX_UnidadesInsumos_UnidadId] ON [UnidadesInsumos] ([UnidadId]);

GO

ALTER TABLE [UnidadesInsumos] DROP CONSTRAINT [FK_UnidadesInsumos_Insumo_InsumoId];

GO

ALTER TABLE [UnidadesInsumos] DROP CONSTRAINT [FK_UnidadesInsumos_Unidades_UnidadId];

GO

DROP INDEX [IX_UnidadesInsumos_InsumoId] ON [UnidadesInsumos];

GO

DROP INDEX [IX_UnidadesInsumos_UnidadId] ON [UnidadesInsumos];

GO

DROP INDEX [IX_InsumosRecetas_InsumoId] ON [InsumosRecetas];

GO

DROP INDEX [IX_InsumosRecetas_RecetaId] ON [InsumosRecetas];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'UnidadesInsumos') AND [c].[name] = N'InsumoId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UnidadesInsumos] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [UnidadesInsumos] DROP COLUMN [InsumoId];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'UnidadesInsumos') AND [c].[name] = N'UnidadId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UnidadesInsumos] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [UnidadesInsumos] DROP COLUMN [UnidadId];

GO

CREATE INDEX [IX_UnidadesInsumos_IdInsumo] ON [UnidadesInsumos] ([IdInsumo]);

GO

ALTER TABLE [UnidadesInsumos] ADD CONSTRAINT [FK_UnidadesInsumos_Unidades_IdInsumo] FOREIGN KEY ([IdInsumo]) REFERENCES [Unidades] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [UnidadesInsumos] ADD CONSTRAINT [FK_UnidadesInsumos_Insumo_IdUnidad] FOREIGN KEY ([IdUnidad]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE;

GO

EXEC sp_rename N'Receta.activo', N'Activo', N'COLUMN';

GO

EXEC sp_rename N'InsumosRecetas.pesoNeto', N'PesoNeto', N'COLUMN';

GO

ALTER TABLE [InsumosRecetas] ADD CONSTRAINT [FK_InsumosRecetas_Unidades_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidades] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Insumo] ADD [UnidadBaseId] int NOT NULL DEFAULT 1;

GO

CREATE INDEX [IX_Insumo_UnidadBaseId] ON [Insumo] ([UnidadBaseId]);

GO

ALTER TABLE [Insumo] ADD CONSTRAINT [FK_Insumo_Unidades_UnidadBaseId] FOREIGN KEY ([UnidadBaseId]) REFERENCES [Unidades] ([Id]);

GO

ALTER TABLE [Receta] DROP CONSTRAINT [FK_Receta_AreaProduccion_AreaProduccionId];

GO

DROP INDEX [IX_Receta_AreaProduccionId] ON [Receta];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Receta') AND [c].[name] = N'AreaProduccionId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Receta] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Receta] ALTER COLUMN [AreaProduccionId] int NOT NULL;
CREATE INDEX [IX_Receta_AreaProduccionId] ON [Receta] ([AreaProduccionId]);

GO

ALTER TABLE [Receta] ADD CONSTRAINT [FK_Receta_AreaProduccion_AreaProduccionId] FOREIGN KEY ([AreaProduccionId]) REFERENCES [AreaProduccion] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Insumo] DROP CONSTRAINT [FK_Insumo_Unidades_UnidadBaseId];

GO

EXEC sp_rename N'Insumo.UnidadBaseId', N'IdUnidadBase', N'COLUMN';

GO

EXEC sp_rename N'Insumo.IX_Insumo_UnidadBaseId', N'IX_Insumo_IdUnidadBase', N'INDEX';

GO

ALTER TABLE [Insumo] ADD CONSTRAINT [FK_Insumo_Unidades_IdUnidadBase] FOREIGN KEY ([IdUnidadBase]) REFERENCES [Unidades] ([Id]);

GO

CREATE TABLE [InsumoPrecioHistorial] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [InsumoId] int NOT NULL,
    [Merma] real NOT NULL,
    [Precio] real NOT NULL,
    [PrecioAjustado] real NOT NULL,
    CONSTRAINT [PK_InsumoPrecioHistorial] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InsumoPrecioHistorial_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_InsumoPrecioHistorial_InsumoId] ON [InsumoPrecioHistorial] ([InsumoId]);

GO

CREATE TABLE [Presentacion] (
    [idPresentacion] int NOT NULL IDENTITY,
    [UnidadId] int NULL,
    [cantidadUnidades] int NOT NULL,
    [fechaCaducidad] bit NOT NULL,
    [nombre] nvarchar(max) NOT NULL,
    [precioPresentacion] float NOT NULL,
    [precioUnitario] float NOT NULL,
    CONSTRAINT [PK_Presentacion] PRIMARY KEY ([idPresentacion]),
    CONSTRAINT [FK_Presentacion_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Presentacion_UnidadId] ON [Presentacion] ([UnidadId]);

GO

ALTER TABLE [Presentacion] DROP CONSTRAINT [FK_Presentacion_Unidad_UnidadId];

GO

DROP INDEX [IX_Presentacion_UnidadId] ON [Presentacion];
DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Presentacion') AND [c].[name] = N'UnidadId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Presentacion] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Presentacion] ALTER COLUMN [UnidadId] int NOT NULL;
CREATE INDEX [IX_Presentacion_UnidadId] ON [Presentacion] ([UnidadId]);

GO

ALTER TABLE [Presentacion] ADD CONSTRAINT [FK_Presentacion_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE CASCADE;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Insumo') AND [c].[name] = N'Marca');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Insumo] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Insumo] DROP COLUMN [Marca];

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Insumo') AND [c].[name] = N'PrecioAjustado');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Insumo] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Insumo] DROP COLUMN [PrecioAjustado];

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Insumo') AND [c].[name] = N'Rendimiento');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Insumo] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Insumo] DROP COLUMN [Rendimiento];

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Insumo') AND [c].[name] = N'Merma');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Insumo] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Insumo] DROP COLUMN [Merma];

GO

CREATE TABLE [Marca] (
    [MarcaId] int NOT NULL IDENTITY,
    [Nombre] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_Marca] PRIMARY KEY ([MarcaId])
);

GO

CREATE TABLE [InsumosMarcas] (
    [InsumoId] int NOT NULL,
    [MarcaId] int NOT NULL,
    CONSTRAINT [PK_InsumosMarcas] PRIMARY KEY ([InsumoId], [MarcaId]),
    CONSTRAINT [FK_InsumosMarcas_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InsumosMarcas_Marca_MarcaId] FOREIGN KEY ([MarcaId]) REFERENCES [Marca] ([MarcaId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_InsumosMarcas_MarcaId] ON [InsumosMarcas] ([MarcaId]);

GO

CREATE TABLE [InsumosPresentaciones] (
    [InsumoId] int NOT NULL,
    [PresentacionId] int NOT NULL,
    CONSTRAINT [PK_InsumosPresentaciones] PRIMARY KEY ([InsumoId], [PresentacionId]),
    CONSTRAINT [FK_InsumosPresentaciones_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InsumosPresentaciones_Presentacion_PresentacionId] FOREIGN KEY ([PresentacionId]) REFERENCES [Presentacion] ([idPresentacion])
);

GO

CREATE INDEX [IX_InsumosPresentaciones_PresentacionId] ON [InsumosPresentaciones] ([PresentacionId]);

GO

ALTER TABLE [Insumo] ADD [Cantidad] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Insumo] ADD [RutaImagen] nvarchar(max) NULL;

GO

ALTER TABLE [Insumo] ADD [StockMinimo] int NOT NULL DEFAULT 0;

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'InsumoPrecioHistorial') AND [c].[name] = N'Merma');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [InsumoPrecioHistorial] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [InsumoPrecioHistorial] DROP COLUMN [Merma];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'InsumoPrecioHistorial') AND [c].[name] = N'PrecioAjustado');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [InsumoPrecioHistorial] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [InsumoPrecioHistorial] DROP COLUMN [PrecioAjustado];

GO

ALTER TABLE [InsumoPrecioHistorial] ADD [BajasMerma] int NOT NULL DEFAULT 0;

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'InsumoPrecioHistorial') AND [c].[name] = N'BajasMerma');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [InsumoPrecioHistorial] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [InsumoPrecioHistorial] DROP COLUMN [BajasMerma];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190322191714_QuitarMermaPrecioAjustado', N'2.0.3-rtm-10026');

GO

CREATE TABLE [Costo] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    [Monto] decimal(18, 2) NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Periodicidad] nvarchar(max) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Costo] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190813194757_AgregacionCostos', N'2.0.3-rtm-10026');

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Costo') AND [c].[name] = N'Nombre');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Costo] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Costo] ALTER COLUMN [Nombre] nvarchar(64) NOT NULL;

GO

ALTER TABLE [Costo] ADD [Deshabilitado] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190823155856_ColumnaDeshabilitadoCostoFijo', N'2.0.3-rtm-10026');

GO

ALTER TABLE [Receta] ADD [RecetasVendidas] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190908230204_AgregarRecetasVendidas', N'2.0.3-rtm-10026');

GO

ALTER TABLE [Receta] ADD [CostoOperacion] decimal(18, 2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190909014152_AgregarCostoOperacion', N'2.0.3-rtm-10026');

GO

ALTER TABLE [Receta] ADD [IngresoProducto] decimal(18, 2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190909021425_AgregarIngresoProducto', N'2.0.3-rtm-10026');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191127220853_llavesForaneas', N'2.0.3-rtm-10026');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200213021452_KFRecetaAReceta2', N'2.0.3-rtm-10026');

GO

