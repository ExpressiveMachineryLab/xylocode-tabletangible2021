using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnLine : MonoBehaviour
{
    public Line lineBase;
    public ElemColor color;
    public Vector2 start;
    public GridPoint startPoint;
    public Vector2 end;
    public GridPoint endPoint;
    public int touchID;
    public bool editing;
    public bool editingEnd;
    public bool selected;
    public Transform lineTransform;
    public Transform snappedTransform;
    public Transform nodeParent;
    public CanvasGroup startMenu;
    public CanvasGroup endMenu;
    bool menuOpen;
    public Collider2D touchCollider;
    public Collider ballCollider;
    public Collider2D startHandle;
    public Collider2D endHandle;
    Tutorial startTutorial;
    Tutorial endTutorial;
    public GameObject nodePrefab;
    List<DrawnNode> nodes;
    public GameObject midnodePrefab;
    List<DrawnNode> midNodes;
    int maxNodes;
    int minNodes;
    public int nodesAmount;
    public float length;
    float lengthFactor = 1f;
    Touch touch;
    bool showingSublilies;
    bool newlyCreated;
    float newlyLifetime = 30f;
    // Start is called before the first frame update
    void Start()
    {
        nodesAmount = 1;
        editing = true;
        startPoint = LineDrawHandler.GetClosestGridPoint(start);
        endPoint = startPoint;
        lengthFactor = LineDrawHandler.instance.lengthFactor;
        minNodes = LineDrawHandler.instance.minNodes;
        maxNodes = LineDrawHandler.instance.maxNodes;
        nodes = new List<DrawnNode>(maxNodes);
        midNodes = new List<DrawnNode>(maxNodes - 1);
        newlyCreated = true;
        startTutorial = startMenu.GetComponentInChildren<Tutorial>();
        endTutorial = endMenu.GetComponentInChildren<Tutorial>();
        for (int i = 0; i < maxNodes; i++)
        {
            GameObject nodeObj = Instantiate(nodePrefab, nodeParent);
            nodes.Add(nodeObj.GetComponent<DrawnNode>());
            nodeObj.SetActive(false);
            if (i < maxNodes - 1)
            {
                GameObject midNodeObj = Instantiate(midnodePrefab, nodeParent);
                DrawnNode midNode = midNodeObj.GetComponent<DrawnNode>();
                midNode.isMidNode = true;
                midNodes.Add(midNode);
                midNodeObj.SetActive(false);
            }
        }
        if (lineBase != null)
        {
            lineBase.color = (ElemColor)Random.Range(0, 4);
            lineBase.OnHit.AddListener(OnLineHit);
        }
        UpdateColors();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasValidAtStartOfFrame = editing;
        if (editing)
        {
            bool foundTouch = false;
            foreach (Touch activeTouch in LineDrawHandler.GetTouches())
            {
                if (activeTouch.fingerId == touchID)
                {
                    touch = activeTouch;
                    foundTouch = true;
                    break;
                }
            }
            if (!foundTouch)
            {
                editing = false;
            }
            else
            {
                //start = touch.startScreenPosition;
                if (touch.phase == TouchPhase.Moved)
                {
                    if (editingEnd)
                    {
                        end += touch.deltaPosition;
                        endPoint = LineDrawHandler.GetClosestGridPoint(end);
                    }
                    else
                    {
                        start += touch.deltaPosition;
                        startPoint = LineDrawHandler.GetClosestGridPoint(start);
                    }

                }

                Vector3 mid = (start + end) / 2f;
                Vector3 startWorld = Camera.main.ScreenToWorldPoint(start);
                startWorld.z = 0f;
                Vector3 endWorld = Camera.main.ScreenToWorldPoint(end);
                endWorld.z = 0f;
                Vector3 midWorld = Camera.main.ScreenToWorldPoint(mid);
                midWorld.z = 0f;
                float dist = Vector3.Distance(startWorld, endWorld);

                lineTransform.position = midWorld;
                lineTransform.rotation = Quaternion.LookRotation(this.transform.forward, (startWorld - endWorld).normalized);
                lineTransform.localScale = new Vector3(lineTransform.localScale.x, dist, lineTransform.localScale.z);

                if (startPoint != null && endPoint != null)
                {
                    Vector3 midSnap = (startPoint.transform.position + endPoint.transform.position) / 2f;
                    midSnap.z = 0f;
                    float distSnap = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);

                    snappedTransform.position = midSnap;
                    snappedTransform.rotation = Quaternion.LookRotation(this.transform.forward, (startPoint.transform.position - endPoint.transform.position).normalized);
                    snappedTransform.localScale = new Vector3(snappedTransform.localScale.x, distSnap, snappedTransform.localScale.z);

                    startHandle.transform.position = (!editingEnd) ? startWorld : startPoint.transform.position;

                    endHandle.transform.position = (editingEnd) ? endWorld : endPoint.transform.position;

                    startHandle.transform.rotation = Quaternion.FromToRotation(Vector3.right, (startWorld - endWorld).normalized);
                    endHandle.transform.rotation = Quaternion.FromToRotation(Vector3.right, -(startWorld - endWorld).normalized);
                }
            }
            UpdateNodes();
        }
        else if (startPoint != null && endPoint != null)
        {
            startHandle.transform.position = startPoint.transform.position;
            endHandle.transform.position = endPoint.transform.position;
        }

        if (!editing && wasValidAtStartOfFrame)
        {
            if (startPoint == null || endPoint == null || startPoint == endPoint)
            {
                Destroy(this.gameObject);
            }
            else
            {
                ShowSubNodes();
                UpdateColors();
                LineDrawHandler.OnLineFinish(this);
            }
        }
        else if (editing && showingSublilies)
        {
            HideSubNodes();
        }
        touchCollider.GetComponent<SpriteRenderer>().color = (selected) ? Color.magenta : Color.gray;

        if (lineBase != null && color != lineBase.color)
        {
            UpdateColors();
        }
        int highlightIndex = 0;
        if (lineBase != null)
        {
            lineBase.pitches = nodesAmount;
            lineBase.pitchLevel = lineBase.pitchLevel % nodesAmount;
            highlightIndex = lineBase.pitchLevel % nodesAmount;
        }

        if ((!editing && selected) && !menuOpen)
        {
            menuOpen = true;

            UpdateColors();
        }
        else if ((!selected || editing) && menuOpen)
        {
            menuOpen = false;

            UpdateColors();
        }

        if (newlyCreated)
        {
            if (newlyLifetime > 0 && !selected)
            {
                newlyLifetime -= Time.deltaTime;
            }
            else
            {
                newlyCreated = false;
            }
        }
        bool tutorialOpen = (startTutorial != null && startTutorial.IsActive()) || (endTutorial != null && endTutorial.IsActive());
        float menuAlpha = (menuOpen || newlyCreated || tutorialOpen) ? 1f : 0f;
        if (startMenu.alpha != menuAlpha)
        {
            endMenu.alpha = startMenu.alpha = Mathf.MoveTowards(startMenu.alpha, menuAlpha, Time.deltaTime * 10f);
        }
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Outline", (selected) ? 1f : 0f);
        block.SetColor("_OutlineColor", TangibleGameController.GetColor(lineBase.color));
        MaterialPropertyBlock highlightBlock = new MaterialPropertyBlock();
        highlightBlock.SetFloat("_Outline", (selected) ? 1f : 0f);
        highlightBlock.SetColor("_OutlineColor", TangibleGameController.GetColor(lineBase.color));
        highlightBlock.SetColor("Color_3720444888204e61a37b50ddfa736fc9", TangibleGameController.lightHighlightColor[lineBase.color]);
        highlightBlock.SetColor("Color_3be713ac39fb4c30ad27fdab664331f3", TangibleGameController.darkHighlightColor[lineBase.color]);

        for (int i = 0; i < nodes.Count; i++)
        {
            DrawnNode node = nodes[i];
            if (!node.gameObject.activeInHierarchy)
            {
                continue;
            }
            else
            {
                node.GetComponent<SpriteRenderer>().SetPropertyBlock((i == highlightIndex) ? highlightBlock : block);
            }
        }
    }

    public void OnLineHit()
    {
        int index = lineBase.pitchLevel % nodesAmount;
        nodes[index].OnNodeHit();
    }
    public void UpdateNodes()
    {
        if (startPoint == null || endPoint == null) return;
        length = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);
        nodesAmount = (int)Mathf.Clamp(Mathf.Floor(length / lengthFactor),minNodes,maxNodes);
        if (nodesAmount <= 0) nodesAmount = 1;
        float startSeg = 0f;
        float endSeg = 0f;
        for (int i = 0; i < maxNodes; i++)
        {
            bool shouldBeActive = i < nodesAmount;
            if (!shouldBeActive && nodes[i].gameObject.activeInHierarchy)
            {
                nodes[i].gameObject.SetActive(false);
            }
            else if (shouldBeActive)
            {
                if (!nodes[i].gameObject.activeInHierarchy)
                {
                    nodes[i].gameObject.SetActive(true);
                }
                DrawnNode node = nodes[i];
                startSeg = endSeg;
                endSeg += length / nodesAmount;
                float midSeg = (startSeg + endSeg) / 2f;
                node.transform.position = startPoint.transform.position + (endPoint.transform.position - startPoint.transform.position).normalized * midSeg;
            }
            // mid nodes

            if (i > 0)
            {
                int j = i - 1;
                bool midActive = shouldBeActive && !editing;
                if (!shouldBeActive && midNodes[j].gameObject.activeInHierarchy)
                {
                    midNodes[j].gameObject.SetActive(false);
                }
                else if (shouldBeActive)
                {
                    if (!midNodes[j].gameObject.activeInHierarchy)
                    {
                        midNodes[j].gameObject.SetActive(true);
                    }
                    Vector3 midPoint = (nodes[i].transform.position + nodes[j].transform.position) / 2f;
                    midNodes[j].transform.position = midPoint;
                    midNodes[j].origin = midPoint;
                }
            }

        }
    }

    public void ShowSubNodes()
    {
        foreach (DrawnNode node in nodes)
        {
            if (node.gameObject.activeInHierarchy) 
                node.CreateSubLilies();
        }
        foreach (DrawnNode midnode in midNodes)
        {
            if (midnode.gameObject.activeInHierarchy)
                midnode.CreateSubLilies();
        }
        showingSublilies = true;
    }

    public void HideSubNodes()
    {
        foreach (DrawnNode node in nodes)
        {
            if (node.gameObject.activeInHierarchy)
                node.HideSublilies();
        }
        foreach (DrawnNode midnode in midNodes)
        {
            if (midnode.gameObject.activeInHierarchy)
                midnode.HideSublilies();
        }
        showingSublilies = false;
    }

    public void SetColor(int c)
    {
        if (lineBase != null) lineBase.color = (ElemColor)c;
        color = (ElemColor)c;
        if (newlyCreated)
        {
            newlyLifetime += 10f;
        }
        UpdateColors();
    }
    public void UpdateColors()
    {
        if (lineBase != null) color = lineBase.color;
        for (int i = 0; i < nodes.Count; i++)
        {
            DrawnNode node = nodes[i];
            node.selected = selected;
            node.elemColor = color;
            node.GetComponent<SpriteRenderer>().sharedMaterial = LineDrawHandler.GetMaterial(color);
            node.UpdateColor(LineDrawHandler.GetMaterial(color));
        }
        for (int j = 0; j < midNodes.Count; j++)
        {
            DrawnNode midnode = midNodes[j];
            midnode.selected = selected;
            midnode.elemColor = color;
            //midnode.GetComponent<SpriteRenderer>().sharedMaterial = LineDrawHandler.GetMaterial(color);
            midnode.UpdateColor(midnode.GetComponent<SpriteRenderer>().sharedMaterial);
        }
    }
}
