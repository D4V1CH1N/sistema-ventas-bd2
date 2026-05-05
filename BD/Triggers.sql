-- Función que resta el stock
CREATE OR REPLACE FUNCTION actualizar_stock_venta()
RETURNS TRIGGER AS $$
BEGIN
    -- Resta la cantidad vendida al stock del producto
    UPDATE producto 
    SET stock = stock - NEW.cantidad
    WHERE id_producto = NEW.id_producto;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger que se activa automáticamente después de insertar un detalle_venta
CREATE TRIGGER trg_restar_stock
AFTER INSERT ON detalle_venta
FOR EACH ROW
EXECUTE FUNCTION actualizar_stock_venta();