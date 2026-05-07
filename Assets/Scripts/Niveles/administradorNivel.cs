using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AdministradorNivel : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject hudPrincipal; // Añadido para que los menús no se superpongan
    public GameObject pantallaFinJuego; 
    public GameObject pantallaEstadisticas; // Conectamos la pantalla aquí

    [Header("Botones Especiales")]
    public GameObject botonModoInfinito; // Conecta aquí tu botón de Modo Infinito

    [Header("Textos")]
    public TMP_Text txtResultado;

    [Header("Estado del juego")]
    public bool juegoFinalizado = false;
    public bool enModoInfinito = false; // Variable para avisar a los enemigos que es infinito

    [Header("Conexiones")]
    public GeneradorEnemigos generadorEnemigos; // <--- PUENTE AL GENERADOR

    void Start()
    {
        // Nos aseguramos de que todo esté en orden al iniciar la partida
        if (hudPrincipal != null) hudPrincipal.SetActive(true);
        if (pantallaFinJuego != null) pantallaFinJuego.SetActive(false);
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false);
    }

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

        if (hudPrincipal != null) hudPrincipal.SetActive(false); // Apagamos el HUD principal
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false); // Ocultamos el botón infinito

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

        if (hudPrincipal != null) hudPrincipal.SetActive(false); // Apagamos el HUD principal

        // ¡ENCENDEMOS EL BOTÓN INFINITO SOLO EN VICTORIA!
        if (botonModoInfinito != null) botonModoInfinito.SetActive(true);

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

        if (hudPrincipal != null) hudPrincipal.SetActive(false); // Apagamos el HUD principal
        if (botonModoInfinito != null) botonModoInfinito.SetActive(false); // Ocultamos el botón infinito

        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); 
        txtResultado.text = "¡TE HAS RENDIDO!"; 
        txtResultado.color = new Color(1f, 0.5f, 0f); 
        Time.timeScale = 0f; 
    }

    // --- NUEVO: FUNCIÓN PARA CONTINUAR JUGANDO ---
// --- NUEVO: FUNCIÓN PARA CONTINUAR JUGANDO ---
    public void IniciarModoInfinito()
    {
        juegoFinalizado = false; // El juego vuelve a estar activo
        enModoInfinito = true;   // Activamos la bandera de modo infinito

        // Reseteamos los datos de la partida para el modo infinito
        if (GestorDatosPartida.instancia != null)
        {
            GestorDatosPartida.instancia.ResetearParaModoInfinito();
        }

        // --------------------------

         if (generadorEnemigos != null)
        {
            generadorEnemigos.ActivarModoInfinito();
        }

        pantallaFinJuego.SetActive(false); // Cerramos el menú de victoria
        
        if (hudPrincipal != null) hudPrincipal.SetActive(true); // Devolvemos el HUD al jugador

        Time.timeScale = 1f; // ¡Descongelamos el tiempo!
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