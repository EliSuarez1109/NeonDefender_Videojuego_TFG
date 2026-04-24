using System.Collections.Generic;
using UnityEngine;

public class BalaPerforante : MonoBehaviour 
{
    [Header("Estadísticas")]
    public float velocidad = 30f;
    public float dano = 2f;
    public float tiempoDeVida = 5f; 
    public float compensacionRotacion = -90f;

    [Header("Efecto Daño Continuo")]
    public bool aplicaDañoContinuo = false; 
    
    // ✅ ¡AQUÍ ESTÁ TU CHECKBOX!
    [Tooltip("Si se marca, cada bala suma un veneno extra. Si se desmarca, solo reinicia el reloj del veneno.")]
    public bool seAcumulaElDaño = false; 
    
    public float danoPorSegundo = 2f;
    public float duracionEfecto = 10f;

    private Vector2 direccionDisparo;
    private List<GameObject> enemigosGolpeados = new List<GameObject>();

    public void ConfigurarDireccion(Vector2 direccion)
    {
        direccionDisparo = direccion.normalized;
        
        float angulo = Mathf.Atan2(direccionDisparo.y, direccionDisparo.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo + compensacionRotacion);

        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        transform.Translate(direccionDisparo * velocidad * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        LogicaEnemigo enemigo = colision.GetComponentInParent<LogicaEnemigo>();

        if (enemigo != null)
        {
            if (!enemigosGolpeados.Contains(colision.gameObject))
            {
                // 1. Daño del impacto inicial
                enemigo.RecibirDaño(dano);

                // 2. Aplicar daño continuo pasándole la nueva Checkbox
                if (aplicaDañoContinuo)
                {
                    enemigo.AplicarDañoContinuo(danoPorSegundo, duracionEfecto, seAcumulaElDaño);
                }

                // 3. Lo anotamos en la lista negra para no volver a darle con ESTA misma bala
                enemigosGolpeados.Add(colision.gameObject); 
                
                Debug.Log("Impacto perforante en: " + colision.name);
            }
        }
    }
}