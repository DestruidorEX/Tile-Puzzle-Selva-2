using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Script_botaoJogo : MonoBehaviour
{
    public void ComecarJogo()
    {
        SceneManager.LoadScene("Jogo");
    }
}

