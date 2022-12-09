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

    public SceneName GetNextScene(SceneName sceneName)
    {
        SceneName nextScene = SceneName.TitleScreen;
        int curIndex = trackScenes.FindIndex(x => x.scene == sceneName);
        if (curIndex >= trackScenes.Count - 1)
        {
            return nextScene;
        }
        int nextIndex = curIndex + 1;
        nextScene = trackScenes[nextIndex].scene;
        return nextScene;
    }
}

