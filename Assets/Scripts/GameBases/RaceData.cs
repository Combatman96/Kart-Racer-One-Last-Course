using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceData
{
    public KartName kartName;
    public int racePosition;
    public int lap;
    public float distance;
    public long startRaceTicks;
    public long endRaceTicks;

    public RaceData(KartName kartName, int lap)
    {
        this.kartName = kartName;
        this.lap = lap;
    }

    public void SetStartRaceTicks(DateTime dateTime) 
    {
        startRaceTicks = dateTime.Ticks;
    }

    public void SetEndRaceTicks(DateTime dateTime) 
    {
        endRaceTicks = dateTime.Ticks;
    }

    public string GetRaceTime()
    {
        DateTime raceTime = new DateTime(endRaceTicks - startRaceTicks);
        string raceTimeStr = raceTime.ToString("mm:ss.s");
        return raceTimeStr;
    }
}
