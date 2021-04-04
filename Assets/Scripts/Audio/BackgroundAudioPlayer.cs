using UnityEngine;

public class BackgroundAudioPlayer : MonoBehaviour {
    public static BackgroundAudioPlayer instance;
    public GameObject BackgroundMusic;
    
    void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(Instantiate(BackgroundMusic));
        }
        
        
    }
}
