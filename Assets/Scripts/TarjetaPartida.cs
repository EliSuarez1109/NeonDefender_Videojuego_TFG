using UnityEngine;
using TMPro;

public class TarjetaPartida : MonoBehaviour
{
    [Header("Textos de la UI")]
    public TMP_Text txtOro;       
    public TMP_Text txtFecha;
    public TMP_Text txtResultado;

    public void ConfigurarTarjeta(string oro, string fecha, string resultado)
    {
        // 1. Asignamos los textos limpios (sin "Fecha:" ni nada extra)
        txtOro.text = oro;
        txtFecha.text = fecha;
        txtResultado.text = resultado;

        // 2. Aplicamos los colores NEÓN
        if(resultado == "Victoria")
        {
            // Verde Neón (#39FF14)
            Color colorVerdeNeon;
            ColorUtility.TryParseHtmlString("#39FF14", out colorVerdeNeon);
            txtResultado.color = colorVerdeNeon;
        }
        else
        {
            // Rojo Neón (#FF3131)
            Color colorRojoNeon;
            ColorUtility.TryParseHtmlString("#FF3131", out colorRojoNeon);
            txtResultado.color = colorRojoNeon;
        }
    }
}