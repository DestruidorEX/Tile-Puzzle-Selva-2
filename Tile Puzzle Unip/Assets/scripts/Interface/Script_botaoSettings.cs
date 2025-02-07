using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; // painel das configura��es
    public Slider volumeSlider; // slider de volume
    public AudioSource audioSource; //som

    private void Start()
    {
        // carrega o volume que j� foi salvo
        float VolumeSalvo = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = VolumeSalvo;
        AjustarVolume(VolumeSalvo);
    }

    public void AbrirSettings()
    {
        settingsPanel.SetActive(true); // abre as configura��es
    }

    public void FecharSettings()
    {
        settingsPanel.SetActive(false); // fecha as configura��es
    }

    public void AjustarVolume(float volume)
    {
        AplicarVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume); // salva o volume ajustado
    }

    private void AplicarVolume(float volume)
    {
        AudioListener.volume = volume; // transforma o volume global

        if (audioSource != null)
        {
            audioSource.mute = volume <= 0.01f; // muta se estiver quase zero, fiz isso pq tava dando problema 
            audioSource.volume = Mathf.Clamp(volume, 0.05f, 1f); 
        }
    }
}
