using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Friendly object.");
                break;
            case "Finish":
                Debug.Log("Congrats, you finished the level!");
                LoadNextLevel();
                break;
            default:
                Debug.Log("Sorry, you blew up.");
                ReloadLevel();
                break;
        }
    }

    void ReloadLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    void LoadNextLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((activeSceneIndex + 1) % SceneManager.sceneCount);
    }
}
