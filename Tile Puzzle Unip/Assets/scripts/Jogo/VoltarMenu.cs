using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Script_botaoVoltarMenu : MonoBehaviour
{
    public void VoltarMenu()
    {
        SceneManager.LoadScene("Interface_Inicio"); //volta para o menu
    }
}

