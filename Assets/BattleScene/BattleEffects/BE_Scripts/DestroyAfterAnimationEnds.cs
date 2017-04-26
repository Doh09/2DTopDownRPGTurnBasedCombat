using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyAfterAnimationEnds : MonoBehaviour
{

    public float offsetInSeconds = 0f;
    public bool destroyParent = false;
    [Tooltip("Must have an audio source if it has to wait for sound to finish.")]
    public bool alsoWaitForSoundToFinish = false;
    // Use this for initialization
    void Start()
    {
        Animator animator = gameObject.GetComponent<Animator>();

        GameObject toDestroy = gameObject;
        if (destroyParent)
            toDestroy = transform.parent.gameObject;
        float timeToWait = animator.GetCurrentAnimatorStateInfo(LayerMask.NameToLayer("Default")).length + offsetInSeconds;
        if (alsoWaitForSoundToFinish)
        {
            
            float soundLength = GetComponent<AudioSource>().clip.length;
            if (soundLength > timeToWait)
                timeToWait = soundLength;
        }
        Destroy(toDestroy, timeToWait);
    }
}
