using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DibujarRango : MonoBehaviour
{
    public float radio = 8f; 
    public float grosorLinea = 0.5f; 
    public int suavidad = 64; // Más puntos = circunferencia más perfecta
    public Color colorNeon = new Color(0, 1, 1, 0.5f); // Cian transparente

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        ConfigurarLineRenderer();
    }

    void Start()
    {
        DibujarCircunferencia();
    }

    // Llamamos a esto si el radio cambia (por ejemplo, al mejorar la torre)
    [ContextMenu("Actualizar Dibujo")]
    public void DibujarCircunferencia()
    {
        lr.positionCount = suavidad + 1;
        float anguloRecorrido = 0f;

        for (int i = 0; i <= suavidad; i++)
        {
            // Trigonometría para posicionar los puntos en el borde del radio
            float x = Mathf.Cos(Mathf.Deg2Rad * anguloRecorrido) * radio;
            float y = Mathf.Sin(Mathf.Deg2Rad * anguloRecorrido) * radio;

            lr.SetPosition(i, new Vector3(x, y, 0));
            anguloRecorrido += (360f / suavidad);
        }
    }

    void ConfigurarLineRenderer()
    {
        lr.useWorldSpace = false; // IMPORTANTE: Para que se mueva pegado a la torre
        lr.startWidth = grosorLinea;
        lr.endWidth = grosorLinea;
        lr.loop = true; // Cierra el círculo
        lr.sortingOrder = 10; // Por encima del mapa
        
        // Material para que no sea un bloque sólido
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = colorNeon;
        lr.endColor = colorNeon;
    }
}