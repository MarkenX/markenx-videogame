using System.Collections.Generic;

// =================================================================================
// CÓDIGO ESTRUCTURAS DEL CORE (BASE DE DATOS)
// =================================================================================

[System.Serializable]
public class Accion
{
    public int idAccion;
    public string categoria; // ATRIBUTOS_PRODUCCION, EXPLORACION, PUBLICIDAD, etc.
    public string nombreAccion;
    public string descripcion;
    public float costo;
    public bool esBloqueadaInicialmente; 
    public int idAccionRequerida; // ID del padre (0 si no tiene padre)
}

[System.Serializable]
public class Subfactor
{
    public int idSubfactor;
    public string factorPrincipal; 
    public string detalle;
}

[System.Serializable]
public class EscenarioPerfilSubfactor
{
    public int idSubfactor;
    public float peso;
    public bool esVisibleInicialmente;
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
    
    // Para guardar historial
    public List<Turno> historialTurnos;
}

[System.Serializable]
public class Turno
{
    public int numeroTurno;
    public float aceptacionTurno;
    public string retroalimentacionIA;
    public string eventoNoticiaTitulo;
    public string eventoNoticiaDetalle;
    public List<int> decisionesAccion;
}

// DATOS DE PRUEBA
public static class MockDataFactory
{
    public static PartidaDataPayload GetMockData()
    {
        PartidaDataPayload data = new PartidaDataPayload();

        // 1. Configuración General
        data.presupuestoInicial = 1000;
        data.aceptacionObjetivo = 80f;
        data.nombreConsumidor = "Barry Seal";
        data.edadConsumidor = 30;

        // 2. Perfil del Consumidor
        data.perfilConsumidor = new List<EscenarioPerfilSubfactor>();
        // Cultural (ID 100) - Oculto
        data.perfilConsumidor.Add(new EscenarioPerfilSubfactor { idSubfactor = 100, peso = 5f, esVisibleInicialmente = false }); 
        // Social (ID 200) - Oculto
        data.perfilConsumidor.Add(new EscenarioPerfilSubfactor { idSubfactor = 200, peso = 3f, esVisibleInicialmente = false });  
        // Personal (ID 300) - Oculto
        data.perfilConsumidor.Add(new EscenarioPerfilSubfactor { idSubfactor = 300, peso = 4f, esVisibleInicialmente = false }); 
        // Psicológico (ID 400) - Oculto
        data.perfilConsumidor.Add(new EscenarioPerfilSubfactor { idSubfactor = 400, peso = 2f, esVisibleInicialmente = false }); 

        // 3. Acciones (Árbol de Habilidades)
        data.accionesDisponibles = new List<Accion>();

        // PRODUCCION
        // Raíz
        data.accionesDisponibles.Add(new Accion { idAccion = 1, categoria = "ATRIBUTOS_PRODUCCION", nombreAccion = "Empaque Reciclado", descripcion = "Usa cartón 100% reciclado.", costo = 150, esBloqueadaInicialmente = false, idAccionRequerida = 0 });
        // Ramas
        data.accionesDisponibles.Add(new Accion { idAccion = 2, categoria = "ATRIBUTOS_PRODUCCION", nombreAccion = "Mat. Biodegradable", descripcion = "Se descompone en 30 días.", costo = 200, esBloqueadaInicialmente = true, idAccionRequerida = 1 });
        data.accionesDisponibles.Add(new Accion { idAccion = 3, categoria = "ATRIBUTOS_PRODUCCION", nombreAccion = "Cert. Carbono Neutro", descripcion = "Sello internacional.", costo = 250, esBloqueadaInicialmente = true, idAccionRequerida = 1 });

        // DISEÑO
        // Raíz
        data.accionesDisponibles.Add(new Accion { idAccion = 4, categoria = "ATRIBUTOS_DISENO", nombreAccion = "Etiqueta Verde", descripcion = "Destaca lo natural.", costo = 100, esBloqueadaInicialmente = false, idAccionRequerida = 0 });
        // Ramas
        data.accionesDisponibles.Add(new Accion { idAccion = 5, categoria = "ATRIBUTOS_DISENO", nombreAccion = "Logo Minimalista", descripcion = "Menos tinta, más estilo.", costo = 150, esBloqueadaInicialmente = true, idAccionRequerida = 4 });

        // PRECIO
        // Raíz
        data.accionesDisponibles.Add(new Accion { idAccion = 7, categoria = "ATRIBUTOS_PRECIO", nombreAccion = "Precio Mercado", descripcion = "Estándar del sector.", costo = 0, esBloqueadaInicialmente = false, idAccionRequerida = 0 });
        // Rama
        data.accionesDisponibles.Add(new Accion { idAccion = 8, categoria = "ATRIBUTOS_PRECIO", nombreAccion = "Desc. Reciclaje", descripcion = "5% menos si traen envase.", costo = 50, esBloqueadaInicialmente = true, idAccionRequerida = 7 });

        // PLAZA
        data.accionesDisponibles.Add(new Accion { idAccion = 10, categoria = "ATRIBUTOS_PLAZA", nombreAccion = "Tienda Online", descripcion = "Venta directa web.", costo = 150, esBloqueadaInicialmente = false, idAccionRequerida = 0 });

        // EXPLORACION
        // Raíz (Revela Social: Ocupación)
        data.accionesDisponibles.Add(new Accion { idAccion = 13, categoria = "EXPLORACION", nombreAccion = "Encuesta General", descripcion = "Datos demográficos básicos.", costo = 100, esBloqueadaInicialmente = false, idAccionRequerida = 0 });
        // Rama (Revela Cultural: Ecológico) - Requiere Encuesta
        data.accionesDisponibles.Add(new Accion { idAccion = 14, categoria = "EXPLORACION", nombreAccion = "Focus Group Eco", descripcion = "Profundiza en valores.", costo = 200, esBloqueadaInicialmente = true, idAccionRequerida = 13 });

        // PUBLICIDAD
        data.accionesDisponibles.Add(new Accion { idAccion = 17, categoria = "PUBLICIDAD", nombreAccion = "Redes Sociales", descripcion = "Ads básicos.", costo = 150, esBloqueadaInicialmente = false, idAccionRequerida = 0 });
        data.accionesDisponibles.Add(new Accion { idAccion = 18, categoria = "PUBLICIDAD", nombreAccion = "Influencers Eco", descripcion = "Colaboración con activistas.", costo = 300, esBloqueadaInicialmente = true, idAccionRequerida = 17 });

        // 4. Reglas de Impacto
        data.reglasImpacto = new List<AccionSubfactorImpacto>();
        // Empaque Reciclado (1) + Ecológico (100) = +10 pts
        data.reglasImpacto.Add(new AccionSubfactorImpacto { idAccion = 1, idSubfactor = 100, impacto = 10 });
        // Biodegradable (2) + Ecológico (100) = +15 pts
        data.reglasImpacto.Add(new AccionSubfactorImpacto { idAccion = 2, idSubfactor = 100, impacto = 15 });
        // Tienda Online (10) + Piloto (200) = +5 pts (Gente ocupada)
        data.reglasImpacto.Add(new AccionSubfactorImpacto { idAccion = 10, idSubfactor = 200, impacto = 5 });

        // 5. Eventos
        data.eventosPosibles = new List<Evento>();
        data.eventosPosibles.Add(new Evento { 
            idEvento = 1, 
            tituloNoticia = "EL MUNDO SE VUELVE MÁS VERDE", 
            detalleNoticia = "Impulso global por la sostenibilidad gana fuerza." 
        });

        // 6. Efectos (El evento sube el peso de lo Ecológico)
        data.efectosEventos = new List<EventoEfecto>();
        data.efectosEventos.Add(new EventoEfecto { idEvento = 1, idSubfactor = 100, modificadorPeso = 5.0f });

        data.historialTurnos = new List<Turno>();

        return data;
    }
}


/* =================================================================================
   CÓDIGO ANTIGUO (BACKEND REAL - COMENTADO)
   =================================================================================
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
================================================================================= */