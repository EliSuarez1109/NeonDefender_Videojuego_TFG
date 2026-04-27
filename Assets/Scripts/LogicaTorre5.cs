using System.Collections.Generic;
using UnityEngine;

public class LogicaTorre5 : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public Transform cañon; 

    [Header("Configuración")]
    public float cadencia = 2f;
    public float compensacionRotacion = -90f; 
    
    private float temporizador;
    
    // Lista para saber quién está dentro del Circle Collider
    private List<Transform> enemigosEnRango = new List<Transform>();
    private Transform objetivoActual;

    void Update()
    {
        // 1. Limpiamos la lista 
        enemigosEnRango.RemoveAll(enemigo => enemigo == null);

        // 2. El temporizador avanza siempre
        temporizador += Time.deltaTime;

        // --- EL TOPE ANTI-AMETRALLADORA ---
        // Si ya cargó la bala, no le dejamos acumular tiempo infinito. Se queda lista (al máximo).
        if (temporizador > cadencia) 
        {
            temporizador = cadencia;
        }

        // 3. Revisamos a quién disparar
        ActualizarObjetivo();

        if (objetivoActual != null)
        {
            // Apuntamos
            Vector2 direccion = objetivoActual.position - cañon.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            cañon.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + compensacionRotacion));

            // Disparamos
            if (temporizador >= cadencia)
            {
                Disparar();
                
                // Restamos la cadencia para conservar los decimales sobrantes y ser precisos en x2
                temporizador -= cadencia; 
            }
        }
    }

    void ActualizarObjetivo()
    {
        // Si ya tenemos un objetivo y SIGUE en nuestra lista (dentro del collider), nos lo quedamos
        if (objetivoActual != null && enemigosEnRango.Contains(objetivoActual))
        {
            return; 
        }

        // Si no tenemos objetivo (o el anterior salió/murió), cogemos al primero de la lista
        if (enemigosEnRango.Count > 0)
        {
            objetivoActual = enemigosEnRango[0];
        }
        else
        {
            objetivoActual = null;
        }
    }

    void Disparar()
    {
        GameObject nuevaBala = Instantiate(prefabBala, puntoDisparo.position, Quaternion.identity);
        Vector2 direccion = objetivoActual.position - puntoDisparo.position;

        BalaPerforante scriptBala = nuevaBala.GetComponent<BalaPerforante>();
        if (scriptBala != null)
        {
            scriptBala.ConfigurarDireccion(direccion);
        }
    }

    // --- EL RADAR FÍSICO (Requiere Circle Collider 2D con 'Is Trigger') ---
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo"))
        {
            enemigosEnRango.Add(colision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D colision)
    {
        if (colision.CompareTag("Enemigo"))
        {
            enemigosEnRango.Remove(colision.transform);
        }
    }
}