using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class StartManager : MonoBehaviour
{   
    public PlayerMovement playerMovement;
    [@SerializeField] private GameObject Restart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.isPlay == true){
            Restart.SetActive(false);
        }
    }

    public void Play(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
