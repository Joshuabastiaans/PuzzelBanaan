using UnityEngine;

public class LaserLineRenderer : MonoBehaviour
{
    public float maxDistance = 100f;
    public int maxBounceCount = 3;
    public LayerMask reflectionLayerMask;
    public LayerMask interactionLayerMask;
    public GameObject destroyEffect;

    private Transform laserFirePoint;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserFirePoint = transform.GetChild(0);
    }

    private void Update()
    {
        EnableLaser();
    }

    private void EnableLaser()
    {
        Vector2 startPos = laserFirePoint.position;
        Vector2 direction = transform.right;
        int bounceCount = 0;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPos);

        while (bounceCount < maxBounceCount)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, reflectionLayerMask | interactionLayerMask);
            if (hit.collider != null)
            {
                if ((reflectionLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    direction = Vector2.Reflect(direction, hit.normal);
                    startPos = hit.point + direction * 0.001f; // Add a small offset to the starting position
                    bounceCount++;
                }
                else if ((interactionLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    // Perform interaction logic here
                    hit.collider.GetComponent<LaserObjectInteract>().LaserInteract();
                    break;
                }
            }
            else
            {
                Vector2 endPos = startPos + direction * maxDistance;
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPos);
                break;
            }
        }
    }
}
