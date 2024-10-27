using BepInEx;
using BepInEx.Configuration;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Managers;

namespace BowUpgrades
{
    [BepInPlugin("TheRonTron.BowUpgrades", "BowUpgrades", "4.2.0")]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class BowUpgrades : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.BowUpgrades";
        public ConfigEntry<int> DraugrFangProjectiles;
        public ConfigEntry<int> HuntsmanProjectiles;
        private ButtonConfig SecondaryAttackButt;

        public void Awake()
        {
            GenerateConfig();
            Jotunn.Logger.ShowDate = true;
            PrefabManager.OnPrefabsRegistered += () =>
            {

                var KickinWing = PrefabManager.Instance.GetPrefab("BowDraugrFang").GetComponent<ItemDrop>().m_itemData.m_shared.m_secondaryAttack;
                PrefabManager.Instance.GetPrefab("BowDraugrFang").GetComponent<ItemDrop>().FixReferences();

                var DraugrFang = PrefabManager.Instance.GetPrefab("BowDraugrFang").GetComponent<ItemDrop>().m_itemData.m_shared;
                DraugrFang.m_attack.m_projectiles = DraugrFangProjectiles.Value;
                DraugrFang.m_attack.m_damageMultiplier = (float)1 / DraugrFangProjectiles.Value;
                DraugrFang.m_secondaryAttack = KickinWing;

                var Huntsman = PrefabManager.Instance.GetPrefab("BowHuntsman").GetComponent<ItemDrop>().m_itemData.m_shared.m_attack;
                Huntsman.m_projectiles = HuntsmanProjectiles.Value;
                Huntsman.m_damageMultiplier = (float)1 / HuntsmanProjectiles.Value;

                SecondaryAttackButt = new ButtonConfig
                {
                    Name = "AltAttack",
                    Key = UnityEngine.KeyCode.Mouse3
                };
                InputManager.Instance.AddButton(PluginGUID, SecondaryAttackButt);
            };
        }
        public void GenerateConfig()
        {
            Config.SaveOnConfigSet = true;

            DraugrFangProjectiles = Config.Bind("Server config", "Draugr Fang Projectiles", 2,
            new ConfigDescription("Number of Draugr Fang Bow projectiles.", null,
            new AcceptableValueRange<int>(1, 100),
            new ConfigurationManagerAttributes { IsAdminOnly = true }));

            HuntsmanProjectiles = Config.Bind("Server config", "Huntsman Bow Projectiles", 3,
            new ConfigDescription("Number of Huntsman Bow projectiles.", null,
            new AcceptableValueRange<int>(1, 100),
            new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }
    }
}