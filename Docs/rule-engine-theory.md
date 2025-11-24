# RuleEngine — De documentos a dimensiones

El **RuleEngine** es la “receta” que transforma datos reales en números interpretables por el sistema psicológico o de producto. Su función principal es tomar información objetiva (edad, ingreso, ciudad, rasgos de personalidad…), procesarla mediante reglas y generar **dimensiones normalizadas** entre 0 y 1.

---

## 1. Idea fundamental

Una *dimensión* es un valor numérico que representa un rasgo psicológico o preferencia relevante para el consumidor o el producto.
Ejemplos:

* **PriceSensitivity** (sensibilidad al precio)
* **SocialRecognition** (necesidad de reconocimiento social)
* **QualityExpectation** (expectativa de calidad)

El RuleEngine aplica **fórmulas** que convierten datos en esos valores.

---

## 2. Fuentes típicas de datos

* **DNI** → edad, ciudad
* **Payroll / rol de pagos** → ingreso mensual, antigüedad
* **Big Five** → apertura, extraversión, responsabilidad, amabilidad, neuroticismo

Cada fuente puede alimentar distintas dimensiones.

---

## 3. Ejemplos de reglas (fórmulas)

### ✔ PriceSensitivity

```
PriceSensitivity = 1 - clamp(income / 5000)
```

Si ingreso = 1200:
1 − (1200 / 5000) = 0.76 → alta sensibilidad al precio.

---

### ✔ SocialRecognition

```
SocialRecognition = extraversion * 0.7 
                    + (1 - ageFactor) * 0.3

donde ageFactor = edad / 100
```

Personas jóvenes suelen buscar más reconocimiento social relativo.

---

### ✔ QualityExpectation

```
QualityExpectation = conscientiousness
```

Personas responsables tienden a valorar más la calidad.

---

## 4. Nota técnica — clamp(x)

`clamp(x)` significa:

* si x < 0 → 0
* si x > 1 → 1
* si no → x

Sirve para garantizar que todos los valores estén en el rango [0, 1].

---

## 5. Por qué NO usar if/else por dimensión

**Mala práctica:**

```
if (dimension == "PriceSensitivity") { ... }
else if (dimension == "SocialRecognition") { ... }
```

Esto vuelve el sistema rígido y difícil de extender.

**Buena práctica:**

* Cada dimensión define *su propia regla* de cálculo.
* O las reglas se registran en un contenedor (polimorfismo / estrategia).

Resultado:
➜ Se puede agregar una nueva dimensión sin tocar el motor central.
➜ El sistema crece sin romper código existente.

---

## 6. Beneficio del RuleEngine

* Extensible
* Fácil de mantener
* Independiente de documentos específicos
* Permite simular perfiles psicológicos y preferencias de forma coherente y escalable

Es, en resumen, el puente entre datos reales y el modelo psicológico/dimensional del sistema.
