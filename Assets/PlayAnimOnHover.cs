using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAnimOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animation animToPlay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        animToPlay.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animToPlay[animToPlay.clip.name].time = 0f;
        animToPlay.Sample();
        animToPlay.Stop();
    }

    private void OnMouseOver()
    {
        //Debug.Log("onover");
    }
}
