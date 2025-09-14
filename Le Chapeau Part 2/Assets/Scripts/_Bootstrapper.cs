using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Bootstrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Load Menu scene right after setting up managers
        SceneManager.LoadScene("Menu");
        
    }
}
