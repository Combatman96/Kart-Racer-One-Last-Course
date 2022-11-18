using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneConfig", menuName = "Configs/SceneConfig")]
public class SceneConfig : ScriptableObject
{
    public List<SceneData> sceneDatas = new List<SceneData>();

    public int GetSceneBuildIndex(SceneName sceneName)
    {
        return sceneDatas.SingleOrDefault(x => x.scene == sceneName).buidldIndex;
    }
}


public enum SceneName
{
    TitleScreen = 0,
    OptionsScreen = 1,
    KartSelectScreen = 2,
    CircusTrack,
    SpaceTrack,
    CityTrack
}
