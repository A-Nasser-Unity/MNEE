using UnityEngine;

public enum NoteType
{
    Green,
    Blue,
    Yellow,
    Red
}

public enum InputKey
{
    UpArrow,
    DownArrow,
    LeftArrow,
    RightArrow,
    W,
    A,
    S,
    D,
    Z,
    X,
    C,
    V,
    Space
}

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public NoteType noteType;
    public InputKey assignedKey = InputKey.UpArrow;
    public float moveSpeed = 15f;

    [Header("Hit Effects")]
    public GameObject greenEffectPrefab;
    public GameObject blueEffectPrefab;
    public GameObject yellowEffectPrefab;
    public GameObject redEffectPrefab;

    [Header("Hit Detection")]
    public float hitLineZ = 0f;
    public float perfectWindow = 0.3f;  // ±0.3 units
    public float goodWindow = 0.8f;     // ±0.8 units

    [Header("Scoring")]
    public int perfectScore = 15;
    public int goodScore = 10;

    private bool hasBeenHit = false;
    private bool isInHitZone = false;

    void Update()
    {
        // Move note toward cars
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;

        // Check if in hit zone
        float distanceFromLine = Mathf.Abs(transform.position.z - hitLineZ);
        isInHitZone = distanceFromLine <= goodWindow;

        // Check for player input
        if (isInHitZone && !hasBeenHit)
        {
            CheckInput();
        }

        // Destroy if passed hit line
        if (transform.position.z < hitLineZ - goodWindow)
        {
            Miss();
        }
    }

    void CheckInput()
    {
        bool keyPressed = IsKeyPressed(assignedKey);

        if (keyPressed)
        {
            Hit();
        }
    }

    bool IsKeyPressed(InputKey key)
    {
        switch (key)
        {
            case InputKey.UpArrow:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case InputKey.DownArrow:
                return Input.GetKeyDown(KeyCode.DownArrow);
            case InputKey.LeftArrow:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case InputKey.RightArrow:
                return Input.GetKeyDown(KeyCode.RightArrow);
            case InputKey.W:
                return Input.GetKeyDown(KeyCode.W);
            case InputKey.A:
                return Input.GetKeyDown(KeyCode.A);
            case InputKey.S:
                return Input.GetKeyDown(KeyCode.S);
            case InputKey.D:
                return Input.GetKeyDown(KeyCode.D);
            case InputKey.Z:
                return Input.GetKeyDown(KeyCode.Z);
            case InputKey.X:
                return Input.GetKeyDown(KeyCode.X);
            case InputKey.C:
                return Input.GetKeyDown(KeyCode.C);
            case InputKey.V:
                return Input.GetKeyDown(KeyCode.V);
            case InputKey.Space:
                return Input.GetKeyDown(KeyCode.Space);
            default:
                return false;
        }
    }

    void Hit()
    {
        hasBeenHit = true;

        float distanceFromLine = Mathf.Abs(transform.position.z - hitLineZ);

        // Determine hit quality
        if (distanceFromLine <= perfectWindow)
        {
            // Perfect Hit
            GameManager.instance.AddPlayerScore(perfectScore, "Perfect");
            GameManager.instance.ShowHitFeedback("Perfect");
            SpawnHitEffect();
            Debug.Log("Perfect!");
        }
        else if (distanceFromLine <= goodWindow)
        {
            // Good Hit
            GameManager.instance.AddPlayerScore(goodScore, "Good");
            GameManager.instance.ShowHitFeedback("Good");
            SpawnHitEffect();
            Debug.Log("Good!");
        }
        else
        {
            // Miss (outside good window)
            Miss();
            return;
        }

        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        GameObject effectPrefab = null;

        switch (noteType)
        {
            case NoteType.Green:
                effectPrefab = greenEffectPrefab;
                break;
            case NoteType.Blue:
                effectPrefab = blueEffectPrefab;
                break;
            case NoteType.Yellow:
                effectPrefab = yellowEffectPrefab;
                break;
            case NoteType.Red:
                effectPrefab = redEffectPrefab;
                break;
        }

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }

    void Miss()
    {
        GameManager.instance.ShowHitFeedback("Miss");
        GameManager.instance.ResetCombo();
        Debug.Log("Miss!");
        Destroy(gameObject);
    }
}