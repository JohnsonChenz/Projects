using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScnene : MonoBehaviour {

	// Use this for initialization
      public void SceneLoader(int SceneIndex)
      {
        SceneManager.LoadScene(SceneIndex);
      }

      public void Exitgame()
      {
        Application.Quit();
      }


      public void Restartgame()
      {
        Application.LoadLevel(Application.loadedLevel);
      }

}
