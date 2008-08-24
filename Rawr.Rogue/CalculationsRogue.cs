﻿using System;
using System.Collections.Generic;

namespace Rawr.Rogue {
    [System.ComponentModel.DisplayName("Rogue|Ability_Rogue_SliceDice")]
    public class CalculationsRogue : CalculationsBase {
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel {
            get {
                if (_calculationOptionsPanel == null) {
                    _calculationOptionsPanel = new CalculationOptionsPanelRogue();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
                        "Base Stats:Health",
                        "Base Stats:Strength",
                        "Base Stats:Agility",
                        "Base Stats:Attack Power",
                        "Base Stats:Hit",
                        "Base Stats:Expertise",
                        "Base Stats:Haste",
                        "Base Stats:Armor Penetration",
                        "Base Stats:Crit",
                        "Base Stats:Weapon Damage",

                        "DPS Breakdown:White",
                        "DPS Breakdown:Overall DPS"
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
                }
                return _subPointNameColors;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Combat Table"
                    };
                }
                return _customChartNames;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[] {
                        Item.ItemType.None,
                        Item.ItemType.Leather,

                        Item.ItemType.Bow,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,

                        Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Rogue; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsRogue(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsRogue(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            CalculationOptionsRogue calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem) {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            int targetLevel = calcOpts.TargetLevel;

            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRogue calculatedStats = new CharacterCalculationsRogue();
            calculatedStats.BasicStats = stats;
            return calculatedStats;
        }

        #region Rogue Racial Stats
        private static float[,] BaseRogueRaceStats = new float[,] 
		{
							//	Agility,	Strength,	Stamina
            /*Empty*/       {   0f,         0f,         0f,     },
			/*Human*/		{	158f,		95f,		89f,	},	
			/*Orc*/			{	155f,		98f,		91f,	},		
			/*Dwarf*/		{	154f,		97f,		92f,	},
			/*Night Elf*/	{	163f,		92f,		88f,	},		
			/*Undead*/		{	156f,		94f,		90f,	},	
			/*Tauren*/		{	0f,			0f,			0f,		},		
			/*Gnome*/		{	161f,		90f,		88f,	},		
			/*Troll*/		{	160f,		96f,		90f,	},	
			/*BloodElf*/	{	160f,		92f,		87f,	},
			/*Draenei*/		{	0f,			0f,			0f,		}
		};

        private Stats GetRaceStats(Character.CharacterRace race) {
            if (race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei)
                return new Stats();

            Stats statsRace = new Stats() {
                Health = 3524f,
                Agility = (float)BaseRogueRaceStats[(int)race, 0],
                Strength = (float)BaseRogueRaceStats[(int)race, 1],
                Stamina = (float)BaseRogueRaceStats[(int)race, 2],

                AttackPower = 120f,

                DodgeRating = (float)(-0.59 * 18.92f),
            };

            if (race == Character.CharacterRace.NightElf)
                statsRace.DodgeRating += 18.92f;

            return statsRace;
        }
        #endregion

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;

            Stats statsRace;

            if (character.Race == Character.CharacterRace.Human) {
                statsRace = GetRaceStats(character.Race);
            }
            else {
                statsRace = GetRaceStats(character.Race);
            }
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            TalentTree tree = character.Talents;

            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));

            Stats statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;

            statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
            statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));
            statsTotal.Stamina = (staBase + (float)Math.Floor((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase) * 10f))));
            
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + (statsTotal.Strength * 1) + (statsTotal.Agility * 1)) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;
            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.Crit = ((statsTotal.Agility / 40f) * 22.08f) + statsTotal.CritRating / 22.08f;
            //statsTotal.CritRating += ((statsTotal.Agility / 40f) * 22.08f);
            //statsTotal.CritRating += statsBuffs.LotPCritRating;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            /* talents later */

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            switch (chartName) {
                case "Combat Table":
                    CharacterCalculationsRogue currentCalculationsRogue = GetCharacterCalculations(character) as CharacterCalculationsRogue;
                    ComparisonCalculationsRogue calcMiss = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcDodge = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcParry = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcBlock = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcGlance = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcCrit = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcHit = new ComparisonCalculationsRogue();

                    if (currentCalculationsRogue != null) {
                        calcMiss.Name = "    Miss    ";
                        calcDodge.Name = "   Dodge   ";
                        calcParry.Name = "   Parry   ";
                        calcBlock.Name = "   Block   ";
                        calcGlance.Name = " Glance ";
                        calcCrit.Name = "  Crit  ";
                        calcHit.Name = "Hit";

                        float crits = 5f;
                        float glancing = 25f;
                        float hits = 100f - (crits + glancing);

                        calcMiss.OverallPoints = 0f;
                        calcDodge.OverallPoints = 0f;
                        calcParry.OverallPoints = 0f;
                        calcBlock.OverallPoints = 0f;
                        calcGlance.OverallPoints = 0f;
                        calcCrit.OverallPoints = 0f;
                        calcHit.OverallPoints = 0f;
                    }
                    return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit };

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats) {
            return new Stats() {
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                Health = stats.Health,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,
                ExecutionerProc = stats.ExecutionerProc,
                BonusCommandingShoutHP = stats.BonusCommandingShoutHP
            };
        }

        public override bool HasRelevantStats(Stats stats) {
            return (stats.Agility + stats.Strength + stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.AttackPower + stats.BonusAttackPowerMultiplier + stats.Stamina + stats.BonusStaminaMultiplier + stats.Health + stats.CritRating + stats.HitRating + stats.HasteRating + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier + stats.WindfuryAPBonus + stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant + stats.ExecutionerProc + stats.BonusCommandingShoutHP) != 0;
        }
    }

    public class RogueConversions {
        public static readonly float StrengthToAP = 1.0f;
        public static readonly float AgilityToAP = 1.0f;
        public static readonly float AgilityToCrit = 1.0f / 40.0f;
        public static readonly float AgilityToDodge = 1.0f / 20.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float HitRatingToHit = 1.0f / 15.7692f;
        public static readonly float CritRatingToCrit = 1.0f / 22.0769f; //14*82/52
        public static readonly float HasteRatingToHaste = 1.0f / 15.77f;
        public static readonly float ExpertiseRatingToExpertise = 1.0f / 3.9423f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float ParryRatingToParry = 1.0f / 23.6538461538462f;
    }
}