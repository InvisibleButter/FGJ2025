using UnityEngine;

namespace Scripts
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private MyGameManager gameManager;
        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                gameManager.ChangeGameState(GameState.GameOver);
            }
        }
    }
}