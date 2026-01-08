# Pruebas Unitarias del Backend 🧪

Este proyecto contiene tests unitarios para validar la lógica de negocio y los controladores de la API.

## Tecnologías
*   **xUnit**: Framework de pruebas.
*   **Moq**: Librería para simular dependencias (Mocks).
*   **dotnet test**: Ejecutor de pruebas.

## Estructura
*   `/Controllers`: Tests de integración de controladores (FavoritesController).
*   `/Helpers`: Tests de lógica de mapeo de datos (BookMapper).

## Cómo ejecutar los tests

1.  Abre una terminal en la carpeta raíz de la solución.
2.  Ejecuta el siguiente comando:

```bash
dotnet test
```

## Descripción de los Tests
Se han implementado los siguientes escenarios:

1.  **AgregarFavorito_LibroDuplicado_RetornaConflict**: Verifica que no se puedan guardar duplicados (409).
2.  **AgregarFavorito_SinTitulo_RetornaBadRequest**: Valida campos obligatorios.
3.  **AgregarFavorito_SinExternalId_RetornaBadRequest**: Valida integridad del ID del libro.
4.  **EliminarFavorito_NoExiste_RetornaNotFound**: Verifica manejo de errores al borrar (404).
5.  **ObtenerFavoritos_UsuarioAutenticado_RetornaListaCorrecta**: Confirma que el usuario recupera su lista.
6.  **MapearLibro_DesdeOpenLibrary_MapeaCorrectamente**: Asegura que la transformación de datos externos a modelo interno sea fiel.

¡Todos los tests deben pasar en verde! ✅
