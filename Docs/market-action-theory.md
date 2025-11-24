# MarketAction: Acciones que Cambian el Producto

## ¿Qué es un MarketAction?

Un **MarketAction** es una acción de negocio que modifica las dimensiones de un producto. Representa estrategias reales de marketing como promociones, campañas publicitarias, eventos o cambios de posicionamiento.

### Analogía: Palancas de Control
Piensa en las dimensiones del producto como diales que puedes ajustar. Cada `MarketAction` es una combinación de movimientos de estos diales para lograr un objetivo de negocio.

```
┌────────────────────────────────────────┐
│        MarketAction                    │
│  "Black Friday Sale"                   │
├────────────────────────────────────────┤
│  Effects:                              │
│    PricePerceived:    -0.30  ⬇️        │
│    ValueForMoney:     +0.20  ⬆️        │
│    Urgency:           +0.40  ⬆️        │
├────────────────────────────────────────┤
│  Cost: $50,000                         │
│  Expected Revenue: $200,000            │
│  ROI: 300%                             │
└────────────────────────────────────────┘
```

---

## Estructura de una MarketAction

### Componentes Clave

```
┌─────────────────────────────────────────────┐
│         MarketAction                        │
├─────────────────────────────────────────────┤
│  Name:        "Holiday Discount"            │
│  Description: "30% off for Christmas"       │
│  Cost:        $25,000                       │
├─────────────────────────────────────────────┤
│  Effects: Dictionary<Dimension, Delta>      │
│    ├─ PricePerceived      → -0.30          │
│    ├─ BrandAwareness       → +0.10          │
│    └─ Urgency              → +0.25          │
└─────────────────────────────────────────────┘
```

### Deltas: Cambios Positivos y Negativos

Los **deltas** pueden ser:
- **Positivos (+)**: Aumentan la dimensión
  - `+0.20` en BrandAwareness → Mayor reconocimiento de marca
  - `+0.15` en SocialRecognition → Más prestigio social
  
- **Negativos (-)**: Disminuyen la dimensión
  - `-0.30` en PricePerceived → Precio más bajo percibido
  - `-0.10` en Exclusivity → Menos exclusivo (más masivo)

---

## Ejemplos Prácticos de MarketActions

### 1️⃣ Promoción: Black Friday Sale

```csharp
var blackFriday = new MarketAction(
    name: "Black Friday Sale",
    description: "Annual 50% discount on electronics",
    cost: 50000m  // $50,000 presupuesto
);

// Efectos de la promoción
blackFriday.AddEffect(pricePerceivedDef, -0.30f);    // Precio 30% más bajo percibido
blackFriday.AddEffect(valueForMoneyDef, 0.20f);      // Mejor relación calidad-precio
blackFriday.AddEffect(urgencyDef, 0.40f);            // Alta urgencia (oferta limitada)
blackFriday.AddEffect(availabilityDef, -0.15f);      // Menor disponibilidad (alta demanda)

// Aplicar al producto
var iPhone = GetProduct("iPhone 15 Pro");
blackFriday.Apply(iPhone);
```

**Interpretación:**
- Precio percibido baja significativamente → atrae consumidores sensibles al precio
- Valor por dinero sube → percepción de "buena oferta"
- Urgencia aumenta → incentiva compra inmediata
- Disponibilidad baja → efecto de escasez (FOMO)

### 2️⃣ Campaña Publicitaria: Super Bowl Ad

```csharp
var superBowlAd = new MarketAction(
    name: "Super Bowl LVIII Commercial",
    description: "30-second prime time advertisement",
    cost: 7000000m  // $7 millones (costo real de anuncio Super Bowl)
);

// Efectos de exposición masiva
superBowlAd.AddEffect(brandAwarenessDef, 0.40f);       // Reconocimiento masivo
superBowlAd.AddEffect(socialRecognitionDef, 0.25f);    // Mayor prestigio
superBowlAd.AddEffect(marketingReachDef, 0.50f);       // Alcance enorme
superBowlAd.AddEffect(trustDef, 0.15f);                // Confianza (inversión grande = empresa seria)

var tesla = GetProduct("Tesla Model Y");
superBowlAd.Apply(tesla);
```

**ROI esperado:**
```csharp
decimal projectedRevenue = 25000000m;  // $25M en ventas adicionales
decimal roi = superBowlAd.CalculateROI(projectedRevenue);
// ROI = (25M - 7M) / 7M = 2.57 = 257%
```

### 3️⃣ Evento de Mercado: Crisis de Suministro

```csharp
var supplyChainCrisis = new MarketAction(
    name: "Chip Shortage 2023",
    description: "Global semiconductor shortage affecting production",
    cost: 0m  // Evento externo, sin costo directo
);

// Efectos negativos del evento
supplyChainCrisis.AddEffect(availabilityDef, -0.40f);     // Escasez severa
supplyChainCrisis.AddEffect(pricePerceivedDef, 0.20f);    // Precios suben
supplyChainCrisis.AddEffect(deliverySpeedDef, -0.30f);    // Entregas lentas
supplyChainCrisis.AddEffect(customerSatisfactionDef, -0.15f);  // Frustración

var gpu = GetProduct("NVIDIA RTX 4090");
supplyChainCrisis.Apply(gpu);
```

### 4️⃣ Actualización de Producto: iOS 18 Release

```csharp
var iOS18Update = new MarketAction(
    name: "iOS 18 Major Update",
    description: "AI-powered features and performance improvements",
    cost: 500000m  // Costo de desarrollo y marketing del update
);

// Mejoras percibidas
iOS18Update.AddEffect(innovationDef, 0.25f);        // Más innovador
iOS18Update.AddEffect(easeOfUseDef, 0.15f);         // Más fácil de usar (mejor UX)
iOS18Update.AddEffect(qualityDef, 0.10f);           // Calidad mejorada
iOS18Update.AddEffect(technologyDef, 0.30f);        // Tecnología de punta (IA)

var iPhone = GetProduct("iPhone 15");
iOS18Update.Apply(iPhone);
```

### 5️⃣ Reposicionamiento: De Económico a Premium

```csharp
var premiumRepositioning = new MarketAction(
    name: "Premium Brand Repositioning",
    description: "Shift from budget brand to premium positioning",
    cost: 2000000m  // Campaña integral de rebranding
);

// Transformación de marca
premiumRepositioning.AddEffect(pricePerceivedDef, 0.35f);       // Precios más altos
premiumRepositioning.AddEffect(qualityDef, 0.40f);              // Calidad percibida sube
premiumRepositioning.AddEffect(socialRecognitionDef, 0.30f);    // Más prestigioso
premiumRepositioning.AddEffect(exclusivityDef, 0.25f);          // Más exclusivo
premiumRepositioning.AddEffect(valueForMoneyDef, -0.10f);       // Menos "ganga"

var xiaomi = GetProduct("Xiaomi Mi 14");
premiumRepositioning.Apply(xiaomi);
```

### 6️⃣ Colaboración: Influencer Partnership

```csharp
var influencerCampaign = new MarketAction(
    name: "Mr. Beast Product Review",
    description: "Sponsored review by top YouTube creator",
    cost: 500000m  // Fee del influencer
);

// Efectos de endorsement
influencerCampaign.AddEffect(brandAwarenessDef, 0.35f);      // Alcance millonario
influencerCampaign.AddEffect(trustDef, 0.20f);               // Credibilidad por asociación
influencerCampaign.AddEffect(youthAppealDef, 0.40f);         // Atractivo para jóvenes
influencerCampaign.AddEffect(viralityDef, 0.50f);            // Potencial viral

var feastables = GetProduct("Feastables Chocolate Bar");
influencerCampaign.Apply(feastables);
```

---

## Categorías de MarketActions

### 📉 **Promociones (Price-Focused)**
| Acción | Efecto Principal | Ejemplo |
|--------|------------------|---------|
| Flash Sale | PricePerceived ⬇️, Urgency ⬆️ | "24 horas: 40% off" |
| Bundle Discount | ValueForMoney ⬆️ | "Compra 2, lleva 3" |
| Loyalty Discount | BrandLoyalty ⬆️, PricePerceived ⬇️ | "15% para miembros" |
| Clearance Sale | PricePerceived ⬇️⬇️, Quality ⬇️ | "Liquidación final" |

### 📢 **Campañas Publicitarias (Awareness-Focused)**
| Acción | Efecto Principal | Ejemplo |
|--------|------------------|---------|
| TV Commercial | BrandAwareness ⬆️⬆️ | Anuncio prime time |
| Social Media Campaign | SocialRecognition ⬆️ | Campaña Instagram |
| Billboard Advertising | BrandAwareness ⬆️ | Vallas en Times Square |
| Podcast Sponsorship | Trust ⬆️, Niche Appeal ⬆️ | "Brought to you by..." |

### 🎯 **Eventos Especiales (Time-Limited)**
| Acción | Efecto Principal | Ejemplo |
|--------|------------------|---------|
| Product Launch | Innovation ⬆️⬆️, Hype ⬆️ | Apple Keynote |
| Pop-Up Store | Exclusivity ⬆️ | Tienda temporal NYC |
| Seasonal Campaign | Urgency ⬆️, Relevance ⬆️ | Campaña navideña |
| Anniversary Sale | BrandLoyalty ⬆️, Value ⬆️ | "10 años celebrando" |

### 🔄 **Mejoras de Producto (Quality-Focused)**
| Acción | Efecto Principal | Ejemplo |
|--------|------------------|---------|
| Feature Update | Innovation ⬆️, Quality ⬆️ | Nueva versión software |
| Quality Improvement | Quality ⬆️⬆️ | Mejores materiales |
| Sustainability Initiative | EcoFriendliness ⬆️⬆️ | Packaging reciclable |
| Design Refresh | Aesthetics ⬆️ | Rediseño visual |

### ⚠️ **Eventos Externos (Incontrolables)**
| Acción | Efecto Principal | Ejemplo |
|--------|------------------|---------|
| Negative PR | Trust ⬇️⬇️, BrandAwareness ⬇️ | Escándalo público |
| Supply Chain Disruption | Availability ⬇️⬇️, Price ⬆️ | Crisis logística |
| Competitor Launch | Relevance ⬇️ | Competidor mejor |
| Economic Recession | PriceSensitivity ⬆️ | Crisis económica |

---

## Simulación: Testear Antes de Ejecutar

Una característica clave es poder **simular** acciones sin modificar el producto real:

```csharp
public class MarketActionSimulator
{
    public SimulationReport SimulateAction(
        ProductProfile product,
        MarketAction action,
        ConsumerProfile[] targetAudience)
    {
        // 1. Simular aplicación
        var simulatedProduct = action.Simulate(product);
        
        // 2. Calcular impacto en matching
        var beforeScores = CalculateMatchScores(product, targetAudience);
        var afterScores = CalculateMatchScores(simulatedProduct, targetAudience);
        
        // 3. Estimar cambio en ventas
        var beforeSales = EstimateSales(beforeScores);
        var afterSales = EstimateSales(afterScores);
        var projectedRevenue = afterSales - beforeSales;
        
        // 4. Calcular ROI
        var roi = action.CalculateROI(projectedRevenue);
        
        return new SimulationReport
        {
            ActionName = action.Name,
            Cost = action.Cost,
            ProjectedRevenue = projectedRevenue,
            ROI = roi,
            RecommendExecution = roi > 0.5m,  // ROI > 50%
            BeforeAvgMatch = beforeScores.Average(),
            AfterAvgMatch = afterScores.Average(),
            ImpactPerDimension = CalculateImpactPerDimension(action)
        };
    }
}

// Uso
var simulator = new MarketActionSimulator();
var report = simulator.SimulateAction(
    myProduct,
    blackFridayPromotion,
    targetConsumers
);

Console.WriteLine($"Acción: {report.ActionName}");
Console.WriteLine($"Costo: ${report.Cost:N0}");
Console.WriteLine($"Ingreso proyectado: ${report.ProjectedRevenue:N0}");
Console.WriteLine($"ROI: {report.ROI * 100:F1}%");
Console.WriteLine($"¿Ejecutar? {report.RecommendExecution}");
```

---

## Combinación de Acciones: Campañas Integradas

Puedes aplicar **múltiples acciones** para crear campañas complejas:

```csharp
// Campaña de lanzamiento de iPhone 16
var launchCampaign = new List<MarketAction>
{
    // 1. Pre-lanzamiento: Generar hype
    new MarketAction("Teaser Campaign", "...", 1000000m)
        .WithEffect(anticipationDef, 0.60f)
        .WithEffect(brandAwarenessDef, 0.30f),
    
    // 2. Evento de lanzamiento
    new MarketAction("Apple Keynote", "...", 5000000m)
        .WithEffect(innovationDef, 0.50f)
        .WithEffect(socialRecognitionDef, 0.40f)
        .WithEffect(mediaReachDef, 0.70f),
    
    // 3. Promoción de pre-orden
    new MarketAction("Pre-Order Bonus", "...", 500000m)
        .WithEffect(urgencyDef, 0.40f)
        .WithEffect(valueForMoneyDef, 0.15f),
    
    // 4. Campaña publicitaria continua
    new MarketAction("TV + Digital Ads", "...", 3000000m)
        .WithEffect(brandAwarenessDef, 0.35f)
        .WithEffect(desirabilityDef, 0.25f)
};

// Aplicar secuencialmente
var iPhone16 = new ProductProfile();
foreach (var action in launchCampaign)
{
    action.Apply(iPhone16);
}

// Costo total: $9.5M
// Efecto acumulado: brandAwareness = 0.65, innovation = 0.50, etc.
```

---

## Patrones de Uso Avanzado

### 🔁 Pattern 1: A/B Testing

```csharp
// Testear dos estrategias diferentes
var strategyA = new MarketAction("Aggressive Discount", "...", 100000m)
    .WithEffect(pricePerceivedDef, -0.40f);

var strategyB = new MarketAction("Premium Positioning", "...", 200000m)
    .WithEffect(qualityDef, 0.30f)
    .WithEffect(exclusivityDef, 0.25f);

// Simular ambas
var resultA = SimulateStrategy(strategyA, targetAudience);
var resultB = SimulateStrategy(strategyB, targetAudience);

// Elegir la mejor
var winner = resultA.ROI > resultB.ROI ? strategyA : strategyB;
```

### 📊 Pattern 2: Budget Optimization

```csharp
// Encontrar la mejor combinación de acciones con presupuesto limitado
public List<MarketAction> OptimizeBudget(
    List<MarketAction> availableActions,
    decimal totalBudget)
{
    // Ordenar por ROI esperado
    var sorted = availableActions
        .Select(a => new {
            Action = a,
            ROI = EstimateROI(a)
        })
        .OrderByDescending(x => x.ROI)
        .ToList();
    
    // Seleccionar hasta agotar presupuesto
    var selected = new List<MarketAction>();
    decimal spent = 0m;
    
    foreach (var item in sorted)
    {
        if (spent + item.Action.Cost <= totalBudget)
        {
            selected.Add(item.Action);
            spent += item.Action.Cost;
        }
    }
    
    return selected;
}
```

### 🎯 Pattern 3: Targeted Actions

```csharp
// Crear acciones específicas para segmentos
public MarketAction CreateTargetedAction(ConsumerSegment segment)
{
    if (segment.AvgPriceSensitivity > 0.7f)
    {
        // Segmento sensible al precio
        return new MarketAction("Budget Promotion", "...", 50000m)
            .WithEffect(pricePerceivedDef, -0.30f);
    }
    else if (segment.AvgQualityExpectation > 0.8f)
    {
        // Segmento premium
        return new MarketAction("Quality Showcase", "...", 100000m)
            .WithEffect(qualityDef, 0.25f)
            .WithEffect(exclusivityDef, 0.20f);
    }
    
    // Segmento general
    return new MarketAction("Balanced Campaign", "...", 75000m)
        .WithEffect(brandAwarenessDef, 0.20f);
}
```

---

## Ciclo de Vida de una MarketAction

```
1. PLANNING                2. SIMULATION           3. EXECUTION
┌─────────────┐           ┌──────────────┐        ┌──────────────┐
│ Define      │           │ Test impact  │        │ Apply to     │
│ effects     │──────────▶│ Calculate    │───────▶│ product      │
│ Set budget  │           │ ROI          │        │ Track results│
└─────────────┘           └──────────────┘        └──────────────┘
                                 │                        │
                                 │ ROI < threshold?       │
                                 ▼                        ▼
                          ┌──────────────┐        ┌──────────────┐
                          │ REJECT       │        │ 4. ANALYSIS  │
                          │ Don't execute│        │ Measure real │
                          └──────────────┘        │ impact       │
                                                  └──────────────┘
                                                         │
                                                         ▼
                                                  ┌──────────────┐
                                                  │ 5. LEARNING  │
                                                  │ Adjust model │
                                                  │ for future   │
                                                  └──────────────┘
```

---

## Ventajas del Sistema de MarketActions

### ✅ Flexibilidad
- Crear cualquier tipo de acción sin codificar casos específicos
- Combinar acciones para campañas complejas

### ✅ Simulación Sin Riesgo
- Testear estrategias antes de gastar presupuesto
- Comparar múltiples opciones (A/B testing)

### ✅ Trazabilidad
- Historial de acciones aplicadas
- Entender qué causó cada cambio en el producto

### ✅ Optimización de ROI
- Calcular retorno antes de ejecutar
- Priorizar acciones con mejor relación costo-beneficio

### ✅ Escalabilidad
- Funciona igual para 1 producto que para 1000
- Fácil agregar nuevos tipos de acciones

---

## Resumen: Checklist de MarketAction

✅ **Representa acciones de marketing reales** (promociones, campañas, eventos)  
✅ **Modifica dimensiones del producto** mediante deltas (+/-)  
✅ **Incluye costo** para análisis de ROI  
✅ **Permite simulación** sin modificar el producto original  
✅ **Soporta combinación** de múltiples acciones  
✅ **Facilita optimización** de presupuesto y estrategia  
✅ **Es trazable** (se puede hacer log de todas las acciones aplicadas)

---

## Próximos Pasos

Con `MarketAction` completado:

1. ✅ Definir dimensiones (`DimensionDefinition`)
2. ✅ Almacenar valores (`DimensionValue`)
3. ✅ Crear perfiles de consumidor (`ConsumerProfile`)
4. ✅ Crear perfiles de producto (`ProductProfile`)
5. ✅ Modificar productos con acciones (`MarketAction`)
6. **MatchCalculator**: Calcular compatibilidad consumidor-producto
7. **ActionOptimizer**: Encontrar la mejor combinación de acciones
8. **CampaignPlanner**: Planificar campañas integradas multi-acción