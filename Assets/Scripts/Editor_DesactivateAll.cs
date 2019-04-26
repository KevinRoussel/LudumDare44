using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Editor_DesactivateAll : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] List<Transform> _except;
    void Awake()
    {
        foreach (Transform child in transform.Cast<Transform>().Where(i => !_except.Contains(i)))
        {
            child.gameObject.SetActive(false);
        }
    }
#endif

}
