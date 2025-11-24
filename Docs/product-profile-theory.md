# ProductProfile: El Vector del Producto

## ¿Qué es un ProductProfile?

Un **ProductProfile** es la representación completa de un producto como un **vector multidimensional**. Mientras que `ConsumerProfile` describe lo que el consumidor **quiere**, `ProductProfile` describe lo que el producto **ofrece**.

### La Dualidad Consumidor-Producto

```
┌─────────────────────┐         ┌─────────────────────┐
│  ConsumerProfile    │         │  ProductProfile     │
├─────────────────────┤         ├─────────────────────┤
│  PriceSensitivity   │ ←──?──→ │  PricePerceived     │
│  QualityExpectation │ ←──?──→ │  Quality            │
│  SocialRecognition  │ ←──?──→ │  SocialRecognition  │
│  EaseOfUse          │ ←──?──→ │  EaseOfUse          │
└─────────────────────┘         └─────────────────────┘
      ¿Qué quiere?                    ¿Qué ofrece?
```

El **matching** consiste en medir qué tan bien coinciden estos dos vectores.

---

## Estructura del Perfil de Producto

### Ejemplo: iPhone 15 Pro

```
ProductProfile:
  PricePerceived      = 0.90  (Muy caro - premium pricing)
  Quality             = 0.95  (Calidad excelente)
  SocialRecognition   = 0.95  (Alto símbolo de estatus)
  EaseOfUse           = 0.90  (Muy fácil de usar)
  Innovation          = 0.85  (Tecnología de punta)
  BrandAwareness      = 0.98  (Marca muy reconocida)
  Durability          = 0.80  (Larga vida útil)
  Availability        = 0.85  (Disponible en muchas tiendas)
```

### Ejemplo: Xiaomi Redmi Note 12

```
ProductProfile:
  PricePerceived      = 0.25  (Muy económico)
  Quality             = 0.60  (Calidad decente)
  SocialRecognition   = 0.40  (Reconocimiento moderado)
  EaseOfUse           = 0.65  (Interfaz amigable)
  Innovation          = 0.55  (Tecnología estándar)
  BrandAwareness      = 0.60  (Marca conocida en ciertos mercados)
  Durability          = 0.50  (Vida útil media)
  Availability        = 0.90  (Muy disponible online)
```

---

## Mapping con las 4P del Marketing

Las dimensiones de un producto mapean directamente a las **4P** clásicas:

### 💰 **Precio (Price)**

| Dimensión | Significado | Ejemplo |
|-----------|-------------|---------|
| **PricePerceived** | Precio percibido normalizado | 0.0 = gratis/muy barato, 1.0 = muy caro |
| **ValueForMoney** | Relación calidad-precio | 0.9 = excelente valor |
| **AffordabilityIndex** | Accesibilidad económica | 0.3 = solo para segmento alto |

```csharp
// Producto económico
product.Set(pricePerceivedDef, 0.20f);
product.Set(valueForMoneyDef, 0.85f);

// Producto premium
product.Set(pricePerceivedDef, 0.95f);
product.Set(valueForMoneyDef, 0.70f);  // Caro pero justificado
```

### 📦 **Producto (Product)**

| Dimensión | Significado | Ejemplo |
|-----------|-------------|---------|
| **Quality** | Calidad percibida | 0.95 = calidad premium |
| **EaseOfUse** | Facilidad de uso | 0.90 = muy intuitivo |
| **Innovation** | Nivel de innovación | 0.85 = tecnología de punta |
| **Durability** | Durabilidad esperada | 0.80 = larga vida útil |
| **Design** | Calidad del diseño | 0.90 = diseño atractivo |

```csharp
// Producto de alta calidad
product.Set(qualityDef, 0.95f);
product.Set(innovationDef, 0.85f);
product.Set(durabilityDef, 0.90f);
```

### 📍 **Plaza/Distribución (Place)**

| Dimensión | Significado | Ejemplo |
|-----------|-------------|---------|
| **Availability** | Disponibilidad | 0.90 = fácil de conseguir |
| **OnlinePresence** | Presencia online | 0.95 = venta online fuerte |
| **PhysicalStores** | Tiendas físicas | 0.70 = disponible en tiendas selectas |
| **DeliverySpeed** | Rapidez de entrega | 0.85 = entrega rápida |

```csharp
// Producto principalmente online
product.Set(availabilityDef, 0.95f);
product.Set(onlinePresenceDef, 0.98f);
product.Set(physicalStoresDef, 0.30f);
```

### 📢 **Promoción (Promotion)**

| Dimensión | Significado | Ejemplo |
|-----------|-------------|---------|
| **BrandAwareness** | Reconocimiento de marca | 0.98 = marca muy conocida |
| **SocialRecognition** | Estatus social | 0.95 = símbolo de estatus |
| **MarketingReach** | Alcance publicitario | 0.85 = mucha publicidad |
| **WordOfMouth** | Boca a boca | 0.75 = recomendado por usuarios |

```csharp
// Marca premium con alta visibilidad
product.Set(brandAwarenessDef, 0.98f);
product.Set(socialRecognitionDef, 0.95f);
product.Set(marketingReachDef, 0.90f);
```

---

## Acciones de Mercado: Modificando el Perfil

Una característica clave de `ProductProfile` es que puede ser **modificado dinámicamente** mediante acciones de mercado.

### Tipos de Acciones

```
┌────────────────────────────────────────────────┐
│         MarketAction (Abstract)                │
├────────────────────────────────────────────────┤
│  + Apply(ProductProfile)                       │
└────────────────┬───────────────────────────────┘
                 │
        ┌────────┴────────────────┬───────────────┐
        │                         │               │
┌───────▼────────┐   ┌───────────▼─────┐  ┌──────▼──────┐
│  PromotionAction│   │ CampaignAction  │  │ EventAction │
├─────────────────┤   ├─────────────────┤  ├─────────────┤
│ - Discounts     │   │ - Advertising   │  │ - Seasonal  │
│ - Offers        │   │ - Sponsorships  │  │ - Crisis    │
│ - Bundles       │   │ - PR            │  │ - Trends    │
└─────────────────┘   └─────────────────┘  └─────────────┘
```

### Ejemplo 1: Promoción de Descuento (Black Friday)

```csharp
public class DiscountAction : MarketAction
{
    private readonly float _discountPercentage;

    public DiscountAction(float discountPercentage)
    {
        _discountPercentage = discountPercentage;
    }

    public override void Apply(ProductProfile product)
    {
        // Reducir precio percibido
        product.Adjust(pricePerceivedDef, -_discountPercentage);
        
        // Aumentar percepción de value for money
        product.Adjust(valueForMoneyDef, 0.15f);
    }
}

// Uso
var iPhone = GetProduct("iPhone 15 Pro");
var blackFriday = new DiscountAction(0.20f);  // 20% descuento
iPhone.ApplyAction(blackFriday);

// Antes: PricePerceived = 0.90
// Después: PricePerceived = 0.70 (más asequible)
```

### Ejemplo 2: Campaña Publicitaria (Super Bowl Ad)

```csharp
public class AdvertisingCampaign : MarketAction
{
    private readonly string _campaignName;
    private readonly float _reach;

    public AdvertisingCampaign(string name, float reach)
    {
        _campaignName = name;
        _reach = reach;
    }

    public override void Apply(ProductProfile product)
    {
        // Aumentar reconocimiento de marca
        product.Adjust(brandAwarenessDef, _reach * 0.3f);
        
        // Aumentar reconocimiento social
        product.Adjust(socialRecognitionDef, _reach * 0.2f);
        
        // Aumentar alcance de marketing
        product.Adjust(marketingReachDef, _reach * 0.4f);
    }
}

// Uso
var tesla = GetProduct("Tesla Model 3");
var superBowl = new AdvertisingCampaign("Super Bowl LVIII", 0.95f);
tesla.ApplyAction(superBowl);

// Incrementa visibilidad y estatus social
```

### Ejemplo 3: Evento de Mercado (Crisis de Suministro)

```csharp
public class SupplyChainCrisis : MarketAction
{
    public override void Apply(ProductProfile product)
    {
        // Reducir disponibilidad
        product.Adjust(availabilityDef, -0.30f);
        
        // Aumentar precio percibido (escasez)
        product.Adjust(pricePerceivedDef, 0.15f);
        
        // Reducir velocidad de entrega
        product.Adjust(deliverySpeedDef, -0.25f);
    }
}

// Uso durante pandemia
var gpu = GetProduct("NVIDIA RTX 4090");
var crisis = new SupplyChainCrisis();
gpu.ApplyAction(crisis);
```

### Ejemplo 4: Mejora de Producto (Update/Upgrade)

```csharp
public class ProductUpgrade : MarketAction
{
    private readonly string _upgradeName;

    public ProductUpgrade(string upgradeName)
    {
        _upgradeName = upgradeName;
    }

    public override void Apply(ProductProfile product)
    {
        // Mejorar calidad
        product.Adjust(qualityDef, 0.10f);
        
        // Aumentar innovación
        product.Adjust(innovationDef, 0.15f);
        
        // Leve aumento en precio percibido
        product.Adjust(pricePerceivedDef, 0.05f);
    }
}

// Uso: iPhone lanza nuevo modelo
var iPhone = GetProduct("iPhone");
var iOS18 = new ProductUpgrade("iOS 18 + AI Features");
iPhone.ApplyAction(iOS18);
```

---

## Casos de Uso Reales

### Caso 1: Comparación de Smartphones

```csharp
// Samsung Galaxy S24 Ultra
var samsung = new ProductProfile();
samsung.Set(pricePerceivedDef, 0.85f);
samsung.Set(qualityDef, 0.90f);
samsung.Set(socialRecognitionDef, 0.75f);
samsung.Set(innovationDef, 0.88f);
samsung.Set(easeOfUseDef, 0.70f);

// iPhone 15 Pro Max
var iPhone = new ProductProfile();
iPhone.Set(pricePerceivedDef, 0.95f);
iPhone.Set(qualityDef, 0.95f);
iPhone.Set(socialRecognitionDef, 0.95f);
iPhone.Set(innovationDef, 0.85f);
iPhone.Set(easeOfUseDef, 0.92f);

// Google Pixel 8 Pro
var pixel = new ProductProfile();
pixel.Set(pricePerceivedDef, 0.75f);
pixel.Set(qualityDef, 0.85f);
pixel.Set(socialRecognitionDef, 0.60f);
pixel.Set(innovationDef, 0.90f);  // Mejor IA
pixel.Set(easeOfUseDef, 0.80f);
```

### Caso 2: Estrategia de Lanzamiento

```csharp
// Producto nuevo: Tesla Cybertruck
var cybertruck = new ProductProfile();

// Fase 1: Pre-lanzamiento
cybertruck.Set(pricePerceivedDef, 0.70f);
cybertruck.Set(innovationDef, 0.95f);
cybertruck.Set(brandAwarenessDef, 0.50f);  // Marca conocida pero producto nuevo

// Fase 2: Campaña de marketing
var campaign = new AdvertisingCampaign("Launch Event", 0.90f);
cybertruck.ApplyAction(campaign);
// BrandAwareness aumenta a ~0.77

// Fase 3: Reviews positivas
var positiveReviews = new ReviewImpact(0.85f);
cybertruck.ApplyAction(positiveReviews);
// Quality y SocialRecognition aumentan

// Fase 4: Producción masiva
var massProduction = new ScaleProduction();
cybertruck.ApplyAction(massProduction);
// Availability aumenta, PricePerceived baja ligeramente
```

### Caso 3: Combate Competitivo

```csharp
// McDonald's vs Burger King
var mcdonalds = new ProductProfile();
mcdonalds.Set(pricePerceivedDef, 0.30f);
mcdonalds.Set(qualityDef, 0.55f);
mcdonalds.Set(brandAwarenessDef, 0.95f);
mcdonalds.Set(availabilityDef, 0.98f);

var burgerKing = new ProductProfile();
burgerKing.Set(pricePerceivedDef, 0.28f);
burgerKing.Set(qualityDef, 0.60f);
burgerKing.Set(brandAwarenessDef, 0.80f);
burgerKing.Set(availabilityDef, 0.85f);

// Burger King lanza promoción agresiva
var whopper = new PromotionAction("Whopper Day", -0.10f);
burgerKing.ApplyAction(whopper);
// PricePerceived baja a 0.18, atrae consumidores sensibles al precio
```

---

## Simulación: Impacto de Acciones

### Predecir Efectos Antes de Ejecutar

```csharp
public class MarketSimulator
{
    public SimulationResult SimulateAction(
        ProductProfile product,
        MarketAction action,
        ConsumerProfile[] targetAudience)
    {
        // Clonar para no modificar original
        var simulation = product.Clone();
        
        // Aplicar acción
        simulation.ApplyAction(action);
        
        // Calcular impacto en ventas
        var beforeMatches = CalculateMatches(product, targetAudience);
        var afterMatches = CalculateMatches(simulation, targetAudience);
        
        return new SimulationResult
        {
            OriginalScore = beforeMatches.Average(),
            ProjectedScore = afterMatches.Average(),
            Improvement = afterMatches.Average() - beforeMatches.Average(),
            RecommendedAction = afterMatches.Average() > beforeMatches.Average()
        };
    }
}

// Uso
var simulator = new MarketSimulator();
var result = simulator.SimulateAction(
    myProduct,
    new DiscountAction(0.25f),
    targetConsumers
);

Console.WriteLine($"Mejora proyectada: {result.Improvement * 100:F1}%");
Console.WriteLine($"Recomendado: {result.RecommendedAction}");
```

---

## Diferencias Clave: ConsumerProfile vs ProductProfile

| Aspecto | ConsumerProfile | ProductProfile |
|---------|----------------|----------------|
| **Representa** | Lo que el consumidor quiere | Lo que el producto ofrece |
| **Origen** | Generado por RuleEngine desde datos del usuario | Definido por equipo de producto/marketing |
| **Estabilidad** | Evoluciona con el tiempo (compras, comportamiento) | Se modifica con acciones de mercado |
| **Dimensiones típicas** | PriceSensitivity, QualityExpectation | PricePerceived, Quality |
| **Uso** | Input para matching | Input para matching |
| **Modificación** | Por comportamiento del usuario | Por estrategias de marketing |

---

## Patrones de Diseño Aplicados

### 🔄 **Strategy Pattern** (MarketAction)
```csharp
// Diferentes estrategias de modificación
product.ApplyAction(new DiscountAction(0.20f));
product.ApplyAction(new CampaignAction(...));
product.ApplyAction(new EventAction(...));
```

### 🎭 **Prototype Pattern** (Clone)
```csharp
// Simular sin modificar original
var simulation = product.Clone();
simulation.ApplyAction(riskyAction);
```

### 📝 **Command Pattern** (Historial de acciones)
```csharp
public class ActionHistory
{
    private List<MarketAction> _history = new();
    
    public void Execute(ProductProfile product, MarketAction action)
    {
        action.Apply(product);
        _history.Add(action);
    }
    
    public void Undo(ProductProfile product)
    {
        // Revertir última acción
    }
}
```

---

## Ventajas del Enfoque de Perfil de Producto

### ✅ Flexibilidad Estratégica
- Probar diferentes escenarios sin riesgo
- Simular impacto de promociones antes de lanzarlas

### ✅ Coherencia con Consumidor
- Mismo framework dimensional → matching directo
- Fácil calcular compatibilidad

### ✅ Trazabilidad
- Historial de acciones aplicadas
- Entender por qué un producto funciona mejor

### ✅ Escalabilidad
- Agregar nuevas dimensiones sin cambiar lógica
- Funciona igual para 5 productos que para 5000

### ✅ Transparencia
- Explicar por qué se recomienda un producto
- Mostrar qué dimensiones coinciden

---

## Resumen: Checklist de ProductProfile

✅ **Representa lo que el producto ofrece** (contraparte de ConsumerProfile)  
✅ **Usa las mismas dimensiones** que los consumidores (para matching)  
✅ **Mapea a las 4P del marketing** (Producto, Precio, Plaza, Promoción)  
✅ **Soporta acciones de mercado** (promociones, campañas, eventos)  
✅ **Permite clonación** para simulaciones sin riesgo  
✅ **Es modificable dinámicamente** según estrategia de marketing  
✅ **Facilita comparación matemática** con perfiles de consumidor

---

## Próximos Pasos

Con `ProductProfile` completado, el sistema tiene ambos lados de la ecuación:

1. ✅ Definir dimensiones (`DimensionDefinition`)
2. ✅ Almacenar valores (`DimensionValue`)
3. ✅ Crear perfiles de consumidor (`ConsumerProfile`)
4. ✅ Crear perfiles de producto (`ProductProfile`)
5. **MatchCalculator**: Calcular compatibilidad entre consumidor y producto
6. **MarketAction**: Implementar acciones concretas (promociones, campañas)
7. **RecommendationEngine**: Motor de recomendaciones basado en matching