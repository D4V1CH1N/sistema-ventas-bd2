-- 1. Insertar Categorías
INSERT INTO categoria (nombre, descripcion) VALUES 
('Música', 'Discos, vinilos y mercadería oficial de bandas'),
('Videojuegos', 'Juegos físicos, digitales y accesorios'),
('Anime', 'Figuras, mangas y coleccionables de animación');

-- 2. Insertar Clientes 
INSERT INTO cliente (nombre, apellido, dni, telefono, email, direccion) VALUES 
('Renata', 'Gómez', '8765432', '77112233', 'renata@email.com', 'Zona Sur, La Paz'),
('Janett Rina', 'Soria', '3456789', '77223344', 'janett.s@email.com', 'Obrajes, La Paz'),
('Carlos', 'Mamani', '9876543', '77998877', 'carlos.m@email.com', 'Sopocachi, La Paz');

-- 3. Insertar Empleados
INSERT INTO empleado (nombre, apellido, cargo, telefono) VALUES 
('David Alexander', 'Zalles Soria', 'Administrador', '71122334'),
('Genesis Alejandra', 'Flores Tarqui', 'Vendedora', '72233445');

-- 4. Insertar Productos
INSERT INTO producto (id_categoria, nombre, descripcion, precio, stock) VALUES 
(1, 'Álbum Gorillaz - Humanz Super Deluxe', 'Edición especial en caja de vinilo', 350.00, 10),
(2, 'Minecraft - Espada de Diamante', 'Réplica oficial a escala real', 120.00, 15),
(3, 'Figura de Acción - Naruto', 'Figura coleccionable de 20cm', 250.00, 8),
(2, 'Código Digital - Cat Quest II', 'Juego cooperativo para 2 jugadores', 90.00, 20),
(3, 'Arknights - Pack de Arte Oficial', 'Libro de ilustraciones exclusivas', 180.00, 5);