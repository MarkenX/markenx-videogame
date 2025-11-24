# DistanceMetric: ¿Qué Tan Compatible es el Consumidor con el Producto?

## ¿Qué es la Distancia Métrica?

La **distancia métrica** es el cálculo matemático que responde la pregunta fundamental del marketing: **"¿Este producto es adecuado para este consumidor?"**

Comparamos el vector del consumidor (lo que quiere) con el vector del producto (lo que ofrece), y convertimos esa comparación en un **porcentaje de aceptación** del 0% al 100%.

### Analogía: Compatibilidad de Pareja

Imagina dos personas en una aplicación de citas:
- **Persona A** busca: deportivo (0.9), intelectual (0.6), romántico (0.4)
- **Persona B** ofrece: deportivo (0.8), intelectual (0.7), romántico (0.3)

La "distancia" mide qué tan compatibles son → 85% de match

---

## Visualización del Concepto

```
Consumidor:                    Producto:
┌─────────────────────┐       ┌─────────────────────┐
│ PriceSensitivity    │       │ PricePerceived      │
│ = 0.76              │  vs   │ = 0.60              │
│                     │       │                     │
│ QualityExpectation  │       │ Quality             │
│ = 0.60              │  vs   │ = 0.70              │
│                     │       │                     │
│ SocialRecognition   │       │ SocialRecognition   │
│ = 0.40              │  vs   │ = 0.80              │
│                     │       │                     │
│ EaseOfUse           │       │ EaseOfUse           │
│ = 0.50              │  vs   │ = 0.40              │
└─────────────────────┘       └─────────────────────┘
      [Vector A]                   [Vector B]
           │                            │
           └────────── Distancia ───────┘
                         ↓
                 Aceptación: 81%
```

---

## Métodos de Cálculo de Distancia

### 1️⃣ Distancia Manhattan (L1) - **RECOMENDADO**

La distancia Manhattan suma las diferencias absolutas entre cada dimensión.

#### Fórmula

```
distance = Σ |consumer[i] - product[i]| / n
acceptance = 1 - distance
```

#### Algoritmo Paso a Paso

```
1. Para cada dimensión común:
   - Calcular: |valor_consumidor - valor_producto|
   
2. Sumar todas las diferencias
   
3. Dividir por el número de dimensiones (normalizar)
   
4. Invertir: acceptance = 1 - distancia_normalizada
```

#### Ejemplo Completo

**Datos:**
```
Consumidor: [0.76, 0.60, 0.40, 0.50]
Producto:   [0.60, 0.70, 0.80, 0.40]
```

**Cálculo:**
```
Dimensión 1 (PriceSensitivity):
  |0.76 - 0.60| = 0.16

Dimensión 2 (QualityExpectation):
  |0.60 - 0.70| = 0.10

Dimensión 3 (SocialRecognition):
  |0.40 - 0.80| = 0.40  ← Gran diferencia!

Dimensión 4 (EaseOfUse):
  |0.50 - 0.40| = 0.10

Suma de diferencias: 0.16 + 0.10 + 0.40 + 0.10 = 0.76
Promedio (normalizado): 0.76 / 4 = 0.19
Aceptación: 1 - 0.19 = 0.81 → 81%
```

**Interpretación:**
- 81% de compatibilidad es un **buen match**
- La dimensión `SocialRecognition` tiene la mayor diferencia (0.40)
- El consumidor busca bajo reconocimiento social (0.40) pero el producto ofrece alto (0.80)

#### ¿Por Qué Manhattan es la Mejor?

✅ **Intuitiva**: Suma simple de diferencias  
✅ **Proporcional**: Cada dimensión contribuye igualmente  
✅ **Rápida**: Operación O(n) simple  
✅ **Interpretable**: Fácil explicar a stakeholders  
✅ **Robusta**: No se afecta por outliers extremos

---

### 2️⃣ Distancia Euclidiana (L2)

La distancia euclidiana mide la línea recta en el espacio multidimensional.

#### Fórmula

```
distance = √(Σ (consumer[i] - product[i])²) / √n
acceptance = 1 - distance
```

#### Ejemplo

```
Consumidor: [0.8, 0.6]
Producto:   [0.6, 0.8]

Diferencias al cuadrado:
  (0.8 - 0.6)² = 0.04
  (0.6 - 0.8)² = 0.04

Suma: 0.08
Distancia: √0.08 = 0.283

Normalizar por máxima distancia posible:
  max_distance = √2 = 1.414
  normalized = 0.283 / 1.414 = 0.20

Aceptación: 1 - 0.20 = 0.80 → 80%
```

#### Cuándo Usar Euclidiana

- Cuando quieres **penalizar fuertemente** diferencias grandes
- El cuadrado amplifica discrepancias (0.4² = 0.16 vs 0.4 en Manhattan)
- Útil cuando ciertas dimensiones son **críticas** (ej: alergias alimentarias)

---

### 3️⃣ Similitud Coseno

Mide el ángulo entre vectores, ignorando magnitudes.

#### Fórmula

```
similarity = (A · B) / (||A|| × ||B||)

Donde:
  A · B = producto punto (dot product)
  ||A|| = magnitud del vector A
```

#### Ejemplo Visual

```
         Consumer (0.8, 0.6, 0.4)
              ↗ 
             /  ángulo pequeño ≈ 8°
            /
           /
          ↗ Product (0.9, 0.7, 0.5)
       /
      /
  Origen (0,0,0)
```

Ángulo pequeño → alta similitud → ambos priorizan las mismas dimensiones

#### Cálculo Completo

```
Consumidor: [0.8, 0.6, 0.4]
Producto:   [0.9, 0.7, 0.5]

Producto punto:
  0.8×0.9 + 0.6×0.7 + 0.4×0.5 = 0.72 + 0.42 + 0.20 = 1.34

Magnitud consumidor:
  √(0.8² + 0.6² + 0.4²) = √(0.64 + 0.36 + 0.16) = √1.16 = 1.077

Magnitud producto:
  √(0.9² + 0.7² + 0.5²) = √(0.81 + 0.49 + 0.25) = √1.55 = 1.245

Similitud coseno:
  1.34 / (1.077 × 1.245) = 1.34 / 1.341 = 0.999 → 99.9%
```

#### Cuándo Usar Coseno

- Cuando importa la **dirección** más que la magnitud
- Ejemplo: Dos consumidores buscan "calidad > precio > social" (mismo patrón)
  - Consumidor A: [0.9, 0.6, 0.3]
  - Consumidor B: [0.6, 0.4, 0.2]
  - Manhattan diría que son diferentes, pero coseno = 1.0 (mismo patrón)

---

### 4️⃣ Distancia Ponderada (Weighted)

Permite dar **más importancia** a ciertas dimensiones.

#### Ejemplo: Compra de Smartphone

```csharp
var weights = new Dictionary<DimensionDefinition, float>
{
    { pricePerceivedDef, 2.0f },     // Precio es 2x importante
    { qualityDef, 1.5f },            // Calidad es 1.5x importante
    { socialRecognitionDef, 0.5f },  // Social es 0.5x importante
    { easeOfUseDef, 1.0f }           // Facilidad es 1x (normal)
};

float score = DistanceMetric.ComputeWeightedAcceptance(
    consumer, 
    product, 
    weights
);
```

#### Cálculo Manual

```
Consumidor: [0.8, 0.6, 0.4, 0.7]
Producto:   [0.6, 0.7, 0.8, 0.6]
Pesos:      [2.0, 1.5, 0.5, 1.0]

Diferencias ponderadas:
  |0.8 - 0.6| × 2.0 = 0.2 × 2.0 = 0.40
  |0.6 - 0.7| × 1.5 = 0.1 × 1.5 = 0.15
  |0.4 - 0.8| × 0.5 = 0.4 × 0.5 = 0.20
  |0.7 - 0.6| × 1.0 = 0.1 × 1.0 = 0.10

Suma ponderada: 0.40 + 0.15 + 0.20 + 0.10 = 0.85
Peso total: 2.0 + 1.5 + 0.5 + 1.0 = 5.0
Promedio ponderado: 0.85 / 5.0 = 0.17
Aceptación: 1 - 0.17 = 0.83 → 83%
```

---

## Casos de Uso Prácticos

### Caso 1: Sistema de Recomendación

```csharp
public class ProductRecommender
{
    public List<ProductMatch> RecommendProducts(
        ConsumerProfile consumer,
        List<ProductProfile> products,
        int topN = 10)
    {
        return products
            .Select(p => new ProductMatch
            {
                Product = p,
                Score = DistanceMetric.ComputeAcceptance(consumer, p)
            })
            .OrderByDescending(m => m.Score)
            .Take(topN)
            .ToList();
    }
}

// Uso
var recommendations = recommender.RecommendProducts(
    studentConsumer,
    allSmartphones,
    topN: 5
);

foreach (var match in recommendations)
{
    Console.WriteLine($"{match.Product.Name}: {match.Score * 100:F1}%");
}

// Output:
// Xiaomi Redmi Note 12: 91.2%
// Samsung Galaxy A54: 87.5%
// Google Pixel 7a: 84.3%
// OnePlus Nord 3: 82.1%
// iPhone SE 2022: 76.8%
```

### Caso 2: Filtrado de Productos

```csharp
// Mostrar solo productos con 70%+ de compatibilidad
var acceptableProducts = allProducts
    .Where(p => DistanceMetric.IsAcceptable(consumer, p, threshold: 0.70f))
    .ToList();

Console.WriteLine($"Encontrados {acceptableProducts.Count} productos compatibles");
```

### Caso 3: Análisis Detallado

```csharp
var report = DistanceMetric.ComputeDetailedMatch(consumer, iPhonePro);

Console.WriteLine($"Compatibilidad general: {report.OverallAcceptance * 100:F1}%");
Console.WriteLine("\nMejores coincidencias:");
foreach (var match in report.GetBestMatches(3))
{
    Console.WriteLine($"  {match.Key.Name}: {match.Value * 100:F1}%");
}

Console.WriteLine("\nPeores coincidencias:");
foreach (var match in report.GetWorstMatches(3))
{
    Console.WriteLine($"  {match.Key.Name}: {match.Value * 100:F1}%");
}
```

**Output:**
```
Compatibilidad general: 78.5%

Mejores coincidencias:
  EaseOfUse: 95.0%
  Quality: 92.0%
  Innovation: 88.0%

Peores coincidencias:
  PricePerceived: 45.0%  ← Muy caro para el consumidor
  ValueForMoney: 60.0%
  Accessibility: 65.0%
```

---

## Comparación de Métodos

| Método | Complejidad | Sensibilidad a Outliers | Mejor Para | Resultado Típico |
|--------|-------------|------------------------|------------|------------------|
| **Manhattan** | O(n) | Baja | Uso general, recomendaciones | 70-90% |
| **Euclidiana** | O(n) | Alta | Cuando hay dimensiones críticas | 65-85% |
| **Coseno** | O(n) | Media | Detectar patrones similares | 80-100% |
| **Ponderada** | O(n) | Variable | Cuando unas dimensiones son más importantes | 60-95% |

---

## Interpretación de Scores

### Escala de Aceptación

| Score | Categoría | Acción Recomendada |
|-------|-----------|-------------------|
| **90-100%** | Excelente match | Recomendar fuertemente, alta probabilidad de compra |
| **75-89%** | Buen match | Recomendar, explicar pequeñas diferencias |
| **60-74%** | Match moderado | Mostrar como opción secundaria |
| **40-59%** | Match débil | No recomendar, a menos que no haya mejores opciones |
| **0-39%** | No match | Excluir de recomendaciones |

### Ejemplos Reales

```csharp
// Estudiante buscando smartphone económico
var student = CreateStudentProfile();  // PriceSensitivity = 0.9

var xiaomi = GetProduct("Xiaomi Redmi");     // Price = 0.2
var iPhone = GetProduct("iPhone 15 Pro");    // Price = 0.95

float xiaomiScore = DistanceMetric.ComputeAcceptance(student, xiaomi);
// → 92% (excelente match)

float iPhoneScore = DistanceMetric.ComputeAcceptance(student, iPhone);
// → 48% (pobre match, muy caro)
```

---

## Optimizaciones y Consideraciones

### 🚀 Performance

```csharp
// Caching para productos populares
private Dictionary<string, float> _scoreCache = new();

public float GetCachedScore(ConsumerProfile consumer, ProductProfile product)
{
    string key = $"{consumer.GetHashCode()}_{product.GetHashCode()}";
    
    if (!_scoreCache.ContainsKey(key))
    {
        _scoreCache[key] = DistanceMetric.ComputeAcceptance(consumer, product);
    }
    
    return _scoreCache[key];
}
```

### 📊 Dimensiones Faltantes

**Problema:** ¿Qué pasa si el consumidor tiene 10 dimensiones pero el producto solo 7?

**Solución actual:** Solo comparamos dimensiones comunes
```csharp
if (count == 0) return 0f;  // No hay dimensiones comunes
```

**Alternativa:** Penalizar dimensiones faltantes
```csharp
int totalDimensions = consumer.Dimensions.Count;
int missingDimensions = totalDimensions - count;
float missingPenalty = missingDimensions * 0.1f;  // 10% penalización por dimensión
return Math.Max(0f, acceptance - missingPenalty);
```

### 🎯 Thresholds Dinámicos

```csharp
public float GetDynamicThreshold(ConsumerProfile consumer)
{
    // Consumidores exigentes (alta expectativa de calidad)
    if (consumer.Get(qualityExpectationDef)?.Value > 0.8f)
        return 0.80f;  // Requieren 80%+ de match
    
    // Consumidores flexibles (baja sensibilidad)
    if (consumer.Get(priceSensitivityDef)?.Value < 0.3f)
        return 0.60f;  // Aceptan 60%+ de match
    
    return 0.70f;  // Default: 70%
}
```

---

## Machine Learning: Aprendiendo de Comportamiento Real

### Calibración con Datos Reales

```csharp
public class ScoreCalibratorML
{
    // Entrenar con datos históricos
    public void Train(List<PurchaseEvent> history)
    {
        foreach (var purchase in history)
        {
            float predictedScore = DistanceMetric.ComputeAcceptance(
                purchase.Consumer,
                purchase.Product
            );
            
            bool actualPurchase = purchase.DidBuy;
            
            // Si score = 0.85 pero NO compró, el threshold debe ser mayor
            // Si score = 0.65 pero SÍ compró, el threshold puede ser menor
            
            AdjustWeights(predictedScore, actualPurchase);
        }
    }
}
```

---

## Ventajas del Sistema de Distancia

### ✅ Objetividad Matemática
- No depende de opiniones subjetivas
- Reproducible y consistente

### ✅ Explicabilidad
- Puedes mostrar POR QUÉ un producto tiene X% de match
- Transparencia para el usuario

### ✅ Escalabilidad
- Funciona igual con 10 productos o 10,000
- O(n) para n productos

### ✅ Flexibilidad
- Múltiples métricas disponibles (Manhattan, Euclidiana, Coseno)
- Pesos ajustables según contexto

### ✅ Adaptabilidad
- Se puede mejorar con machine learning
- Ajustar pesos basados en comportamiento real

---

## Resumen: Checklist de DistanceMetric

✅ **Mide compatibilidad** entre consumidor y producto  
✅ **Manhattan es la métrica por defecto** (simple y efectiva)  
✅ **Retorna score 0-1** (0% a 100% de aceptación)  
✅ **Soporta múltiples métodos** (Manhattan, Euclidiana, Coseno, Ponderada)  
✅ **Permite análisis detallado** por dimensión  
✅ **Es rápido** (O(n) donde n = dimensiones)  
✅ **Es explicable** (puedes justificar cada score)

---

## Próximos Pasos

Con `DistanceMetric` completado, ahora puedes:

1. ✅ Definir dimensiones (`DimensionDefinition`)
2. ✅ Almacenar valores (`DimensionValue`)
3. ✅ Crear perfiles de consumidor (`ConsumerProfile`)
4. ✅ Crear perfiles de producto (`ProductProfile`)
5. ✅ Modificar productos (`MarketAction`)
6. ✅ Calcular compatibilidad (`DistanceMetric`)
7. **RecommendationEngine**: Sistema completo de recomendación
8. **SegmentationAnalyzer**: Agrupar consumidores similares
9. **ABTestingFramework**: Comparar estrategias de marketing