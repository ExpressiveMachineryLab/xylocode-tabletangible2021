using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour {
	public Vector2 ShadowOffset;
	public float relativeScale = 1f;
	public float relativeRotation = 0f;
	public Material ShadowMaterial;

	SpriteRenderer spriteRenderer;
	GameObject shadowGameobject;
	SpriteRenderer shadowSpriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		//create a new gameobject to be used as drop shadow
		shadowGameobject = new GameObject("Shadow");
		shadowGameobject.transform.localScale = transform.localScale * relativeScale;
		shadowGameobject.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, relativeRotation);
		shadowGameobject.transform.parent = transform;

		//create a new SpriteRenderer for Shadow gameobject
		shadowSpriteRenderer = shadowGameobject.AddComponent<SpriteRenderer>();

		//set the shadow gameobject's sprite to the original sprite
		shadowSpriteRenderer.sprite = spriteRenderer.sprite;
		//set the shadow gameobject's material to the shadow material we created
		shadowSpriteRenderer.material = ShadowMaterial;

		//update the sorting layer of the shadow to always lie behind the sprite
		shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
		shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
	}

	void LateUpdate() {
		//update the position and rotation of the sprite's shadow with moving sprite
		shadowGameobject.transform.position = transform.position + (Vector3)ShadowOffset;
		shadowSpriteRenderer.sprite = spriteRenderer.sprite;
	}
}