using Quintessential.Settings;

namespace HalvingMetallurgy;

public class HalvingMetallurgySettings
{
    public static HalvingMetallurgySettings Instance => HalvingMetallurgy.Instance.Settings as HalvingMetallurgySettings;

    [SettingsLabelKey("halving_metallurgy.settings.quickcopper_radioactive")]
    public bool quickcopperRadioactive = true;
}