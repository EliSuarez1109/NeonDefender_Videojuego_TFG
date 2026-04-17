using System.Collections.Generic;
using UnityEngine;

public class TorreTesla : MonoBehaviour
{
    [Header("Efecto Visual")]
    public GameObject prefabCuadradoAmarillo; 
    public LayerMask capaCamino;              
    public float tamanoCasilla = 100f;        
    public int ordenVisualEfecto = 10; 

    [Header("Estadísticas")]
    public float danoPorSegundo = 1f;
    public float multiplicadorVelocidad = 0.5f; 

    private List<LogicaEnemigo> enemigosEnRango = new List<LogicaEnemigo>();
    private float temporizadorDano = 0f;

    void Start()
    {
        EscanearYGenerarEfectos();
    }

    // 1. EL RADAR (Pinta los cuadrados amarillos)
    void EscanearYGenerarEfectos()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 posicionCasilla = new Vector2(
                    transform.position.x + (x * tamanoCasilla),
                    transform.position.y + (y * tamanoCasilla)
                );

                Collider2D impacto = Physics2D.OverlapPoint(posicionCasilla, capaCamino);

                if (impacto != null)
                {
                    GameObject efecto = Instantiate(prefabCuadradoAmarillo, posicionCasilla, Quaternion.identity);
                    efecto.transform.SetParent(transform);

                    SpriteRenderer sr = efecto.GetComponent<SpriteRenderer>();
                    if (sr != null) {
                        sr.sortingOrder = ordenVisualEfecto; 
                    }
                }
            }
        }
    }

    // 2. EL TEMPORIZADOR (Aplica daño 1 vez por segundo)
    void Update()
    {
        temporizadorDano += Time.deltaTime;
        
        if (temporizadorDano >= 1f) 
        {
            AplicarDanoATodos();
            temporizadorDano = 0f;
        }
    }

    void AplicarDanoATodos()
    {
        for (int i = enemigosEnRango.Count - 1; i >= 0; i--)
        {
            if (enemigosEnRango[i] != null)
            {
                enemigosEnRango[i].RecibirDaño(danoPorSegundo);
            }
            else
            {
                enemigosEnRango.RemoveAt(i); 
            }
        }
    }

    // 3. LAS FÍSICAS (Detecta cuándo entran y salen para la velocidad)
    private void OnTriggerEnter2D(Collider2D colision)
    {
        // Chivato de consola
        Debug.Log("¡Chispa! Algo ha entrado: " + colision.gameObject.name);

        // Buscamos el script en el objeto o en su padre
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        
        if (enemigo != null && !enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Add(enemigo);
            enemigo.AlterarVelocidad(multiplicadorVelocidad); 
            Debug.Log("¡Enemigo ralentizado con éxito!");
        }
    }

    private void OnTriggerExit2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        if (enemigo != null && enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Remove(enemigo);
            enemigo.RestaurarVelocidad(); 
        }
    }
}