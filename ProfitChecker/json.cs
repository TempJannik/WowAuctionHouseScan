using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProfitChecker
{
    #region BaseFile
    public class Filer
    {
        public string url { get; set; }
        public long lastModified { get; set; }
    }

    public class RootObjectBase
    {
        public List<Filer> files { get; set; }
    }
    #endregion
    #region Auction
    public partial class RootObjectAuction
    {
        public Realm[] Realms { get; set; }

        public Auction[] Auctions { get; set; }
    }

    public partial class Auction
    {
        public long Auc { get; set; }

        public long Item { get; set; }

        public string Owner { get; set; }

        public OwnerRealm OwnerRealm { get; set; }

        public long Bid { get; set; }

        public long Buyout { get; set; }

        public long Quantity { get; set; }

        public string TimeLeft { get; set; }

        public long Rand { get; set; }
        
        public long Seed { get; set; }

        public long Context { get; set; }

        public BonusList[] BonusLists { get; set; }
        
        public Modifier[] Modifiers { get; set; }

        public long? PetSpeciesId { get; set; }

        public long? PetBreedId { get; set; }

        public long? PetLevel { get; set; }

        public long? PetQualityId { get; set; }
    }

    public partial class BonusList
    { 
        public long BonusListId { get; set; }
    }

    public partial class Modifier
    {
        public long Type { get; set; }

        public long Value { get; set; }
    }

    public partial class Realm
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }

    public enum OwnerRealm { Baelgun, Lothar };

    public enum TimeLeft { Long, Medium, Short, VeryLong };
    #endregion
    #region Item
    public class BonusStat
    {
        public int stat { get; set; }
        public int amount { get; set; }
    }

    public class Damage
    {
        public int min { get; set; }
        public int max { get; set; }
        public double exactMin { get; set; }
        public double exactMax { get; set; }
    }

    public class WeaponInfo
    {
        public Damage damage { get; set; }
        public double weaponSpeed { get; set; }
        public double dps { get; set; }
    }

    public class ItemSource
    {
        public int sourceId { get; set; }
        public string sourceType { get; set; }
    }

    public class BonusSummary
    {
        public List<object> defaultBonusLists { get; set; }
        public List<object> chanceBonusLists { get; set; }
        public List<object> bonusChances { get; set; }
    }

    public class RootObjectItem
    {
        public int id { get; set; }
        public int disenchantingSkillRank { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public int stackable { get; set; }
        public int itemBind { get; set; }
        public List<BonusStat> bonusStats { get; set; }
        public List<object> itemSpells { get; set; }
        public int buyPrice { get; set; }
        public int itemClass { get; set; }
        public int itemSubClass { get; set; }
        public int containerSlots { get; set; }
        public WeaponInfo weaponInfo { get; set; }
        public int inventoryType { get; set; }
        public bool equippable { get; set; }
        public int itemLevel { get; set; }
        public int maxCount { get; set; }
        public int maxDurability { get; set; }
        public int minFactionId { get; set; }
        public int minReputation { get; set; }
        public int quality { get; set; }
        public int sellPrice { get; set; }
        public int requiredSkill { get; set; }
        public int requiredLevel { get; set; }
        public int requiredSkillRank { get; set; }
        public ItemSource itemSource { get; set; }
        public int baseArmor { get; set; }
        public bool hasSockets { get; set; }
        public bool isAuctionable { get; set; }
        public int armor { get; set; }
        public int displayInfoId { get; set; }
        public string nameDescription { get; set; }
        public string nameDescriptionColor { get; set; }
        public bool upgradable { get; set; }
        public bool heroicTooltip { get; set; }
        public string context { get; set; }
        public List<object> bonusLists { get; set; }
        public List<string> availableContexts { get; set; }
        public BonusSummary bonusSummary { get; set; }
        public int artifactId { get; set; }
    }
    public class RootObjectFailed
    {
        public string status { get; set; }
        public string reason { get; set; }
    }
    #endregion
}
