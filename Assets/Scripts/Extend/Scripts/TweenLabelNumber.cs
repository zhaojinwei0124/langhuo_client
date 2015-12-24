using UnityEngine;

/// <summary>
/// Tween the number in UILabel
/// </summary>

public class TweenLabelNumber : UITweener
{
    public float from = 1;
    public float to = 1;

    UILabel mLabel;

    /// <summary>
    /// Current number.
    /// </summary>

	public float number
    {
        get
        {
			float n = 0;
			if (mLabel != null) float.TryParse(mLabel.text, out n);
            return n;
        }
        set
        {
            if (mLabel != null) mLabel.text = value.ToString("F1");
        }
    }

    /// <summary>
    /// Find all needed components.
    /// </summary>

    void Awake()
    {
        mLabel = GetComponentInChildren<UILabel>();
    }

    /// <summary>
    /// Interpolate and update the number in label.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        number = Mathf.Lerp(from, to, factor);
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

	static public TweenLabelNumber Begin(GameObject go, float duration, float from, float to)
    {
        TweenLabelNumber comp = UITweener.Begin<TweenLabelNumber>(go, duration);
        comp.from = from;
        comp.to = to;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
