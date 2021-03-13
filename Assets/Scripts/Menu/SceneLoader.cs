using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Ziqi Li
/// Script for transition between scenes
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public Animator Animator;  // the crossfade transition animator
    private PhotonView PhotonView;

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading Photon scene with transition effect
    /// The scene to be loaded has to be in build setting
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadPhotonSceneWithName(string sceneName)
    {
        // call the RPC loading function
        PhotonView.RPC("RPC_LoadSceneWithName", RpcTarget.All, sceneName);
    }

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
    /// RPC function for loading Photon scene with transition effect
    /// The scene to be loaded has to be in build setting
    /// </summary>
    /// <param name="sceneName"></param>
    [PunRPC]
    private void RPC_LoadSceneWithName(string sceneName)
    {
        StartCoroutine(LoadPhotonSceneWithTransition(sceneName));
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
        // get the current animation duration time
        float animDuration = Animator.GetCurrentAnimatorStateInfo(0).length;

        // wait for a delay before loading the next level 
        yield return new WaitForSeconds(animDuration);

        // load the scene 
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine function to load the level (scene) with transition animation 
    /// </summary>
    /// <param name="sceneNameToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadSceneWithTransition(string sceneNameToLoad)
    {
        // trigger the exit transition animation
        Animator.SetTrigger("IsExit");
        float animDuration = Animator.GetCurrentAnimatorStateInfo(0).length;

        // wait for a delay before loading the next level 
        yield return new WaitForSeconds(animDuration);

        // load the scene 
        SceneManager.LoadScene(sceneNameToLoad);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine function to load the Photon level (scene) with transition animation 
    /// </summary>
    /// <param name="sceneIndexToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadPhotonSceneWithTransition(string sceneNameToLoad)
    {
        // trigger the exit transition animation
        Animator.SetTrigger("IsExit");
        float animDuration = Animator.GetCurrentAnimatorStateInfo(0).length;

        // wait for a delay before loading the next level 
        yield return new WaitForSeconds(animDuration);

        // Let the master client to load the scene
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(sceneNameToLoad);
    }
}
