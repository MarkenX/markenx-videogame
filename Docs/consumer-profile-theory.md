# ConsumerProfile: El Vector del Consumidor

## ¿Qué es un ConsumerProfile?

Un **ConsumerProfile** es la representación completa de un consumidor como un **vector multidimensional**. En lugar de describir al consumidor con etiquetas simples como "premium" o "económico", lo representamos mediante una colección de valores numéricos en diferentes dimensiones psicológicas y de comportamiento.

### Analogía: El ADN del Consumidor
Así como el ADN define las características genéticas mediante secuencias, el perfil del consumidor define sus preferencias mediante un conjunto de valores dimensionales.

```
ConsumerProfile:
  PriceSensitivity    = 0.76  (Bastante sensible al precio)
  QualityExpectation  = 0.60  (Espera buena calidad)
  SocialRecognition   = 0.40  (Poco interés en impresionar)
  EaseOfUse           = 0.50  (Preferencia moderada por simplicidad)
```

---

## Estructura del Perfil

### Componentes Clave

```
┌─────────────────────────────────────────────┐
│         ConsumerProfile                      │
├─────────────────────────────────────────────┤
│  Dimensions: Dictionary                      │
│    ├─ PriceSensitivity     → 0.76           │
│    ├─ QualityExpectation   → 0.60           │
│    ├─ SocialRecognition    → 0.40           │
│    ├─ EaseOfUse            → 0.50           │
│    ├─ BrandLoyalty         → 0.35           │
│    └─ ... (más dimensiones)                 │
└─────────────────────────────────────────────┘
```

### ¿Por Qué un Dictionary?

Usamos `Dictionary<DimensionDefinition, DimensionValue>` porque:
1. **Búsqueda rápida**: O(1) para acceder a cualquier dimensión
2. **Flexibilidad**: Cada perfil puede tener diferentes dimensiones
3. **Extensibilidad**: Fácil agregar nuevas dimensiones sin cambiar código
4. **Seguridad de tipo**: La clave garantiza que cada dimensión esté bien definida

---

## Generación del Perfil: El Pipeline

El perfil del consumidor se genera mediante un **RuleEngine** que procesa datos de entrada:

```
┌──────────────────┐
│  Datos de Input  │
└────────┬─────────┘
         │
    ┌────▼─────────────────────────────────┐
    │  1. DNI/Identificación               │
    │     - Edad, género, ubicación        │
    └────┬─────────────────────────────────┘
         │
    ┌────▼─────────────────────────────────┐
    │  2. Datos Económicos                 │
    │     - Salario, ingresos              │
    │     - Nivel socioeconómico           │
    └────┬─────────────────────────────────┘
         │
    ┌────▼─────────────────────────────────┐
    │  3. Perfil Psicológico               │
    │     - Personalidad (Big Five)        │
    │     - Valores, intereses             │
    └────┬─────────────────────────────────┘
         │
    ┌────▼─────────────────────────────────┐
    │  4. Historial de Compras             │
    │     - Productos comprados            │
    │     - Frecuencia, categorías         │
    └────┬─────────────────────────────────┘
         │
    ┌────▼─────────────────────────────────┐
    │        RuleEngine                    │
    │  Aplica reglas de transformación     │
    └────┬─────────────────────────────────┘
         │
    ┌────▼─────────────────────────────────┐
    │     ConsumerProfile                  │
    │  (Vector de dimensiones 0-1)         │
    └──────────────────────────────────────┘
```

---

## Ejemplos de Reglas de Transformación

### Regla 1: Salario → PriceSensitivity

```csharp
// Regla: A menor salario, mayor sensibilidad al precio
if (salario < 1500)
    profile.Set(priceSensitivityDef, 0.85f);  // Alta sensibilidad
else if (salario < 3000)
    profile.Set(priceSensitivityDef, 0.60f);  // Sensibilidad media
else
    profile.Set(priceSensitivityDef, 0.30f);  // Baja sensibilidad
```

### Regla 2: Edad → TechnologyAdoption

```csharp
// Regla: Jóvenes adoptan tecnología más rápido
if (edad < 25)
    profile.Set(technologyAdoptionDef, 0.90f);
else if (edad < 45)
    profile.Set(technologyAdoptionDef, 0.65f);
else
    profile.Set(technologyAdoptionDef, 0.35f);
```

### Regla 3: Personalidad → SocialRecognition

```csharp
// Regla: Extrovertidos valoran más el reconocimiento social
float extroversion = psychProfile.GetTrait("Extroversion");
profile.Set(socialRecognitionDef, extroversion * 0.8f);
```

### Regla 4: Historial de Compras → BrandLoyalty

```csharp
// Regla: Si compró la misma marca 5+ veces, alta lealtad
int repeatPurchases = purchaseHistory.CountBrand("Nike");
float loyalty = Math.Min(repeatPurchases / 10f, 1.0f);
profile.Set(brandLoyaltyDef, loyalty);
```

---

## Casos de Uso Reales

### Ejemplo 1: Estudiante Universitario

**Datos de entrada:**
- Edad: 21 años
- Salario: $800/mes
- Personalidad: Extrovertido (0.75), Consciente (0.50)
- Compras: Principalmente online, ropa de moda

**Perfil generado:**
```
ConsumerProfile:
  PriceSensitivity    = 0.90  (Muy sensible - presupuesto limitado)
  QualityExpectation  = 0.45  (Moderada-baja - prioriza precio)
  SocialRecognition   = 0.75  (Alta - quiere impresionar)
  EaseOfUse           = 0.60  (Moderada - cómodo con tecnología)
  BrandLoyalty        = 0.30  (Baja - prueba marcas nuevas)
  TechnologyAdoption  = 0.85  (Alta - generación digital)
```

### Ejemplo 2: Ejecutivo Senior

**Datos de entrada:**
- Edad: 45 años
- Salario: $8,000/mes
- Personalidad: Consciente (0.85), Introvertido (0.40)
- Compras: Productos premium, marcas establecidas

**Perfil generado:**
```
ConsumerProfile:
  PriceSensitivity    = 0.25  (Baja - puede pagar más)
  QualityExpectation  = 0.95  (Muy alta - exige lo mejor)
  SocialRecognition   = 0.50  (Moderada - no necesita impresionar)
  EaseOfUse           = 0.80  (Alta - valora eficiencia)
  BrandLoyalty        = 0.85  (Muy alta - confía en marcas conocidas)
  TechnologyAdoption  = 0.60  (Moderada - usa tecnología establecida)
```

### Ejemplo 3: Madre de Familia

**Datos de entrada:**
- Edad: 35 años
- Salario: $3,500/mes (ingreso familiar)
- Personalidad: Amigable (0.80), Práctica (0.75)
- Compras: Productos para familia, compra en volumen

**Perfil generado:**
```
ConsumerProfile:
  PriceSensitivity    = 0.70  (Alta - busca buenos precios)
  QualityExpectation  = 0.65  (Buena - pero balanceada con precio)
  SocialRecognition   = 0.35  (Baja - prioriza funcionalidad)
  EaseOfUse           = 0.85  (Muy alta - necesita simplicidad)
  BrandLoyalty        = 0.60  (Moderada - leal a marcas probadas)
  ValueForMoney       = 0.90  (Muy alta - relación calidad-precio)
```

---

## Operaciones con Perfiles

### 1️⃣ Crear y Configurar

```csharp
var profile = new ConsumerProfile();

// Método básico: Set
profile.Set(priceSensitivityDef, 0.76f);
profile.Set(qualityExpectationDef, 0.60f);

// Verificar existencia
if (profile.HasDimension(priceSensitivityDef))
{
    Console.WriteLine("PriceSensitivity está definida");
}
```

### 2️⃣ Leer Valores

```csharp
// Obtener valor específico
var priceValue = profile.Get(priceSensitivityDef);
if (priceValue != null)
{
    Console.WriteLine($"Sensibilidad: {priceValue.Value}");
}

// Iterar todas las dimensiones
foreach (var dimension in profile.GetAllValues())
{
    Console.WriteLine($"{dimension.Definition.Name}: {dimension.Value}");
}
```

### 3️⃣ Ajustar Dinámicamente

```csharp
// El usuario vio 3 anuncios de descuentos
profile.Adjust(priceSensitivityDef, 0.10f);

// Tuvo mala experiencia con un producto
profile.Adjust(brandLoyaltyDef, -0.25f);

// Completó curso sobre calidad
profile.Adjust(qualityExpectationDef, 0.20f);
```

---

## Patrones de Uso: RuleEngine Integration

### Pipeline Típico

```csharp
public class ConsumerProfileBuilder
{
    private readonly RuleEngine _ruleEngine;

    public ConsumerProfile BuildProfile(ConsumerData data)
    {
        var profile = new ConsumerProfile();

        // 1. Aplicar reglas demográficas
        _ruleEngine.ApplyDemographicRules(data, profile);

        // 2. Aplicar reglas económicas
        _ruleEngine.ApplyEconomicRules(data.Salary, profile);

        // 3. Aplicar reglas psicológicas
        _ruleEngine.ApplyPsychologicRules(data.Personality, profile);

        // 4. Aplicar reglas de historial
        _ruleEngine.ApplyHistoryRules(data.PurchaseHistory, profile);

        return profile;
    }
}
```

### Ejemplo de Regla Compleja

```csharp
public void ApplyComplexRule(ConsumerData data, ConsumerProfile profile)
{
    // Regla: Millennials urbanos con salario medio-alto
    // → Alta adopción tecnológica + moderada sensibilidad al precio
    if (data.Age >= 25 && data.Age <= 40 &&
        data.Location.IsUrban &&
        data.Salary > 2500)
    {
        profile.Set(technologyAdoptionDef, 0.85f);
        profile.Set(priceSensitivityDef, 0.55f);
        profile.Set(sustainabilityDef, 0.70f);  // Valoran sostenibilidad
    }
}
```

---

## Ventajas del Enfoque de Perfil Vectorial

### ✅ Precisión
- Captura matices que etiquetas simples no pueden ("premium" vs "económico")
- Cada consumidor es único en su combinación de dimensiones

### ✅ Escalabilidad
- Agregar nuevas dimensiones no requiere reescribir lógica
- Funciona igual para 5 dimensiones que para 50

### ✅ Matching Matemático
- Fácil calcular similitud entre consumidores
- Comparación directa con perfiles de productos

### ✅ Machine Learning Ready
- El vector puede usarse como input para algoritmos de ML
- Clustering, clasificación, predicción

### ✅ Transparencia
- Puedes explicar por qué se recomienda un producto
- "Este producto coincide en 85% con tu perfil porque..."

---

## Aplicaciones Prácticas

### 🎯 Recomendación de Productos
```csharp
// Encontrar productos que mejor coincidan con el perfil
var matches = products
    .Select(p => new {
        Product = p,
        Score = CalculateMatch(profile, p.Profile)
    })
    .OrderByDescending(m => m.Score)
    .Take(10);
```

### 📊 Segmentación de Mercado
```csharp
// Agrupar consumidores similares
var segments = consumers
    .GroupBy(c => ClusterProfile(c.Profile))
    .Select(g => new Segment {
        Name = g.Key,
        Consumers = g.ToList(),
        AverageProfile = CalculateAverage(g.Select(c => c.Profile))
    });
```

### 🔮 Predicción de Comportamiento
```csharp
// Predecir si comprará un producto
bool WillPurchase(ConsumerProfile consumer, ProductProfile product)
{
    float matchScore = CalculateMatch(consumer, product);
    return matchScore > 0.75f;
}
```

---

## Evolución del Perfil en el Tiempo

Los perfiles no son estáticos - evolucionan con el comportamiento:

```csharp
// Después de cada compra, actualizar el perfil
public void UpdateProfileAfterPurchase(
    ConsumerProfile profile,
    Product purchasedProduct)
{
    // Si compró producto caro, reducir sensibilidad al precio
    if (purchasedProduct.Price > 0.7f)
    {
        profile.Adjust(priceSensitivityDef, -0.05f);
    }

    // Si compró producto premium, aumentar expectativa de calidad
    if (purchasedProduct.Quality > 0.8f)
    {
        profile.Adjust(qualityExpectationDef, 0.10f);
    }

    // Aumentar lealtad a la marca
    profile.Adjust(brandLoyaltyDef, 0.08f);
}
```

---

## Resumen: Checklist de ConsumerProfile

✅ **Es un vector multidimensional** (no una etiqueta simple)  
✅ **Se genera mediante RuleEngine** a partir de datos reales  
✅ **Usa Dictionary para flexibilidad** y rendimiento  
✅ **Permite operaciones de lectura, escritura y ajuste**  
✅ **Facilita matching con productos** mediante comparación matemática  
✅ **Es transparente y explicable** (sabes por qué el sistema recomienda algo)  
✅ **Evoluciona con el tiempo** según comportamiento del consumidor

---

## Próximos Pasos

Con `ConsumerProfile` completado, el siguiente paso natural es:

1. ✅ Definir dimensiones (`DimensionDefinition`)
2. ✅ Almacenar valores (`DimensionValue`)
3. ✅ Crear perfiles de consumidor (`ConsumerProfile`)
4. **ProductProfile**: Perfil equivalente para productos
5. **MatchCalculator**: Calcular compatibilidad consumidor-producto
6. **RuleEngine**: Motor que genera perfiles a partir de datos crudos