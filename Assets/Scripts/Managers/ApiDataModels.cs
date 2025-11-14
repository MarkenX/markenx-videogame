using System.Collections.Generic;

[System.Serializable]
public class Accion
{
    public int idAccion;
    public string categoria; // ATRIBUTOS_PRODUCCION, EXPLORACION, etc.
    public string nombreAccion;
    public string descripcion;
    public float costo;
    public bool esBloqueadaInicialmente; // Para árbol de habilidades
    public int idAccionRequerida; // El ID de la acción que desbloquea esta
}

[System.Serializable]
public class Subfactor
{
    public int idSubfactor;
    public string factorPrincipal; // CULTURAL, SOCIAL, etc.
    public string detalle;
}

[System.Serializable]
public class EscenarioPerfilSubfactor
{
    public int idSubfactor;
    public float peso;
    public bool esVisibleInicialmente; // Si es 'false', se muestra como "???"
}

[System.Serializable]
public class AccionSubfactorImpacto
{
    public int idAccion;
    public int idSubfactor;
    public int impacto;
}

[System.Serializable]
public class Evento
{
    public int idEvento;
    public string detalleNoticia;
}

[System.Serializable]
public class EventoEfecto
{
    public int idEvento;
    public int idSubfactor;
    public float modificadorPeso;
}

// CLASE "CONTENEDORA" QUE SE RECIBE DEL BACKEND
// Contiene todas las reglas para UNA partida.
[System.Serializable]
public class PartidaDataPayload
{
    public int presupuestoInicial;
    public float aceptacionObjetivo;
    public string nombreConsumidor;
    public int edadConsumidor;

    public List<Accion> accionesDisponibles;
    public List<EscenarioPerfilSubfactor> perfilConsumidor;
    public List<AccionSubfactorImpacto> reglasImpacto;
    public List<Evento> eventosPosibles;
    public List<EventoEfecto> efectosEventos;
    
    public List<Turno> historialTurnos;
}

// Modelo para el historial de turnos
[System.Serializable]
public class Turno
{
    // Se omite idTurno e idPartida, ya que el backend los asignará
    public int numeroTurno;
    public float aceptacionTurno;
    public string retroalimentacionIA;
    public string eventoNoticiaOcurrido;
    public List<int> decisionesAccion; // Lista de idAccion tomadas en el turno
}