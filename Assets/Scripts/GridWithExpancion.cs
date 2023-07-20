using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridWithExpancion : MonoBehaviour {
	public Button addStatementButton;
	public float paddingLeft, paddingTop, spacing;

	public List<RectTransform> kids;

	public float noExpandOffset = 128;
	public float expandOffset = 128 + 108;

	float startingY;
	private int showing = 1;
	/*
	void Awake() {
		for (int i = 0; i < transform.childCount; i++) {
			RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
			kids.Add(child);
			if (i >= showing)
            {
				//child.gameObject.SetActive(false);
            }
		}
	}
	*/

    private void Start()
    {
		for (int i = 0; i < 4; i++)
		{
			if (i >= showing)
			{
				kids[i].gameObject.SetActive(false);
			}
		}
		startingY = addStatementButton.transform.position.y;
	}

    void Update() {
		float currentY = -paddingTop;

		bool anyExpanded = false;
		int count = 0;
		foreach (RectTransform pos in kids) {
			pos.anchoredPosition = new Vector3(paddingLeft, currentY, 0);
			currentY -= pos.rect.height + spacing;

			Rect rect = addStatementButton.GetComponent<RectTransform>().rect;

			if (pos.gameObject.activeInHierarchy && pos.gameObject.TryGetComponent<LinePanel>(out LinePanel panel)) {
				//LinePanel panel = pos.GetComponent<LinePanel>();
				if (panel.expanded)
				{
					anyExpanded = true;
				}
				
				count++;
			}
		}
		float offsetDiff = expandOffset - noExpandOffset;
		float offset = (count - 1) * noExpandOffset;
		if (anyExpanded)
        {
			offset += offsetDiff;
        }
		//addStatementButton.transform.position = new Vector3(addStatementButton.transform.position.x, startingY + offset, addStatementButton.transform.position.z);

		//Rect rect = addStatementButton.GetComponent<RectTransform>().rect;
		//rect.Set(rect.x, rect.y - (rect.height + spacing), rect.width, rect.height);
	}

	public void AddStatement()
    {
		if (showing < 4)
        {
			showing++;
			for (int i = 0; i < 4; i++)
            {
				if (i < showing)
                {
					kids[i].gameObject.SetActive(true);
                }
            }
			Rect rect = addStatementButton.GetComponent<RectTransform>().rect;
			//addStatementButton.GetComponent<RectTransform>().Translate(Vector3.up * -1 * (108 + spacing + 10));
			//Rect rect = addStatementButton.GetComponent<RectTransform>().rect;
			//rect.Set(rect.x, rect.y - (rect.height + spacing), rect.width, rect.height);
		}
		if (showing >= 4)
        {
			addStatementButton.gameObject.SetActive(false);
        }
    }
}
