-- 1. VISTAS (VIEWS) - Integración C#
-- Vista para el Dashboard Principal (FrmMenuPrincipal)
CREATE OR REPLACE VIEW vw_reporte_ventas AS
SELECT 
    v.id_venta,
    v.fecha_venta,
    c.nombre || ' ' || c.apellido AS cliente,
    e.nombre AS vendedor,
    v.estado,
    v.total
FROM venta v
INNER JOIN cliente c ON v.id_cliente = c.id_cliente
INNER JOIN empleado e ON v.id_empleado = e.id_empleado;

-- Vista para detalles y comprobantes
CREATE OR REPLACE VIEW vw_comprobante_detalle AS
SELECT 
    dv.id_venta,
    p.nombre AS descripcion_producto,
    dv.cantidad,
    dv.precio_unitario,
    (dv.cantidad * dv.precio_unitario) AS subtotal
FROM detalle_venta dv
INNER JOIN producto p ON dv.id_producto = p.id_producto;
