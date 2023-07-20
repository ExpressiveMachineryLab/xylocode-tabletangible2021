using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.InputSystem.EnhancedTouch;



public class GameManager : MonoBehaviour {
	private SelectionManager selectionManager;
	public float speed = 1f;
	private TMP_InputField speedMultiplier;

	public int maxLines, maxEmitters, maxBalls;
	public List<GameObjectAgePair> linePool = new List<GameObjectAgePair>();
	public List<GameObjectAgePair> emitterPool = new List<GameObjectAgePair>();
	public List<GameObjectAgePair> ballPool = new List<GameObjectAgePair>();

	public GameObject linePrefabs, emitterPrefabs, ballPrefabs;

	private CountLogger logger;

	public GameObject sceneContainer;
	public InspectorPanel inspector;

	public List<DrawnLine> drawnLines;

	private void Awake()
	{

		InspectorPanel.inspector = inspector;
		//EnhancedTouchSupport.Enable();
	}
	void Start() {
		//speedMultiplier = GameObject.Find("GlobalSpeedMultiplier").GetComponent<TMP_InputField>();
		//UpdateSpeed();

		selectionManager = GameObject.Find("SelectedObject").GetComponent<SelectionManager>();

		logger = FindObjectOfType<CountLogger>();

		for (int i = 0; i < 10; i++) {
			GameObjectAgePair newLine = new GameObjectAgePair();
			newLine.heldObject = Instantiate(linePrefabs, sceneContainer.transform);
			newLine.heldObject.SetActive(false);
			newLine.heldObject.name = "Line" + i;
			newLine.age = 0f;
			linePool.Add(newLine);
		}
		for (int i = 0; i < 10; i++) {
			GameObjectAgePair newEmitter = new GameObjectAgePair();
			newEmitter.heldObject = Instantiate(emitterPrefabs, sceneContainer.transform);
			newEmitter.heldObject.SetActive(false);
			newEmitter.heldObject.name = "Emitter" + i;
			newEmitter.age = 0f;
			emitterPool.Add(newEmitter);
		}
		for (int i = 0; i < 10; i++) {
			GameObjectAgePair newBall = new GameObjectAgePair();
			newBall.heldObject = Instantiate(ballPrefabs, sceneContainer.transform);
			newBall.heldObject.SetActive(false);
			newBall.heldObject.name = "Ball" + i;
			newBall.age = 0f;
			ballPool.Add(newBall);
		}

		drawnLines = new List<DrawnLine>();
	}

    public void UpdateSpeed() {
		if (speedMultiplier.text == "") return;
		float checkSpeed = float.Parse(speedMultiplier.text) * 2;
		if (checkSpeed < 0.2f) checkSpeed = 0.2f;
		speed = checkSpeed;
	}

	public float GetSpeedMultiplier() {
		return speed;
	}

	public void SetSpeedMultiplier(float newSpeed) {
		speed = newSpeed;
		if (speedMultiplier != null)
        {
			speedMultiplier.text = newSpeed.ToString();
		}
		
	}

	public void OnClickDelete() {
		
		if(selectionManager.square.GetSelected().Count > 0)
        {
			selectionManager.square.Delete();
			selectionManager.square.StopSelecting();
		}
		else if (selectionManager.selectionLength() > 0)
		{
			selectionManager.DeleteSelection();
		}
	}

	public void ResetApplication() {
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		LinePanel[] linePanels = FindObjectsOfType<LinePanel>();

		foreach (GameObjectAgePair pair in linePool) {
			pair.heldObject.gameObject.SetActive(false);
		}
		foreach (GameObjectAgePair pair in emitterPool) {
			pair.heldObject.gameObject.SetActive(false);
		}
		foreach (GameObjectAgePair pair in ballPool) {
			pair.heldObject.gameObject.SetActive(false);
		}
		foreach (LinePanel panel in linePanels) {
			panel.ResetPanel();
		}

		LineDrawHandler.instance.DestroyLines();

		if (logger != null) logger.restartButtonClicks++;

		SetSpeedMultiplier(5f);
	}

	public void ResetAllButEmitters()
    {
		LinePanel[] linePanels = FindObjectsOfType<LinePanel>();

		foreach (GameObjectAgePair pair in linePool)
		{
			pair.heldObject.gameObject.SetActive(false);
		}
		foreach (GameObjectAgePair pair in ballPool)
		{
			pair.heldObject.gameObject.SetActive(false);
		}
		foreach (LinePanel panel in linePanels)
		{
			panel.ResetPanel();
		}

		LineDrawHandler.instance.DestroyLines();

		ClearFrogRules();

		SetSpeedMultiplier(5f);

		if (logger != null) logger.restartButtonClicks++;
	}

	public void ClearFrogRules()
    {
		foreach (TangibleController tangibleController in FindObjectsOfType<TangibleController>())
        {
			if (tangibleController != null)
            {
				tangibleController.RemoveAllRules();
            }
        }
    }
	public void ResetBalls()
    {
		foreach (GameObjectAgePair pair in ballPool)
		{
			pair.heldObject.gameObject.SetActive(false);
		}
	}

	public GameObject AssignLine(GameObject lineModel) {
		if (logger != null) logger.totalLinesCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;
		
		for (int i = 0; i < linePool.Count; i++) {
			if (linePool[i] == null) {
				linePool[i] = new GameObjectAgePair();
				linePool[i].heldObject = Instantiate(linePrefabs, sceneContainer.transform);
				linePool[i].age = 0f;
				linePool[i].heldObject.SetActive(false);
			}
			if (!linePool[i].heldObject.activeSelf) {
				myObject = linePool[i];
				index = i;
				break;
			}
			if (linePool[i].age < lowestAge) {
				lowestAge = linePool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (linePool.Count < maxLines) {
				index = linePool.Count;

				GameObjectAgePair newLine = new GameObjectAgePair();
				newLine.heldObject = Instantiate(linePrefabs, sceneContainer.transform);
				newLine.age = 0f;
				newLine.heldObject.SetActive(false);
				linePool.Add(newLine);
				myObject = newLine;
			} else {
				myObject = linePool[index];
			}
		}

		myObject.heldObject.name = lineModel.name + index;
		myObject.age = Time.time;

		myObject.heldObject.SetActive(true);
		myObject.heldObject.GetComponent<Line>().BecomeCloneOf(lineModel);

		return myObject.heldObject;
	}

	public GameObject AssignEmitter(GameObject emitterModel) {
		if (logger != null) logger.totalEmittersCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;

		for (int i = 0; i < emitterPool.Count; i++) {
			if (emitterPool[i] == null) {
				emitterPool[i] = new GameObjectAgePair();
				emitterPool[i].heldObject = Instantiate(emitterModel, sceneContainer.transform);
				emitterPool[i].age = 0f;
				emitterPool[i].heldObject.SetActive(false);
			}
			if (!emitterPool[i].heldObject.activeSelf) {
				myObject = emitterPool[i];
				index = i;
				break;
			}
			if (emitterPool[i].age < lowestAge) {
				lowestAge = emitterPool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (emitterPool.Count < maxEmitters) {
				index = emitterPool.Count;

				GameObjectAgePair newEmitter = new GameObjectAgePair();
				newEmitter.heldObject = Instantiate(emitterModel, sceneContainer.transform);
				newEmitter.age = 0f;
				newEmitter.heldObject.SetActive(false);
				emitterPool.Add(newEmitter);
				myObject = newEmitter;
			} else {
				myObject = emitterPool[index];
			}
		}
		
		myObject.heldObject.name = emitterModel.name + index;
		myObject.age = Time.time;

		myObject.heldObject.SetActive(true);
		if (myObject.heldObject.TryGetComponent<Emitter>(out Emitter e))
        {
			e.BecomeCloneOf(emitterModel);
        }
		else if (myObject.heldObject.TryGetComponent<EmitterTangible>(out EmitterTangible et))
        {
			et.BecomeCloneOf(emitterModel);
        }
		
		return myObject.heldObject;
	}

	public GameObject AssignBall(GameObject ballModel) {
		if (logger != null) logger.totalBallsCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;

		for (int i = 0; i < ballPool.Count; i++) {
			if (ballPool[i] == null) {
				ballPool[i] = new GameObjectAgePair();
				ballPool[i].heldObject = Instantiate(ballPrefabs, sceneContainer.transform);
				ballPool[i].age = Time.time;
				ballPool[i].heldObject.SetActive(false);
			}
			if (!ballPool[i].heldObject.activeSelf) {
				myObject = ballPool[i];
				index = i;
				break;
			}
			if (ballPool[i].age < lowestAge) {
				lowestAge = ballPool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (ballPool.Count < maxBalls) {
				index = ballPool.Count;

				GameObjectAgePair newBall = new GameObjectAgePair();
				newBall.heldObject = Instantiate(ballPrefabs, sceneContainer.transform);
				newBall.age = 0f;
				newBall.heldObject.SetActive(false);
				ballPool.Add(newBall);
				myObject = newBall;
			} else {
				myObject = ballPool[index];
			}
		}

		myObject.heldObject.name = ballModel.name + index;
		myObject.age = Time.time;

		myObject.heldObject.SetActive(true);
		myObject.heldObject.GetComponent<Ball>().BecomeCloneOf(ballModel);

		return myObject.heldObject;
	}

	public void ReplaceLine(GameObject lineReplacement) {
		if (logger != null) logger.totalLinesCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;

		for (int i = 0; i < linePool.Count; i++) {
			if (linePool[i] == null) {
				linePool[i] = new GameObjectAgePair();
				linePool[i].heldObject = Instantiate(linePrefabs, sceneContainer.transform);
				linePool[i].age = 0f;
				linePool[i].heldObject.SetActive(false);
			}
			if (!linePool[i].heldObject.activeSelf) {
				myObject = linePool[i];
				index = i;
				break;
			}
			if (linePool[i].age < lowestAge) {
				lowestAge = linePool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (linePool.Count < maxLines) {
				index = linePool.Count;

				GameObjectAgePair newLine = new GameObjectAgePair();
				newLine.heldObject = Instantiate(linePrefabs, sceneContainer.transform);
				newLine.age = 0f;
				newLine.heldObject.SetActive(false);
				linePool.Add(newLine);
				myObject = newLine;
			} else {
				myObject = linePool[index];
			}
		}

		Destroy(myObject.heldObject.gameObject);
		linePool[index].heldObject = lineReplacement;

		linePool[index].heldObject.name = lineReplacement.name + index;
		linePool[index].age = Time.time;

		linePool[index].heldObject.SetActive(true);

	}

	public void ReplaceEmitter(GameObject emitterReplacement) {
		if (logger != null) logger.totalEmittersCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;

		for (int i = 0; i < emitterPool.Count; i++) {
			if (emitterPool[i] == null) {
				emitterPool[i] = new GameObjectAgePair();
				emitterPool[i].heldObject = Instantiate(emitterPrefabs, sceneContainer.transform);
				emitterPool[i].age = 0f;
				emitterPool[i].heldObject.SetActive(false);
			}
			if (!emitterPool[i].heldObject.activeSelf) {
				myObject = emitterPool[i];
				index = i;
				break;
			}
			if (emitterPool[i].age < lowestAge) {
				lowestAge = emitterPool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (emitterPool.Count < maxEmitters) {
				index = emitterPool.Count;

				GameObjectAgePair newEmitter = new GameObjectAgePair();
				newEmitter.heldObject = Instantiate(emitterPrefabs, sceneContainer.transform);
				newEmitter.age = 0f;
				newEmitter.heldObject.SetActive(false);
				emitterPool.Add(newEmitter);
				myObject = newEmitter;
			} else {
				myObject = emitterPool[index];
			}
		}

		Destroy(myObject.heldObject.gameObject);
		emitterPool[index].heldObject = emitterReplacement;

		emitterPool[index].heldObject.name = emitterReplacement.name + index;
		emitterPool[index].age = Time.time;

		emitterPool[index].heldObject.SetActive(true);

	}

	public void ReplaceBall(GameObject ballReplacement) {
		if (logger != null) logger.totalBallsCreated++;

		float lowestAge = Time.time;
		int index = 0;
		GameObjectAgePair myObject = null;

		for (int i = 0; i < ballPool.Count; i++) {
			if (ballPool[i] == null) {
				ballPool[i] = new GameObjectAgePair();
				ballPool[i].heldObject = Instantiate(ballPrefabs, sceneContainer.transform);
				ballPool[i].age = 0f;
				ballPool[i].heldObject.SetActive(false);
			}
			if (!ballPool[i].heldObject.activeSelf) {
				myObject = ballPool[i];
				index = i;
				break;
			}
			if (ballPool[i].age < lowestAge) {
				lowestAge = ballPool[i].age;
				index = i;
			}
		}

		if (myObject == null) {
			if (ballPool.Count < maxBalls) {
				index = ballPool.Count;

				GameObjectAgePair newBall = new GameObjectAgePair();
				newBall.heldObject = Instantiate(ballPrefabs, sceneContainer.transform);
				newBall.age = 0f;
				newBall.heldObject.SetActive(false);
				ballPool.Add(newBall);
				myObject = newBall;
			} else {
				myObject = ballPool[index];
			}
		}

		Destroy(myObject.heldObject.gameObject);
		ballPool[index].heldObject = ballReplacement;

		ballPool[index].heldObject.name = ballReplacement.name + index;
		ballPool[index].age = Time.time;

		ballPool[index].heldObject.SetActive(true);

	}

	public void MarkNewDrawnLine()
    {
		if (logger != null) logger.totalLinesCreated++;
	}

	public void MarkTutorialOpened(Tutorial t)
    {
		if (logger != null)
		{
			logger.helpButtonClicks++;
			logger.IncTutorialClicks(t.replaceString);
		}
    }

	public void MarkNewRule()
    {
		if (logger != null) logger.totalRulesCreated++;
    }

	public static bool IsAnyElementActive()
    {
		//if (FindObjectsOfType<EmitterTangible>(false).Length > 0) return true;
		if (FindObjectsOfType<DrawnLine>(false).Length > 0) return true;
		if (FindObjectsOfType<Ball>(false).Length > 0) return true;

		return false;
	}
}

[System.Serializable]
public class GameObjectAgePair {
	public GameObject heldObject;
	public float age;
}