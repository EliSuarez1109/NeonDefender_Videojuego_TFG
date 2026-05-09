using UnityEngine;
using TMPro;

public class TarjetaPartida : MonoBehaviour
{
    [Header("Referencias de Texto")]
    public TMP_Text textResultado;
    public TMP_Text textFecha;
    public TMP_Text textDificultad;
    public TMP_Text textOroObtenido;
    public TMP_Text textEnemigos;
    public TMP_Text textTorres;

    // Esta es la función que llama el GestorPantallas pasando todos los datos
    public void ConfigurarTarjeta(string oro, string fecha, string resultado, string dificultad, string enemigos, string torres)
    {
        // Asignamos los datos numéricos y fechas que no necesitan traducción
        if (textOroObtenido != null) textOroObtenido.text = oro;
        if (textFecha != null) textFecha.text = fecha;
        if (textEnemigos != null) textEnemigos.text = enemigos;
        if (textTorres != null) textTorres.text = torres;

        // --- MAGIA DEL IDIOMA AQUÍ ---
        // Traducimos el resultado y la dificultad usando nuestro Gestor
        if (textResultado != null) textResultado.text = GestorIdiomas.ObtenerResultadoTraducido(resultado);
        if (textDificultad != null) textDificultad.text = GestorIdiomas.ObtenerDificultadTraducida(dificultad);

        // Extra: Cambio de color automático
        // Usamos 'resultado.ToLower()' original de la base de datos para no romper los colores al cambiar a inglés
        string resCrudo = resultado.ToLower();
        
        if (resCrudo.Contains("victoria"))
            textResultado.color = Color.green;
        else if (resCrudo.Contains("derrota"))
            textResultado.color = Color.red;
        else if (resCrudo.Contains("infinito"))
            textResultado.color = Color.cyan;
        else
            textResultado.color = new Color(1f, 0.5f, 0f); // Naranja para Rendición/otros
    }
}