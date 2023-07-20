using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TangibleController : MonoBehaviour
{

    public Guid Guid;
    private float _yPos;
    public Vector3 Dir;
    public int RULES_LIMIT = 6;
//    public List<Rule> AllRules = new List<Rule>();
    public List<Rule> ActiveRules =  new List<Rule>();

    public Canvas Canvas;
//    public GameObject PlaceHolder;
    // Start is called before the first frame updates
    void Start()
    {
        _yPos = transform.position.y;
        Guid = new Guid();
        Canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * TangibleGameController.Singleton.RotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * TangibleGameController.Singleton.RotationSpeed);
        }*/
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        Dir = transform.forward;

    }

    
    private void OnMouseDrag()
    {
        
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        objPosition = new Vector3(objPosition.x, _yPos, objPosition.z);
        transform.position = objPosition;
    }

    public void AddRule(GameObject toggle)
    {
        if (ActiveRules.Count >= RULES_LIMIT) return;
        HideAllRules();
        GameObject t = Instantiate(toggle, transform.position + new Vector3(1,1,0), Quaternion.identity);
        Rule r = t.GetComponent<Rule>();
        ActiveRules.Add(r);

        r.ToggleBackground.color = TangibleGameController.Singleton.ColorBinding[r.TargetColor];
        r.ToggleIcon.color = Color.white;
        t.transform.SetParent(Canvas.transform, false);
        r.ResetColorToggles();
        r.sourceTangible = this;
        r.SetActive(true);
        Reposition();

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.MarkNewRule();
        }
    }
    
    public void RemoveRule(GameObject toggle)
    {
        
        Rule r = toggle.GetComponent<Rule>();
        ActiveRules.Remove(r);
        toggle.SetActive(false);
        Reposition();
        Destroy(toggle);

    }

    public void RemoveAllRules()
    {
        foreach (Rule rule in ActiveRules)
        {
            Destroy(rule.gameObject);
        }
        ActiveRules.Clear();
        Reposition();
    }
    public void Reposition()
    {
        var count = ActiveRules.Count;
        var angle = Mathf.PI / (Mathf.Ceil(count * 0.5f) + 1);
        for (int i = 0; i < count; i++)
        {
            var sideIndex = i % 2;
            var secIndex = Mathf.Ceil((i+1)*0.5f);
            var go = ActiveRules[i].gameObject;
            var composIndex = sideIndex * Mathf.PI + secIndex * angle;
            go.GetComponent<RectTransform>().localPosition = new Vector3(4 * Mathf.Sin(composIndex),4 * Mathf.Cos(composIndex),0);
            ActiveRules[i].CheckSide(sideIndex==1);
        }
    }    

    public void Shoot()
    {
        var bullet = Instantiate(TangibleGameController.Singleton.BulletPrefab, transform.position + Vector3.up*0.02f, Quaternion.identity);
        var b = bullet.GetComponent<Bullet>();
        b.SetDir(Dir);
        b.SetParent(this);
    }  
    
    public void HideOtherRules(Rule activeRule)
    {
        foreach (Rule rule in ActiveRules)
        {
            if (rule != null && rule != activeRule)
            {
                rule.Hide();
            }
        }
    }

    public void HideAllRules()
    {
        foreach (Rule rule in ActiveRules)
        {
            if (rule != null)
            {
                rule.Hide();
            }
        }
    }
}
