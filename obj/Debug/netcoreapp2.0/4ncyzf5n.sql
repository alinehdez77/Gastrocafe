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

CREATE TABLE [Costo] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    [Deshabilitado] bit NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    [Monto] decimal(18, 2) NOT NULL,
    [Nombre] nvarchar(64) NOT NULL,
    [Periodicidad] nvarchar(max) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Costo] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Marca] (
    [MarcaId] int NOT NULL IDENTITY,
    [Nombre] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_Marca] PRIMARY KEY ([MarcaId])
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
    [CostoOperacion] decimal(18, 2) NOT NULL,
    [CostoOtrosConUtilidad] decimal(18, 2) NOT NULL,
    [CostoUnitario] decimal(18, 2) NOT NULL,
    [IngresoProducto] decimal(18, 2) NOT NULL,
    [MetodoPreparacion] nvarchar(max) NULL,
    [Nombre] nvarchar(max) NULL,
    [Porciones] int NOT NULL,
    [PrecioDefinidoPorUsuario] decimal(18, 2) NOT NULL,
    [PrecioSugerido] decimal(18, 2) NOT NULL,
    [PrecioVentaConIva] decimal(18, 2) NOT NULL,
    [RecetasVendidas] int NOT NULL,
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
    [Cantidad] int NOT NULL,
    [CategoriaId] int NOT NULL,
    [Deshabilitado] bit NOT NULL,
    [FechaDeRegistro] datetime2 NOT NULL,
    [Nombre] nvarchar(64) NOT NULL,
    [Precio] real NOT NULL,
    [RutaImagen] nvarchar(max) NULL,
    [StockMinimo] int NOT NULL,
    [Tienda] nvarchar(64) NOT NULL,
    [UnidadId] int NOT NULL,
    CONSTRAINT [PK_Insumo] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Insumo_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Insumo_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Presentacion] (
    [idPresentacion] int NOT NULL IDENTITY,
    [UnidadId] int NOT NULL,
    [cantidadUnidades] int NOT NULL,
    [fechaCaducidad] bit NOT NULL,
    [nombre] nvarchar(max) NOT NULL,
    [precioPresentacion] float NOT NULL,
    [precioUnitario] float NOT NULL,
    CONSTRAINT [PK_Presentacion] PRIMARY KEY ([idPresentacion]),
    CONSTRAINT [FK_Presentacion_Unidad_UnidadId] FOREIGN KEY ([UnidadId]) REFERENCES [Unidad] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [RecetaAReceta] (
    [IdRecetaHijo] int NOT NULL,
    [IdRecetaPadre] int NOT NULL,
    [Porciones] int NOT NULL,
    CONSTRAINT [PK_RecetaAReceta] PRIMARY KEY ([IdRecetaHijo], [IdRecetaPadre]),
    CONSTRAINT [FK_RecetaAReceta_Receta_IdRecetaHijo] FOREIGN KEY ([IdRecetaHijo]) REFERENCES [Receta] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RecetaAReceta_Receta_IdRecetaPadre] FOREIGN KEY ([IdRecetaPadre]) REFERENCES [Receta] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InsumoPrecioHistorial] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [InsumoId] int NOT NULL,
    [Precio] real NOT NULL,
    CONSTRAINT [PK_InsumoPrecioHistorial] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InsumoPrecioHistorial_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE CASCADE
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

CREATE TABLE [InsumosPresentaciones] (
    [InsumoId] int NOT NULL,
    [PresentacionId] int NOT NULL,
    CONSTRAINT [PK_InsumosPresentaciones] PRIMARY KEY ([InsumoId], [PresentacionId]),
    CONSTRAINT [FK_InsumosPresentaciones_Insumo_InsumoId] FOREIGN KEY ([InsumoId]) REFERENCES [Insumo] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InsumosPresentaciones_Presentacion_PresentacionId] FOREIGN KEY ([PresentacionId]) REFERENCES [Presentacion] ([idPresentacion]) ON DELETE CASCADE
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

CREATE INDEX [IX_InsumoPrecioHistorial_InsumoId] ON [InsumoPrecioHistorial] ([InsumoId]);

GO

CREATE INDEX [IX_InsumosMarcas_MarcaId] ON [InsumosMarcas] ([MarcaId]);

GO

CREATE UNIQUE INDEX [IX_InsumosPresentaciones_InsumoId] ON [InsumosPresentaciones] ([InsumoId]);

GO

CREATE INDEX [IX_InsumosPresentaciones_PresentacionId] ON [InsumosPresentaciones] ([PresentacionId]);

GO

CREATE INDEX [IX_InsumosRecetas_IdReceta] ON [InsumosRecetas] ([IdReceta]);

GO

CREATE INDEX [IX_InsumosRecetas_UnidadId] ON [InsumosRecetas] ([UnidadId]);

GO

CREATE INDEX [IX_Presentacion_UnidadId] ON [Presentacion] ([UnidadId]);

GO

CREATE INDEX [IX_Receta_AreaProduccionId] ON [Receta] ([AreaProduccionId]);

GO

CREATE INDEX [IX_RecetaAReceta_IdRecetaPadre] ON [RecetaAReceta] ([IdRecetaPadre]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200620212813_CrearTablas', N'2.0.3-rtm-10026');

GO

