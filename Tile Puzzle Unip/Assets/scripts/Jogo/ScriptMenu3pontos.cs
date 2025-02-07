using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu2 : MonoBehaviour
{
    public GameObject settingsPanel2; // painel das configurações
    public Slider volumeSlider2; // slider de volume
    public AudioSource audioSource2; //som

    private void Start()
    {
        // carrega o volume que já foi salvo
        float VolumeSalvo = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider2.value = VolumeSalvo;
        AjustarVolume2(VolumeSalvo);
    }

    public void AbrirSettings2()
    {
        settingsPanel2.SetActive(true); // abre as configurações
    }

    public void FecharSettings2()
    {
        settingsPanel2.SetActive(false); // fecha as configurações
    }

    public void AjustarVolume2(float volume)
    {
        AplicarVolume2(volume);
        PlayerPrefs.SetFloat("Volume", volume); // salva o volume ajustado
    }

    private void AplicarVolume2(float volume)
    {
        AudioListener.volume = volume; // transforma o volume global

        if (audioSource2 != null)
        {
            audioSource2.mute = volume <= 0.01f; // muta se estiver quase zero, fiz isso pq tava dando problema 
            audioSource2.volume = Mathf.Clamp(volume, 0.05f, 1f);
        }
    }
}
