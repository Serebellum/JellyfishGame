using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float timeToWait = 1.5f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip winAudio;

    AudioSource audioSource;

    int sceneCount;
    bool isTransitioning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }

    private void OnCollisionEnter(Collision other) {
        if (isTransitioning) return;

        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Friendly object.");
                break;
            case "Finish":
                WinLevelSequence();
                break;
            default:
                CrashSequence();
                break;
        }
    }

    void CrashSequence() {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        Invoke("ReloadLevel", timeToWait);
    }

    void WinLevelSequence() {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(winAudio);
        Invoke("LoadNextLevel", timeToWait);
    }

    void ReloadLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        isTransitioning = false;
    }

    void LoadNextLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((activeSceneIndex + 1) % sceneCount);
        isTransitioning = false;
    }
}
