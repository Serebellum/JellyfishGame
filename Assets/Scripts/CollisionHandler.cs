using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float timeToWait = 1.5f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip winAudio;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem winParticles;

    AudioSource audioSource;

    int sceneCount;
    bool isTransitioning = false;
    bool disableCrashing = false;

    public void EnableCrashing() {
        disableCrashing = false;
    }
    public void DisableCrashing() {
        disableCrashing = true;
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }

    private void OnCollisionEnter(Collision other) {
        if (isTransitioning) return;

        switch (other.gameObject.tag) {
            case "Friendly":
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
        if (disableCrashing) {
            return;
        }
        isTransitioning = true;
        GetComponent<Movement>().DisableMovementParticles();
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
        Invoke("ReloadLevel", timeToWait);
    }

    void WinLevelSequence() {
        isTransitioning = true;
        GetComponent<Movement>().DisableMovementParticles();
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(winAudio);
        winParticles.Play();
        Invoke("LoadNextLevel", timeToWait);
    }

    void ReloadLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        isTransitioning = false;
    }

    public void LoadNextLevel() {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((activeSceneIndex + 1) % sceneCount);
        isTransitioning = false;
    }
}
