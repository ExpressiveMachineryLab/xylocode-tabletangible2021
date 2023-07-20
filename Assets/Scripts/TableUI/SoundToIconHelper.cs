using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToIconHelper : MonoBehaviour
{

    public Sprite piano;
    public Sprite drum;
    public Sprite guitar;
    public Sprite sax;
    public Sprite flute;

    public static Dictionary<string, string[]> bankToIconNames = new Dictionary<string, string[]>()
    {
        {"orchestral", new string[] { "piano","drum","flute","drum" }},
        {"helm synth", new string[] { "piano","piano","piano","piano"}},
        { "vital synth", new string[] { "piano", "piano", "piano", "piano" }} ,
        { "electric", new string[] { "guitar", "guitar", "guitar", "guitar" }} ,
        { "violin", new string[] { "guitar", "guitar", "guitar", "guitar" }},
        { "drums", new string[] { "drum", "drum", "drum", "piano" }} ,
        { "saxophone", new string[] { "sax", "sax", "sax", "sax" }} ,
    };

    public static Dictionary<string, Sprite> iconNamesToSprite;
    private void Awake()
    {
        iconNamesToSprite = new Dictionary<string, Sprite>()
        {
            {"piano", piano },
            {"drum", drum },
            {"guitar", guitar },
            {"sax", sax },
            {"flute", flute },
        };
    }
}
