using System.Collections;
using System.Collections.Generic;
using Kuchinashi;
using UnityEngine;

public class SettlementManager
{
    private static SettlementManager _instance;
    public static SettlementManager Instance
    {
        get => _instance ??= new SettlementManager();
        private set => _instance = value;
    }

    public static void SettleGame(int levelId, List<int> rakugoList)
    {
        AudioManager.Instance.SwitchMusic();
        GameProgressData.FinishRound(levelId, rakugoList);

        var levelConfig = GameDesignData.GetLevel(levelId);
        var totalInfluence = new Element();

        foreach (var rakugo in rakugoList)
        {
            totalInfluence += GameDesignData.GetRakugo(rakugo).Influence;
            GameProgressData.UnlockRakugo(rakugo);
        }

        var score = levelConfig.Weight[0] * totalInfluence.Hell
                  + levelConfig.Weight[1] * totalInfluence.Cold
                  + levelConfig.Weight[2] * totalInfluence.Ero
                  + levelConfig.Weight[3] * totalInfluence.Nonsense;

        var nextLevelId = Random.Range(0, GameDesignData.Instance.LevelList.Count);
        while (GameProgressData.Instance.RoundDataList.Find(x => x.LevelId == nextLevelId) != null)
        {
            nextLevelId = Random.Range(0, GameDesignData.Instance.LevelList.Count);
        }

        var deltaFans = 0;
        if (score >= 10) 
        {
            deltaFans = 2000 + Mathf.RoundToInt(Random.Range(500, (score - 10) * 100 + 500));
            SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "绝好调！你这不是超会讲笑话的嘛！", nextLevelId);
        }
        else if (score is < 10 and > -10)
        {
            deltaFans = Mathf.RoundToInt(Random.Range(0, (score + 10) * 50));
            SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "嘛，中规中矩吧！", nextLevelId);
        }
        else
        {
            deltaFans = -500 + Mathf.RoundToInt(Random.Range(-1000, score * 10));
            SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "喂喂，超不妙的啊。这不是根本没有让观众 GET 到笑点嘛……", nextLevelId);
        }
    }
}
