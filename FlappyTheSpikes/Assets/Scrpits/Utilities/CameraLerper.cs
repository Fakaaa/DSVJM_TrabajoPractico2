using UnityEngine;
using UnityEngine.Events;

public class CameraLerper : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float speedLerp;
    public UnityAction OnLerpTargetEnded;
    #endregion

    #region PRIVATE_FIELDS
    private bool goTarget;
    private bool goOriginalPosition;
    private GManagerReference referenceGm;
    #endregion

    void Start()
    {
        referenceGm = FindObjectOfType<GManagerReference>();
        if(referenceGm != null)
        {
            referenceGm.OnGoShopMainMenu = OnGoTarget;
            referenceGm.OnExitShopMainMenu = OnGoOriginalPosition;
        }
        originalPosition = transform.position;
    }

    void LateUpdate()
    {
        if(goTarget)
        {
            if (transform.position != targetPosition.position)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speedLerp * Time.deltaTime);
            else
                OnLerpTargetEnded?.Invoke();
        }
        else if(goOriginalPosition)
        {
            if (transform.position != originalPosition)
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speedLerp * Time.deltaTime);
        }
    }
    void OnGoTarget()
    {
        goOriginalPosition = false;
        goTarget = true;
    }

    void OnGoOriginalPosition()
    {
        goTarget = false;
        goOriginalPosition = true;
    }
}
