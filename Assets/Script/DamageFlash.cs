
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.25f;
    [SerializeField] private float maxAlpha = 0.55f;

    private Coroutine flashCoroutine;

    public void PlayFlash()
    {
        if (flashImage == null)
        {
            Debug.LogWarning("DamageFlash: Flash Image가 연결되지 않았습니다.");
            return;
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float elapsed = 0f;
        Color color = Color.red;

        flashImage.gameObject.SetActive(true);

        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / flashDuration);
            color.a = Mathf.Lerp(maxAlpha, 0f, t);

            flashImage.color = color;
            yield return null;
        }

        color.a = 0f;
        flashImage.color = color;

        flashCoroutine = null;
    }
}
