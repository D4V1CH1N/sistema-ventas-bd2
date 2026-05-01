-- =========================================================================
-- SISTEMA DE VENTAS — BASE DE DATOS II
-- Autores: Flores Tarqui Genesis Alejandra | Zalles Soria David Alexander
-- Motor: PostgreSQL
-- =========================================================================

-- NOTA PARA EJECUCIÓN EN PGADMIN:
-- 1. Primero crear la base de datos ejecutando esta línea de forma aislada:
-- CREATE DATABASE db_ventas WITH ENCODING 'UTF8' TEMPLATE = template0;
-- 2. Conectarse a la base de datos "db_ventas" y ejecutar el resto del script.

-- =========================================================================
-- CREACIÓN DE TABLAS
-- =========================================================================

-- Tabla: CATEGORIA
CREATE TABLE categoria (
  id_categoria  SERIAL PRIMARY KEY,
  nombre        VARCHAR(80)  NOT NULL UNIQUE,
  descripcion   TEXT
);

-- Tabla: PRODUCTO
CREATE TABLE producto (
  id_producto   SERIAL PRIMARY KEY,
  id_categoria  INT NOT NULL REFERENCES categoria(id_categoria),
  nombre        VARCHAR(150) NOT NULL,
  descripcion   TEXT,
  precio        DECIMAL(10,2) NOT NULL CHECK (precio > 0),
  stock         INT NOT NULL DEFAULT 0 CHECK (stock >= 0)
);

-- Tabla: CLIENTE
CREATE TABLE cliente (
  id_cliente    SERIAL PRIMARY KEY,
  nombre        VARCHAR(100) NOT NULL,
  apellido      VARCHAR(100) NOT NULL,
  dni           VARCHAR(20)  NOT NULL UNIQUE,
  telefono      VARCHAR(20),
  email         VARCHAR(100) UNIQUE,
  direccion     TEXT,
  fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE
);

-- Tabla: EMPLEADO
CREATE TABLE empleado (
  id_empleado   SERIAL PRIMARY KEY,
  nombre        VARCHAR(100) NOT NULL,
  apellido      VARCHAR(100) NOT NULL,
  cargo         VARCHAR(50)  NOT NULL,
  telefono      VARCHAR(20)
);

-- Tabla: VENTA
CREATE TABLE venta (
  id_venta      SERIAL PRIMARY KEY,
  id_cliente    INT NOT NULL REFERENCES cliente(id_cliente),
  id_empleado   INT NOT NULL REFERENCES empleado(id_empleado),
  fecha_venta   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  estado        VARCHAR(10) NOT NULL DEFAULT 'Pendiente'
                  CHECK (estado IN ('Pendiente','Pagada','Anulada')),
  metodo_pago   VARCHAR(20) NOT NULL
                  CHECK (metodo_pago IN ('Efectivo','Tarjeta debito','Tarjeta credito','Transferencia')),
  total         DECIMAL(12,2) NOT NULL DEFAULT 0
);

-- Tabla: DETALLE_VENTA
CREATE TABLE detalle_venta (
  id_detalle      SERIAL PRIMARY KEY,
  id_venta        INT NOT NULL REFERENCES venta(id_venta),
  id_producto     INT NOT NULL REFERENCES producto(id_producto),
  cantidad        INT NOT NULL CHECK (cantidad > 0),
  precio_unitario DECIMAL(10,2) NOT NULL CHECK (precio_unitario > 0),
  subtotal        DECIMAL(12,2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED
);