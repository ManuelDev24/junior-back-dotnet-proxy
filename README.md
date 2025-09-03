 🎯 Objetivo general

1. Backend .NET que actúa como **proxy/adaptador** hacia una API abierta.

> API pública sugerida: Fake Store API (productos y categorías).
> 
> 
> Puedes usar otra similar (JSONPlaceholder, DummyJSON), pero no guardes datos localmente.
> 

---

 1) Prueba Backend — .NET 8 (sin SQL)

 Alcance

- Exponer una API interna bajo `/api/*`.
- Consumir datos remotos desde **Fake Store API**.
- Adaptar y normalizar datos (nombres de campos, paginación, filtro).
- No persistir nada. Datos en memoria solo para cache opcional.

 Endpoints internos (propios)

- `GET /api/products?page=1&pageSize=10&q=texto&category=men's%20clothing`
    - Funcionalidad:
        - Pide a `https://fakestoreapi.com/products`
        - Filtra por `q` en `title`, `description` y `category` (case-insensitive).
        - Filtra por `category` si viene el parámetro.
        - Pagina en el backend (`page`, `pageSize`).
    - Respuesta (ejemplo):
        
        ```json
        {
          "items": [
            {
              "id": 1,
              "name": "Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops",
              "brand": "unknown",
              "price": 109.95,
              "status": "new",
              "stock": 0,
              "image": "https://...",
              "category": "men's clothing",
              "createdAt": "2025-01-01T00:00:00Z"
            }
          ],
          "page": 1,
          "pageSize": 10,
          "total": 200
        }
        
        ```
        
- `GET /api/products/{id}`
    - GET a `https://fakestoreapi.com/products/{id}` y mapea al formato interno.
- `GET /api/categories`
    - GET a `https://fakestoreapi.com/products/categories` y devuelve un arreglo de strings.
- `POST /api/products`
    - Simular creación sin persistencia.
    - Validar campos requeridos y devolver 201 con el payload normalizado más un `id` ficticio.
- Manejo de errores:
    - 400 para validaciones, 404 si `id` no existe, 502 si falla la API externa, 500 para fallas no controladas.
- CORS: permitir `http://localhost:5173` (React) y GitHub Pages.

 Modelo interno sugerido

```json
{
  "id": number,
  "name": "string",
  "brand": "string",
  "price": number,
  "status": "new" | "used",
  "stock": number,
  "image": "string",
  "category": "string",
  "createdAt": "ISO-8601 string"
}

```

Notas de mapeo desde Fake Store:

- `title` → `name`
- `price` → `price`
- `category` → `category`
- `image` → `image`
- `brand` → `"unknown"` (Fake Store no trae marca)
- `status` → `"new"` por defecto
- `stock` → `0` por defecto
- `createdAt` → fecha generada en el backend (UTC)

 Requisitos técnicos

- .NET 8 Minimal API, `HttpClient`, `System.Text.Json`.
- `Swagger` habilitado.
- `appsettings.json` con `ExternalApis:FakeStoreBaseUrl`.
- README claro.
- Tests: al menos 2 (por ejemplo: filtrado local y paginación) (Opcional).

### Criterios de evaluación (100 pts)

- Correcto consumo de API externa y mapeo (30)
- Paginación y filtrado interno (25)
- Manejo de errores y respuestas (15)
- Swagger + README (15)
- Orden del código y commits (15)

 Entregables (GitHub)

- Repo: `junior-back-dotnet-proxy`
- Ramas y commits claros
- Opcional: `Dockerfile`
