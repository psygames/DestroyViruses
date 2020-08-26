using UnityEngine;

public class RadioGroup : RadioBase
{
    [SerializeField]
    private RadioItem[] radios = new RadioItem[1];

    public override void Radio(int index)
    {
        base.Radio(index);

        if (radios == null)
            return;

        foreach (var radio in radios)
        {
            radio.Radio(index);
        }
    }

    public override int GetOptionCount()
    {
        if (radios == null)
            return 0;

        var min = int.MaxValue;
        foreach (var radio in radios)
        {
            if (radio == null)
                continue;
            var count = radio.GetOptionCount();
            if (min >= count)
                min = count;
        }
        if (min == int.MaxValue)
            return 0;
        return min;
    }
}
