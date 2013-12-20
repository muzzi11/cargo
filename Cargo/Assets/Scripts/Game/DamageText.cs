using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
	public float scroll = 0.05f;
	public float duration = 1.5f;
	private float startTime;
	
	void Start()
	{
		startTime = Time.time;
	}
	
	void Update()
	{
		float dt = Time.time - startTime;

		if(dt < duration)
		{
			var position = transform.position;
			var color = guiText.material.color;

			position.y += scroll * Time.deltaTime;
			transform.position = position;

			float alpha = dt / duration;
			alpha = 1.0f - alpha * alpha * alpha;
			color.a = alpha;
			guiText.material.color = color;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
