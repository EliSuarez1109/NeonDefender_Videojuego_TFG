using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GestorPantallas : MonoBehaviour
{
    [Header("Conecta tus pantallas aquí")]
    public GameObject pantallaInicioSesion;
    public GameObject pantallaRegistro;
    public GameObject pantallaMenu;
    public GameObject pantallaHistorial;
    public GameObject pantallaEstadisticas;
    public GameObject pantallaSeleccionMapa; 
    public GameObject pantallaConfiguracion; // <--- ¡NUEVA PANTALLA AÑADIDA!

    [Header("Campos de Texto (Inputs)")]
    public TMP_InputField userLogin;
    public TMP_InputField passLogin;
    public TMP_InputField userRegister;
    public TMP_InputField passRegister;

    [Header("Configuración del Historial")]
    public GameObject prefabTarjeta;
    public Transform contenedorContent;

    [Header("Textos de Estadísticas")]
    public TMP_Text txtDanoRecibido;
    public TMP_Text txtDanoInfligido;
    public TMP_Text txtTotalTorres;

    [Header("Configuración AWS")]
    public string apiURL = "https://pr3m2sbom5.execute-api.us-east-1.amazonaws.com/default/L_Unity_Login_towerdefens";

    // --- NAVEGACIÓN BÁSICA ---

    public void IrARegistro()
    {
        DesactivarTodasLasPantallas();
        pantallaRegistro.SetActive(true);
    }

    public void IrAInicioSesion()
    {
        Debug.Log("¡El botón ha hecho clic y ha llamado a la función!"); 
        DesactivarTodasLasPantallas();
        pantallaInicioSesion.SetActive(true);
    }

    public void IrAMenuPrincipal()
    {
        DesactivarTodasLasPantallas();
        pantallaMenu.SetActive(true);
    }

    public void AbrirHistorial()
    {
        DesactivarTodasLasPantallas();
        pantallaHistorial.SetActive(true);
    }

    public void AbrirEstadisticas()
    {
        DesactivarTodasLasPantallas();
        pantallaEstadisticas.SetActive(true);

        // Datos de prueba para estadísticas
        txtDanoRecibido.text = "15,400";
        txtDanoInfligido.text = "42,850";
        txtTotalTorres.text = "128";
    }

    // <--- ¡NUEVA FUNCIÓN PARA ABRIR LOS AJUSTES! --->
    public void AbrirConfiguracion()
    {
        DesactivarTodasLasPantallas();
        pantallaConfiguracion.SetActive(true);
    }

    // Esta es la función que te faltaba para que no de error
    private void DesactivarTodasLasPantallas()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaRegistro.SetActive(false);
        pantallaMenu.SetActive(false);
        pantallaHistorial.SetActive(false);
        pantallaEstadisticas.SetActive(false);
        pantallaSeleccionMapa.SetActive(false);
        
        // Apagamos también la de configuración si está encendida
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 
    }

    void Start()
    {
        // El menú arranca y comprueba si hay una nota secreta de volver a jugar
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            // 1. Borramos la nota
            PlayerPrefs.SetInt("AbrirCarrusel", 0);

            // 2. Apagamos todo y encendemos el Carrusel
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true); 
        }
        else
        {
            // Si NO hay nota, abrimos la pantalla normal del menú
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true); // O la pantalla inicial que uses
        }
    }

    // --- LÓGICA DE AWS ---

    public void ClickLoginrapido()
    {
        IrAMenuPrincipal(); // Salto directo
    }

    public void ClickJugar()
    {
        DesactivarTodasLasPantallas();
        pantallaSeleccionMapa.SetActive(true); // Abre el carrusel
    }
    
    public void ClickLogin()
    {
        if (string.IsNullOrEmpty(userLogin.text) || string.IsNullOrEmpty(passLogin.text)) {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            return;
        }
        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    // --- CORRUTINA DE RED ---

    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        AuthRequest datos = new AuthRequest {
            nickname = user,
            contrasena = pass,
            accion = accion
        };

        string json = JsonUtility.ToJson(datos);

        Debug.Log("JSON enviado: " + json);
        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Conectando a AWS para {accion}...");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error de Red/DNS: " + request.error);
                Debug.LogError("Causa probable: URL mal escrita, falta de internet o firewall.");
            }
            else
            {
                Debug.Log("Respuesta recibida: " + request.downloadHandler.text);
                
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    if (accion == "registro") IrAInicioSesion();
                    else
                    {
                        IrAMenuPrincipal();
                        ActualizarHistorialReal(request.downloadHandler.text);
                    } 
                }
                else {
                    Debug.LogWarning("AWS rechazó la petición: " + request.downloadHandler.text);
                }
            }
        }
    }

    [System.Serializable]
    public class AuthRequest {
        public string nickname;
        public string contrasena;
        public string accion;
    }

    [System.Serializable]
    public class PartidaData{
        public int id_partida;
        public string fecha;
        public string estado;
        public string nivel;
        public int oro_ganado;
        public int total_enemigos;
        public int total_torres;
    }

    [System.Serializable]
    public class RespuestaAWS {
        public string status;
        public int id_user;
        public List<PartidaData> partidas;
    }

    // --- LÓGICA DE HISTORIAL (TARJETAS) ---

    private void ActualizarHistorialReal(string json)
    {
        RespuestaAWS datos = JsonUtility.FromJson<RespuestaAWS>(json);
        if (datos == null || datos.partidas == null || contenedorContent == null) return;

        foreach (Transform hijo in contenedorContent) { Destroy(hijo.gameObject); }

        foreach (PartidaData p in datos.partidas)
        {
            GameObject nueva = Instantiate(prefabTarjeta, contenedorContent, false);
            nueva.transform.localScale = Vector3.one;
            TarjetaPartida script = nueva.GetComponent<TarjetaPartida>();
            if (script != null)
            {
                script.ConfigurarTarjeta(
                    p.oro_ganado.ToString(),
                    p.fecha,
                    p.estado,
                    p.nivel,
                    p.total_enemigos.ToString(),
                    p.total_torres.ToString()
                );
            }
        }
    }
}