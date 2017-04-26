using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementPath : MonoBehaviour
{

    public PathTypes PathType; //Indicates type of path (Linear or Looping)
   // public int movementDirection = 1; //1 clockwise/forward || -1 counter clockwise/backwards
   // public int movingTo = 0; //used to identify point in PathSequence we are moving to
    public Transform[] PathSequence; //Array of all points in path

    //types of movement paths..
    public enum PathTypes
    {
        Linear,
        Loop
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //OnDraw will draw lines between our points in the Unity Editor
    //these lines will allow us to easily see path that our moving object will follow in the game
    public void OnDrawGizmos()
    {
        //Make sure that your sequence has points in it 
        //and that there are at least two points to constitute a path
        if (PathSequence == null || PathSequence.Length < 2)
        {
            return; //Exits OnDrawGizmos if no lines is needed
        }

        //Loop though all of the points in the sequence of points
        for (var i = 1; i < PathSequence.Length; i++)
        {
            //draw a line between the points
            Gizmos.DrawLine(PathSequence[i -1].position, PathSequence[i].position);
        }

        //if your path loops back to the begining when it reaches the end
        if (PathType == PathTypes.Loop)
        {
            //draw a line from last point to the first point in the sequence
            Gizmos.DrawLine(PathSequence[0].position, PathSequence[PathSequence.Length -1].position);
        }
    }

}
