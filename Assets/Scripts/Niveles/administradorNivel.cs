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

    // Se llama cuando los enemigos destruyen tu torre
    public void MostrarDerrota()
    {
        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); // Nos aseguramos de que estadísticas esté apagada
        txtResultado.text = "¡BASE DESTRUIDA!";
        txtResultado.color = Color.red; 
        Time.timeScale = 0f; // El juego se congela
    }

    // --- ¡NUEVA FUNCIÓN PARA EL BOTÓN DE SALIR/RENDIRSE! ---
    public void Rendirse()
    {
        pantallaFinJuego.SetActive(true);
        pantallaEstadisticas.SetActive(false); // Nos aseguramos de que estadísticas esté apagada
        txtResultado.text = "¡TE HAS RENDIDO!"; // Cambiamos el mensaje
        txtResultado.color = new Color(1f, 0.5f, 0f); // Le puse color Naranja para diferenciarlo, ¡cámbialo si quieres!
        Time.timeScale = 0f; // El juego se congela
    }

    // El botón "Ver Estadísticas" de la pantalla de Fin de Partida llamará a esto
    public void AbrirEstadisticas()
    {
        pantallaFinJuego.SetActive(false);     // Apagamos la pantalla de derrota
        pantallaEstadisticas.SetActive(true);  // Encendemos la de estadísticas
    }

    // El botón "Volver al Menú" (tanto el de derrota como el de estadísticas) llamará a esto
    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Descongelamos el tiempo
        SceneManager.LoadScene("InterfazUsuario"); // Viajamos al menú principal
    }

    // --- NUEVO BOTÓN: VOLVER AL CARRUSEL DE MAPAS ---
    public void VolverASeleccionMapa()
    {
        Time.timeScale = 1f; // Descongelamos el tiempo

        // Dejamos una "nota secreta" para que el menú sepa qué abrir
        PlayerPrefs.SetInt("AbrirCarrusel", 1); 

        // Viajamos a la escena del menú
        SceneManager.LoadScene("InterfazUsuario"); 
    }

    // --- NUEVO BOTÓN: VOLVER DE ESTADÍSTICAS A FIN DE PARTIDA ---
    public void VolverAFinDePartida()
    {
        pantallaEstadisticas.SetActive(false); // Apagamos las estadísticas
        pantallaFinJuego.SetActive(true);      // Encendemos de nuevo la derrota
    }
}