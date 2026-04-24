using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TorreRayo : MonoBehaviour
{
    [Header("Estadísticas de Daño")]
    // Le he cambiado el nombre a "danoBasePorSegundo" para que en el Inspector quede más claro
    public float danoBasePorSegundo = 5f; 
    
    [Tooltip("Daño que se suma cada segundo extra que el rayo sigue conectado al mismo objetivo")]
    public float aumentoDeDanoPorSegundo = 10f;
    
    [Tooltip("El tope máximo de daño para no romper el juego contra los jefes")]
    public float danoMaximo = 50f;

    [Header("Referencias")]
    public Transform puntoDisparo; // Este debe ser hijo del objeto que gira

    [Header("Configuración Visual")]
    public float grosorRayo = 0.5f;

    // --- NUEVAS VARIABLES DE MEMORIA ---
    private Transform objetivoActual;
    private Transform objetivoAnterior; // Recuerda a quién estábamos atacando
    private float danoActualPorSegundo; // El daño en tiempo real
    // -----------------------------------

    private List<Transform> enemigosEnRango = new List<Transform>();
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        // Al nacer la torre, el daño empieza en el mínimo
        danoActualPorSegundo = danoBasePorSegundo;
    }

    void Update()
    {
        enemigosEnRango.RemoveAll(enemigo => enemigo == null);

        ActualizarObjetivo();

        if (objetivoActual != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.startWidth = grosorRayo;
            lineRenderer.endWidth = grosorRayo;
            
            // El punto de disparo se moverá solo porque su padre está girando
            lineRenderer.SetPosition(0, puntoDisparo.position);
            lineRenderer.SetPosition(1, objetivoActual.position);

            // --- LÓGICA DE DAÑO PROGRESIVO ---
            if (objetivoActual == objetivoAnterior)
            {
                // Si seguimos disparando al MISMO enemigo, el daño sube
                danoActualPorSegundo += aumentoDeDanoPorSegundo * Time.deltaTime;

                // Ponemos el tope para no pasarnos
                if (danoActualPorSegundo > danoMaximo)
                {
                    danoActualPorSegundo = danoMaximo;
                }
            }
            else
            {
                // Si hemos cambiado de enemigo, el rayo se enfría de golpe
                danoActualPorSegundo = danoBasePorSegundo;
                objetivoAnterior = objetivoActual; // Actualizamos la memoria
            }
            // ---------------------------------

            LogicaEnemigo scriptEnemigo = objetivoActual.GetComponent<LogicaEnemigo>();
            if (scriptEnemigo != null)
            {
                // Ahora aplicamos el daño actual, no el base
                scriptEnemigo.RecibirDaño(danoActualPorSegundo * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.enabled = false;

            // Si no hay enemigos en rango, el rayo se apaga y se enfría por completo
            danoActualPorSegundo = danoBasePorSegundo;
            objetivoAnterior = null;
        }
    }

    void ActualizarObjetivo()
    {
        if (objetivoActual != null && enemigosEnRango.Contains(objetivoActual)) return;
        if (enemigosEnRango.Count > 0) objetivoActual = enemigosEnRango[0];
        else objetivoActual = null;
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo")) enemigosEnRango.Add(colision.transform);
    }

    private void OnTriggerExit2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo")) enemigosEnRango.Remove(colision.transform);
    }
}