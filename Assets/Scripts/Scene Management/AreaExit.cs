using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float waitToLoadTime = 1f;

    private SceneManagement sceneManagement;
    private void Awake()
    {
        sceneManagement = SceneManagement.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadScene());
        }
    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene("Scene_" + sceneToLoad);
    }
}
