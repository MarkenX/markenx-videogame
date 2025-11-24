# DimensionValue: Almacenando Valores de Dimensiones

## ¿Qué es un DimensionValue?

Un **DimensionValue** es el contenedor que almacena el **valor numérico** de una dimensión específica para un consumidor o producto. Mientras que `DimensionDefinition` define **qué** es la dimensión (el concepto), `DimensionValue` guarda **cuánto** de esa dimensión tiene una entidad.

### Analogía: Planilla de Excel
- **DimensionDefinition** = Encabezado de columna ("Precio", "Calidad")
- **DimensionValue** = Valor en una celda (0.8, 0.5)

---

## Normalización: Todo en Escala 0–1

Todas las dimensiones se almacenan en una escala **normalizada** de 0 a 1:

| Valor | Significado | Ejemplo (PriceSensitivity) | Ejemplo (QualityExpectation) |
|-------|-------------|---------------------------|------------------------------|
| **0.0** | Nada / Mínimo | No le importa el precio (millonario) | No espera calidad (compra lo más barato) |
| **0.3** | Bajo | Poco sensible al precio | Expectativa baja de calidad |
| **0.5** | Medio | Sensibilidad promedio | Espera calidad estándar |
| **0.7** | Alto | Bastante sensible al precio | Espera buena calidad |
| **1.0** | Máximo | Extremadamente sensible (compra solo ofertas) | Exige calidad premium |

---

## ¿Por Qué Normalizar?

### ✅ Ventajas de la Escala 0–1

1. **Comparabilidad**: Todas las dimensiones usan la misma escala
   - Puedes sumar, restar y comparar dimensiones diferentes sin problemas
   - Ejemplo: `PriceSensitivity=0.8` es comparable con `QualityExpectation=0.6`

2. **Simplicidad matemática**: Facilita cálculos de matching
   - Distancia entre consumidor y producto: `|consumer.Value - product.Value|`
   - Score total: promedio ponderado de todas las dimensiones

3. **Interpretación universal**: Fácil de entender para humanos
   - 0.9 siempre significa "muy alto" sin importar la dimensión
   - No necesitas recordar si "100" es bueno o malo en cada escala

4. **Prevención de errores**: El clamping evita valores inválidos
   - Si una regla suma 0.3 a un valor de 0.9, se ajusta a 1.0 automáticamente

---

## Clamping: Manteniendo Valores Válidos

El **clamping** es la técnica de forzar un valor a estar dentro de un rango:

```
Value = Math.Clamp(input, min, max)
```

### Ejemplos de Clamping

```csharp
// Constructor clampea automáticamente
var priceValue = new DimensionValue(priceDef, 1.5f);  // Se guarda como 1.0
var qualityValue = new DimensionValue(qualityDef, -0.2f); // Se guarda como 0.0

// Método Add() también clampea
var brandLoyalty = new DimensionValue(brandDef, 0.8f);
brandLoyalty.Add(0.5f);  // 0.8 + 0.5 = 1.3 → clampeado a 1.0
brandLoyalty.Add(-1.2f); // 1.0 - 1.2 = -0.2 → clampeado a 0.0
```

### ¿Por Qué es Importante?

Imagina una regla de negocio que dice:
> "Si el usuario ve 3 anuncios, aumentar PromotionAwareness en 0.3 por anuncio"

Sin clamping:
```
Inicial: 0.5
Después de 3 anuncios: 0.5 + 0.3 + 0.3 + 0.3 = 1.4 ❌ (valor inválido)
```

Con clamping:
```
Inicial: 0.5
Primer anuncio: 0.5 + 0.3 = 0.8
Segundo anuncio: 0.8 + 0.3 = 1.0 (clampeado)
Tercer anuncio: 1.0 + 0.3 = 1.0 (ya en máximo, no cambia)
```

---

## Casos de Uso Comunes

### 1️⃣ Representar Consumidores

```csharp
// Estudiante universitario
var student = new List<DimensionValue>
{
    new(priceSensitivity, 0.9f),     // Muy sensible al precio
    new(qualityExpectation, 0.5f),   // Calidad moderada
    new(socialRecognition, 0.7f),    // Quiere impresionar un poco
    new(easeOfUse, 0.6f)             // Prefiere algo simple
};
```

### 2️⃣ Representar Productos

```csharp
// iPhone 15 Pro
var iPhone = new List<DimensionValue>
{
    new(price, 0.95f),               // Muy caro
    new(quality, 0.98f),             // Calidad excelente
    new(socialStatus, 0.95f),        // Alto reconocimiento social
    new(usability, 0.90f)            // Muy fácil de usar
};

// Xiaomi Redmi Note
var xiaomi = new List<DimensionValue>
{
    new(price, 0.25f),               // Muy barato
    new(quality, 0.55f),             // Calidad decente
    new(socialStatus, 0.40f),        // Reconocimiento moderado
    new(usability, 0.60f)            // Usabilidad aceptable
};
```

### 3️⃣ Aplicar Cambios Dinámicos

```csharp
// El usuario vio una campaña publicitaria
promotionAwareness.Add(0.2f);

// El producto tuvo una rebaja
productPrice.Add(-0.3f);  // Reduce el precio

// El cliente tuvo mala experiencia
brandLoyalty.Add(-0.5f);  // Reduce lealtad
```

---

## Conversiones: Del Mundo Real a 0–1

A veces necesitas convertir valores del mundo real a la escala 0–1:

### Ejemplo 1: Precio en Dólares → PriceSensitivity

```csharp
// Rango de referencia: productos de $10 a $1000
float minPrice = 10f;
float maxPrice = 1000f;
float actualPrice = 500f;

float normalizedPrice = (actualPrice - minPrice) / (maxPrice - minPrice);
// normalizedPrice = (500 - 10) / (1000 - 10) = 0.495 ≈ 0.5
```

### Ejemplo 2: Edad → AgeGroup

```csharp
// Mapear edades a dimensión de edad normalizada
float NormalizeAge(int age)
{
    if (age < 18) return 0.0f;        // Menor
    if (age < 30) return 0.3f;        // Joven adulto
    if (age < 50) return 0.6f;        // Adulto
    if (age < 70) return 0.8f;        // Adulto mayor
    return 1.0f;                      // Tercera edad
}
```

### Ejemplo 3: Calificación 1–5 estrellas → QualityExpectation

```csharp
float stars = 4.2f;
float normalized = (stars - 1f) / 4f;  // 4.2 → 0.8
```

---

## Operaciones Comunes con DimensionValue

### ➕ Sumar Incrementos

```csharp
// El usuario completó un curso de calidad
qualityAwareness.Add(0.15f);
```

### ➖ Restar Decrementos

```csharp
// Promoción temporal reduce percepción de precio
pricePerception.Add(-0.25f);
```

### 🔄 Actualizar Completamente

```csharp
// Recalibrar basado en nueva información
qualityExpectation.SetValue(0.75f);
```

### 📊 Comparar Valores

```csharp
float difference = Math.Abs(consumer.Value - product.Value);
bool isGoodMatch = difference < 0.2f;  // Tolerancia de 20%
```

---

## Patrones de Diseño Aplicados

### 🔒 **Encapsulation**
- `Value` tiene setter privado → solo se modifica mediante métodos controlados
- Garantiza que siempre esté en rango válido

### ✅ **Invariant Enforcement**
- Constructor y métodos siempre clampean → imposible tener valores inválidos
- No necesitas validar externamente

### 🔗 **Association**
- Cada `DimensionValue` conoce su `DimensionDefinition`
- Facilita trazabilidad y debugging

---

## Errores Comunes a Evitar

### ❌ Error 1: Olvidar Normalizar Inputs

```csharp
// MAL: usar valor directo sin normalizar
var price = new DimensionValue(priceDef, 1200f);  // ¡Clampeado a 1.0!

// BIEN: normalizar primero
float normalizedPrice = NormalizePrice(1200f);  // → 0.8
var price = new DimensionValue(priceDef, normalizedPrice);
```

### ❌ Error 2: Comparar Dimensiones Incompatibles

```csharp
// MAL: comparar dimensiones diferentes sin contexto
if (priceSensitivity.Value > qualityExpectation.Value) { ... }

// BIEN: comparar la misma dimensión entre consumidor y producto
float priceMatch = Math.Abs(consumer.PriceSensitivity - product.Price);
```

### ❌ Error 3: Asumir Linealidad

```csharp
// MAL: asumir que 0.5 es siempre "neutro"
// En realidad, 0.5 en PriceSensitivity podría ser diferente que 0.5 en Quality

// BIEN: interpretar según contexto de cada dimensión
bool isHighPriceSensitivity = priceSensitivity.Value > 0.7f;
```

---

## Resumen: Checklist de DimensionValue

✅ **Todos los valores están entre 0 y 1**  
✅ **Constructor y métodos clampean automáticamente**  
✅ **Cada valor está vinculado a una DimensionDefinition**  
✅ **Método `Add()` permite modificaciones seguras**  
✅ **Valores son comparables entre sí**  
✅ **Facilita cálculos de matching y scoring**

---

## Próximos Pasos

Con `DimensionValue` ya puedes:
1. ✅ Definir dimensiones (`DimensionDefinition`)
2. ✅ Almacenar valores para esas dimensiones (`DimensionValue`)

Lo que sigue:
3. **DimensionalProfile**: Agrupar múltiples `DimensionValue` en un perfil completo
4. **MatchCalculator**: Calcular qué tan compatible es un consumidor con un producto
5. **WeightedScoring**: Dar más peso a ciertas dimensiones según el contexto