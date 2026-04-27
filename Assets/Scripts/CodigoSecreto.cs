using UnityEngine;

public class CodigoSecreto : MonoBehaviour
{
    [Header("Configuración del Truco")]
    public int oroDeRecompensa = 9999;

    [Tooltip("Añade aquí las teclas en orden. Ej: UpArrow, UpArrow, DownArrow...")]
    public KeyCode[] secuenciaSecreta;

    private int indiceActual = 0;

    void Update()
    {
        // Solo comprobamos si el jugador ha pulsado ALGUNA tecla en este frame
        if (Input.anyKeyDown)
        {
            // Ignoramos los clics del ratón para que no te rompan el código mientras juegas y pones torres
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;

            // ¿La tecla que pulsó es la que toca en la secuencia?
            if (Input.GetKeyDown(secuenciaSecreta[indiceActual]))
            {
                indiceActual++; // Acertó, pasamos a la siguiente tecla

                // ¿Ha completado toda la secuencia?
                if (indiceActual >= secuenciaSecreta.Length)
                {
                    ActivarTruco();
                    indiceActual = 0; // Reseteamos por si quiere volver a meterlo
                }
            }
            else
            {
                // Si se equivoca de tecla, el combo se rompe y vuelve a empezar
                indiceActual = 0;
            }
        }
    }

    void ActivarTruco()
    {
        Debug.Log("¡CÓDIGO SECRETO ACTIVADO! +" + oroDeRecompensa + " de Oro");

        // Usamos tu Gestor de Economía para dar la recompensa real
        if (GestorEconomia.instancia != null)
        {
            GestorEconomia.instancia.SumarOro(oroDeRecompensa);
        }
    }
}