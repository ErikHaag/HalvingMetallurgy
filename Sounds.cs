using System.Collections.Generic;
using System.Reflection;

namespace HalvingMetallurgy
{
    internal static class Sounds
    {


        public static Sound Halves;
        public static Sound Quake;
        public static Sound SumpConsume;
        public static Sound SumpEject;
        public static Sound SumpDiscard;
        public static Sound Remission;
        public static Sound Shearing;
        public static Sound ShearingMakingQuickcopper;
        public static Sound Osmosis;

        public static void LoadSounds()
        {
            Halves = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/halves").method_1087();
            Quake = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/quake").method_1087();
            SumpConsume = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/sump_consume").method_1087();
            SumpEject = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/sump_eject").method_1087();
            SumpDiscard = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/sump_discard").method_1087();
            Remission = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/remission").method_1087();
            Shearing = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/shearing").method_1087();
            ShearingMakingQuickcopper = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/shearing_making_quickcopper").method_1087();
            Osmosis = Brimstone.API.GetSound(HalvingMetallurgy.contentPath, "sounds/osmosis").method_1087();

            FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

            volumeDictionary.Add("halves", 0.5f);
            volumeDictionary.Add("quake", 0.3f);
            volumeDictionary.Add("sump_consume", 0.3f);
            volumeDictionary.Add("sump_eject", 0.3f);
            volumeDictionary.Add("sump_discard", 0.3f);
            volumeDictionary.Add("remission", 0.3f);
            volumeDictionary.Add("shearing", 0.3f);
            volumeDictionary.Add("shearing_making_quickcopper", 0.2f);
            volumeDictionary.Add("osmosis", 0.3f);

            On.class_201.method_540 += Sounds.Method_540;
        }

        public static void Unload()
        {
            On.class_201.method_540 -= Sounds.Method_540;
        }

        public static void Method_540(On.class_201.orig_method_540 orig, class_201 self)
        {
            orig(self);
            Halves.field_4062 = false;
            Quake.field_4062 = false;
            SumpConsume.field_4062 = false;
            SumpEject.field_4062 = false;
            SumpDiscard.field_4062 = false;
            Remission.field_4062 = false;
            Shearing.field_4062 = false;
            ShearingMakingQuickcopper.field_4062 = false;
            Osmosis.field_4062 = false;
        }
    }
}