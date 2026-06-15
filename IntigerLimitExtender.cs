global using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper;
using IntigerLimitExtender;
using MelonLoader;
using System;

[assembly: MelonInfo(typeof(IntigerLimitExtender.IntigerLimitExtender), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6-Epic")]

namespace IntigerLimitExtender;

public class IntigerLimitExtender : BloonsTD6Mod
{
    // Uses a double to hold cash way beyond 2.14 billion
    public static double actualCash = 0.0;
    public static double cashMultiplier = 1.0;

    // MelonLoader preferences entries for an enable toggle
    private static MelonPreferences_Category _prefsCategory;
    private static MelonPreferences_Entry<bool> _enabledEntry;

    public override void OnApplicationStart()
    {
    // Nothing required here for now
    }

    // The base implementation of OnInitializeMelon is sealed in the upstream
    // BloonsMod class, so hide it here instead of attempting to override it.
    public new void OnInitializeMelon()
    {
        MelonLogger.Msg("BTD6 Intiger Limit Extender Mod Initialized!");

        // Create a preferences category and an enabled toggle so users can turn the mod on/off
        _prefsCategory = MelonPreferences.CreateCategory(ModHelperData.Name);
        _enabledEntry = _prefsCategory.CreateEntry("Enabled", true, description: "Enable Intiger Limit Extender");
    }

    // Expose whether the mod is enabled via the preferences entry (default true)
    public static bool Enabled => _enabledEntry == null ? true : _enabledEntry.Value;

    // Intercepting the player's cash addition; only act when enabled
    public static void AddCash(double amount)
    {
        if (!Enabled)
            return;

        actualCash += amount * cashMultiplier;
    }

    // Method to check if you can afford an item
    public static bool CanAfford(double cost)
    {
        return actualCash >= cost;
    }

    // Method to deduct cash; only act when enabled
    public static void SpendCash(double amount)
    {
        if (!Enabled)
            return;

        if (CanAfford(amount))
        {
            actualCash -= amount;
        }
    }

    // This splits the cash into two parts for display:
    // e.g. Part 1 = Millions, Part 2 = Thousands
    public static void GetCashSplit(out int majorCash, out int minorCash)
    {
        majorCash = (int)(actualCash / 1000000);
        minorCash = (int)(actualCash % 1000000);
    }
}