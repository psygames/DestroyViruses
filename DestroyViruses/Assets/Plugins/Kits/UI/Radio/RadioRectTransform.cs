using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RadioRectTransform : RadioItem
{
    private RectTransform rectTransform = null;

    [SerializeField]
    private bool useAnchorPos = false;
    [SerializeField]
    private bool useSizeDelta = false;
    [SerializeField]
    private bool useOffset = false;

    [SerializeField]
    private Vector2[] anchorPosition = new Vector2[2];
    [SerializeField]
    private Vector2[] sizeDelta = new Vector2[2];
    [SerializeField]
    private Vector2[] offsetMin = new Vector2[2];
    [SerializeField]
    private Vector2[] offsetMax = new Vector2[2];

    public override void Radio(int index)
    {
        base.Radio(index);
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
            return;
        if (useSizeDelta && sizeDelta != null && index < sizeDelta.Length)
            rectTransform.sizeDelta = sizeDelta[index];
        if (useAnchorPos && anchorPosition != null && index < anchorPosition.Length)
            rectTransform.anchoredPosition = anchorPosition[index];
        if (useOffset && offsetMin != null && index < offsetMin.Length)
            rectTransform.offsetMin = offsetMin[index];
        if (useOffset && offsetMax != null && index < offsetMax.Length)
            rectTransform.offsetMax = offsetMax[index];
    }

    public override int GetOptionCount()
    {
        if (!useAnchorPos && !useSizeDelta && !useOffset)
            return 0;
        return Mathf.Min(useAnchorPos && anchorPosition != null ? anchorPosition.Length : int.MaxValue,
            useSizeDelta && sizeDelta != null ? sizeDelta.Length : int.MaxValue,
            useOffset && offsetMin != null ? offsetMin.Length : int.MaxValue,
            useOffset && offsetMax != null ? offsetMax.Length : int.MaxValue);
    }
}
