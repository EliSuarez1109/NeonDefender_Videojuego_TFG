using UnityEngine;
using UnityEngine.UI;

public class BasePrincipal : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Interfaz (UI)")]
    public Image barraDeVida; 

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarraVida();
    }

    public void RecibirDano(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima); 
        ActualizarBarraVida();

        if (vidaActual <= 0)
        {
            Debug.Log("¡GAME OVER!");
        }
    }

    void ActualizarBarraVida()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = vidaActual / vidaMaxima;
        }
    }
}