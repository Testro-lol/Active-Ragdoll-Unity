using UnityEngine;
using System.Collections;

public class BetterIKFootSolver : MonoBehaviour
{
    //This is the thing the leg will try to reach
    public Transform homeTransform;

    //This is for floor calculations
    public Transform floorHeightTracker;

    // for calculating the speed of the character
    public Rigidbody hips;

    public LayerMask ground;

    public float floorHeight = 0.05f;

    //How much you want the step to overshoot the home, can get buggy at High values
    public float stepOvershootFraction;

    //How far away the leg is from the home until it wants to move
    public float wantStepAtDistance;

    //The step speed, the higher it is, the slower, I know, kinda counter intuitive
    public float maxSpeed = 0.1f;  // Maximum speed
    public float minSpeed = 0.22f; // Minimum speed
    public float moveDuration;

    public float stepHeight;

    public bool isMoving;

    Vector3 currentPosition;

    private void Start()
    {
        currentPosition = transform.position; 
    }

    public void TryMove()
    {
        if (isMoving) return;

        float distFromHome = Vector3.Distance(transform.position, homeTransform.position);

        // If we are too far off in position or rotation
        if (distFromHome > wantStepAtDistance)
        {
            StartCoroutine(Move());
        }
    }
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(floorHeightTracker.position, Vector3.down, out hit, 3f, ground))
        {
            float groundHeight = hit.point.y + floorHeight;

            // Update the positions of left and right targets to touch the ground
            currentPosition.y = groundHeight;

            // Keep the targets from moving when the body does
            transform.position = currentPosition;

            // Set the target homeTransform position to touch the ground
            var newHomePos = homeTransform.position;
            newHomePos.y = groundHeight;
            homeTransform.position = newHomePos;
        }

        float velocityMagnitude = hips.velocity.magnitude;

        //Change the speed depending on how fast your going, the clamp is to get rid of extreme values
        moveDuration = Mathf.Lerp(minSpeed, maxSpeed, Mathf.InverseLerp(0f, 10f, velocityMagnitude));
        moveDuration = Mathf.Clamp(moveDuration, maxSpeed, minSpeed);

    }

    IEnumerator Move()
    {
        isMoving = true;

        Vector3 startPoint = transform.position;
        float totalDistance = Vector3.Distance(startPoint, homeTransform.position);

        // Directional vector from the foot to the home position
        Vector3 towardHome = (homeTransform.position - startPoint);
        // Total distance to overshoot by
        float overshootDistance = wantStepAtDistance * stepOvershootFraction;

        // Clamping the overshoot distance to a minimum value
        float minOvershootDistance = 0.1f; // Set your minimum overshoot distance here

        float clampedOvershootDistance = Mathf.Max(overshootDistance, minOvershootDistance);

        float timeElapsed = 0;
        do
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / moveDuration;

            // Gradually reduce overshoot as the object gets closer to the home
            float remainingDistance = Vector3.Distance(transform.position, homeTransform.position);
            float adjustedOvershootDistance = Mathf.Lerp(clampedOvershootDistance, 0f, remainingDistance / totalDistance);

            Vector3 overshootVector = towardHome * adjustedOvershootDistance;
            Vector3 endPoint = homeTransform.position + overshootVector;

            // Calculate the sine curve
            float sineValue = Mathf.Sin(normalizedTime * Mathf.PI); // Sine value ranges from -1 to 1
            Vector3 centerOffset = homeTransform.up * sineValue * stepHeight; // Adjust amplitude here

            // Move along the sine curve
            transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime) + centerOffset;

            currentPosition = transform.position;

            yield return null;
        }
        while (timeElapsed < moveDuration);

        isMoving = false;
    }
}