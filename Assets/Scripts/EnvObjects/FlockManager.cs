using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Ziqi Li
/// Manager for flock of birds
/// </summary>
public class FlockManager : MonoBehaviour
{
    [Header("Steering Behaviour variables")]
    public float FleeWeight = 1f;
    public float FollowWeight = 1.5f;
    public float SeekWeight = 2.5f;
    [Header("Flocks Random Motion variables")]
    public float RandTargetMagnitude = 8f;  // the magnitude of a random target for a flock
    public float RandTargetRadius = 3f;  // the radius range of a random target for a flock
    public GameObject FlyingAreaCenter;  // the center which every flock will fly around
    public float MaxRangeFromCenter = 50f;  // the max distance from flocks to the center of their flying area
    public float ChangeTargetColddown = 5f;
    public float MotionMultiplier_Y = 0.4f;


    [System.Serializable]
    public class FlocksList
    {
        public List<GameObject> Birds;  // the units of a single flock
        [HideInInspector]
        public Vector3 FlockMovingTarget;
    }
    public List<FlocksList> BirdFlocksList = new List<FlocksList>(); // list of list for moving units

    public List<GameObject> AllBirdsList { get; set; } = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        // add all units in every flock list into a single list
        foreach(FlocksList list in BirdFlocksList)
        {
            foreach(GameObject bird in list.Birds)
            {
                AllBirdsList.Add(bird);
            }
        }

        StartCoroutine(ChangeMovingTarget());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBirdFlockMotion();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to update the motion of units in flocks
    /// </summary>
    void UpdateBirdFlockMotion()
    {
        foreach (FlocksList list in BirdFlocksList)
        {
            foreach (GameObject unit in list.Birds)
            {
                if(unit != null)
                {
                    Rigidbody BirdRB = unit.GetComponent<Rigidbody>();
                    Bird BirdScript = unit.GetComponent<Bird>();
                    BirdRB.AddForce(BirdScript.Flee() * FleeWeight);  // apply flee force
                    BirdRB.AddForce(BirdScript.Follow(list.Birds) * FollowWeight);  // apply flee force
                    BirdRB.AddForce(BirdScript.Seek(list.FlockMovingTarget) * SeekWeight);
                }
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine to keep changing the flock moving target
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeMovingTarget()
    {
        do
        {
            foreach (FlocksList list in BirdFlocksList)
            {
                list.FlockMovingTarget = GetRandTarget(list);
            }
            yield return new WaitForSeconds(ChangeTargetColddown);
        } while (BirdFlocksList.Count > 0);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to get the geometric center of a flock of units
    /// </summary>
    /// <param name="list"></param>
    Vector3 GetFlockAvgPosition(FlocksList list)
    {
        Vector3 avgPosition = Vector3.zero;
        foreach(GameObject unit in list.Birds)
        {
            avgPosition += unit.transform.position;
        }
        return avgPosition / list.Birds.Count;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to get the average velocity of a flock of units
    /// </summary>
    /// <param name="list"></param>
    Vector3 GetFlockAvgVelocity(FlocksList list)
    {
        Vector3 avgVelocity = Vector3.zero;
        foreach (GameObject unit in list.Birds)
        {
            avgVelocity += unit.GetComponent<Rigidbody>().velocity;
        }
        return avgVelocity / list.Birds.Count;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to get a random target for a flock of birds
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Vector3 GetRandTarget(FlocksList list)
    {
        Vector3 randTarget;
        Vector3 avgPosition = GetFlockAvgPosition(list);
        Vector3 avgVelocity = GetFlockAvgVelocity(list).normalized;
        avgVelocity.y *= MotionMultiplier_Y;  // decrease the impact of vertical movement

        // find a random target in the moving direction of this flock
        Vector3 centerPos = FlyingAreaCenter.transform.position;

        // ignore z-axis
        Vector3 avgPos_xy = new Vector3(avgPosition.x, avgPosition.y, 0);
        centerPos.z = 0;
        // if this flock is out of flying range, set the center of flying area as target
        if (Vector3.Distance(avgPos_xy, centerPos) > MaxRangeFromCenter)
        {
            Vector2 rand = Random.insideUnitCircle * RandTargetRadius;
            randTarget = new Vector3(centerPos.x + rand.x, centerPos.y + rand.y, avgPosition.z);
        }
        else  // randomly select a forward point
        {
            Vector3 randCircleCenter = avgPosition + avgVelocity * RandTargetMagnitude;
            Vector2 rand = Random.insideUnitCircle * RandTargetRadius;
            randTarget = new Vector3(randCircleCenter.x + rand.x, randCircleCenter.y + rand.y, avgPosition.z);
        }
        return randTarget;
    }
}
