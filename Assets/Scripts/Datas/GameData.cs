using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public KartName playerKartName;
    public GameMode gameMode;
    public int maxKart = 5;
    public SceneName track;

    public void SetMaxKart(int amount)
    {
        maxKart = Mathf.Clamp(amount, 2, 5);
    }
}

public enum GameMode
{

}
