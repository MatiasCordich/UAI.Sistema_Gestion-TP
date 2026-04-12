-- =============================================
-- SCRIPTS: SISTEMA DE GESTIÓN DE TPS (UAI)
-- =============================================

-- 1. Tabla de Usuarios (Admin, Profesores y Alumnos)
CREATE TABLE Usuario (
	
	-- Datos Básicos
    ID_USUARIO INT PRIMARY KEY IDENTITY(1,1),
    DNI VARCHAR(10) UNIQUE NOT NULL,
    NOMBRE VARCHAR(50) NOT NULL,
    APELLIDO VARCHAR(50) NOT NULL,
    EMAIL VARCHAR(100) UNIQUE NOT NULL,
    PASSWORD VARCHAR(64) NOT NULL,

	-- Roles
    ROL VARCHAR(15) NOT NULL CHECK (ROL IN ('ADMIN', 'PROFESOR', 'ALUMNO')),

	-- Datos específicos
    LEGAJO VARCHAR(20) NULL,        -- Solo para Alumnos
    NRO_EMPLEADO VARCHAR(20) NULL,  -- Solo para Profesores

	-- Estado del Usuario (Soft Delete)
    ACTIVO BIT DEFAULT 1            -- Soft Delete
);

-- 2. Tabla de Materias
CREATE TABLE Materia (
    ID_MATERIA INT PRIMARY KEY IDENTITY(1,1),
    NOMBRE VARCHAR(100) NOT NULL,
    ACTIVO BIT DEFAULT 1           
);

-- 3. Tabla Intermedia: Profesores - Materias (Asignación de cátedra)
-- Una materia puede tener varios profesores disponibles.
CREATE TABLE Profesor_Materia (
    ID_PROFESOR INT NOT NULL,
    ID_MATERIA INT NOT NULL,
    PRIMARY KEY (ID_PROFESOR, ID_MATERIA),
    FOREIGN KEY (ID_PROFESOR) REFERENCES Usuario(ID_USUARIO),
    FOREIGN KEY (ID_MATERIA) REFERENCES Materia(ID_MATERIA)
);

-- 4. Tabla Intermedia: Alumnos - Materias (Inscripción a cursada)
-- Un alumno tiene 1 profesor por Materia
CREATE TABLE Alumno_Materia (
    ID_ALUMNO INT NOT NULL,
    ID_MATERIA INT NOT NULL,
	ID_PROFESOR INT NOT NULL,
    PRIMARY KEY (ID_ALUMNO, ID_MATERIA),
    FOREIGN KEY (ID_ALUMNO) REFERENCES Usuario(ID_USUARIO),
    FOREIGN KEY (ID_MATERIA) REFERENCES Materia(ID_MATERIA),
	FOREIGN KEY (ID_PROFESOR) REFERENCES Usuario(ID_USUARIO)
);

-- 5. Tabla de Trabajos Prácticos 
CREATE TABLE TrabajoPractico (
    ID_TP INT PRIMARY KEY IDENTITY(1,1),
    ID_MATERIA INT NOT NULL,
    TITULO VARCHAR(150) NOT NULL,
    DESCRIPCION VARCHAR(MAX) NOT NULL,
    FECHA_LIMITE DATETIME NOT NULL,
    ACTIVO BIT DEFAULT 1,
    FOREIGN KEY (ID_MATERIA) REFERENCES Materia(ID_MATERIA)
);

-- 6. Tabla de Entregas (Nexo Alumno - TP - Profesor)
CREATE TABLE Entrega (
    ID_ENTREGA INT PRIMARY KEY IDENTITY(1,1),
    ID_ALUMNO INT NOT NULL,
    ID_TP INT NOT NULL,
    ID_PROFESOR INT NOT NULL, -- Se hereda de Alumno_Materia al crear la entrega
    
    CONTENIDO_LINK VARCHAR(MAX) NULL,
    
    -- Estados solicitados
    ESTADO_ENTREGA VARCHAR(20) DEFAULT 'No Entregado' CHECK (ESTADO_ENTREGA IN ('No Entregado', 'Entregado')),
    ESTADO_CORRECCION VARCHAR(30) DEFAULT 'PENDIENTE' CHECK (ESTADO_CORRECCION IN ('PENDIENTE', 'CORREGIDO')),
    
    NOTA INT NULL CHECK (NOTA BETWEEN 0 AND 10),
    DEVOLUCION VARCHAR(MAX) NULL,
    
    FECHA_ENTREGA DATETIME NULL,
    FECHA_CORRECCION DATETIME NULL,

    FOREIGN KEY (ID_ALUMNO) REFERENCES Usuario(ID_USUARIO),
    FOREIGN KEY (ID_TP) REFERENCES TrabajoPractico(ID_TP),
    FOREIGN KEY (ID_PROFESOR) REFERENCES Usuario(ID_USUARIO)
);

-- =============================================
-- DATOS INICIALES PARA PRUEBAS 
-- =============================================

-- El Admin (Password: admin123)
INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL)
VALUES ('000', 'Admin', 'Sistema', 'admin@uai.edu.ar', 'admin123', 'ADMIN');

-- Materias Iniciales
INSERT INTO Materia (NOMBRE) VALUES 
('Programación I'), 
('Programación II'), 
('Base de Datos I'), 
('Arquitectura de Software'), 
('Ingeniería de Software');

-- Profesores Iniciales
INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL, NRO_EMPLEADO)
VALUES ('26787898', 'Carlos', 'Gomez', 'cgomez@uai.edu.ar', 'teclado123', 'PROFESOR', 'EMP26787898');

INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL, NRO_EMPLEADO)
VALUES ('32546878', 'Laura', 'Sanz', 'lsanz@uai.edu.ar', 'teclado123', 'PROFESOR', 'EMP32546878');

-- Alumno Inicial
INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL, LEGAJO)
VALUES ('43657878', 'Juan', 'Perez', 'jperez@uai.edu.ar', '123', 'ALUMNO', 'ALU43657878');

-- ASIGNACIONES (Hechas por Admin)
-- La materia Programación II tiene dos profes disponibles
INSERT INTO Profesor_Materia (ID_PROFESOR, ID_MATERIA) VALUES (2, 2), (3, 2);

-- El Alumno Juan se inscribe en Programación II y el Admin le asigna al Profesor Carlos Gomez (ID 2)
INSERT INTO Alumno_Materia (ID_ALUMNO, ID_MATERIA, ID_PROFESOR) VALUES (4, 2, 2);