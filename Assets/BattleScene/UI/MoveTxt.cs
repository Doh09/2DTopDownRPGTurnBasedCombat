using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTxt : MonoBehaviour
{
    private CanvasRenderer cRenderer;
    public float moveSpeed = 0.01f;
    [Tooltip("Lower is faster")]
    public float fadeSpeed = 0.98f;

    // Use this for initialization
	void Start ()
	{
        cRenderer = GetComponentInChildren<CanvasRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        cRenderer.SetAlpha(cRenderer.GetAlpha() * fadeSpeed);
        transform.position += Vector3.up* moveSpeed;
	}
}
