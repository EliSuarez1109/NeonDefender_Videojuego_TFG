using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AdministradorNivel : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject pantallaFinJuego; 
    public GameObject pantallaEstadisticas; // Conectamos la pantalla aquí

    [Header("Textos")]
    public TMP_Text txtResultado;

    [Header("Estado del juego")]
    public bool juegoFinalizado = false;

    // Se llama cuando los enemigos destruyen tu torre
    public void MostrarDerrota()
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("derrota");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); // Nos aseguramos de que estadísticas esté apagada
        txtResultado.text = "¡BASE DESTRUIDA!";
        txtResultado.color = Color.red; 
        Time.timeScale = 0f; // El juego se congela
    }

    public void MostrarVictoria()
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("victoria");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); // Nos aseguramos de que estadísticas esté apagada
        txtResultado.text = "¡VICTORIA!";
        txtResultado.color = Color.green; 
        Time.timeScale = 0f; // El juego se congela
    }

    // --- FUNCIÓN PARA EL BOTÓN DE SALIR/RENDIRSE ---
    public void Rendirse()
    {
        if (juegoFinalizado) return;
        juegoFinalizado = true;

        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.EstablecerEstado("rendicion");
            GestorDatosPartida.instancia.GuardarPartidaAWS();
        }

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); 
        txtResultado.text = "¡TE HAS RENDIDO!"; 
        txtResultado.color = new Color(1f, 0.5f, 0f); 
        Time.timeScale = 0f; 
    }

    // El botón "Ver Estadísticas" de la pantalla de Fin de Partida llamará a esto
    public void AbrirEstadisticas()
    {
        pantallaFinJuego.SetActive(false);     
        pantallaEstadisticas.SetActive(true);  
    }

// El botón "Volver al Menú" (tanto el de derrota como el de estadísticas) llamará a esto
    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Descongelamos el tiempo
        
        SceneManager.LoadScene("InterfazUsuario"); // Viajamos al menú principal
    }

    // --- BOTÓN: VOLVER AL CARRUSEL DE MAPAS ---
    public void VolverASeleccionMapa()
    {
        Time.timeScale = 1f; // Descongelamos el tiempo

        // Dejamos una "nota secreta" para que el menú sepa qué abrir
        PlayerPrefs.SetInt("AbrirCarrusel", 1); 
        PlayerPrefs.Save(); // Guardamos la nota

        // Viajamos a la escena del menú
        SceneManager.LoadScene("InterfazUsuario"); 
    }


    // --- BOTÓN: VOLVER DE ESTADÍSTICAS A FIN DE PARTIDA ---
    public void VolverAFinDePartida()
    {
        pantallaEstadisticas.SetActive(false); // Apagamos las estadísticas
        pantallaFinJuego.SetActive(true);      // Encendemos de nuevo la derrota
    }
}