using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public float StartingValue;

    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = Mathf.Max(0f, value);

            if(_value <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Awake()
    {
        Value = StartingValue;
    }

    public float Extract(float amount)
    {
        float extracted = Mathf.Min(amount, _value);
        Value -= extracted;
        return extracted;
    }
}
