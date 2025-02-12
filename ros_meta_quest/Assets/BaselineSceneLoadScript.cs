using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoadScript : MonoBehaviour
{
    public string sceneToLoad = "UserStudyScene";
    public string cameraTag = "NewSceneCamera";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        Debug.Log("Loading Scene...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        // Wait until the new scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded successfully");

        /*
        // Find and activate cameras from the new scene
        Camera[] newSceneCameras = GameObject.FindGameObjectsWithTag(cameraTag).Select(obj => obj.GetComponent<Camera>()).ToArray();
        foreach (Camera camera in newSceneCameras) 
        {
            Debug.Log("Activating camera: " + camera.gameObject.name);
            camera.enabled = true;
        }
        */
        //SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
    }
}
