using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionTorre : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject rangoVisual; // El anillo de neón que copiamos

    // Esta variable 'static' es compartida por todas las torres.
    // Sirve para recordar cuál fue la última torre que tocamos.
    private static SeleccionTorre torreSeleccionadaActual;

    // Unity llama automáticamente a esta función cuando haces clic en el Collider de este objeto
    void OnMouseDown()
    {
        // Si hacemos clic encima de un botón de la UI, ignoramos el clic
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Si ya había OTRA torre seleccionada distinta a esta, la apagamos
        if (torreSeleccionadaActual != null && torreSeleccionadaActual != this)
        {
            torreSeleccionadaActual.Deseleccionar();
        }

        // Encendemos el láser de ESTA torre y la guardamos como la actual
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
        // Si yo soy la torre seleccionada, vigilo si el jugador hace clic en otra parte
        if (torreSeleccionadaActual == this && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Lanzamos un rayo invisible desde el ratón a ver qué tocamos
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Si pinchamos en el vacío (suelo) o en un objeto que NO sea esta torre, me deselecciono
            if (hit.collider == null || hit.collider.gameObject != this.gameObject)
            {
                Deseleccionar();
                torreSeleccionadaActual = null;
            }
        }
    }
}