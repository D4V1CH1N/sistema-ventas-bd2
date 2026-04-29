# 🛒 Sistema de Ventas — Base de Datos II

**Autores:**
- 👤 Flores Tarqui Genesis Alejandra
- 👤 Zalles Soria David Alexander

**Lenguaje de Desarrollo:** C# (Windows Forms)
**Motor de Base de Datos:** PostgreSQL
**Fecha:** Abril 2026

---

## 📋 Descripción y Alcance

Sistema transaccional enfocado en automatizar y gestionar las operaciones comerciales de un negocio, permitiendo:

- Registrar y consultar clientes, productos y empleados
- Procesar ventas con detalle de productos y cantidades
- Controlar el stock de inventario en tiempo real
- Generar comprobantes de venta
- Gestionar métodos de pago y estados de transacción

---

## 🔧 Tecnologías Utilizadas

| Herramienta | Versión | Uso en el proyecto |
| :--- | :---: | :--- |
| **C# / Windows Forms** | .NET 6+ | Lenguaje principal — interfaz de escritorio |
| **PostgreSQL** | 15.x | Motor de base de datos relacional |
| **Npgsql** | Driver | Conexión entre C# y PostgreSQL |
| **Git / Gitbash** | 2.x | Control de versiones del proyecto |
| **GitHub** | — | Repositorio remoto para compartir el código |

---

## 📁 Repositorios de Referencia

Para el desarrollo y estructuración técnica de este proyecto, nos basamos en los siguientes repositorios:

| Repositorio | Tecnologías | Descripción y uso en el proyecto |
| :--- | :--- | :--- |
| **QuickBite-food_ordering** | Python (Flask) + JSON | Referencia lógica para la gestión del "carrito de compras" usando variables de sesión y cálculo de subtotales |
| **punto-venta-csharp** | C# (WinForms) + SQL Server | Arquitectura MVC y diseño de interfaz base centrada en el registro de ventas — se adapta a PostgreSQL |
| **Ventas-SQL (gonzaloAven201)** | C# (WinForms) + SQL Server | Referencia principal para la lógica transaccional con DataGridView — se migra el motor a PostgreSQL |

---

## 📐 Reglas del Negocio

### Clientes
1. Cada cliente se identifica de forma única por su DNI o Cédula de Identidad.
2. Un cliente puede realizar múltiples ventas a lo largo del tiempo.
3. **No se puede eliminar un cliente que tenga ventas registradas** (integridad referencial).

### Productos
1. Cada producto pertenece obligatoriamente a una categoría.
2. **El precio queda fijo al momento de la transacción** (dato histórico).
3. **No se puede registrar una venta con un producto cuyo stock sea igual a cero.**
4. Al anular una venta, el stock de los productos involucrados debe restaurarse automáticamente.

### Ventas
1. Toda venta debe estar asociada a un **cliente** y a un **empleado** registrado.
2. Una venta debe contener **al menos un producto** en su detalle.
3. El total = suma de los subtotales de cada ítem del detalle.
4. Estados posibles: `Pendiente` | `Pagada` | `Anulada`
5. Métodos de pago: `Efectivo` | `Tarjeta débito` | `Tarjeta crédito` | `Transferencia`

### Inventario
1. Al registrar una venta, el stock se reduce según la cantidad vendida.
2. **La venta afecta directamente al inventario mediante el detalle de venta.**
3. Al anular una venta, el stock se restituye de forma automática.

---

## 🗃️ Relevamiento — Entidades, Atributos y Relaciones

### Entidades Principales

#### 1. CLIENTE
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_cliente | INT | PK / NOT NULL | Identificador único del cliente |
| nombre | VARCHAR(100) | NOT NULL | Nombre del cliente |
| apellido | VARCHAR(100) | NOT NULL | Apellido del cliente |
| dni | VARCHAR(20) | UNIQUE / NOT NULL | Documento de identidad / CI |
| telefono | VARCHAR(20) | NULL | Número de contacto |
| email | VARCHAR(100) | UNIQUE / NULL | Correo electrónico |
| direccion | TEXT | NULL | Dirección física |
| fecha_registro | DATE | NOT NULL | Fecha de alta en el sistema |

#### 2. EMPLEADO
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_empleado | INT | PK / NOT NULL | Identificador único del empleado |
| nombre | VARCHAR(100) | NOT NULL | Nombre del empleado |
| apellido | VARCHAR(100) | NOT NULL | Apellido del empleado |
| cargo | VARCHAR(50) | NOT NULL | Cargo o rol en la empresa |
| telefono | VARCHAR(20) | NULL | Número de contacto |

#### 3. CATEGORIA
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_categoria | INT | PK / NOT NULL | Identificador único de la categoría |
| nombre | VARCHAR(80) | NOT NULL / UNIQUE | Nombre de la categoría |
| descripcion | TEXT | NULL | Descripción opcional |

#### 4. PRODUCTO
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_producto | INT | PK / NOT NULL | Identificador único del producto |
| id_categoria | INT | FK / NOT NULL | Categoría a la que pertenece |
| nombre | VARCHAR(150) | NOT NULL | Nombre del producto |
| descripcion | TEXT | NULL | Descripción detallada |
| precio | DECIMAL(10,2) | NOT NULL / > 0 | Precio de venta actual |
| stock | INT | NOT NULL / >= 0 | Cantidad disponible en inventario |

#### 5. VENTA
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_venta | INT | PK / NOT NULL | Identificador único de la venta |
| id_cliente | INT | FK / NOT NULL | Cliente que realiza la compra |
| id_empleado | INT | FK / NOT NULL | Empleado que atiende la venta |
| fecha_venta | TIMESTAMP | NOT NULL | Fecha y hora de la transacción |
| estado | VARCHAR(10) | NOT NULL | Pendiente / Pagada / Anulada |
| metodo_pago | VARCHAR(20) | NOT NULL | Efectivo / Tarjeta / Transferencia |
| total | DECIMAL(12,2) | NOT NULL / >= 0 | Suma total de la venta |

#### 6. DETALLE_VENTA *(Entidad Débil)*
| Atributo | Tipo | Restricción | Descripción |
| :--- | :--- | :---: | :--- |
| id_detalle | INT | PK / NOT NULL | Identificador único del detalle |
| id_venta | INT | FK / NOT NULL | Venta a la que pertenece |
| id_producto | INT | FK / NOT NULL | Producto vendido |
| cantidad | INT | NOT NULL / > 0 | Unidades vendidas |
| precio_unitario | DECIMAL(10,2) | NOT NULL / > 0 | Precio al momento de la venta |
| subtotal | DECIMAL(12,2) | GENERADO | cantidad × precio_unitario |

---

### Relaciones entre Entidades

| Relación | Cardinalidad | Descripción |
| :--- | :---: | :--- |
| CLIENTE → VENTA | 1 : N | Un cliente puede tener múltiples ventas |
| EMPLEADO → VENTA | 1 : N | Un empleado puede registrar múltiples ventas |
| VENTA → DETALLE_VENTA | 1 : N | Una venta contiene múltiples detalles (productos) |
| PRODUCTO → DETALLE_VENTA | 1 : N | Un producto puede estar en múltiples detalles |
| CATEGORIA → PRODUCTO | 1 : N | Una categoría agrupa múltiples productos |

### Diagrama de Relaciones

```
CATEGORIA  (1) ──────► (N)  PRODUCTO  (1) ──────► (N)  DETALLE_VENTA
                                                               ▲
CLIENTE    (1) ──────► (N)  VENTA     (1) ──────► (N)  DETALLE_VENTA
                                                               ▲
EMPLEADO   (1) ──────► (N)  VENTA
```

---

## 🗄️ Script SQL — Creación de la Base de Datos (PostgreSQL)

```sql
-- ============================================
-- BASE DE DATOS: SISTEMA DE VENTAS
-- Autores: Flores Tarqui Genesis Alejandra
--          Zalles Soria David Alexander
-- Motor: PostgreSQL | Lenguaje: C# WinForms
-- ============================================

CREATE DATABASE db_ventas
  WITH ENCODING 'UTF8' TEMPLATE = template0;

\c db_ventas;

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
```

---

## 🚀 Primer Commit — Subir al Repositorio

```bash
# 1. Inicializar el repositorio local
git init

# 2. Agregar el README al staging
git add README.md

# 3. Primer commit con mensaje profesional
git commit -m "docs: relevamiento inicial, reglas de negocio y repositorios de referencia"

# 4. Asegurarse de estar en la rama principal
git branch -M main

# 5. Conectar con el repositorio remoto (reemplazar con tu URL de GitHub)
git remote add origin https://github.com/tu-usuario/sistema-ventas.git

# 6. Subir el código
git push -u origin main
```

> 💡 **Tip — Commits incrementales:** El docente pidió commits diarios. Usa mensajes descriptivos:
> - `"feat: agregar modelos de entidades C#"`
> - `"fix: corregir relación FK en detalle_venta"`
> - `"docs: actualizar README con script SQL"`

---

## 📁 Estructura del Repositorio

```
sistema-ventas/
├── README.md                      ← Este archivo (Markdown)
├── database/
│   └── schema.sql                 ← Script SQL de creación de la BD
├── src/
│   ├── SistemaVentas.sln          ← Solución C# Visual Studio
│   └── SistemaVentas/
│       ├── Models/                ← Clases entidad (Cliente, Producto, etc.)
│       ├── Forms/                 ← Formularios Windows Forms
│       └── Data/                  ← Conexión y consultas PostgreSQL
└── docs/
    └── Tarea1_SistemaVentas.docx  ← Documento de relevamiento
```

---

*Tarea 1 — Trabajo Martes | Base de Datos II | Abril 2026*
*Flores Tarqui Genesis Alejandra | Zalles Soria David Alexander*
