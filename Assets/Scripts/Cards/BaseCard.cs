using UnityEngine;
public class BaseCard: MonoBehaviour
{
    public Rakugo rakugoData;
    //bool isPreviewing = false;
    //public bool GetIsPreviewing() => isPreviewing;

    public void Init()
    {
        rakugoData = null;
    }
    public void Init(Rakugo rakugo)
    {
        this.rakugoData = rakugo;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
