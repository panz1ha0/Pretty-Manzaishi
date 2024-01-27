using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameProgressData
{
    private static GameProgressData _instance;
    public static GameProgressData Instance
    {
        get => _instance ??= new GameProgressData();
        private set => _instance = value;
    }

    public List<RoundData> RoundDataList = new List<RoundData>();

    public static void FinishRound(int levelId, List<int> usedRakugoId)
    {
        var round = new RoundData() {
            LevelId = levelId,
            UsedRakugo = usedRakugoId
        };
        Instance.RoundDataList.Add(round);
    }

    public static RoundData GetLastRoundData()
    {
        return Instance.RoundDataList.Count == 0 ? null : Instance.RoundDataList[Instance.RoundDataList.Count - 1];
    }
}

public class RoundData
{
    public List<int> UsedRakugo = new List<int>();
    public int LevelId;
}
