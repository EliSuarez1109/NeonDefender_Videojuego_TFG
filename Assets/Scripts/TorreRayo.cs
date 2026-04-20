using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TorreRayo : MonoBehaviour
{
    [Header("Estadísticas")]
    public float danoPorSegundo = 10f;

    [Header("Referencias")]
    public Transform puntoDisparo; // Este debe ser hijo del objeto que gira

    [Header("Configuración Visual")]
    public float grosorRayo = 0.5f;

    private Transform objetivoActual;
    private List<Transform> enemigosEnRango = new List<Transform>();
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
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

            LogicaEnemigo scriptEnemigo = objetivoActual.GetComponent<LogicaEnemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.RecibirDaño(danoPorSegundo * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.enabled = false;
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