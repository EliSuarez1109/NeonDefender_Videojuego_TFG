using UnityEngine;
using UnityEngine.UI; 

public class LogicaEnemigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform[] puntos;    
    public float velocidad = 30f;
    private int indicePunto = 0;

    private float velocidadOriginal = -1f; 

    private int zonasTeslaPisadas = 0; 
    private float danoTeslaActual = 0f; 
    private float temporizadorChoqueTesla = 0f; 

    [Header("Configuración de Vida")]
    public float vidaMax = 3f;
    private float vidaActual;
    public Slider barraDeVida;    

    [Header("Ataque a Base")]
    public float danoABase = 20f; 

    [Header("Recompensa")]
    public int oroAlMorir = 2; 

    // --- NUEVA VARIABLE PARA EL VENENO ---
    // Aquí el enemigo guardará el veneno único para poder borrarlo si le disparan otra vez
    private Coroutine rutinaVenenoUnico; 

    void Start()
    {
        vidaActual = vidaMax;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMax;
            barraDeVida.value = vidaMax;
        }
    }

    void Update()
    {
        MoverEnemigo();

        if (zonasTeslaPisadas > 0)
        {
            temporizadorChoqueTesla += Time.deltaTime;
            if (temporizadorChoqueTesla >= 1f)
            {
                RecibirDaño(danoTeslaActual);
                temporizadorChoqueTesla = 0f; 
            }
        }
        else
        {
            temporizadorChoqueTesla = 0f;
        }
    }

    void MoverEnemigo()
    {
        if (indicePunto >= puntos.Length)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, puntos[indicePunto].position, velocidad * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntos[indicePunto].position) < 0.1f)
        {
            indicePunto++;
        }
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;
        if (barraDeVida != null) barraDeVida.value = vidaActual;
        if (vidaActual <= 0) Morir();
    }

    void Morir()
    {
        if(GestorEconomia.instancia != null) GestorEconomia.instancia.SumarOro(oroAlMorir);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Base"))
        {
            BasePrincipal baseScript = colision.GetComponent<BasePrincipal>();
            if (baseScript != null)
            {
                baseScript.RecibirDano(danoABase);
                Destroy(gameObject);
            }
        }
    }

    public void EntrarEnTesla(float multiplicador, float danoQueHaceLaTorre)
    {
        zonasTeslaPisadas++;
        
        if (zonasTeslaPisadas == 1) 
        {
            if (velocidadOriginal == -1f) velocidadOriginal = velocidad;
            velocidad = velocidadOriginal * multiplicador;
            danoTeslaActual = danoQueHaceLaTorre;
            temporizadorChoqueTesla = 0.9f; 
        }
    }

    public void SalirDeTesla()
    {
        zonasTeslaPisadas--;
        
        if (zonasTeslaPisadas <= 0)
        {
            zonasTeslaPisadas = 0; 
            if (velocidadOriginal != -1f) velocidad = velocidadOriginal;
        }
    }

    // --- SISTEMA DE DAÑO CONTINUO (AHORA CON CHECKBOX) ---
    public void AplicarDañoContinuo(float dps, float duracion, bool seAcumula)
    {
        if (seAcumula)
        {
            // Si SÍ se acumula, simplemente lanzamos uno nuevo sin importar los demás (Como estaba antes)
            StartCoroutine(RutinaDañoContinuo(dps, duracion));
        }
        else
        {
            // Si NO se acumula, miramos si ya estaba envenenado...
            if (rutinaVenenoUnico != null)
            {
                // Si lo estaba, paramos ese veneno viejo...
                StopCoroutine(rutinaVenenoUnico);
            }
            // ...y lanzamos el nuevo, guardándolo en la memoria
            rutinaVenenoUnico = StartCoroutine(RutinaDañoContinuo(dps, duracion));
        }
    }

    private System.Collections.IEnumerator RutinaDañoContinuo(float dps, float duracion)
    {
        float tiempoPasado = 0f;
        while (tiempoPasado < duracion)
        {
            yield return new WaitForSeconds(1f); 
            RecibirDaño(dps);
            tiempoPasado += 1f;
        }
    }
}