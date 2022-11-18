using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ArcadeConfig", menuName = "Configs/ArcadeConfig")]
public class ArcadeConfig : ScriptableObject
{
    public List<SceneData> trackScenes = new List<SceneData>();

    public int GetSceneBuildIndex(SceneName sceneName)
    {
        return trackScenes.SingleOrDefault(x => x.scene == sceneName).buidldIndex;
    }
}

