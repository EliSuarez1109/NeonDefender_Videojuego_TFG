using UnityEngine;
using TMPro;

public class ControladorTiempo : MonoBehaviour
{
    // 0 = x1, 1 = x1.5, 2 = x2
    private int estadoVelocidad = 0;
    
    [Header("Configuración")]
    public TextMeshProUGUI textoBoton;

    void Start()
    {
        // Nos aseguramos de empezar en velocidad normal
        Time.timeScale = 1f;
        ActualizarInterfaz();
    }

    public void AlternarVelocidad()
    {
        // Ciclo: 0 -> 1 -> 2 -> 0...
        estadoVelocidad++;

        if (estadoVelocidad > 2)
        {
            estadoVelocidad = 0;
        }

        // Aplicamos la velocidad según el estado
        switch (estadoVelocidad)
        {
            case 0:
                Time.timeScale = 1f;
                break;
            case 1:
                Time.timeScale = 1.5f;
                break;
            case 2:
                Time.timeScale = 2f;
                break;
        }

        ActualizarInterfaz();
    }

    void ActualizarInterfaz()
    {
        if (textoBoton != null)
        {
            // Usamos CultureInfo.InvariantCulture para forzar el punto en vez de la coma
            // El formato "0.#" quita automáticamente los decimales si acaban en cero
            textoBoton.text = "x" + Time.timeScale.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    private void OnDestroy()
    {
        // Siempre devolver el tiempo a la normalidad al salir
        Time.timeScale = 1f;
    }
}