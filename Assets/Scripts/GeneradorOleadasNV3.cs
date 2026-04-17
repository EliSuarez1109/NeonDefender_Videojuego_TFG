using UnityEngine;
using System.Collections;

public class GeneradorOleadasNV3 : MonoBehaviour
{
    [Header("Prefabs de Enemigos")]
    public GameObject enemigoNormalPrefab;
    public GameObject enemigoFuertePrefab;

    [Header("Las 3 Rutas")]
    public Transform[] rutaArriba;
    public Transform[] rutaCentro;
    public Transform[] rutaAbajo;

    [Header("Tiempos")]
    public float tiempoEntreNormales = 1.5f;
    public float esperaParaElFuerte = 3.0f;

    // Esta función la llama el Botón. 
    // Como no hay "candado", si pulsas 10 veces, saldrán 10 oleadas simultáneas.
    public void EmpezarRonda()
    {
        StartCoroutine(SpawnOleada());
    }

    IEnumerator SpawnOleada()
    {
        // 1. Salen 3 oleadas de enemigos normales (uno por cada camino a la vez)
        for (int i = 0; i < 3; i++)
        {
            CrearEnemigo(enemigoNormalPrefab, rutaArriba);
            CrearEnemigo(enemigoNormalPrefab, rutaCentro);
            CrearEnemigo(enemigoNormalPrefab, rutaAbajo);
            
            yield return new WaitForSeconds(tiempoEntreNormales);
        }

        // 2. Pausa dramática antes del jefe
        yield return new WaitForSeconds(esperaParaElFuerte);

        // 3. Sale un enemigo fuerte por cada camino
        CrearEnemigo(enemigoFuertePrefab, rutaArriba);
        CrearEnemigo(enemigoFuertePrefab, rutaCentro);
        CrearEnemigo(enemigoFuertePrefab, rutaAbajo);
        
        Debug.Log("¡Oleada triple generada!");
    }

    // Función adaptada para que acepte el Prefab y la Ruta por la que debe ir
    void CrearEnemigo(GameObject prefab, Transform[] rutaAsignada)
    {
        // Seguridad: comprobamos que el prefab existe y la ruta tiene puntos
        if (prefab != null && rutaAsignada != null && rutaAsignada.Length > 0)
        {
            // Creamos al enemigo en el Punto 0 de la ruta que le ha tocado
            GameObject nuevoEnemigo = Instantiate(prefab, rutaAsignada[0].position, Quaternion.identity);
            LogicaEnemigo scriptEnemigo = nuevoEnemigo.GetComponent<LogicaEnemigo>();
            
            // Le inyectamos su ruta en el cerebro
            if (scriptEnemigo != null)
            {
                scriptEnemigo.puntos = rutaAsignada;
            }
        }
    }
}