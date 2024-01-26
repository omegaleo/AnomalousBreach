using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ShutdownButton : MonoBehaviour
{
    public void Shutdown()
    {
        AudioManager.instance.Play("Shutdown");
        SceneManager.LoadScene(0);
    }
}
