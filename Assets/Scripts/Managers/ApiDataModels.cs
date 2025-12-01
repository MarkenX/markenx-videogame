using System.Collections.Generic;
using System;

// UI de Unity (Iconos, Textos, Botones)
[System.Serializable]
public class AccionInfo 
{
    public int idAccion;
    public string categoria; // "ATRIBUTOS_PRODUCCION", etc.
    public string nombreAccion;
    public string descripcion;
    public float costo;
    public bool esBloqueadaInicialmente; 
    public int idAccionRequerida; 
}

[System.Serializable]
public class Evento
{
    public int idEvento;
    public string tituloNoticia;
    public string detalleNoticia;
}

[System.Serializable]
public class EventoEfecto
{
    public int idEvento;
    public int idSubfactor;
    public float modificadorPeso;
}

[System.Serializable]
public class Turno
{
    public int numeroTurno;
    public float aceptacionTurno;
    public string eventoNoticiaTitulo; // Para guardar en historial
    public List<int> decisionesAccion;
}

public class GameContext
{
    // Definiciones de Dimensiones
    public DimensionDefinition PriceSensitivity;
    public DimensionDefinition SocialRecognition;
    public DimensionDefinition QualityExpectation;
    public DimensionDefinition EaseOfUse;
    public DimensionDefinition EcoInterest;

    // Perfiles
    public ConsumerProfile Consumer;
    public ProductProfile Product;

    // ID Botón (Unity) -> Acción Lógica (Paquete)
    public Dictionary<int, MarketAction> ActionsMap;
    
    // Lista visual para la UI
    public List<AccionInfo> AccionesVisuales;

    public List<Evento> eventosPosibles;
    public List<EventoEfecto> efectosEventos;
    public List<Turno> historialTurnos;

    // Configuración
    public float Presupuesto;
    public float AceptacionObjetivo;
    public string NombreConsumidor;
    public int EdadConsumidor;
}