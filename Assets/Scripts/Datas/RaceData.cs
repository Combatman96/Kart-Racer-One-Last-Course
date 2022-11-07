using System;

[Serializable]
public class RaceData
{
    public KartName kartName;
    public int racePosition = 0;
    public int lap = 0;
    public float distance = 0;
    public long startRaceTicks = 0;
    public long endRaceTicks = 0;

    public RaceData() { }
    
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
