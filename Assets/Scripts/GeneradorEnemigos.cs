using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

[System.Serializable]
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

    public void EmpezarSiguienteRonda()
    {
        if (indiceRondaActual < rondas.Length)
        {
            StartCoroutine(SpawnRonda(rondas[indiceRondaActual]));
            indiceRondaActual++;
        }
        else
        {
            Debug.Log("¡Has completado todas las rondas de este nivel!");
        }
    }

    IEnumerator SpawnRonda(Ronda rondaActual)
    {
        // 1. BLOQUEAMOS EL BOTÓN
        if (botonEmpezar != null) 
        {
            botonEmpezar.interactable = false;
        }

        // 2. Soltamos a todos los enemigos de la ronda
        foreach (GrupoEnemigos grupo in rondaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                CrearEnemigo(grupo.prefabEnemigo);
                yield return new WaitForSeconds(grupo.tiempoEntreEllos);
            }
            yield return new WaitForSeconds(grupo.tiempoEsperaDespues);
        }

        // --- LA MAGIA NUEVA: EL VIGILANTE ---
        // Mientras siga existiendo al menos un objeto con el Tag "Enemigo" en el juego...
        while (GameObject.FindGameObjectWithTag("Enemigo") != null)
        {
            // ...esperamos medio segundo y volvemos a mirar (así no saturamos el procesador)
            yield return new WaitForSeconds(0.5f); 
        }
        // ------------------------------------

        Debug.Log("¡Mapa limpio! Ronda completada.");

        // 3. DESBLOQUEAMOS EL BOTÓN para la siguiente ronda
        if (botonEmpezar != null) 
        {
            botonEmpezar.interactable = true;
        }
    }

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
}