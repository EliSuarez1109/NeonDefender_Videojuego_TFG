using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DibujarRango : MonoBehaviour
{
    [Header("Forma del Rango")]
    public bool esCuadrado = false; // ✅ NUEVO: Activa esto para la Torre Tesla

    [Header("Ajustes Visuales")]
    public float radio = 12f;
    public float grosorLinea = 0.2f;
    public int suavidad = 60;
    public Color colorNeon = Color.cyan;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Dibujar();
    }

    // OnValidate hace que el rango se actualice en tiempo real en la escena de Unity 
    // mientras cambias los valores en el Inspector, sin tener que darle a Play.
    void OnValidate()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        Dibujar();
    }

    public void Dibujar()
    {
        if (lineRenderer == null) return;

        // Le pasamos tus ajustes al Line Renderer
        lineRenderer.startWidth = grosorLinea;
        lineRenderer.endWidth = grosorLinea;
        lineRenderer.startColor = colorNeon;
        lineRenderer.endColor = colorNeon;
        
        // Vital para que el rango se cierre perfectamente
        lineRenderer.loop = true; 
        
        // Vital para que el holograma se mueva con el ratón al construir
        lineRenderer.useWorldSpace = false; 

        if (esCuadrado)
        {
            GenerarPuntosCuadrado();
        }
        else
        {
            GenerarPuntosCirculo();
        }
    }

    void GenerarPuntosCirculo()
    {
        lineRenderer.positionCount = suavidad;
        float angulo = 0f;

        for (int i = 0; i < suavidad; i++)
        {
            // Trigonometría básica para el círculo
            float x = Mathf.Sin(Mathf.Deg2Rad * angulo) * radio;
            float y = Mathf.Cos(Mathf.Deg2Rad * angulo) * radio;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
            angulo += (360f / suavidad);
        }
    }

    void GenerarPuntosCuadrado()
    {
        // Un cuadrado solo necesita 4 esquinas
        lineRenderer.positionCount = 4;

        // El "radio" en un cuadrado es la distancia del centro al borde
        lineRenderer.SetPosition(0, new Vector3(-radio, -radio, 0f)); // Abajo Izquierda
        lineRenderer.SetPosition(1, new Vector3(-radio, radio, 0f));  // Arriba Izquierda
        lineRenderer.SetPosition(2, new Vector3(radio, radio, 0f));   // Arriba Derecha
        lineRenderer.SetPosition(3, new Vector3(radio, -radio, 0f));  // Abajo Derecha
    }
}