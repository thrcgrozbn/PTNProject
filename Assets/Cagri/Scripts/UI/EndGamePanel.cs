using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cagri.Scripts.UI
{
    public class EndGamePanel : MonoBehaviour
    {


        public void RestartButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        public void QuitGameButtonClicked()
        {
            StopAllCoroutines();
            Application.Quit();
        }
       
    }
}