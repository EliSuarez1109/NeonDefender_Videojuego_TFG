using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using TMPro;

[System.Serializable]
public class GrupoEnemigos
{
    public string nombreGrupo = "Grupo (Ej: 5 Normales)"; 
    public GameObject prefabEnemigo;
    public int cantidad;
    public float tiempoEntreEllos = 1.5f;
    public float tiempoEsperaDespues = 3.0f;
    public string nombreEnemigoDB = ""; 
}

[System.Serializable]
public class Ronda
{
    public string nombreRonda = "Ronda 1"; 
    public GrupoEnemigos[] grupos;
}

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Configuración del Camino")]
    public Transform puntoDeSalida;
    public Transform[] puntosCamino;

    [Header("Configuración de Oleadas")]
    public Ronda[] rondas;
    private int indiceRondaActual = 0;

    [Header("Interfaz")]
    public Button botonEmpezar;
    public AdministradorNivel adminNivel;
    
    [Header("HUD de Rondas")]
    public TextMeshProUGUI textoContadorRondas;

    // --- MODO INFINITO ---
    [Header("Modo Infinito")]
    public bool modoInfinitoActivo = false; 
    private int nivelInfinitoActual = 1; 
    
    [Tooltip("¿Cuántos grupos aleatorios salen en la primera ronda infinita?")]
    public int gruposInicialesInfinito = 5;
    
    [Tooltip("Crea aquí todas las plantillas de enemigos que el juego puede elegir al azar")]
    public GrupoEnemigos[] poolGruposInfinitos;
    // ---------------------

    void Start()
    {
        ActualizarTextoRondas(1);
    }

    public void EmpezarSiguienteRonda()
    {
        // Si estamos en modo infinito...
        if (modoInfinitoActivo)
        {
            ActualizarTextoRondas(nivelInfinitoActual);
            StartCoroutine(SpawnRondaInfinita());
        }
        // Si estamos en las rondas normales...
        else if (indiceRondaActual < rondas.Length)
        {
            ActualizarTextoRondas(indiceRondaActual + 1); 
            StartCoroutine(SpawnRonda(rondas[indiceRondaActual]));
            indiceRondaActual++;
        }
        else
        {
            Debug.Log("¡Has completado todas las rondas de este nivel!");
        }
    }

    // --- RUTINA NORMAL ---
    IEnumerator SpawnRonda(Ronda rondaActual)
    {
        if (botonEmpezar != null) botonEmpezar.interactable = false;

        foreach (GrupoEnemigos grupo in rondaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                CrearEnemigo(grupo.prefabEnemigo, grupo.nombreEnemigoDB);
                yield return new WaitForSeconds(grupo.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupo.tiempoEsperaDespues);
        }

        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f); 
        }

        Debug.Log("¡Mapa limpio! Ronda completada.");

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarRondaCompletada();
        }

        if (botonEmpezar != null) botonEmpezar.interactable = true;

        if (indiceRondaActual >= rondas.Length && adminNivel != null && !adminNivel.juegoFinalizado)
        {
            adminNivel.MostrarVictoria();
        }
        else
        {
            ActualizarTextoRondas(indiceRondaActual + 1);
        }
    }

    // --- RUTINA INFINITA ---
    IEnumerator SpawnRondaInfinita()
    {
        if (poolGruposInfinitos == null || poolGruposInfinitos.Length == 0)
        {
            Debug.LogError("¡No hay grupos en el Pool Infinito! Añádelos en el Inspector.");
            yield break;
        }

        if (botonEmpezar != null) botonEmpezar.interactable = false;

        int cantidadGruposEstaRonda = gruposInicialesInfinito + (nivelInfinitoActual - 1);

        for (int g = 0; g < cantidadGruposEstaRonda; g++)
        {
            int indiceAleatorio = Random.Range(0, poolGruposInfinitos.Length);
            GrupoEnemigos grupoElegido = poolGruposInfinitos[indiceAleatorio];

            for (int i = 0; i < grupoElegido.cantidad; i++)
            {
                CrearEnemigo(grupoElegido.prefabEnemigo, grupoElegido.nombreEnemigoDB);
                yield return new WaitForSeconds(grupoElegido.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupoElegido.tiempoEsperaDespues);
        }

        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("¡Oleada Infinita " + nivelInfinitoActual + " superada!");

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.RegistrarRondaCompletada();
        }

        nivelInfinitoActual++; 
        ActualizarTextoRondas(nivelInfinitoActual);

        if (botonEmpezar != null) botonEmpezar.interactable = true;
    }

    void CrearEnemigo(GameObject prefab, string nombreEnemigoDB = "")
    {
        if (prefab != null && puntoDeSalida != null)
        {
            GameObject nuevoEnemigo = Instantiate(prefab, puntoDeSalida.position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();

            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = puntosCamino;
                scriptEnemigo.nombreEnemigo = !string.IsNullOrEmpty(nombreEnemigoDB) ? nombreEnemigoDB : prefab.name;

                if (GestorDatosPartida.instancia != null)
                {
                    GestorDatosPartida.instancia.AgregarEnemigoActivo(scriptEnemigo.nombreEnemigo);
                }
            }
        }
    }

    public void ActualizarTextoRondas(int numeroDeRondaActual)
    {
        if (textoContadorRondas != null)
        {
            if (modoInfinitoActivo)
            {
                string etiqueta = GestorIdiomas.ObtenerEtiquetaInfinita();
                textoContadorRondas.text = etiqueta + ": " + numeroDeRondaActual;
            }
            else
            {
                int rondaVisual = Mathf.Min(numeroDeRondaActual, rondas.Length);
                textoContadorRondas.text = rondaVisual + " / " + rondas.Length;
            }
        }
    }

    // --- FUNCIÓN ORIGINAL (Para tu nivel 1) ---
    public void ActivarModoInfinito()
    {
        modoInfinitoActivo = true;
        ActualizarTextoRondas(nivelInfinitoActual);
        Debug.Log("Modo Infinito Activado (Individual).");
    }

    // --- FUNCIÓN NUEVA (Para tu nivel 3 y futuros niveles con varios caminos) ---
    public void ActivarModoInfinitoEnTodosLosScripts()
    {
        GeneradorEnemigos[] todosLosGeneradores = GetComponents<GeneradorEnemigos>();

        foreach (GeneradorEnemigos generador in todosLosGeneradores)
        {
            generador.modoInfinitoActivo = true;
            generador.ActualizarTextoRondas(generador.nivelInfinitoActual);
        }
        
        Debug.Log("Modo Infinito Activado en los " + todosLosGeneradores.Length + " caminos a la vez.");
    }
}