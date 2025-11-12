using System.Collections.Generic;

// [System.Serializable]
// Le dice a Unity que esta clase puede ser convertida a/desde JSON.

// 1. MODELOS PARA ENVIAR (Request)

[System.Serializable]
public class DecisionRequest
{
    public int idProducto;
    public int idPrecio;
    public int idPlaza;
    public int idPromocion;
}

[System.Serializable]
public class ContextoFactor
{
    public string detalle;
    public string importancia;
}

[System.Serializable]
public class ContextoRequest
{
    public List<ContextoFactor> macroentorno;
    public List<ContextoFactor> perfilConsumidor;
}

[System.Serializable]
public class SimulationRequest
{
    public DecisionRequest decision;
    public ContextoRequest contexto;
}


// 2. MODELOS PARA RECIBIR (Response)

[System.Serializable]
public class SimulationResponse
{
    public float nivelAceptacion; // 95.0
    public int puntaje;           // 950
    public string feedback;       // "¡Excelente!..."
}