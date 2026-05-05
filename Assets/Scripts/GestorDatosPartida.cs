using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GestorDatosPartida : MonoBehaviour
{
    public static GestorDatosPartida instancia;

    [Header("Datos de la Partida")]
    public PartidaJSON datosPartida = new PartidaJSON();

    [Header("AWS Guardado de Partida")]
    public string savePartidaAPI = "https://q2v7qbfux6.execute-api.us-east-1.amazonaws.com/default/L_Unity_Save_TD";

    void Awake()
    {
        // Configuramos el Singleton para acceder desde cualquier lado
        if (instancia == null)
        {
            instancia = this;
            // Hacemos que persista entre cambios de escena
            DontDestroyOnLoad(gameObject);
            Debug.Log("✓ GestorDatosPartida inicializado");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Verificamos que esté inicializado
        if (instancia == null)
        {
            Debug.LogError("✗ ERROR: GestorDatosPartida no está inicializado");
        }
        else
        {
            Debug.Log("✓ GestorDatosPartida disponible en escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    // Métodos de acceso rápido
    public void RegistrarTorre(string nombreTorre) => datosPartida.RegistrarTorre(nombreTorre);
    public void DesvincularTorre(string nombreTorre) => datosPartida.DesvincularTorre(nombreTorre);
    public void RegistrarEnemigo(string nombreEnemigo) => datosPartida.RegistrarEnemigo(nombreEnemigo);
    public void AgregarEnemigoActivo(string nombreEnemigo) => datosPartida.AgregarEnemigoActivo(nombreEnemigo);
    public void RemoverEnemigoActivo(string nombreEnemigo) => datosPartida.RemoverEnemigoActivo(nombreEnemigo);
    public void RegistrarOroGanado(int cantidad) => datosPartida.RegistrarOroGanado(cantidad);
    public void RegistrarOroGastado(int cantidad) => datosPartida.RegistrarOroGastado(cantidad);
    public void RestarOroGastado(int cantidad) => datosPartida.RestarOroGastado(cantidad);
    public void RegistrarDanoRecibido(float cantidad) => datosPartida.RegistrarDanoRecibido(cantidad);
    public void RegistrarDanoInfligido(float cantidad) => datosPartida.RegistrarDanoInfligido(cantidad);
    public void RegistrarRondaCompletada() => datosPartida.RegistrarRondaCompletada();
    public void EstablecerEstado(string estado) => datosPartida.EstablecerEstado(estado);
    public void EstablecerNivel(string nombreNivel) => datosPartida.EstablecerNivel(nombreNivel);

    public void GuardarPartidaAWS()
    {
        StartCoroutine(EnviarPartidaAWS());
    }

    IEnumerator EnviarPartidaAWS()
    {
        string json = JsonUtility.ToJson(datosPartida);

        using (UnityWebRequest request = new UnityWebRequest(savePartidaAPI, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error guardando partida: " + request.error);
            }
            else
            {
                Debug.Log("Partida enviada correctamente: " + request.responseCode + " | " + request.downloadHandler.text);
            }
        }
    }

    // public void ResetDatosPartida()
    // {
    //     //int userId = datosPartida.id_user;
    //     datosPartida = new PartidaJSON();
    //     //datosPartida.id_user = userId;
    //     datosPartida.estado = string.Empty;
    // }

    public void ResetDatosPartida()
    {
        // 1. Guardamos el ID del usuario actual antes de borrar nada
        int userId = datosPartida.id_user; 
        
        // 2. Creamos una partida completamente en blanco
        datosPartida = new PartidaJSON();
        
        // 3. Le devolvemos su ID de usuario para que siga logueado
        datosPartida.id_user = userId;
        datosPartida.estado = string.Empty;
    }

    
}
