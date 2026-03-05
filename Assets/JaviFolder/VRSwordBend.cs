using UnityEngine;

public class VRSwordBend : MonoBehaviour
{
    public Renderer swordRenderer;

    [Header("Tuning")]
    public float bendSensitivity = 0.005f;
    public float maxBend = 0.12f;
    public float springStrength = 120f;
    public float damping = 10f;
    public float tipLagStrength = 10f;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    private float bendX;
    private float bendY;

    private float bendVelX;
    private float bendVelY;

    private float tipX;
    private float tipY;

    private Vector3 previousAccel = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    MaterialPropertyBlock mpb;

    void Start()
    {
        lastPosition = transform.position;
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {

        float dt = Time.deltaTime;

        // --- Compute velocity ---
        Vector3 velocity = (transform.position - lastPosition) / dt;
        currentVelocity = velocity;

        Vector3 acceleration = (velocity - lastVelocity) / dt;
        acceleration = Vector3.Lerp(previousAccel, acceleration, 0.5f);
        previousAccel = acceleration;

        lastPosition = transform.position;
        lastVelocity = velocity;

        

        // --- Convert acceleration into local space ---
        Vector3 localAccel = transform.InverseTransformDirection(acceleration);

        // Opposite direction (blade lags behind movement)
        float targetX = -localAccel.x * bendSensitivity;
        float targetY = -localAccel.y * bendSensitivity;

        targetX = Mathf.Clamp(targetX, -maxBend, maxBend);
        targetY = Mathf.Clamp(targetY, -maxBend, maxBend);

        // --- Spring physics for base bend ---
        ApplySpring(ref bendX, ref bendVelX, targetX, dt);
        ApplySpring(ref bendY, ref bendVelY, targetY, dt);

        // --- Tip lag (secondary spring following base) ---
        tipX = Mathf.Lerp(tipX, bendX, tipLagStrength * dt);
        tipY = Mathf.Lerp(tipY, bendY, tipLagStrength * dt);

        // --- Send to shader ---
        swordRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_HandVelocityX", bendX);
        mpb.SetFloat("_HandVelocityY", bendY);
        mpb.SetFloat("_TipOffsetX", tipX);
        mpb.SetFloat("_TipOffsetY", tipY);

        swordRenderer.SetPropertyBlock(mpb);
    }

    void ApplySpring(ref float value, ref float velocity, float target, float dt)
    {
        float force = (target - value) * springStrength;
        velocity += force * dt;
        velocity -= velocity * damping * dt;
        value += velocity * dt;
    }

    

    public Vector3 GetVelocity()
    {
        return currentVelocity;
    }

   
}