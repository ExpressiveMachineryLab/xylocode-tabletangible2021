using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnNode : MonoBehaviour
{
    List<SubLily> sublilies;
    float t;
    public GameObject subLilyPrefab;
    public float timeToExpand = 1f;
    public int subLilyCount = 2;
    public float expansionOffset = 1f;
    public AnimationCurve expandCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve blipCurve;
    public bool areNodesShowing;
    public bool selected;
    public ElemColor elemColor;
    ParticleSystem particles;
    [Header("Mid Node Settings")]
    public bool isMidNode = false;
    public float midExpansionOffset = 1f;
    public Vector3 origin;
    public Vector3 offset;
    public Quaternion rotation;
    Vector3 targetScale;

    private void Start()
    {
        InstantiateLilies();
        if (isMidNode)
        {
            targetScale = this.transform.localScale;
            this.transform.localScale = Vector3.zero;
        }
        particles = GetComponentInChildren<ParticleSystem>();
    }
    class SubLily
    {
        public SpriteRenderer renderer;
        public Vector3 offset;
        public Quaternion rotation;
    }

    public void OnNodeHit()
    {
        if (isMidNode || particles==null) return;

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            var main = particle.main;
            main.startColor = TangibleGameController.GetColor(elemColor);
        }
        particles.Play();
        StopCoroutine("ExpandNode");
        StartCoroutine("ExpandNode");
    }

    IEnumerator ExpandNode()
    {
        float t = 0f;
        float clock = 0f;
        float timeToExpand = 0.25f;

        while (clock < timeToExpand)
        {
            clock += Time.deltaTime;
            t = Mathf.Clamp01(clock / timeToExpand);
            transform.localScale = Vector3.one * blipCurve.Evaluate(t);
            yield return null;
        }
    }
    void InstantiateLilies()
    {
        if (sublilies != null) return;
        sublilies = new List<SubLily>();
        int sign = 1;
        for (int i = 0; i < subLilyCount; i++)
        {
            GameObject lilyObj = Instantiate(subLilyPrefab, this.transform);

            SubLily lily = new SubLily()
            {
                renderer = lilyObj.GetComponent<SpriteRenderer>(),
            };
            //lily.renderer.enabled = false;
            lilyObj.SetActive(false);
            sublilies.Add(lily);
        }
    }
    public void CreateSubLilies()
    {
        if (sublilies == null)
        {
            InstantiateLilies();
        }
        int sign = 1;
        for (int i = 0; i < subLilyCount; i++)
        {
            GameObject lilyObj = sublilies[i].renderer.gameObject;
            lilyObj.transform.localPosition = Vector3.zero;
            lilyObj.transform.localRotation = Quaternion.identity;

            Vector3 pos = Random.insideUnitCircle;
            if (pos == Vector3.zero)
            {
                pos = Vector3.up;
            }
            pos.y = Mathf.Abs(pos.y) * sign;
            pos.Normalize();
            Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

            SubLily lily = sublilies[i];
            lily.offset = pos * expansionOffset;
            lily.rotation = rot;
            sign = -sign;
            //lily.renderer.enabled = true;
            lilyObj.SetActive(true);
            if (!this.isMidNode)
            {
                lily.renderer.sharedMaterial = this.GetComponent<SpriteRenderer>().sharedMaterial;
            }
        }
        if (this.isMidNode)
        {
            Vector3 pos = Random.insideUnitCircle;
            if (pos == Vector3.zero)
            {
                pos = Vector3.up;
            }
            pos.Normalize();
            offset = pos * midExpansionOffset;

            rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
            this.transform.rotation = rotation;
        }
        StartCoroutine("FanOutSublilies");
    }

    IEnumerator FanOutSublilies()
    {
        areNodesShowing = true;
        float t;
        float clock = 0f;
        while (clock < timeToExpand)
        {
            clock += Time.deltaTime;
            t = expandCurve.Evaluate(Mathf.Clamp01(clock / timeToExpand));
            foreach (SubLily lily in sublilies)
            {
                lily.renderer.transform.localPosition = Vector3.Lerp(Vector3.zero, lily.offset, t);
                lily.renderer.transform.localRotation = Quaternion.Lerp(Quaternion.identity, lily.rotation, t);
            }
            if (isMidNode)
            {
                this.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                this.transform.position = origin + Vector3.Lerp(Vector3.zero, offset, t);
            }
            yield return null;
        }
    }

    public void HideSublilies()
    {
        if (areNodesShowing)
        {
            StartCoroutine("CloseSublilies");
        }
    }

    public void SetSelected(bool s)
    {
        if (s && !selected)
        {

        }
    }
    IEnumerator CloseSublilies()
    {
        float t;
        float clock = 0f;
        while (clock < timeToExpand)
        {
            clock += Time.deltaTime;
            t = expandCurve.Evaluate(Mathf.Clamp01(clock / timeToExpand));
            foreach (SubLily lily in sublilies)
            {
                lily.renderer.transform.localPosition = Vector3.Lerp(lily.offset, Vector3.zero, t);
                lily.renderer.transform.localRotation = Quaternion.Lerp(lily.rotation, Quaternion.identity, t);
            }
            if (isMidNode)
            {
                this.transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, t);
                this.transform.position = origin + Vector3.Lerp(offset, Vector3.zero, t);
            }
            yield return null;
        }
        foreach (SubLily lily in sublilies)
        {
            //lily.renderer.enabled = false;
            lily.renderer.gameObject.SetActive(false);
        }
        areNodesShowing = false;
    }

    public void UpdateColor(Material mat)
    {
        if (sublilies == null) return;
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetFloat("_Outline", (selected) ? 1f : 0f);
        block.SetColor("_OutlineColor", TangibleGameController.GetColor(elemColor));
        renderer.SetPropertyBlock(block);
        if (!isMidNode)
        {
            foreach (SubLily lily in sublilies)
            {
                lily.renderer.sharedMaterial = mat;
                lily.renderer.SetPropertyBlock(block);
            }
        }

    }
}