using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TE;
using UnityEngine;


// Receives tangible events from the TangibleEngine and manages creating and moving the Emitters that correspond to each Tangible.
// Also contains static helper functions for loading Tangible profiles from StreamingAssets.
public class TangibleManager : MonoBehaviour, IOnTangibleAdded, IOnTangibleUpdated, IOnTangibleRemoved
{
    private GameManager gameManager;
    public static string ProfilePath = "XyloPROFILE";//"C:/Users/MT-User/AppData/Roaming/Ideum/TangibleEngine/Profiles/XyloPROFILE.json";
    public static string MappingFilename = "tangible_mapping.txt";
    public static string ProfileJSON;
    public GameObject emitterPrefabRed;
    public GameObject emitterPrefabBlue;
    public GameObject emitterPrefabYellow;
    public GameObject emitterPrefabGreen;

    Dictionary<int, GameObject> tangibleObjectLookup; // map of Tangible IDs to the GameObjects with the Tangible component

    Dictionary<string, ElemColor> tangibleIDtoColorLookup;  // map of Tangible IDs to the Frog color of each Tangible

    // Default maps of tangible names to colors if filereading fails
    Dictionary<string, ElemColor> tangibleIDtoColorDefault = new Dictionary<string, ElemColor>
    {
        {"tangible_D",ElemColor.red},
        {"tangible_2A",ElemColor.red},
        {"tangible_B",ElemColor.yellow},
        {"tangible_1C",ElemColor.blue},
        {"tangible_E",ElemColor.green},

    };

    void Start()
    {
        // Subscribe to the Tangible Engine to receive events when Tangibles are Placed, Moved, or Removed
        TangibleEngine.Subscribe(this);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tangibleObjectLookup = new Dictionary<int, GameObject>();

        PopulateTangibleMapFromFile();

    }

    void OnDestroy()
    {

        // Unsubscribe to the Tangible Engine when this object is deleted (such as when changing scenes)
        TangibleEngine.Unsubscribe(this);
    }

    // Implementation of IOnTangibleAdded
    // Creates an Emitter (frog) when placed.
    public void OnTangibleAdded(Tangible t) {

        if (!tangibleObjectLookup.TryGetValue(t.Id, out GameObject obj) || obj == null)
        {
            Debug.Log("added tangible with id: " + t.Id + " and pattern " + t.Pattern.Name);
            ElemColor color;
            // Get a Tangible Color for the Tangible ID.
            if (tangibleIDtoColorLookup.TryGetValue(t.PatternName, out ElemColor c))
            {
                color = c;
            }
            else
            {
                color = ElemColor.red;
            }

            // Get an Emitter Object from GameManager and assign it to the tangible.
            GameObject emitter;
            switch (color) {
                default:
                case ElemColor.red:
                    emitter = gameManager.AssignEmitter(emitterPrefabRed);
                    break;
                case ElemColor.blue:
                    emitter = gameManager.AssignEmitter(emitterPrefabBlue);
                    break;
                case ElemColor.green:
                    emitter = gameManager.AssignEmitter(emitterPrefabGreen);
                    break;
                case ElemColor.yellow:
                    emitter = gameManager.AssignEmitter(emitterPrefabYellow);
                    break;
            }
            EmitterTangible et = emitter.GetComponent<EmitterTangible>();
            et.tangible = t;
            tangibleObjectLookup[t.Id] = emitter;
            et.color = color;
        }
    }


    // Implementation of IOnTangibleUpdated
    // Rotates and moves the Emitter when the tangible is moved.
    public void OnTangibleUpdated(Tangible t) {

        if (tangibleObjectLookup.TryGetValue(t.Id, out GameObject emitterObj))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(t.Pos); ;
            emitterObj.transform.position = new Vector3(worldPos.x, worldPos.y);
            EmitterTangible emitterTangible = emitterObj.GetComponent<EmitterTangible>();
            emitterTangible.lastRot = TangibleManager.GetRotation(t) - emitterTangible.angleOffset;
            emitterObj.transform.rotation = Quaternion.AngleAxis(emitterTangible.lastRot, Vector3.forward);
        }

    }

    // Implementation of IOnTangibleRemoved
    // Disables the Emitter when the Tangible is removed from the table. 
    public void OnTangibleRemoved(Tangible t)
    {
        if (tangibleObjectLookup.TryGetValue(t.Id, out GameObject emitterObj))
        {
            emitterObj.SetActive(false);
            tangibleObjectLookup[t.Id] = null;
        }
    }

    // Get the rotation of the Tangible in Degrees
    public static float GetRotation(Tangible t)
    {
        return (t.R * Mathf.Rad2Deg);
    }

    // Load Tangible setup/patterns from file.
    public static void LoadPatterns()
    {
        try
        {
            ProfileJSON = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/" + ProfilePath + ".json");
        }
        catch (System.IO.FileNotFoundException ex)
        {
            Debug.LogError(ex);
            ProfileJSON = "";
        }
    }

    // Loads a List of Patterns from JSON obtained from file rather than using the Tangible Engine's default profiles.
    // Currently the Tangible Engine in Unity does not use the JSON pattern files created in the Trainer,
    // So they must be loaded manually.
    public static List<Pattern> ForcePatternsFromJSON()
    {
        try
        {
            string json = ProfileJSON;

            List<Pattern> patterns = new List<Pattern>();
            JsonSerializer jsonSerializer = new JsonSerializer();
            JsonPatterns jsonData = jsonSerializer.Deserialize<JsonPatterns>(new JsonTextReader(new StringReader(json)));
            patterns.AddRange(jsonData.Patterns);
            return patterns;
        } catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }

    }


    // Get the current mapping of Tangible IDs to Frog Colors from File.
    public void PopulateTangibleMapFromFile()
    {
        Dictionary<string, ElemColor> map = new Dictionary<string, ElemColor>();
        try
        {
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/" + MappingFilename);
            string log = "";
            foreach (string line in lines)
            {
                string[] split = line.Split(',');
                string name = split[0].Trim();
                map[name] = GetElemColorFromString(split[1]);
                Debug.Log(string.Format("set tangible \"{0}\"  to {1}\n", name, map[name].ToString()));

            }
        }
        catch (System.IO.FileNotFoundException ex)
        {
            Debug.LogError(ex);
            tangibleIDtoColorLookup = tangibleIDtoColorDefault;
            return;
        }
        tangibleIDtoColorLookup = map;
    }

    static ElemColor GetElemColorFromString(string colorName)
    {
        string name = colorName.Trim().ToLower();
        if (name.Contains("red"))
        {
            return ElemColor.red;
        }
        else if (name.Contains("blue"))
        {
            return ElemColor.blue;
        }
        else if (name.Contains("yellow"))
        {
            return ElemColor.yellow;
        }
        else if (name.Contains("green"))
        {
            return ElemColor.green;
        }
        else
        {
            return ElemColor.red;
        }
    }
}
