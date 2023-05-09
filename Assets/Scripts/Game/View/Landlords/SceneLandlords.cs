using UnityEngine;
using System.Collections;

public class SceneLandlords : MonoBehaviour {

    void Awake()
    {
        AudioManager.Instance.PlayMusic(GameModel.musicLandlords);
        LandlordsModel.InitAudio();
        GameService.Instance.GetGameStatus();
    }
}
