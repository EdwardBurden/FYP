using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    private bool playerSetting;

    public void Test() {
    
        SceneManager.LoadScene(0);
   
    }

    public void SetPlayerMode(bool newvalue) {
        playerSetting = newvalue;
    }

}
