using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionTorre : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject rangoVisual; // El anillo de neón que copiamos

    // ¡CAMBIO!: Ahora es public para que GestorTorres sepa cuál está seleccionada
    public static SeleccionTorre torreSeleccionadaActual;

    // --- MAGIA NUEVA: Datos de Venta ---
    [HideInInspector] public int precioPagado;
    [HideInInspector] public int indiceCatalogo;

    // Esta función la llamará el Gestor justo al construir la torre
    public void ConfigurarDatosDeCompra(int precio, int indice)
    {
        precioPagado = precio;
        indiceCatalogo = indice;
    }
    // -----------------------------------

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (torreSeleccionadaActual != null && torreSeleccionadaActual != this)
        {
            torreSeleccionadaActual.Deseleccionar();
        }

        if (rangoVisual != null) rangoVisual.SetActive(true);
        torreSeleccionadaActual = this;
    }

    public void Deseleccionar()
    {
        if (rangoVisual != null)
        {
            rangoVisual.SetActive(false);
        }
    }

    void Update()
    {
        if (torreSeleccionadaActual == this && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null || hit.collider.gameObject != this.gameObject)
            {
                Deseleccionar();
                torreSeleccionadaActual = null;
            }
        }
    }
}