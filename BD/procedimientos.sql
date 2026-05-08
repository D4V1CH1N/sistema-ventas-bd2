-- 4. PROCEDIMIENTOS ALMACENADOS (STORED PROCEDURES)
-- Registro transaccional completo (Lab 6)
CREATE OR REPLACE PROCEDURE pr_registrar_venta(
    p_id_cliente INT,
    p_id_empleado INT,
    p_metodo_pago VARCHAR,
    p_total DECIMAL,
    p_id_producto INT,
    p_cantidad INT,
    p_precio_unitario DECIMAL
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_id_venta INT;
BEGIN
    -- Cabecera de venta
    INSERT INTO venta (id_cliente, id_empleado, metodo_pago, estado, total, fecha_venta)
    VALUES (p_id_cliente, p_id_empleado, p_metodo_pago, 'Pagada', p_total, CURRENT_TIMESTAMP)
    RETURNING id_venta INTO v_id_venta;

    -- Detalle de venta
    INSERT INTO detalle_venta (id_venta, id_producto, cantidad, precio_unitario)
    VALUES (v_id_venta, p_id_producto, p_cantidad, p_precio_unitario);
    
    COMMIT;
END;
$$;

-- Inserción rápida de categorías (Práctica 15)
CREATE OR REPLACE PROCEDURE pr_insertar_categoria(
    p_nombre VARCHAR, 
    p_descripcion TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO categoria (nombre, descripcion)
    VALUES (p_nombre, p_descripcion);
END;
$$;
