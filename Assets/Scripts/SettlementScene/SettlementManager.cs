using System;
using System.Collections;
using System.Collections.Generic;
using Kuchinashi;
using UnityEngine;
using Random = UnityEngine.Random;

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

        var score = Math.Abs(levelConfig.Weight[0] * totalInfluence.Hell
                  + levelConfig.Weight[1] * totalInfluence.Cold
                  + levelConfig.Weight[2] * totalInfluence.Ero
                  + levelConfig.Weight[3] * totalInfluence.Nonsense);
            var deltaFans = 0;
        if (GameProgressData.Instance.RoundDataList.Count < 5)
        {
            var nextLevelId = Random.Range(0, GameDesignData.Instance.LevelList.Count);
            while (GameProgressData.Instance.RoundDataList.Find(x => x.LevelId == nextLevelId) != null)
            {
                nextLevelId = Random.Range(0, GameDesignData.Instance.LevelList.Count);
            }

            if (score > 4)
            {
                deltaFans = Mathf.RoundToInt((float)(100 * (score + 1) * Math.Log(score + 1) - score + 1000));
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "绝好调！你这不是超会讲笑话的嘛！", nextLevelId);
            }
            else if (score is <= 4 and >= 3)
            {
                deltaFans = Mathf.RoundToInt(1000 * score);
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "嘛，中规中矩吧！", nextLevelId);
            }
            else
            {
                deltaFans = Mathf.RoundToInt(500 * score);
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "喂喂，超不妙的啊。这不是根本没有让观众 GET 到笑点嘛……", nextLevelId);
            }
            DataRepeater.Instance.CurrentLevelId = nextLevelId;
        }
        else
        {
            if (score > 4)
            {
                deltaFans = Mathf.RoundToInt((float)(100 * (score + 1) * Math.Log(score + 1) - score + 1000));
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "绝好调！你这不是超会讲笑话的嘛！");
            }
            else if (score is <= 4 and >= 3)
            {
                deltaFans = Mathf.RoundToInt(1000 * score);
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "嘛，中规中矩吧！");
            }
            else
            {
                deltaFans = Mathf.RoundToInt(500 * score);
                SettlementSceneController.Action(Mathf.RoundToInt(score), DataRepeater.Instance.CurrentFans, deltaFans, "喂喂，超不妙的啊。这不是根本没有让观众 GET 到笑点嘛……");
            }
        }
        DataRepeater.Instance.CurrentFans += deltaFans;
        DataRepeater.Instance.CurrentElements += totalInfluence;
    }
}
