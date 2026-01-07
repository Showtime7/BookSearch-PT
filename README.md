# Book Search Application

## Descripción del Proyecto
Esta es una aplicación web desarrollada como prueba técnica para buscar libros. La solución implementa una arquitectura moderna con separación de responsabilidades, donde un frontend en Angular consume un backend propio en ASP.NET Core, el cual actúa como intermediario para consultar la API pública de Open Library.

## Stack Tecnológico Utilizado
- **Frontend**: Angular (v21+) con Tailwind CSS
- **Backend**: ASP.NET Core Web API (.NET 8)
- **API Externa**: Open Library Search API

## Características Principales
- **Búsqueda de Libros**: Funcionalidad pública que permite buscar libros por título o autor sin necesidad de registro.
- **Sistema de Login Opcional**: Acceso para usuarios administradores.
- **Filtros y Paginación**: Capacidad para filtrar por género/año y navegar entre páginas de resultados.
- **Sistema de Favoritos Completo**: 
  - Gestión de favoritos (Añadir/Eliminar) protegida por autenticación.
- **Almacenamiento en Memoria (Backend)** con validaciones. (Despreciado)
  - **Persistencia en SQL Server**: Base de datos relacional para usuarios y favoritos.
  - Página dedicada "Mis Favoritos" para usuarios registrados.
- **Diseño Responsive**: Interfaz adaptada a dispositivos móviles y escritorio (Mobile-first) con una estética cálida y minimalista.

## Requisitos Previos para que funcione la app
- **.NET SDK**: Versión 8.0 o superior.
- **SQL Server**: Instancia local (Express o Developer).
- **Node.js**: Versión 20 (LTS) o superior recomendada.
- **Angular CLI**: Versión 17 o superior (`npm install -g @angular/cli`).

## Instalación y Configuración

### 1. Clonar/Descargar el proyecto
Asegúrate de tener la carpeta raíz `Prueba_Tecnica` con los subdirectorios `BookSearchAPI` y `book-search-client`.

### 2. Backend (BookSearchAPI)
1. Abrir una terminal en la carpeta `BookSearchAPI`.
2. Restaurar dependencias y ejecutar:
   ```bash
   dotnet restore
   dotnet run --launch-profile http
   ```
   El servidor iniciará en `http://localhost:5258`.

### 3. Frontend (book-search-client)
1. Abrir una nueva terminal en la carpeta `book-search-client`.
2. Instalar dependencias:
   ```bash
   npm install
   ```
3. Iniciar el servidor de desarrollo:
   ```bash
   ng serve
   ```
   La aplicación estará disponible en `http://localhost:4200`.

## Configuración Adicional
- **CORS**: El backend está configurado en `Program.cs` para permitir peticiones desde `http://localhost:4200`. Si el puerto del frontend cambia, debe actualizarse esta política.
- **URL de API**: El frontend apunta a `http://localhost:5258` en `auth.service.ts` y `favorite.service.ts`. Si el backend se ejecuta en otro puerto, actualiza estos archivos.
- **Persistencia**: Los favoritos se almacenan en memoria volátil. Al reiniciar el backend, la lista se limpiará.

## Credenciales de Prueba
Para verificar las funcionalidades restringidas (botón de favoritos):
- **Usuario**: `admin`
- **Contraseña**: `admin123`

## Estructura del Proyecto
```
Prueba_Tecnica/
├── BookSearchAPI/          # Backend ASP.NET Core
│   ├── Controllers/        # Endpoints (Auth, Books, Favorites)
│   ├── Services/           # Lógica de negocio (FavoriteService, AuthService)
│   └── Models/             # DTOs y modelos de datos (Favorites, Books)
│
└── book-search-client/     # Frontend Angular
    ├── src/app/core/       # Servicios (Auth, Book, Favorite) y Modelos
    ├── src/app/features/   # Vistas (Login, Search, Favorites, BookCard)
    └── src/app/shared/     # Componentes comunes (Header, Toast)
```

## API Utilizada
El proyecto consume la **Open Library Search API**.
- **Documentación**: [https://openlibrary.org/dev/docs/api/search](https://openlibrary.org/dev/docs/api/search)

## Funcionalidades Pendientes
- [ ] Conexión a una base de datos real (SQL Server/PostgreSQL) para usuarios y favoritos.
- [ ] Registro de nuevos usuarios.
