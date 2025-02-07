using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Script_botaoTutorial : MonoBehaviour
{
    public void ComecarTutorial()
    {
        SceneManager.LoadScene("Tutorial"); //carrega a cena Tutorial
    }
}
