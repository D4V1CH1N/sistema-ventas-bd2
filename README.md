# \# 🛒 Sistema de Ventas — Base de Datos II

# 

# \*\*Autores:\*\*

# \* 👤 Flores Tarqui Genesis Alejandra

# \* 👤 Zalles Soria David Alexander

# 

# \*\*Lenguaje de Desarrollo:\*\* C# (Windows Forms)

# \*\*Motor de Base de Datos:\*\* PostgreSQL

# \*\*Fecha:\*\* Abril 2026

# 

# \---

# 

# \## 📋 Descripción y Alcance

# 

# Sistema transaccional enfocado en automatizar y gestionar las operaciones comerciales de un negocio, permitiendo:

# \* Registrar y consultar clientes, productos y empleados

# \* Procesar ventas con detalle de productos y cantidades

# \* Controlar el stock de inventario en tiempo real

# \* Generar comprobantes de venta

# \* Gestionar métodos de pago y estados de transacción

# 

# \---

# 

# \## 🔧 Tecnologías Utilizadas

# 

# | Herramienta | Versión | Uso en el proyecto |

# | :--- | :--- | :--- |

# | \*\*C# / Windows Forms\*\* | .NET 6+ | Lenguaje principal — interfaz de escritorio |

# | \*\*PostgreSQL\*\* | 15.x | Motor de base de datos relacional |

# | \*\*Npgsql\*\* | Driver | Conexión entre C# y PostgreSQL |

# | \*\*Git / Gitbash\*\* | 2.x | Control de versiones del proyecto |

# | \*\*GitHub\*\* | — | Repositorio remoto para compartir el código |

# 

# \---

# 

# \## 📁 Repositorios de Referencia

# 

# Para el desarrollo y estructuración técnica de este proyecto, nos basamos en los siguientes repositorios:

# 

# | Repositorio | Tecnologías | Descripción y uso en el proyecto |

# | :--- | :--- | :--- |

# | \*\*QuickBite-food\_ordering\*\* | Python (Flask) + JSON | Referencia lógica para la gestión del "carrito de compras" usando variables de sesión y cálculo de subtotales |

# | \*\*punto-venta-csharp\*\* | C# (WinForms) + SQL Server | Arquitectura MVC y diseño de interfaz base centrada en el registro de ventas — se adapta a PostgreSQL |

# | \*\*Ventas-SQL (gonzaloAven201)\*\* | C# (WinForms) + SQL Server | Referencia principal para la lógica transaccional con DataGridView — se migra el motor a PostgreSQL |

# 

# \---

# 

# \## 📐 Reglas del Negocio

# 

# \*\*Clientes\*\*

# \* Cada cliente se identifica de forma única por su DNI o Cédula de Identidad.

# \* Un cliente puede realizar múltiples ventas a lo largo del tiempo.

# \* No se puede eliminar un cliente que tenga ventas registradas (integridad referencial).

# 

# \*\*Productos\*\*

# \* Cada producto pertenece obligatoriamente a una categoría.

# \* El precio queda fijo al momento de la transacción (dato histórico).

# \* No se puede registrar una venta con un producto cuyo stock sea igual a cero.

# \* Al anular una venta, el stock de los productos involucrados debe restaurarse automáticamente.

# 

# \*\*Ventas\*\*

# \* Toda venta debe estar asociada a un cliente y a un empleado registrado.

# \* Una venta debe contener al menos un producto en su detalle.

# \* El total = suma de los subtotales de cada ítem del detalle.

# \* Estados posibles: `Pendiente` | `Pagada` | `Anulada`

# \* Métodos de pago: `Efectivo` | `Tarjeta débito` | `Tarjeta crédito` | `Transferencia`

# 

# \*\*Inventario\*\*

# \* Al registrar una venta, el stock se reduce según la cantidad vendida.

# \* La venta afecta directamente al inventario mediante el detalle de venta.

# \* Al anular una venta, el stock se restituye de forma automática.

# 

# \---

# 

# \## 🗃️ Relevamiento — Entidades, Atributos y Relaciones

# 

# \### Entidades Principales

# 

# \*\*1. CLIENTE\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_cliente | INT | PK / NOT NULL | Identificador único del cliente |

# | nombre | VARCHAR(100) | NOT NULL | Nombre del cliente |

# | apellido | VARCHAR(100) | NOT NULL | Apellido del cliente |

# | dni | VARCHAR(20) | UNIQUE / NOT NULL | Documento de identidad / CI |

# | telefono | VARCHAR(20) | NULL | Número de contacto |

# | email | VARCHAR(100) | UNIQUE / NULL | Correo electrónico |

# | direccion | TEXT | NULL | Dirección física |

# | fecha\_registro | DATE | NOT NULL | Fecha de alta en el sistema |

# 

# \*\*2. EMPLEADO\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_empleado | INT | PK / NOT NULL | Identificador único del empleado |

# | nombre | VARCHAR(100) | NOT NULL | Nombre del empleado |

# | apellido | VARCHAR(100) | NOT NULL | Apellido del empleado |

# | cargo | VARCHAR(50) | NOT NULL | Cargo o rol en la empresa |

# | telefono | VARCHAR(20) | NULL | Número de contacto |

# 

# \*\*3. CATEGORIA\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_categoria | INT | PK / NOT NULL | Identificador único de la categoría |

# | nombre | VARCHAR(80) | NOT NULL / UNIQUE | Nombre de la categoría |

# | descripcion | TEXT | NULL | Descripción opcional |

# 

# \*\*4. PRODUCTO\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_producto | INT | PK / NOT NULL | Identificador único del producto |

# | id\_categoria | INT | FK / NOT NULL | Categoría a la que pertenece |

# | nombre | VARCHAR(150) | NOT NULL | Nombre del producto |

# | descripcion | TEXT | NULL | Descripción detallada |

# | precio | DECIMAL(10,2) | NOT NULL / > 0 | Precio de venta actual |

# | stock | INT | NOT NULL / >= 0 | Cantidad disponible en inventario |

# 

# \*\*5. VENTA\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_venta | INT | PK / NOT NULL | Identificador único de la venta |

# | id\_cliente | INT | FK / NOT NULL | Cliente que realiza la compra |

# | id\_empleado | INT | FK / NOT NULL | Empleado que atiende la venta |

# | fecha\_venta | TIMESTAMP | NOT NULL | Fecha y hora de la transacción |

# | estado | VARCHAR(10) | NOT NULL | Pendiente / Pagada / Anulada |

# | metodo\_pago | VARCHAR(20) | NOT NULL | Efectivo / Tarjeta / Transferencia |

# | total | DECIMAL(12,2) | NOT NULL / >= 0 | Suma total de la venta |

# 

# \*\*6. DETALLE\_VENTA (Entidad Débil)\*\*

# | Atributo | Tipo | Restricción | Descripción |

# | :--- | :--- | :--- | :--- |

# | id\_detalle | INT | PK / NOT NULL | Identificador único del detalle |

# | id\_venta | INT | FK / NOT NULL | Venta a la que pertenece |

# | id\_producto | INT | FK / NOT NULL | Producto vendido |

# | cantidad | INT | NOT NULL / > 0 | Unidades vendidas |

# | precio\_unitario | DECIMAL(10,2) | NOT NULL / > 0 | Precio al momento de la venta |

# | subtotal | DECIMAL(12,2) | GENERADO | cantidad × precio\_unitario |

# 

# \---

# 

# \### Relaciones entre Entidades

# 

# | Relación | Cardinalidad | Descripción |

# | :--- | :--- | :--- |

# | CLIENTE → VENTA | 1 : N | Un cliente puede tener múltiples ventas |

# | EMPLEADO → VENTA | 1 : N | Un empleado puede registrar múltiples ventas |

# | VENTA → DETALLE\_VENTA | 1 : N | Una venta contiene múltiples detalles (productos) |

# | PRODUCTO → DETALLE\_VENTA | 1 : N | Un producto puede estar en múltiples detalles |

# | CATEGORIA → PRODUCTO | 1 : N | Una categoría agrupa múltiples productos |

# 

# \*\*Diagrama de Relaciones\*\*

# ```text

# CATEGORIA  (1) ──────► (N)  PRODUCTO  (1) ──────► (N)  DETALLE\_VENTA

# &#x20;                                                        ▲

# CLIENTE    (1) ──────► (N)  VENTA     (1) ──────► (N)  DETALLE\_VENTA

# &#x20;                                                        ▲

# EMPLEADO   (1) ──────► (N)  VENTA

# ```

# 

# \---

# 

# \## 📊 DIAGRAMAS GRÁFICOS

# 

# !\[Diagrama del Sistema](DIAGRAMA.png)

# 

# !\[Esquema de la BD](diagrama\_erd.png)

