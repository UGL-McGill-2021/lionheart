using UnityEngine;

public class BackgroundAudioPlayer : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(this);
    }
}
