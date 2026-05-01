using UnityEngine;
using TMPro;

public class CargarEstadisticas : MonoBehaviour
{
    [Header("Textos de la Interfaz")]
    public TextMeshProUGUI txtOroGanado;
    public TextMeshProUGUI txtOroGastado;
    public TextMeshProUGUI txtDanoRecibido;
    public TextMeshProUGUI txtDanoInfligido;
    
    // --- NUEVO: Huecos para Torretas y Enemigos ---
    public TextMeshProUGUI txtTorretas;
    public TextMeshProUGUI txtEnemigos;
    // ----------------------------------------------

    void Start()
    {
        // Al encenderse la pantalla, cargamos los datos
        MostrarDatos();
    }

    public void MostrarDatos()
    {
        // Verificamos que el Gestor sobrevivió y existe en esta escena
        if (GestorDatosPartida.instancia != null)
        {
            // 1. Cargamos los datos directos (Oro y Daño)
            txtOroGanado.text = GestorDatosPartida.instancia.datosPartida.oro_ganado.ToString();
            txtOroGastado.text = GestorDatosPartida.instancia.datosPartida.oro_gastado.ToString();
            txtDanoRecibido.text = GestorDatosPartida.instancia.datosPartida.dano_total_recibido.ToString();
            txtDanoInfligido.text = GestorDatosPartida.instancia.datosPartida.dano_total_infligido.ToString();
            
            // 2. --- NUEVO: Lógica para sumar todas las Torretas ---
            int totalTorretas = 0;
            // Recorremos la lista de torres y sumamos sus cantidades
            foreach (DetalleTorre torre in GestorDatosPartida.instancia.datosPartida.torres)
            {
                totalTorretas += torre.cantidad;
            }
            // Si asignaste el texto en el Inspector, mostramos el total
            if (txtTorretas != null) txtTorretas.text = totalTorretas.ToString();

            // 3. --- NUEVO: Lógica para sumar todos los Enemigos Derrotados ---
            int totalEnemigos = 0;
            // Recorremos la lista de enemigos y sumamos sus cantidades
            foreach (DetalleEnemigo enemigo in GestorDatosPartida.instancia.datosPartida.enemigos)
            {
                totalEnemigos += enemigo.cantidad;
            }
            // Si asignaste el texto en el Inspector, mostramos el total
            if (txtEnemigos != null) txtEnemigos.text = totalEnemigos.ToString();
            
            Debug.Log("✓ Todas las estadísticas (incluyendo torretas y enemigos) se cargaron correctamente.");
        }
        else
        {
            Debug.LogWarning("✗ No se encontró GestorDatosPartida. Recuerda iniciar el juego desde la escena del Menú para que se genere.");
        }
    }
}