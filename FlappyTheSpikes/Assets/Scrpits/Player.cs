using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class Player : MonoBehaviour
{
    [SerializeField] LayerMask wall;
    Animator animator;

    [SerializeField] public Color playerColor;
    [SerializeField] bool colorInterpolation;
    [SerializeField] float durationLerp;
    float interpolationTime;
    Color lastColorInterpolated;

    PlayerController playerController;
    ObstaclesBehaviour obstacles;

    public float horizontalSpeed;
    public float jumpPower;

    public int amountMoney;

    public UnityAction playerDefeat;

    Light playerEmmision;
    public Material playerMatAndTrail;
    SpriteRenderer waveEffectSprite;

    int randInterpolation;
    public bool colorLerping;

    private void Start()
    {
        obstacles = FindObjectOfType<ObstaclesBehaviour>();

        waveEffectSprite = GetComponentInChildren<SpriteRenderer>();
        playerEmmision = GetComponentInChildren<Light>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        playerColor = GameManager.Instance.colorPlayerSelected;

        colorLerping = false;

        if (playerEmmision != null)
        {
            if(playerColor == Color.black)
                playerEmmision.color = Color.white;
            else
                playerEmmision.color = playerColor;
        }
        if(playerMatAndTrail != null)
            playerMatAndTrail.SetColor("_EmissionColor", playerColor);
        if (waveEffectSprite != null)
            waveEffectSprite.color = playerColor;

        lastColorInterpolated = playerColor;
    }

    private void Update()
    {
        if (!colorInterpolation)
            return;

        if(!colorLerping)
        {
            randInterpolation = Random.Range(0,100);
            StartCoroutine(LerpColors(randInterpolation));
        }

        if (playerEmmision != null)
        {
            if (playerColor == Color.black)
                playerEmmision.color = Color.white;
            else
                playerEmmision.color = playerColor;
        }
        if (playerMatAndTrail != null)
            playerMatAndTrail.SetColor("_EmissionColor", playerColor);
        if (waveEffectSprite != null)
            waveEffectSprite.color = playerColor;
    }

    IEnumerator LerpColors(int randomValue)
    {
        colorLerping = true;

        while (interpolationTime < durationLerp+durationLerp)
        {
            float mLerp = Mathf.PingPong(Time.time, durationLerp) / durationLerp;
            interpolationTime += Time.deltaTime;

            if (randomValue < 25)
                playerColor = Color.Lerp(Color.cyan, Color.blue, mLerp);
            else if (randomValue > 25 && randomValue < 50)
                playerColor = Color.Lerp(Color.green, Color.magenta, mLerp);
            else if (randomValue > 50 && randomValue < 75)
                playerColor = Color.Lerp(Color.yellow, Color.red, mLerp);
            else if (randomValue > 75 && randomValue <= 100)
                playerColor = Color.Lerp(Color.white, Color.black, mLerp);

            yield return new WaitForEndOfFrame();
        }
        colorLerping = false;
        interpolationTime = 0;
        yield return null;
    }

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Contains(wall, collision.gameObject.layer))
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
                StartCoroutine(shake.Shake(0.1f, 0.2f));

            animator.SetTrigger("Die");
            obstacles.obstaclesActivated = false;
            playerController.rb.isKinematic = true;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
