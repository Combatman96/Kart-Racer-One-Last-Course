using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Configs/Data/SceneData")]
public class SceneData : ScriptableObject
{  
    public SceneName scene;
    public int buidldIndex;
}