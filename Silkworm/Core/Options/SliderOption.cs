namespace Silkworm.Core.Options;

public class SliderOption : Option<float>
{
    public float MinValue { get; internal set; }
    public float MaxValue { get; internal set; }
    public string ValueFormat { get; internal set; }

    public SliderOption(string id, string text, float minValue, float maxValue, float defaultvalue, string valueFormat) : base(id, text, defaultvalue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        ValueFormat = valueFormat;
    }
}
