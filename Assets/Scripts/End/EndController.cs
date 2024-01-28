using Kuchinashi;
using Kuchinashi.SceneControl;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    Image image;
    TMP_Text discription;
    Button button;
    bool canGoBack = false;

    string[] EndDiscriptions = new string[]
    {
        "通过你有意的口才和明智的选择，突破了10000粉丝后，大众认定你是开了艺眼的合格落语家！出道成功！(✿◡‿◡)",
        "你讲的落语不够地狱，大众不喜欢你，演出冷场了。/_ \\",
        "你讲的落语不够冷，大众不喜欢你，演出冷场了。/_ \\",
        "你讲的落语不够工口，大众不喜欢你，演出冷场了。/_ \\",
        "你讲的落语不够无厘头，大众不喜欢你，演出冷场了。/_ \\",
        "你的演出过于地狱......整个场馆被火海吞噬，地狱降临了。(ﾟДﾟ*)ﾉ",
        "你的演出过于冰冷......宛如一阵强劲的北冰洋的风，地球陷入了第二次冰川期。ε(┬┬﹏┬┬)3",
        "你的演出过于污......由于不可抗力缘故被全网下架，查水表了。X﹏X",
        "你的演出太没品了......落语界评定你为不入三教九流者，最多也就只能给呆头那种人讲讲笑话了。＞︿＜",
        "你的演出平平无奇，最终还是没能突破万粉，真废物啊(* ￣︿￣)"
    };

    //public Dictionary<string, Sprite> EndImage;
    public Sprite[] EndImage;

    private void Awake()
    {
        image = GameObject.Find("Image").GetComponent<Image>();
        discription = GameObject.Find("Discription").GetComponent<TMP_Text>();
        button = GetComponentInChildren<Button>();
        Color color = image.color;
        color.a = 0;
        image.color = color;
        discription.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        button.onClick.AddListener(() =>
        {
            SceneControl.SwitchSceneWithoutConfirm("StartScene");
        });
        button.gameObject.SetActive(false);
    }
    private void Start()
    {
        ChooseEnd();
    }
    void Update()
    {
        if (!canGoBack) StartCoroutine(ShowEnd());
    }
    private void ChooseEnd()
    {
        Element FinalElement = DataRepeater.Instance.CurrentElements;
        int FinalFans = DataRepeater.Instance.CurrentFans;
        int Cold = FinalElement.Cold;
        int Hell = FinalElement.Hell;
        int Ero = FinalElement.Ero;
        int Nonsense = FinalElement.Nonsense;
        if (Cold <= -15)
        {
            image.sprite = EndImage[0];
            discription.SetText($"{EndDiscriptions[2]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Hell <= -15)
        {
            image.sprite = EndImage[0];
            discription.SetText($"{EndDiscriptions[1]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Ero <= -15)
        {
            image.sprite = EndImage[0];
            discription.SetText($"{EndDiscriptions[3]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Nonsense <= -15)
        {
            image.sprite = EndImage[0];
            discription.SetText($"{EndDiscriptions[4]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (FinalFans < 10000)
        {
            image.sprite = EndImage[0];
            discription.SetText($"{EndDiscriptions[9]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Cold >= 15)
        {
            image.sprite = EndImage[2];
            discription.SetText($"{EndDiscriptions[6]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Hell >= 15)
        {
            image.sprite = EndImage[4];
            discription.SetText($"{EndDiscriptions[5]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Ero >= 15)
        {
            image.sprite = EndImage[3];
            discription.SetText($"{EndDiscriptions[7]}" + "\n\n" + "......点击以返回主菜单");
        }
        else if (Nonsense >= 15)
        {
            image.sprite = EndImage[5];
            discription.SetText($"{EndDiscriptions[8]}" + "\n\n" + "......点击以返回主菜单");
        }
        else
        {
            image.sprite = EndImage[1];
            discription.SetText($"{EndDiscriptions[0]}" + "\n\n" + "......点击以返回主菜单");
        }
    }
    IEnumerator ShowEnd()
    {
        yield return new WaitForSeconds(2f);
        while (image.color.a < 1)
        {
            Color color = image.color;
            color.a += 0.1f * Time.deltaTime;
            image.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        while (discription.gameObject.GetComponent<CanvasGroup>().alpha < 1)
        {
            discription.gameObject.GetComponent<CanvasGroup>().alpha += 0.1f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        button.gameObject.SetActive(true);
        canGoBack = true;
    }
}
