using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PopTexts : MonoBehaviour
{
    [SerializeField] CanvasGroup alphaGroup;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform startPosition;

    public TextMeshProUGUI scoreGived;
    //public TextMeshProUGUI currencyEarned;

    void Start()
    {
        alphaGroup.alpha = 0;

        scoreGived.transform.position = startPosition.position;
        //currencyEarned.transform.position = startPosition.position;

        if(GameManager.Instance !=null)
        {
            GameManager.Instance.OnGainPoints += LerpPOPText;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGainPoints -= LerpPOPText;
        }

        StopAllCoroutines();
    }

    void LerpPOPText(TextMeshProUGUI text, float speed, float secondsToBanish)
    {
        IEnumerator LerpText()
        {
            float time = 0;

            while (time < secondsToBanish)
            {
                time += Time.deltaTime;

                alphaGroup.alpha += Mathf.Clamp(Time.deltaTime * 2,0,1);

                text.gameObject.transform.localPosition = Vector3.MoveTowards(text.gameObject.transform.localPosition, targetPosition.localPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(secondsToBanish);

            while (alphaGroup.alpha > 0.2)
            {
                alphaGroup.alpha -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            alphaGroup.alpha = 0;
            text.gameObject.transform.localPosition = startPosition.localPosition;

            yield break;
        }
        StartCoroutine(LerpText());
    }
}
