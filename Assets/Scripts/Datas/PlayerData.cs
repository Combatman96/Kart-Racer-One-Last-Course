using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PlayerData 
{
    public GameSetting gameSetting;

    public List<Record> records = new List<Record>();

    public void InitRecords()
    {
        if(records.Count > 0) return;
        var tracks = Enum.GetValues(typeof(SceneName)).Cast<SceneName>().ToList();
        foreach(var track in tracks)
        {
            records.Add(new Record(track, new RaceData()));
        }
    }

    public RaceData GetRecordByTrack(SceneName track)
    {
        return records.SingleOrDefault(x => x.track == track).raceData;
    }
}

[Serializable]
public class Record 
{
    public SceneName track;
    public RaceData raceData;

    public Record(SceneName track, RaceData raceData)
    {
        this.track = track;
        this.raceData = raceData;
    }
}

[Serializable]
public class GameSetting
{
    public int lapPerRace = 3;
}
