using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private int sceneToLoadNum;
    [SerializeField] private string sceneToLoadName = "Scene_";
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float waitToLoadTime = 1f;
    [SerializeField] private bool portalActive = true;



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (portalActive && other.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadScene());
        }
    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoadName + sceneToLoadNum);
    }
}
