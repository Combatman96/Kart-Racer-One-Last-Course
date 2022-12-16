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
        if (records.Count > 0) return;
        var tracks = Enum.GetValues(typeof(SceneName)).Cast<SceneName>().ToList();
        foreach (var track in tracks)
        {
            records.Add(new Record(track, new RaceData()));
        }
    }

    public RaceData GetRecordDataByTrack(SceneName track)
    {
        return records.SingleOrDefault(x => x.track == track).raceData;
    }

    public Record GetRecordByTrack(SceneName track)
    {
        return records.SingleOrDefault(x => x.track == track);
    }

    public bool IsNewRecord(SceneName track, RaceData raceData)
    {
        InitRecords();
        RaceData data = GetRecordDataByTrack(track);
        var newTime = raceData.endRaceTicks - raceData.startRaceTicks;
        var oldTime = data.endRaceTicks - data.startRaceTicks;
        if (oldTime > newTime || oldTime == 0)
            return true;
        return false;
    }

    public void UpdateRaceData(SceneName track, RaceData raceData)
    {
        var data = GetRecordDataByTrack(track);
        data = raceData;
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

    public int GetRecordPos()
    {
        return raceData.racePosition;
    }

    public string GetRecordTime()
    {
        return raceData.GetRaceTime();
    }
}

[Serializable]
public class GameSetting
{
    public int lapPerRace = 1;
}
