-- 2. ÍNDICES (INDEXES) - Lab 5 & Práctica 14
-- Optimización de búsqueda por nombre de producto
CREATE INDEX IF NOT EXISTS idx_producto_nombre ON producto (nombre);

-- Optimización de búsqueda por DNI (Requisito Práctica 14)
CREATE INDEX IF NOT EXISTS idx_cliente_dni ON cliente (dni);

-- Optimización de búsqueda por apellido (Requisito Lab 5)
CREATE INDEX IF NOT EXISTS idx_cliente_apellido ON cliente (apellido);
