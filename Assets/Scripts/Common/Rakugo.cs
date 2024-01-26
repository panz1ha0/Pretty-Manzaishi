using System;

[Serializable]
public class Rakugo
{
    public int Id;
    public string Content;
    public Element Influence;
}

[Serializable]
public class Element
{
    public int Hell;  // Element A
    public int Cold;  // Element B
    public int Ero;  // Element C
    public int Nonsense;  // Element D
}
