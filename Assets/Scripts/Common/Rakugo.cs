using System;

[Serializable]
public enum Type
{
    Hell,
    Cold,
    Ero,
    Nonsense
}

[Serializable]
public class Rakugo
{
    public int Id;
    public Type Type;
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

    public static Element operator+ (Element a, Element b)
    {
        return new Element() {
            Hell = a.Hell + b.Hell,
            Cold = a.Cold + b.Cold,
            Ero = a.Ero + b.Ero,
            Nonsense = a.Nonsense + b.Nonsense
        };
    }
}
