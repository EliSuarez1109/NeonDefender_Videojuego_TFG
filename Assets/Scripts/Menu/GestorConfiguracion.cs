using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class GestorConfiguracion : MonoBehaviour
{
    [Header("Conexiones Audio")]
    public AudioMixer mezcladorAudio;

    [Header("Controladores de volumen")]
    public Slider sliderMusica;
    public Slider sliderFX;

    private const float MIN_VOL = 0.0001f;

    void OnEnable()
    {
        // Modificar sliders segun el volumen
        if (sliderMusica != null) sliderMusica.value = PlayerPrefs.GetFloat("GuardadoVolMusica", 0.5f);
        if (sliderFX != null) sliderFX.value = PlayerPrefs.GetFloat("GuardadoVolFXs", 0.5f);
    }

    public void CambiarVolumenMusica(float valorSlider)
    {
        mezcladorAudio.SetFloat("VolMusica", Mathf.Log10(Mathf.Max(MIN_VOL, valorSlider)) * 20);

        PlayerPrefs.SetFloat("GuardadoVolMusica", valorSlider);
        PlayerPrefs.Save();
    }

    public void CambiarVolumenFX(float valorSlider)
    {
        mezcladorAudio.SetFloat("VolFXs", Mathf.Log10(Mathf.Max(MIN_VOL, valorSlider)) * 20);

        PlayerPrefs.SetFloat("GuardadoVolFXs", valorSlider);
        PlayerPrefs.Save();
    }

    public void CambiarModoPantalla(int indiceDropdown)
    {
        switch (indiceDropdown)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }
}