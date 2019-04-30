using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    [SerializeField] Slider s;

    Coroutine _cooldown;

    public void StartCooldown(float duration)
    {
        _cooldown = StartCoroutine(Cooldown());

        IEnumerator Cooldown()
        {
            s.gameObject.SetActive(true);

            CountDown cd = new CountDown(duration);
            while(!cd.isDone)
            {
                s.value = cd.Progress;
                yield return null;
            }

            s.gameObject.SetActive(false);
            _cooldown = null;
            yield break;
        }
    }
}
