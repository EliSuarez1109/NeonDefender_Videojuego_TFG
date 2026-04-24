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

    // Memoria compartida de baldosas
    public static List<Vector2> casillasInfectadasGlobales = new List<Vector2>();
    private List<Vector2> misCasillasInfectadas = new List<Vector2>();

    // --- MAGIA NUEVA: El grupo de comunicación de las Teslas ---
    public static List<TorreTesla> todasLasTeslas = new List<TorreTesla>();

    void Start()
    {
        // Al nacer, me uno al grupo
        todasLasTeslas.Add(this);
        EscanearYGenerarEfectos();
    }

    public void EscanearYGenerarEfectos()
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
                    Vector2 posRedondeada = new Vector2(Mathf.Round(posicionCasilla.x), Mathf.Round(posicionCasilla.y));

                    // Si nadie ha reclamado esta casilla, me la quedo yo y la pinto
                    if (!casillasInfectadasGlobales.Contains(posRedondeada))
                    {
                        casillasInfectadasGlobales.Add(posRedondeada);
                        misCasillasInfectadas.Add(posRedondeada);

                        GameObject efecto = Instantiate(prefabCuadradoAmarillo, posicionCasilla, Quaternion.identity);
                        efecto.transform.SetParent(transform);

                        SpriteRenderer sr = efecto.GetComponent<SpriteRenderer>();
                        if (sr != null) sr.sortingOrder = ordenVisualEfecto; 
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        
        if (enemigo != null && !enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Add(enemigo);
            enemigo.EntrarEnTesla(multiplicadorVelocidad, danoPorSegundo); 
        }
    }

    private void OnTriggerExit2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();
        if (enemigo != null && enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Remove(enemigo);
            enemigo.SalirDeTesla(); 
        }
    }

    void OnDestroy()
    {
        // 1. Me salgo del grupo de comunicación
        todasLasTeslas.Remove(this);

        // 2. Limpio mis baldosas de la memoria global
        foreach (Vector2 baldosa in misCasillasInfectadas)
        {
            casillasInfectadasGlobales.Remove(baldosa);
        }

        // 3. Libero a los enemigos
        foreach (LogicaEnemigo enemigo in enemigosEnRango)
        {
            if (enemigo != null) enemigo.SalirDeTesla();
        }

        // --- MAGIA NUEVA: Aviso a mis compañeras ---
        // Les digo a todas las torres que quedan que revisen si he dejado huecos en su zona
        foreach (TorreTesla torre in todasLasTeslas)
        {
            if (torre != null)
            {
                torre.EscanearYGenerarEfectos();
            }
        }
    }
}