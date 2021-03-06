using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Ziqi Li
/// Script for transition between scenes
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public Animator Animator;  // the crossfade transition animator
    public float TransitionTime = 2f;  // the transition animation length


    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading scene with transition effect
    /// The scene to be loaded has to be in build setting
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneWithName(string sceneName)
    {
        // using coroutine to delay the loading for having time playing the transition animation
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading scene with transition effect
    /// The scene to be loaded has to be in build setting
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneWithIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneWithTransition(sceneIndex));
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine function to load the level (scene) with transition animation 
    /// </summary>
    /// <param name="sceneIndexToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadSceneWithTransition(int sceneIndexToLoad)
    {
        // trigger the exit transition animation
        Animator.SetTrigger("IsExit");

        // wait for a delay before loading the next level (2 seconds to play the animation)
        yield return new WaitForSeconds(TransitionTime);

        // load the scene 
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine function to load the level (scene) with transition animation 
    /// </summary>
    /// <param name="sceneIndexToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // trigger the exit transition animation
        Animator.SetTrigger("IsExit");

        // wait for a delay before loading the next level (2 seconds to play the animation)
        yield return new WaitForSeconds(TransitionTime);

        // load the scene 
        SceneManager.LoadScene(sceneName);
    }
}
