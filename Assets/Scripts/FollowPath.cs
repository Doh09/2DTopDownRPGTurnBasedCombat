using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FollowPath : MonoBehaviour
{

    public MovementPath Path;

    private MovementPath.PathTypes pathType;
    private IEnumerator<Transform> pointInPath;//used to reference points returned from MyPath.GetNextPathPoint

    public MovementType Type = MovementType.MoveTowards; //Movement type used
    public static MovementPath MyPath; //Reference to Movement Path Used
    public float Speed = 1; //Speed object is moving
    public float MaxDistanceToGoal = .1f; //How close does it have to be to the point to be considered at point
    public Transform StartingPoint;

    public PathTypes PathType; //Indicates type of path (Linear or Looping)
    public int movementDirection = 1; //1 clockwise/forward || -1 counter clockwise/backwards

    public int movingTo = 0; //used to identify point in PathSequence we are moving to
   

    public enum PathTypes
    {
        Linear,
        Loop
    }

    public enum MovementType //Type of Movement
    {
        MoveTowards,
        LerpTowards
    }

	// Use this for initialization
	void Start ()
	{

        //Make sure there is a path assigned
        if (Path.PathSequence  == null)
        {
            Debug.LogError("Movement Path cannot be null, I must have a path to follow.", gameObject);
            return;
        }

        //Sets up a reference to an instance of the coroutine GetNextPathPoint
        pointInPath = GetNextPathPoint();

        //Get the next point in the path to move to (Gets the Default 1st value)
        pointInPath.MoveNext();

        //Make sure there is a point to move to
	    if (pointInPath.Current == null)
	    {
	        Debug.LogError("A path must have points in it to follow", gameObject);
            return; //Exit start() if there is no point to move to
	    }

        //Set the position of this object to the position of our starting point
	    transform.position = StartingPoint.position;
	}
	
	// Update is called once per frame
	void Update () {

        //Validate there is a path with a point in it
	    if (pointInPath == null || pointInPath.Current == null)
	    {
	        return; // exit if no point is found
	    }

	    if (Type == MovementType.MoveTowards) // if you are using MoveTowards movement type
	    {
	        //Move to the next point in path using MoveTowards
	        transform.position = Vector3.MoveTowards(transform.position,
                pointInPath.Current.position ,
                Time.deltaTime * Speed);
	    }
	    else if(Type == MovementType.LerpTowards) //if you are using LerpTowards movement type
	    {
	         //Move towards the next point in path using Lerp
	        transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime*Speed);
	    }


        //Check to see if you are close enough to the next point to start moving to the following one
        //using pythagorean theorem
        //per unity suaring a number is faster than the square root of a number
        //using .sqrMagnitude
	    var distanceSquared = (transform.position - pointInPath.Current.position).sqrMagnitude;
	    if (distanceSquared < MaxDistanceToGoal*MaxDistanceToGoal)
	    {
	        pointInPath.MoveNext(); //Get next point in MovementPath
	    }     
}

    public IEnumerator<Transform> GetNextPathPoint()
    {
        //makes sure that your sequence has points in it
        //and that there at least two points to constitute a path

        if (Path.PathSequence == null || Path.PathSequence.Length < 1)
        {
            yield break; //exits the coroutine sequence length check fails
        }

        while (true) //does not infinite loop due to yield return!!
        {
            //Return the current point in PathSequence
            //and wait for the next cell of enumerator (prevents infinite loop)
            yield return Path.PathSequence[movingTo];

            //If Linear path move from start to end then end to start then repeat
            if (PathType == PathTypes.Linear)
            {
                //If you are at the begining of the path
                if (movingTo <= 0)
                {
                    movementDirection = 1; //set to 1 to move forward
                }

                else if (movingTo >= Path.PathSequence.Length - 1)
                {
                    movementDirection = -1; //set to -1 to move backwards
                }
            }

            movingTo = movingTo + movementDirection;
            //movementDirection should always be either 1 or -1
            //We add direction to the index to move us to the 
            //next point in the sequence of points in our path

            //For Looping path you must move the index when you reach
            //the begíning or end of the PathSequence to loop the path
            if (PathType == PathTypes.Loop)
            {
                //If you just moved past the last point(moving forward)
                if (movingTo >= Path.PathSequence.Length)
                {
                    movingTo = 0;
                }
                //If you just moved past the first point(moving backwards)
                if (movingTo < 0)
                {
                    //Set the next point to move to as the last point in sequence
                    movingTo = Path.PathSequence.Length - 1;
                }
            }
        }
    }


}
