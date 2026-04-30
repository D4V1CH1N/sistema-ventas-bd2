## 📝 Tarea 2: Enunciado y Descripción del Proyecto

### 1. Enunciado del Proyecto
**"Desarrollo de un Sistema Transaccional de Ventas y Control de Inventario para un Comercio Minorista"**
El presente proyecto consiste en el modelado y desarrollo de un sistema de punto de venta (POS) y gestión de catálogo desarrollado en C# y conectado a una base de datos PostgreSQL. Su propósito es digitalizar y automatizar el flujo comercial de una tienda, permitiendo el registro ágil de clientes y productos, el procesamiento de ventas, y la actualización automática del stock de inventario en tiempo real, garantizando la integridad de los datos financieros e históricos del negocio.

### 2. Descripción Completa del Proyecto (Contexto y Reglas del Negocio)
Nuestro proyecto está basado en un **Sistema de Ventas estándar**, adaptable a negocios como minimarkets, ferreterías o empresas importadoras. 

Para diseñar la base de datos, consideramos el ciclo de vida completo de una compra física en mostrador o mediante catálogo. El flujo que nuestro sistema debe soportar es el siguiente:

* **Gestión de Catálogo:** El negocio cuenta con un inventario de `PRODUCTOS`, los cuales, para mantener el orden, se agrupan por `CATEGORIAS` (ej. Lácteos, Limpieza, Electrónica). Cada producto tiene un precio de venta y una cantidad finita en stock.
* **Actores del Sistema:** Cuando una persona llega a comprar, el sistema debe registrar quién está comprando (`CLIENTE`) para mantener un historial y fidelización, y quién está operando el sistema (`USUARIO`/`EMPLEADO`) para temas de auditoría y responsabilidades.
* **La Transacción (El corazón del negocio):** La compra se registra en la entidad `VENTA`, donde se define la fecha, el método de pago y los montos totales. 
* **El Carrito (El Detalle):** Como un cliente no siempre compra un solo producto, consideramos crucial implementar un `DETALLE_VENTA`. Esta entidad intermedia actúa como el "carrito de compras", anotando cada producto seleccionado, su cantidad y, muy importante, **el precio unitario en ese instante**. Esto garantiza que si un producto sube de precio mañana, los reportes financieros de las ventas de hoy no se alteren matemáticamente.
* **Impacto en Inventario:** Por regla de negocio estricta, el sistema no permite vender si no hay existencias. Si el stock es 0, la venta se bloquea. Si la venta es exitosa, el stock disminuye automáticamente mediante la relación con el detalle de venta.

### 3. Diagrama Entidad-Relación (ERD)
*Nota: Este diagrama está generado con Mermaid para su visualización nativa en GitHub.*
```mermaid
erDiagram
    CLIENTE ||--o{ VENTA : "realiza"
    EMPLEADO ||--o{ VENTA : "atiende"
    
    VENTA ||--|{ DETALLE_VENTA : "contiene"
    PRODUCTO ||--o{ DETALLE_VENTA : "es incluido en"
    
    CATEGORIA ||--|{ PRODUCTO : "agrupa"

    CLIENTE {
        int id_cliente PK
        string nombre
        string documento_identidad UK
    }
    EMPLEADO {
        int id_empleado PK
        string nombre
        string cargo
    }
    VENTA {
        int id_venta PK
        int id_cliente FK
        int id_empleado FK
        timestamp fecha
        decimal total
    }
    DETALLE_VENTA {
        int id_detalle PK
        int id_venta FK
        int id_producto FK
        int cantidad
        decimal precio_unitario
    }
    PRODUCTO {
        int id_producto PK
        int id_categoria FK
        string nombre
        int stock
    }
    CATEGORIA {
        int id_categoria PK
        string nombre
    }