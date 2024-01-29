using Kuchinashi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

enum IndicatorState
{
    Low,
    Median,
    High,
}

public class IndicatorListener : MonoBehaviour
{
    CardController cardController;
    Image image;
    Element currentElements = new Element();

    public Sprite[] sprites;
    public Type type;
    const float HIGH = 10;
    const float LOW = -10;
    private void Awake()
    {
        cardController = transform.parent.GetComponent<CardController>();
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (currentElements != DataRepeater.Instance.CurrentElements)
        {
            CheckElements(currentElements, type);
            currentElements = DataRepeater.Instance.CurrentElements;
        }
        currentElements = DataRepeater.Instance.CurrentElements;
        //CheckElements(currentElements, type);
    }

    private void CheckElements(Element element, Type type)
    {
        float value = 0f;
        switch (type)
        {
            case Type.Hell:
                value = element.Hell;
                break;
            case Type.Cold:
                value = element.Cold;
                break;
            case Type.Ero:
                value = element.Ero;
                break;
            case Type.Nonsense:
                value = element.Nonsense;
                break;
            default:
                break;
        }
        if (value >= HIGH)
        {
            image.sprite = sprites[0];
            AudioManager.Instance.PlaySFX("AlertTooHigh");
        }
        else if (value <= LOW)
        {
            image.sprite = sprites[2];
            AudioManager.Instance.PlaySFX("AlertTooLow");
        }
        else
        {
            image.sprite = sprites[1];
        }
    }
}
