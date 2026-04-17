using System.Collections;
using UnityEngine;

public class EnemigosPruebaNV3 : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabEnemigo;
    public float tiempoEntreGrupos = 3f; // Cada cuántos segundos salen 3 enemigos

    [Header("Las 3 Rutas")]
    public Transform[] rutaArriba;
    public Transform[] rutaCentro;
    public Transform[] rutaAbajo;

    void Start()
    {
        // Empezamos a generar enemigos infinitamente
        StartCoroutine(GenerarEnemigosInfinitos());
    }

    IEnumerator GenerarEnemigosInfinitos()
    {
        // Bucle infinito para probar las torres
        while (true)
        {
            GenerarUnEnemigo(rutaArriba);
            GenerarUnEnemigo(rutaCentro);
            GenerarUnEnemigo(rutaAbajo);

            // Esperamos X segundos antes de lanzar el siguiente grupo
            yield return new WaitForSeconds(tiempoEntreGrupos);
        }
    }

    void GenerarUnEnemigo(Transform[] rutaAsignada)
    {
        // Seguridad: si la ruta está vacía, no hacemos nada
        if (rutaAsignada.Length == 0) return;

        // 1. Creamos al enemigo exactamente en la posición del Waypoint 1 de ESA ruta
        GameObject nuevoEnemigo = Instantiate(prefabEnemigo, rutaAsignada[0].position, Quaternion.identity);

        // 2. Buscamos su "cerebro"
        LogicaEnemigo logica = nuevoEnemigo.GetComponent<LogicaEnemigo>();

        // 3. Le inyectamos su ruta específica para que sepa por dónde ir
        if (logica != null)
        {
            logica.puntos = rutaAsignada;
        }
    }
}