using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: Ziqi Li
/// Script for moving platform object
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public List<GameObject> PathPointObjects = new List<GameObject>();
    public float delay = 2f;
    public float speed = 3f;
    public float epsilon = 0.2f;
    public bool isAutomatic = true;  // whether this platform will move automatically

    [SerializeField]
    private List<Vector3> PathPoints = new List<Vector3>();
    private Vector3 CurrentTarget;
    private int CurrentTargetIndex;
    private bool isMovingToward = true;
    private bool isTriggered = false;  // motion of platform have to be triggered if !isAutomatic
    private float DelayTimer;
    private Coroutine CurrentMovingCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        // initialization
        isTriggered = isAutomatic;
        DelayTimer = delay;

        // Need at least 2 points to move
        if (PathPointObjects.Count > 1)
        {
            // convert to a Vector3 list
            foreach (GameObject obj in PathPointObjects) PathPoints.Add(obj.transform.position);

            CurrentTarget = PathPoints[0];
            CurrentTargetIndex = 0;
            //CurrentMovingCoroutine = StartCoroutine(MovePlatform());  // start the coroutine
        }
    }

    private void FixedUpdate()
    {
        DelayTimer += Time.deltaTime;

        if ((PathPoints.Count) > 1 && isTriggered && (DelayTimer > delay))
        {
            if (Vector3.Distance(this.transform.position, CurrentTarget) < epsilon)
            {
                updateTarget();

                // stop for a delay when reaching two ends of path
                if ((CurrentTargetIndex == 1 && isMovingToward) || (CurrentTargetIndex == PathPoints.Count - 2 && !isMovingToward))
                {
                    DelayTimer = 0;
                }
            }
            MovePlatform();
        }

    }


    /// <summary>
    /// Author: Ziqi Li
    /// Function for moving this platform
    /// </summary>
    private void MovePlatform()
    {
        Vector3 TargetDirection = CurrentTarget - this.transform.position;
        this.transform.Translate(TargetDirection.normalized * speed * Time.deltaTime);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for updating the next target position of the moving path from the PathPoints list
    /// </summary>
    private void updateTarget()
    {
        // If the platform goes back and forth along the path points
        if (isMovingToward)
        {
            if (CurrentTargetIndex < PathPoints.Count - 1)
            {
                CurrentTargetIndex++;
            }
            else
            {
                isMovingToward = false;
                CurrentTargetIndex--;
            }
        }
        else
        {
            if (CurrentTargetIndex == 0)
            {
                isMovingToward = true;
                CurrentTargetIndex++;
            }
            else
            {
                CurrentTargetIndex--;
            }
        }
        CurrentTarget = PathPoints[CurrentTargetIndex];  // update the current target
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isAutomatic) isTriggered = true;
        other.gameObject.transform.parent = transform;  // transport object on this platform
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == this.gameObject.transform) other.transform.parent = null;
    }

}
