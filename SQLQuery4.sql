
create table targeta_titular
(
	id_titular int IDENTITY(1, 1),
	Nombre_titular varchar(200) not  NULL,
	numero_targeta varchar (16) not NULL,
	limite_credito decimal(12, 2) NOT NULL default 0.00,
	saldo_actual decimal(12, 2)  default 0.00,
	saldo_disponible decimal(12, 2)  default 0.00,
	primary key(id_titular)
);
create table compras 
(
id_compras int primary key IDENTITY(1, 1),
id_titular int not null,
decription varchar(200),
monto decimal(12,2) default 0.00,
tipo varchar(25)not null,
fecha_compra date,
  FOREIGN KEY (id_titular) REFERENCES targeta_titular(id_titular)
);
create table pagos 
(
id_pagos int primary key IDENTITY(1, 1),
id_titular int not null,
decription varchar(200),
monto decimal(12,2) default 0.00,
tipo varchar(25)not null,
fecha_pago date,
  FOREIGN KEY (id_titular) REFERENCES targeta_titular(id_titular)
);

