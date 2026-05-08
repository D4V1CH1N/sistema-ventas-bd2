-- 3. TRIGGERS Y FUNCIONES - Lab 6 & Práctica 15
-- Lógica para reducción de stock automática
CREATE OR REPLACE FUNCTION fn_actualizar_stock()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE producto 
    SET stock = stock - NEW.cantidad
    WHERE id_producto = NEW.id_producto;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_restar_stock ON detalle_venta;
CREATE TRIGGER trg_restar_stock
AFTER INSERT ON detalle_venta
FOR EACH ROW
EXECUTE FUNCTION fn_actualizar_stock();

-- Auditoría de cambios de precio (Práctica 15)
CREATE TABLE IF NOT EXISTS auditoria_precio (
    id_auditoria SERIAL PRIMARY KEY,
    id_producto INT,
    precio_anterior DECIMAL(10,2),
    precio_nuevo DECIMAL(10,2),
    fecha_cambio TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION fn_auditar_precio()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD.precio <> NEW.precio THEN
        INSERT INTO auditoria_precio (id_producto, precio_anterior, precio_nuevo)
        VALUES (OLD.id_producto, OLD.precio, NEW.precio);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_auditoria_precio ON producto;
CREATE TRIGGER trg_auditoria_precio
BEFORE UPDATE ON producto
FOR EACH ROW
EXECUTE FUNCTION fn_auditar_precio();
