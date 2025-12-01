using System.Collections.Generic;

public static class MockDataFactory
{
    public static GameContext GetEcoGameContext()
    {
        GameContext ctx = new GameContext();

        // 1. DEFINIR LAS DIMENSIONES
        ctx.PriceSensitivity = new DimensionDefinition("PriceSensitivity", "Sensibilidad al precio");
        ctx.SocialRecognition = new DimensionDefinition("SocialRecognition", "Búsqueda de estatus");
        ctx.QualityExpectation = new DimensionDefinition("QualityExpectation", "Exigencia de calidad");
        ctx.EcoInterest = new DimensionDefinition("EcoInterest", "Interés ecológico");
        ctx.EaseOfUse = new DimensionDefinition("EaseOfUse", "Facilidad de uso");

        // 2. CREAR CONSUMIDOR
        ctx.Consumer = new ConsumerProfile();
        
        // Setear valores iniciales
        ctx.Consumer.Set(ctx.PriceSensitivity, 0.4f);    
        ctx.Consumer.Set(ctx.SocialRecognition, 0.6f);   
        ctx.Consumer.Set(ctx.QualityExpectation, 0.8f);  
        ctx.Consumer.Set(ctx.EcoInterest, 0.95f);        

        ctx.NombreConsumidor = "Barry Seal";
        ctx.EdadConsumidor = 30;
        ctx.Presupuesto = 1300;
        ctx.AceptacionObjetivo = 0.80f;

        // 3. CREAR PRODUCTO INICIAL
        ctx.Product = new ProductProfile();
        ctx.Product.Set(ctx.PriceSensitivity, 0.5f);   
        ctx.Product.Set(ctx.SocialRecognition, 0.3f);  
        ctx.Product.Set(ctx.QualityExpectation, 0.4f); 
        ctx.Product.Set(ctx.EcoInterest, 0.1f);        

        // 4. MAPEAR ACCIONES
        ctx.ActionsMap = new Dictionary<int, MarketAction>();
        ctx.AccionesVisuales = new List<AccionInfo>();

        // A. PRODUCCIÓN
        // Nivel 1
        CrearAccion(ctx, 1, "Empaque Reciclado", "Cartón 100% reciclado.", 150, "ATRIBUTOS_PRODUCCION", false, 0, ctx.EcoInterest, 0.15f);
        // Nivel 2
        CrearAccion(ctx, 2, "Mat. Biodegradable", "Se degrada en 30 días.", 200, "ATRIBUTOS_PRODUCCION", true, 1, ctx.EcoInterest, 0.25f);
        CrearAccion(ctx, 3, "Producción Local", "Menor huella de carbono.", 100, "ATRIBUTOS_PRODUCCION", true, 1, ctx.EcoInterest, 0.10f);
        // Nivel 3
        CrearAccion(ctx, 4, "Cert. Carbono Neutro", "Sello internacional.", 250, "ATRIBUTOS_PRODUCCION", true, 2, ctx.EcoInterest, 0.30f);

        // B. DISEÑO
        // Nivel 1
        CrearAccion(ctx, 10, "Etiqueta Verde", "Look natural.", 100, "ATRIBUTOS_DISENO", false, 0, ctx.SocialRecognition, 0.05f);
        // Nivel 2
        CrearAccion(ctx, 11, "Logo Minimalista", "Estilo moderno.", 150, "ATRIBUTOS_DISENO", true, 10, ctx.SocialRecognition, 0.10f);
        CrearAccion(ctx, 12, "Envase Ergonómico", "Fácil de agarrar.", 120, "ATRIBUTOS_DISENO", true, 10, ctx.QualityExpectation, 0.05f);

        // C. PRECIO
        // Nivel 1
        CrearAccion(ctx, 20, "Precio Estándar", "Promedio del mercado.", 50, "ATRIBUTOS_PRECIO", false, 0, ctx.PriceSensitivity, 0.02f);
        // Nivel 2
        CrearAccion(ctx, 21, "Desc. por Reciclaje", "5% si traen envase.", 80, "ATRIBUTOS_PRECIO", true, 20, ctx.EcoInterest, 0.10f);
        CrearAccion(ctx, 22, "Suscripción Mensual", "Envío automático.", 100, "ATRIBUTOS_PRECIO", true, 20, ctx.PriceSensitivity, 0.05f);

        // D. PLAZA
        // Nivel 1
        CrearAccion(ctx, 30, "Tienda Online", "Venta web directa.", 150, "ATRIBUTOS_PLAZA", false, 0, ctx.EaseOfUse, 0.10f);
        // Nivel 2
        CrearAccion(ctx, 31, "Mercados Orgánicos", "Puntos de venta eco.", 100, "ATRIBUTOS_PLAZA", true, 30, ctx.EcoInterest, 0.15f);
        CrearAccion(ctx, 32, "Apps de Delivery", "UberEats/Rappi.", 200, "ATRIBUTOS_PLAZA", true, 30, ctx.PriceSensitivity, -0.05f); // Más caro

        // E. EXPLORACIÓN
        // Nivel 1
        CrearAccion(ctx, 40, "Encuesta General", "Datos demográficos.", 100, "EXPLORACION", false, 0, null, 0); // Revela Social (200)
        // Nivel 2
        CrearAccion(ctx, 41, "Focus Group Eco", "Valores ambientales.", 200, "EXPLORACION", true, 40, null, 0); // Revela Cultural (100)
        CrearAccion(ctx, 42, "Análisis Estilo", "Hábitos de vida.", 150, "EXPLORACION", true, 40, null, 0); // Revela Personal (300)
        CrearAccion(ctx, 43, "Test Motivacional", "Impulsos de compra.", 150, "EXPLORACION", true, 40, null, 0); // Revela Psico (400)

        // F. PUBLICIDAD
        // Nivel 1
        CrearAccion(ctx, 50, "Redes Sociales", "Facebook/Instagram.", 150, "PUBLICIDAD", false, 0, ctx.SocialRecognition, 0.05f);
        // Nivel 2
        CrearAccion(ctx, 51, "Influencers Eco", "Activistas verdes.", 300, "PUBLICIDAD", true, 50, ctx.EcoInterest, 0.20f);
        CrearAccion(ctx, 52, "Google Ads", "Búsqueda pagada.", 200, "PUBLICIDAD", true, 50, ctx.PriceSensitivity, 0.02f);

        // 5. EVENTOS
        ctx.eventosPosibles = new List<Evento>();
        ctx.eventosPosibles.Add(new Evento { 
            idEvento = 1, 
            tituloNoticia = "El Mundo Se Vuelve Más Verde", 
            detalleNoticia = "Impulso global por la sostenibilidad gana fuerza." 
        });

        // 6. EFECTOS DE EVENTOS
        ctx.efectosEventos = new List<EventoEfecto>();
        ctx.efectosEventos.Add(new EventoEfecto { idEvento = 1, idSubfactor = 100, modificadorPeso = 5.0f });

        ctx.historialTurnos = new List<Turno>();

        return ctx;
    }

    // Helper para crear acciones rápido
    private static void CrearAccion(GameContext ctx, int id, string nombre, string desc, float costo, string cat, bool bloqueada, int req, DimensionDefinition dimAfectada, float impacto)
    {
        var accionLogica = new MarketAction(nombre, desc, (decimal)costo);
        if (dimAfectada != null) accionLogica.AddEffect(dimAfectada, impacto);
        
        ctx.ActionsMap.Add(id, accionLogica);
        ctx.AccionesVisuales.Add(new AccionInfo {
            idAccion = id,
            nombreAccion = nombre,
            descripcion = desc,
            costo = costo,
            categoria = cat,
            esBloqueadaInicialmente = bloqueada,
            idAccionRequerida = req
        });
    }
}