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
    public GameObject pantallaConfiguracion; 

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

    // ==========================================
    // NAVEGACIÓN BÁSICA
    // ==========================================

    public void IrARegistro()
    {
        DesactivarTodasLasPantallas();
        pantallaRegistro.SetActive(true);
    }

    public void IrAInicioSesion()
    {
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

    public void AbrirConfiguracion()
    {
        DesactivarTodasLasPantallas();
        pantallaConfiguracion.SetActive(true);
    }

    // Función para borrar los datos guardados y salir
    public void CerrarSesion()
    {
        Debug.Log("Cerrando sesión y borrando memoria...");
        PlayerPrefs.SetInt("UsuarioLogueado", 0);
        PlayerPrefs.Save();
        IrAInicioSesion();
    }

    private void DesactivarTodasLasPantallas()
    {
        pantallaInicioSesion.SetActive(false);
        pantallaRegistro.SetActive(false);
        pantallaMenu.SetActive(false);
        pantallaHistorial.SetActive(false);
        pantallaEstadisticas.SetActive(false);
        pantallaSeleccionMapa.SetActive(false);
        
        if (pantallaConfiguracion != null) pantallaConfiguracion.SetActive(false); 
    }

    // ==========================================
    // INICIO DEL JUEGO (PERSISTENCIA)
    // ==========================================

   /* void Start()
    {
        // 1. Comprueba si venimos de jugar un nivel (Carrusel)
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            PlayerPrefs.SetInt("AbrirCarrusel", 0);
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true); 
        }
        // 2. Comprueba si el usuario ya inició sesión antes
        else if (PlayerPrefs.GetInt("UsuarioLogueado", 0) == 1)
        {
            Debug.Log("✓ Sesión recordada. Saltando al Menú Principal.");
            DesactivarTodasLasPantallas();
            pantallaMenu.SetActive(true);
        }
        // 3. Si no hay datos, abre el Login normalmente
        else
        {
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true); 
        }
    }*/

     void Start()
    {
        // --- TRUCO DE DESARROLLADOR ---
        // Quita las dos barras diagonales (//) de la línea de abajo para borrar la memoria de Unity.
        // Cuando tu juego ya esté listo para publicarse, vuelve a ponerle las // para que recuerde a los jugadores.
        
        PlayerPrefs.DeleteAll(); 

        // ------------------------------

        // 1. Primero comprueba si venimos de jugar un nivel
        if (PlayerPrefs.GetInt("AbrirCarrusel", 0) == 1)
        {
            PlayerPrefs.SetInt("AbrirCarrusel", 0);
            DesactivarTodasLasPantallas();
            pantallaSeleccionMapa.SetActive(true); 
        }
        // 2. Comprueba si el usuario ya inició sesión antes
        else if (PlayerPrefs.GetInt("UsuarioLogueado", 0) == 1)
        {
            Debug.Log("✓ Sesión recordada. Saltando al Menú Principal.");
            DesactivarTodasLasPantallas();
            pantallaMenu.SetActive(true);
        }
        // 3. Si no hay nota ni sesión, abre el Login
        else
        {
            DesactivarTodasLasPantallas();
            pantallaInicioSesion.SetActive(true); 
        }
    }

    // ==========================================
    // LÓGICA DE BOTONES Y CONEXIÓN
    // ==========================================

    public void ClickLoginrapido()
    {
        IrAMenuPrincipal(); 
    }

    public void ClickJugar()
    {
        DesactivarTodasLasPantallas();
        pantallaSeleccionMapa.SetActive(true); 
    }
    
    public void ClickLogin()
    {
        if (string.IsNullOrEmpty(userLogin.text) || string.IsNullOrEmpty(passLogin.text)) {
            Debug.LogWarning("Por favor, rellena todos los campos.");
            return;
        }

        // --- MODO DESARROLLO SIN AWS (TEMPORAL) ---
       // Debug.Log("Simulando conexión a AWS... ¡Login Exitoso!");
        
        // Guardamos la sesión en memoria
        //PlayerPrefs.SetInt("UsuarioLogueado", 1);
        //PlayerPrefs.Save();
        
        // Entramos al menú principal
        //IrAMenuPrincipal();
        // ------------------------------------------

        // NOTA: Cuando vuelvas a encender tu servidor AWS, borra el bloque "MODO DESARROLLO" 
        // de arriba y quítale las dos barras (//) a la línea de abajo para activar la conexión real:
        StartCoroutine(EnviarPeticion("login", userLogin.text, passLogin.text));
    }

    public void ClickRegistro()
    {
        StartCoroutine(EnviarPeticion("registro", userRegister.text, passRegister.text));
    }

    // ==========================================
    // CORRUTINA DE RED (AWS)
    // ==========================================

    IEnumerator EnviarPeticion(string accion, string user, string pass)
    {
        AuthRequest datos = new AuthRequest {
            nickname = user,
            contrasena = pass,
            accion = accion
        };

        string json = JsonUtility.ToJson(datos);

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error de Red/DNS: " + request.error);
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    if (accion == "registro") IrAInicioSesion();
                    else
                    {
                        // Guardamos el Login en memoria real con AWS
                        PlayerPrefs.SetInt("UsuarioLogueado", 1);
                        PlayerPrefs.Save();

                        if (GestorDatosPartida.instancia != null)
                        {
                            RespuestaAWS awsDatos = JsonUtility.FromJson<RespuestaAWS>(request.downloadHandler.text);
                            if (awsDatos != null)
                            {
                                GestorDatosPartida.instancia.datosPartida.id_user = awsDatos.id_user;
                                Debug.Log("✓ id_user guardado en GestorDatosPartida: " + awsDatos.id_user);
                            }
                        }

                        IrAMenuPrincipal();
                        ActualizarHistorialReal(request.downloadHandler.text);
                    } 
                }
            }
        }
    }

    // ==========================================
    // CLASES DE ESTRUCTURA DE DATOS
    // ==========================================

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

    // ==========================================
    // LÓGICA DE HISTORIAL (TARJETAS)
    // ==========================================

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