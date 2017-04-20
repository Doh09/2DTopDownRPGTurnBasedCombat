using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;
    public bool Activated;
    public float TransitionSpeed;
    public float currentValue;

    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            Debug.Log("b is pressed");
            Activated = !Activated;
        }
        if (Activated)
        {
            currentValue = TransitionMaterial.GetFloat("_Cutoff");
            if (currentValue < 1f)
            { 
            currentValue += TransitionSpeed;
            TransitionMaterial.SetFloat("_Cutoff", currentValue);
            }
        }
        else if (!Activated)
        {
            currentValue = TransitionMaterial.GetFloat("_Cutoff");
            if (currentValue > 0f)
            {
                currentValue -= TransitionSpeed;
                TransitionMaterial.SetFloat("_Cutoff", currentValue);
            }
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
}
