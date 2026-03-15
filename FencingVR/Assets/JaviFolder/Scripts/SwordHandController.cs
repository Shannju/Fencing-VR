using UnityEngine;
using UnityEngine.XR;

public class SwordHandController : MonoBehaviour
{
    [Header("References")]
    public Renderer swordRenderer;
    public XRNode handNode = XRNode.RightHand;
    public XRInputProvider inputProvider;

    [Header("Displacement Settings")]
    public float velocityMultiplier = 1.5f;
    public float maxDisplacement = 1.0f;

    [Header("Spring Settings")]
    [Tooltip("How strongly the blade snaps back to rest")]
    public float springStiffness = 12f;
    [Tooltip("How quickly oscillation dies out — 1 = critically damped")]
    public float springDamping = 4f;

    [Header("Trail Settings")]
    [Tooltip("How much the tip lags behind direction changes")]
    public float tipLag = 6f;

    private InputDevice handDevice;
    private Material mat;

    // Spring state for X and Y axes
    private float dispX, dispY;
    private float velX, velY;

    // Tip has extra lag for a whip-like feel
    private float tipX, tipY;

    void Start()
    {
        mat = swordRenderer.material;
        TryGetXRDevice();
    }

    void Update()
    {
        if (!handDevice.isValid)
            TryGetXRDevice();

        // --- Get controller velocity ---
        Vector3 handVelocity = GetVelocity(handDevice);

        // Scale target by how fast the hand is moving
        float speed = handVelocity.magnitude;
        float dynamicMultiplier = velocityMultiplier * (1f + speed * 0.5f); // faster swing = more displacement


        // Target displacement is driven by hand velocity
        float targetX = Mathf.Clamp(-handVelocity.x * dynamicMultiplier, -maxDisplacement, maxDisplacement);
        float targetZ = Mathf.Clamp(-handVelocity.z * dynamicMultiplier, -maxDisplacement, maxDisplacement);

        // Dynamic damping — less damping on fast swings so motion isn't killed
        float dynamicDamping = springDamping / (1f + speed * 0.5f);

        // --- Spring physics per axis ---
        // Moving toward target with spring, damping kills oscillation
        float forceX = (targetX - dispX) * springStiffness - velX * dynamicDamping;
        float forceZ = (targetZ - dispY) * springStiffness - velY * dynamicDamping;

        velX += forceX * Time.deltaTime;
        velY += forceZ * Time.deltaTime;

        dispX += velX * Time.deltaTime;
        dispY += velY * Time.deltaTime;

        // --- Tip lags slightly behind base for whip/flex feel ---
        tipX = -dispX; // Mathf.Lerp(tipX, dispX, Time.deltaTime * tipLag);
        tipY = dispY; // Mathf.Lerp(tipY, dispY, Time.deltaTime * tipLag);

        //Debug.Log($"dispX: {dispX:F3}  dispY: {dispY:F3}  tipX: {tipX:F3}  tipY: {tipY:F3}");

        // Send to shader
        mat.SetFloat("_HandVelocityX", dispX);
        mat.SetFloat("_HandVelocityY", dispY);
        mat.SetFloat("_TipOffsetX", tipX);
        mat.SetFloat("_TipOffsetY", tipY);
    }

    private void TryGetXRDevice()
    {
        if (handNode == XRNode.LeftHand)
            handDevice = inputProvider.LeftHand;
        else if (handNode == XRNode.RightHand)
            handDevice = inputProvider.RightHand;
    }

    Vector3 GetVelocity(InputDevice device)
    {
        if (device.isValid &&
            device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 v))
            return v;
        return Vector3.zero;
    }

    void OnDestroy()
    {
        if (mat != null) Destroy(mat);
    }
}