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

        // 2. CREAR CONSUMIDOR
        ctx.Consumer = new ConsumerProfile();
        
        // Setear valores iniciales
        ctx.Consumer.Set(ctx.PriceSensitivity, 0.4f);    
        ctx.Consumer.Set(ctx.SocialRecognition, 0.6f);   
        ctx.Consumer.Set(ctx.QualityExpectation, 0.8f);  
        ctx.Consumer.Set(ctx.EcoInterest, 0.95f);        

        ctx.NombreConsumidor = "Barry Seal";
        ctx.EdadConsumidor = 30;
        ctx.Presupuesto = 1000;
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
        
        // Acción 1: Empaque Reciclado
        var actEmpaque = new MarketAction("Empaque Reciclado", "Usa cartón reciclado", 150m);
        actEmpaque.AddEffect(ctx.EcoInterest, 0.30f);       
        actEmpaque.AddEffect(ctx.PriceSensitivity, 0.05f);  
        
        AgregarAccion(ctx, 1, actEmpaque, "ATRIBUTOS_PRODUCCION", false, 0);

        // Acción 2: Material Biodegradable
        var actBio = new MarketAction("Mat. Biodegradable", "Se descompone rápido", 200m);
        actBio.AddEffect(ctx.EcoInterest, 0.40f);           
        actBio.AddEffect(ctx.QualityExpectation, 0.10f);    
        
        AgregarAccion(ctx, 2, actBio, "ATRIBUTOS_PRODUCCION", true, 1);

        // B. DISEÑO
        var actEtiqueta = new MarketAction("Etiqueta Verde", "Look natural", 100m);
        actEtiqueta.AddEffect(ctx.SocialRecognition, 0.10f);
        actEtiqueta.AddEffect(ctx.EcoInterest, 0.05f);       
        AgregarAccion(ctx, 4, actEtiqueta, "ATRIBUTOS_DISENO", false, 0);

        // C. EXPLORACIÓN (Lógica visual)
        var actEncuesta = new MarketAction("Encuesta General", "Datos básicos", 100m);
        AgregarAccion(ctx, 13, actEncuesta, "EXPLORACION", false, 0);

        // D. PUBLICIDAD
        var actRedes = new MarketAction("Redes Sociales", "Ads básicos", 150m);
        actRedes.AddEffect(ctx.SocialRecognition, 0.15f);
        AgregarAccion(ctx, 17, actRedes, "PUBLICIDAD", false, 0);

        return ctx;
    }

    private static void AgregarAccion(GameContext ctx, int id, MarketAction logicAction, string cat, bool bloqueada, int req)
    {
        ctx.ActionsMap.Add(id, logicAction);
        ctx.AccionesVisuales.Add(new AccionInfo {
            idAccion = id,
            nombreAccion = logicAction.Name,
            descripcion = logicAction.Description,
            costo = (float)logicAction.Cost,
            categoria = cat,
            esBloqueadaInicialmente = bloqueada,
            idAccionRequerida = req
        });
    }
}