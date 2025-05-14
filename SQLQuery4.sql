CREATE DATABASE [pruebaTecnica];

USE [pruebaTecnica]
GO
/****** Object:  Table [dbo].[targetatitular]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[targetatitular](
	[id_titular] [int] IDENTITY(1,1) NOT NULL,
	[Nombre_titular] [varchar](200) NOT NULL,
	[numero_targeta] [varchar](16) NOT NULL,
	[limite_credito] [decimal](12, 2) NOT NULL,
	[saldo_actual] [decimal](12, 2) NULL,
	[saldo_disponible] [decimal](12, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_titular] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transacciones]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transacciones](
	[id_transaccion] [int] IDENTITY(1,1) NOT NULL,
	[id_titular] [int] NOT NULL,
	[decription] [varchar](200) NULL,
	[monto] [decimal](12, 2) NULL,
	[tipo] [varchar](25) NOT NULL,
	[fecha_transaccion] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_transaccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[transacciones] ADD  DEFAULT ((0.00)) FOR [monto]
GO
/****** Object:  StoredProcedure [dbo].[actualizar_saldos]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[actualizar_saldos] @saldo_actual decimal(12, 2), @saldo_disponible decimal(12, 2),@CodCliente INT
as 
begin
UPDATE targetatitular SET saldo_actual=@saldo_actual,saldo_disponible=@saldo_disponible WHERE id_titular=@CodCliente; 
end;
GO
/****** Object:  StoredProcedure [dbo].[CLIENTE]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CLIENTE] @codCliente INT
AS 
BEGIN
SELECT * FROM targetatitular WHERE id_titular=@codCliente;
END;
GO
/****** Object:  StoredProcedure [dbo].[ESTADO_CUENTAS]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ESTADO_CUENTAS] @CodCliente INT
AS 
BEGIN
select * from transacciones where id_titular=@CodCliente;
END;
GO
/****** Object:  StoredProcedure [dbo].[LISTA_CLIENTES]    Script Date: 14/5/2025 15:58:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LISTA_CLIENTES]
AS 
BEGIN
SELECT * FROM targetatitular;
END;
GO
