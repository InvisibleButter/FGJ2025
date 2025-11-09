using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void Show()
        {
            panel.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
        }
        
        public void Retry()
        {
           SceneManager.LoadScene("MainScene");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}