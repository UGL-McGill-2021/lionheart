using System.Collections;
using System.Collections.Generic;
using Lionheart.Player.Movement;
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
        if(GameObject.FindGameObjectWithTag("GameManager") != null) StartCoroutine(ControlActivationDelay());
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine for delay the player control activation
    /// </summary>
    IEnumerator ControlActivationDelay()
    {
        yield return new WaitForFixedUpdate();  // wait for next frame to update the animator current state
        float animDuration = Animator.GetCurrentAnimatorStateInfo(0).length;
        foreach (GameObject player in GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList)
        {
            player.GetComponent<MultiplayerActivator>().ActivatePlayer();
            yield return new WaitForFixedUpdate();  // wait for next frame to update the animator current state
            player.GetComponent<MultiplayerActivator>().DisableControls();
        }


        // wait for a delay before loading the next level 
        yield return new WaitForSeconds(animDuration);
        foreach(GameObject player in GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList)
        {
            player.GetComponent<MultiplayerActivator>().ActivatePlayer();
        }
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
        yield return new WaitForFixedUpdate();  // wait for next frame to update the animator current state
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
        yield return new WaitForFixedUpdate();  // wait for next frame to update the animator current state
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
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForFixedUpdate();  // wait for next frame to update the animator current state
        float animDuration = Animator.GetCurrentAnimatorStateInfo(0).length;

        // wait for a delay before loading the next level 
        yield return new WaitForSeconds(animDuration);

        // Let the master client to load the scene
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneNameToLoad);
        }
    }
}
