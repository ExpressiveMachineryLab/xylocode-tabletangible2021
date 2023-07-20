using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TE;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using Input = InputWrapper.Input;
#endif

public class LineDrawHandler : MonoBehaviour
{
    public static LineDrawHandler instance;
    [SerializeField] private GridPoint[] gridPoints;
    TangibleEngine te;
    public GameObject DrawnLinePrefab;
    [Header("UI Settings")]
    public float lengthFactor;
    public int minNodes = 1;
    public int maxNodes = 8;
    public bool isFirst;
    [Space(10)]
    public List<DrawnLine> createdLines;
    [Header("Colors")]
    public Material red;
    [Space(10)]
    public Material yellow;
    [Space(10)]
    public Material blue;
    [Space(10)]
    public Material green;
    [Space(10)]
    public Material highlight;

    List<Tutorial> firstTutorial;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //input = this.GetComponent<PlayerInput>();
        gridPoints = FindObjectsOfType<GridPoint>();
        //SetupInputs();
        isFirst = true;
        te = FindObjectOfType<TangibleEngine>();
        createdLines = new List<DrawnLine>();
        firstTutorial = new List<Tutorial>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckTouches();
    }

    public void CheckTouches()
    {
        var touches = GetTouches();

        foreach (UnityEngine.Touch touch in touches)
        {
            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);

                bool acceptTouch = true;
                foreach (GraphicRaycaster raycaster in FindObjectsOfType<GraphicRaycaster>())
                {
                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = touch.position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    raycaster.Raycast(pointer, results);
                    if (results.Count > 0)
                    {
                        acceptTouch = false;
                        break;
                    }
                }
                RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint + Vector3.forward * -100f, Vector3.forward);
                List<Collider2D> colliders = new List<Collider2D>(hits.Length);
                foreach(RaycastHit2D hit in hits)
                {
                    colliders.Add(hit.collider);
                }
                GridPoint closestGridPoint = GetClosestGridPoint(touch.position);
                foreach (DrawnLine line in createdLines)
                {
                    if (line == null) continue;
                    if (line.selected && line.startPoint == closestGridPoint)
                    {
                        acceptTouch = false;
                        line.editing = true;
                        line.touchID = touch.fingerId;
                        line.editingEnd = false;
                    }
                    else if (line.selected && line.endPoint == closestGridPoint)
                    {
                        acceptTouch = false;
                        line.editing = true;
                        line.touchID = touch.fingerId;
                        line.editingEnd = true;
                    }
                    else if (colliders.Contains(line.touchCollider))
                    {
                        acceptTouch = false;
                        if (!line.selected)
                        {
                            line.selected = true;
                        }
                        else if (line.selected && colliders.Contains(line.startHandle))
                        {
                            line.editing = true;
                            line.touchID = touch.fingerId;
                            line.editingEnd = false;
                        }
                        else if (line.selected && colliders.Contains(line.endHandle))
                        {
                            line.editing = true;
                            line.touchID = touch.fingerId;
                            line.editingEnd = true;
                        }
                        else
                        {
                            line.selected = false;
                        }
                        
                    }
                    
                }
                //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector3.forward, Mathf.Infinity, ~LayerMask.GetMask("GridPoint"));
                if (Physics.Raycast(worldPoint + Vector3.forward * -100f, Vector3.forward, out RaycastHit hit2, Mathf.Infinity, ~LayerMask.GetMask("GridPoint")))
                {
                    acceptTouch = false;
                }
                
                if (acceptTouch)
                {
                    GameObject newLineObj = Instantiate(DrawnLinePrefab);
                    DrawnLine newLine = newLineObj.GetComponent<DrawnLine>();
                    newLine.touchID = touch.fingerId;
                    newLine.start = touch.position;
                    newLine.end = touch.position;

                    createdLines.Add(newLine);
                    GameManager gm = FindObjectOfType<GameManager>();
                    if (gm != null)
                    {
                        gm.MarkNewDrawnLine();
                    }
                }
                else
                {
                    bool b = false;
                }

            }
        }
    }

    public static IEnumerable<Touch> GetTouches()
    {
        return Input.touches.Where((t) => { return !IsTouchAssociatedWithTangible(t); });
    }

    public static bool IsTouchAssociatedWithTangible(Touch touch)
    {
        var tangibleTouches = TangibleEngine.Instance.AssociatedInputTouchPoints;
        foreach (var tangibleTouch in tangibleTouches)
        {
            if (tangibleTouch.fingerId == touch.fingerId)
            {
                return true;
            }
        }
        return false;
    }

    public static GridPoint GetClosestGridPoint(Vector2 screenPosition)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("GridPoint"));
        float leadingDist = Mathf.Infinity;
        GridPoint leader = null;
        foreach (RaycastHit2D hit in hits)
        {
            float dist = Vector2.Distance(hit.collider.transform.position, worldPoint);
            if (dist < leadingDist)
            {
                leadingDist = dist;
                leader = hit.collider.GetComponentInParent<GridPoint>();
            }
        }
        return leader;
    }
    public GridPoint[] GetGridPoints()
    {
        return gridPoints;
    }

    public static void OnLineFinish(DrawnLine line)
    {
        if (instance.isFirst) {
            instance.firstTutorial.Clear();
            foreach (Tutorial t in line.GetComponentsInChildren<Tutorial>())
            {
                t.enableOnUpdate = true;
                instance.firstTutorial.Add(t);
            }
            instance.isFirst = false;
        }
        else
        {
            foreach (Tutorial t in instance.firstTutorial)
            {
                t.DisableTutorial();
            }
            instance.firstTutorial.Clear();
        }
    }
    public void DestroyLines()
    {
        foreach (DrawnLine line in createdLines)
        {
            if (line != null)
            {
                Destroy(line.gameObject);
            }
        }
        isFirst = true;
        createdLines.Clear();
    }
    public static Material GetMaterial(ElemColor color)
    {
        switch (color)
        {
            case ElemColor.red:
                return instance.red;
            case ElemColor.yellow:
                return instance.yellow;
            case ElemColor.blue:
                return instance.blue;
            case ElemColor.green:
                return instance.green;
            default:
                return instance.highlight;
        }
    }

    
    public static Material GetHighlightMaterial()
    {
        return instance.highlight;
    }
}
