
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBoxChat : MonoBehaviour
{
    protected BaseText text;


    protected virtual void Awake()
    {
        text = GetComponentInChildren<BaseText>();
    }

    public abstract void SetData(string content, bool isActive);
    public abstract void SetData(string content, float maxWidth, bool isActive);
    public abstract void DoAppear(float duration);
}
