using Quintessential.Settings;

namespace HalvingMetallurgy;
public class MySettings
{
    public static MySettings Instance => HalvingMetallurgy.self.Settings as MySettings;

    [SettingsLabel("Radioactive Quickcopper")]
    public bool quickcopperRadioactive = true;
}