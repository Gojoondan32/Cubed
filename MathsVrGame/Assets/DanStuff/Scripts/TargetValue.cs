using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetValue : MonoBehaviour
{
    #region Singleton
    public static TargetValue instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private int result = 0;
    [SerializeField] private TextMeshProUGUI tmpText;
    public int Result { get { return result; } }

    private void Start()
    {
        UpdateResult();
    }

    public void UpdateResult()
    {
        result = Random.Range(10, 100);
        tmpText.text = result.ToString();
    }
}
