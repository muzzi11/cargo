using UnityEngine;
using System.Collections;

public class Laserbeam : MonoBehaviour
{
	public float travelTime = 0.08f;
	public float duration = 0.4f;
	private float startTime;
	private LineRenderer lineRenderer;
	private Vector3 start, end;

	// Use this for initialization
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		startTime = Time.time;
	}

	public void SetRay(Vector3 start, Vector3 end)
	{
		this.start = start;
		this.end = end;

	}
	
	// Update is called once per frame
	void Update()
	{
		float dt = Time.time - startTime;

		if(dt < duration)
		{
			float lengthFraction = Mathf.Clamp(dt / travelTime, 0.0f, 1.0f);
			float alpha = 1.0f;
			if(dt > travelTime)
			{
				alpha = (dt - travelTime) / (duration - travelTime);
				alpha = 1.0f - alpha * alpha;
			}
			Color color = new Color(1, 1, 1, alpha);

			lineRenderer.SetPosition(0, start);
			lineRenderer.SetPosition(1, start * (1 - lengthFraction) + end * lengthFraction);
			lineRenderer.SetColors(color, color); 
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
