using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : BasePopup
{
    [SerializeField] private TMP_Text contentText;
    private Coroutine currentCoroutine;
    public void Show(string content, float duration = 5f)
    {
        base.Show(duration);
        contentText.text = content;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(HideAfterDuration(duration));
    }

    private IEnumerator HideAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Hide();
    }
}
