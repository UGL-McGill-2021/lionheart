using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Ziqi
/// Script for background birds using Steering Behaviour method
/// </summary>
public class Bird : MonoBehaviour
{
    [Header("Steering behaviour variables")]
    public float maxForce = 5f;  // the force limit
    public float maxSpeed = 4f;  // the speed limit of this slime
    public float neighborRadius = 0f;  // the radius for neighbor consideration
    public float desiredSeparationDistance = 1f;  // the disired separation distance for this bird

    private Rigidbody ThisRB;  // rigid body component of this bird
    private FlockManager BirdsManager;
    private SpriteRenderer ThisSR;

    // Start is called before the first frame update
    void Start()
    {
        ThisRB = this.gameObject.GetComponent<Rigidbody>();
        ThisSR = this.GetComponent<SpriteRenderer>();
        BirdsManager = GameObject.FindWithTag("BirdsManager").GetComponent<FlockManager>(); // get the instance of the game flow manager script
        InitializeMotion();
    }

    private void Update()
    {
        // flip the spirite depending on the moving direction
        if (ThisRB.velocity.x >= 0 && ThisSR.flipX)
        {
            ThisSR.flipX = false;
        }
        else if(ThisRB.velocity.x < 0 && !ThisSR.flipX)
        {
            ThisSR.flipX = true;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to calculate the flee force for this bird
    /// </summary>
    /// <returns></returns>
    void InitializeMotion()
    {
        //ThisRB.AddForce(new Vector3(maxForce*3, 0, 0));
        ThisRB.velocity = new Vector3(maxForce * 2, 0.5f, 0);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to calculate the flee force for this bird
    /// </summary>
    /// <returns></returns>
    public Vector3 Flee()
    {
        Vector3 totalFleeVector = Vector3.zero;  // the total force caused by flee behavior of this bird
        int numNeighborUnit = 0;  // keep count the objects which are too close to this slime

        // iterate the list containing all units
        foreach (GameObject unit in BirdsManager.AllBirdsList)
        {
            // if the unit is not this bird itself
            if (unit != this.gameObject)
            {
                Vector3 UnitPostion = unit.transform.position;
                //objPostion.y = this.transform.position.y;  // calculate position based on the same x-z plane
                float distance = Vector3.Distance(this.transform.position, UnitPostion);  // calculate the distance between this slime and the object

                // if the distance is too close, we have to add flee force
                if (distance < this.desiredSeparationDistance)
                {
                    numNeighborUnit++;
                    // get the normalized vector from the object to this unit
                    Vector3 normalSepVector = (this.transform.position - UnitPostion).normalized;

                    normalSepVector /= distance;  // if the distance is small, the separation vector will be big (we will flee with more force)
                    totalFleeVector += normalSepVector;
                }
            }
        }

        // if this bird has to flee from any object
        if (numNeighborUnit > 0)
        {
            // calculate the average flee vector for this bird
            Vector3 avgFleeVector = (totalFleeVector / numNeighborUnit).normalized;
            avgFleeVector *= maxSpeed;  // apply the max speed to this flee vector -> getting the expected flee velocity

            // compute the supplementary flee force we have to apply (expected velocity - current velocity)
            Vector3 fleeForce = avgFleeVector - this.ThisRB.velocity;

            // Check whether the flee force exceed the force limit
            if (fleeForce.magnitude > this.maxForce)
            {
                fleeForce = fleeForce.normalized * maxForce;
            }
            return fleeForce;
        }
        return Vector3.zero;  // return 0 force if no unit is close to this bird (no flee force)
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to calculate the seek force for this bird
    /// </summary>
    /// <returns></returns>
    public Vector3 Seek(Vector3 target)
    {
        Vector3 seekForce = (target - transform.position).normalized * maxSpeed - ThisRB.velocity;

        // Check whether the flee force exceed the force limit
        if (seekForce.magnitude > this.maxForce)
        {
            seekForce = seekForce.normalized * maxForce;
        }
        return seekForce;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to calculate the follow force for this bird
    /// </summary>
    /// <returns></returns>
    public Vector3 Follow(List<GameObject> FlockList)
    {
        Vector3 avgFlockVelocity = Vector3.zero;  // the average veocity of the flock that this bird belongs to

        foreach(GameObject unit in FlockList)
        {
            avgFlockVelocity += unit.GetComponent<Rigidbody>().velocity;
        }
        avgFlockVelocity /= FlockList.Count;

        // compute the supplementary follow force we have to apply (expected velocity - current velocity)
        Vector3 followForce = avgFlockVelocity - this.ThisRB.velocity;

        // Check whether the follow force exceed the force limit
        if (followForce.magnitude > this.maxForce)
        {
            followForce = followForce.normalized * maxForce;
        }
        return followForce;
    }
}
