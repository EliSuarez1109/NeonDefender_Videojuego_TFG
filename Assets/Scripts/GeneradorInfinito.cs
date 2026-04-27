using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

/*[System.Serializable]
public class GrupoEnemigos
{
    public string nombreGrupo = "Grupo (Ej: 5 Normales)"; 
    public GameObject prefabEnemigo;
    public int cantidad;
    public float tiempoEntreEllos = 1.5f;
    public float tiempoEsperaDespues = 3.0f; 
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

    // --- PREPARACIÓN PARA EL FUTURO: MODO INFINITO ---
    [Header("Modo Infinito")]
    public bool modoInfinitoActivo = false; // Se activará desde un botón de UI en el futuro
    private int nivelInfinitoActual = 1;
    // Necesitamos saber qué prefabs usar para el infinito. Por defecto cogeremos los del primer grupo que hayas configurado.
    // -------------------------------------------------

    public void EmpezarSiguienteRonda()
    {
        // 1. Si estamos en modo infinito, generamos la ronda por matemáticas
        if (modoInfinitoActivo)
        {
            StartCoroutine(SpawnRondaInfinita());
        }
        // 2. Si quedan rondas normales, las jugamos
        else if (indiceRondaActual < rondas.Length)
        {
            StartCoroutine(SpawnRonda(rondas[indiceRondaActual]));
            indiceRondaActual++;
        }
        // 3. Si se han acabado las normales, avisamos (Aquí irá tu futura pantalla de victoria)
        else
        {
            Debug.Log("¡Has completado todas las rondas de este nivel! Mostrar pantalla de Victoria/Modo Infinito.");
            // FUTURO: PantallaVictoriaUI.SetActive(true);
        }
    }

    IEnumerator SpawnRonda(Ronda rondaActual)
    {
        if (botonEmpezar != null) botonEmpezar.interactable = false;

        foreach (GrupoEnemigos grupo in rondaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                CrearEnemigo(grupo.prefabEnemigo);
                yield return new WaitForSeconds(grupo.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupo.tiempoEsperaDespues);
        }

        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f); 
        }

        Debug.Log("¡Mapa limpio! Ronda completada.");

        if (botonEmpezar != null) botonEmpezar.interactable = true;
    }

    // --- RUTINA DEL MODO INFINITO (GENERACIÓN PROCEDURAL) ---
    IEnumerator SpawnRondaInfinita()
    {
        if (botonEmpezar != null) botonEmpezar.interactable = false;

        Debug.Log("Comenzando Oleada Infinita: " + nivelInfinitoActual);

        // Para el infinito, cogemos el prefab del primer enemigo normal y el primero fuerte que pusiste en la Ronda 0
        // (Asegúrate de tener al menos 1 ronda configurada para que esto no dé error)
        GameObject prefabNormal = rondas[0].grupos[0].prefabEnemigo;
        
        // Calculamos cuántos salen según el nivel infinito (Ej: Nivel 1 = 10, Nivel 2 = 15, Nivel 3 = 20...)
        int cantidadEnemigos = 5 + (nivelInfinitoActual * 5); 
        
        // Cada nivel salen más deprisa (con un límite de 0.2s para que no salgan todos de golpe)
        float tiempoSpawn = Mathf.Max(0.2f, 1.5f - (nivelInfinitoActual * 0.1f)); 

        for (int i = 0; i < cantidadEnemigos; i++)
        {
            CrearEnemigo(prefabNormal);
            yield return new WaitForSeconds(tiempoSpawn);
        }

        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            yield return new WaitForSeconds(0.5f); 
        }

        Debug.Log("¡Oleada Infinita " + nivelInfinitoActual + " superada!");
        nivelInfinitoActual++; // Subimos la dificultad para la próxima vez que le den al botón

        if (botonEmpezar != null) botonEmpezar.interactable = true;
    }
    // --------------------------------------------------------

    void CrearEnemigo(GameObject prefab)
    {
        if (prefab != null && puntoDeSalida != null)
        {
            GameObject nuevoEnemigo = Instantiate(prefab, puntoDeSalida.position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();

            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = puntosCamino;
            }
        }
    }
}*/