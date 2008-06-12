﻿using System;
using System.Collections.Generic;

namespace Rawr.HolyPriest
{
	[System.ComponentModel.DisplayName("HolyPriest|Spell_Holy_HolyBolt")]
    public class CalculationsHolyPriest : CalculationsBase
    {
        
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHolyPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Healing",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",
					"Cycle Stats:Total Healed",
					"Cycle Stats:Average Hps",
					"Cycle Stats:Average Hpm",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
					"Healing per second",
					"Healing per mana",
					"Mana per second",
                    "Average heal"
					};
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Fight HPS", System.Drawing.Color.Red);
                }
                return _subPointNameColors;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
                        Item.ItemType.Cloth,
                        Item.ItemType.None,
						Item.ItemType.Wand,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandMace,
						Item.ItemType.Staff,
					});
                }
                return _relevantItemTypes;
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHolyPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHolyPriest(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHolyPriest));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsHolyPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsHolyPriest;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            //_cachedCharacter = character;
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsHolyPriest calculatedStats = new CharacterCalculationsHolyPriest();
            //CharacterCalculations oldStats = _cachedCharacterStatsWithSlotEmpty as CharacterCalculations;
            calculatedStats.BasicStats = stats;

			CalculationOptionsHolyPriest calcOpts = character.CalculationOptions as CalculationOptionsHolyPriest;
            if (calcOpts == null) calcOpts = new CalculationOptionsHolyPriest();
			float activity = calcOpts.Activity / 100f;
            float time = calcOpts.Length * 60;
			float length = time * activity;
            float totalMana = stats.Mana + (time * stats.Mp5 / 5) + (calcOpts.Spriest * time / 5) +
                ((1 + stats.BonusManaPotion) * calcOpts.ManaAmt * (float)Math.Ceiling((time / 60 - 1) / calcOpts.ManaTime))
                + calcOpts.Spiritual;
            if (stats.MementoProc > 0)
            {
                totalMana += (float)Math.Ceiling(time / 60 - .25) * stats.MementoProc * 3;
            }

            calculatedStats[0] = new Spell("Flash Heal", 7);
            calculatedStats[1] = new Spell("Renew", 12);


            Spell FoL = calculatedStats[0];
            int rank1 = 12 - calcOpts.Rank1;
            int rank2 = 12 - calcOpts.Rank2;

            float HL_Mps = calculatedStats[rank1].Mps * (1f - calcOpts.Ratio) + calculatedStats[rank2].Mps * calcOpts.Ratio;
            float HL_Hps = calculatedStats[rank1].Hps * (1f - calcOpts.Ratio) + calculatedStats[rank2].Hps * calcOpts.Ratio;


            float time_hl = Math.Min(length, Math.Max(0, (totalMana - (length * FoL.Mps)) / (HL_Mps - FoL.Mps)));
            float time_fol = length - time_hl;
            if (time_hl == 0)
            {
                time_fol = Math.Min(length, totalMana / FoL.Mps);
            }
            calculatedStats.TimeHL = time_hl / length;

            float healing_fol = time_fol * FoL.Hps;
            float healing_hl = time_hl * HL_Hps;

            calculatedStats.Healed += healing_fol + healing_hl;
            calculatedStats.HLHPS = HL_Hps;
            calculatedStats.FoLHPS = FoL.Hps;

            calculatedStats.ThroughputPoints = calculatedStats.Healed / time;// FoL.Hps* activity;
            //calculatedStats.LongevityPoints = calculatedStats.Healed / time - FoL.Hps;

            /*if (oldStats == null)
            {
                calculatedStats.ThroughputPoints = FoL.Hps * length;
                calculatedStats.LongevityPoints = calculatedStats.Healed - calculatedStats.ThroughputPoints;
            }
            else
            {
                float otime = Math.Max(oldStats.TimeHL * length, time_hl);
                calculatedStats.LongevityPoints = (length-otime) * oldStats.FoLHPS + otime * oldStats.HLHPS;
                calculatedStats.ThroughputPoints = calculatedStats.Healed - calculatedStats.LongevityPoints;
            }*/

            calculatedStats.OverallPoints = calculatedStats.ThroughputPoints;// +calculatedStats.LongevityPoints;

            calculatedStats.HealHL = healing_hl / calculatedStats.Healed;
            calculatedStats.AvgHPS = calculatedStats.Healed / length * activity;
            calculatedStats.AvgHPM = calculatedStats.Healed / totalMana;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace;
            if (character.Race == Character.CharacterRace.Draenei)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 123, Intellect = 84, Spirit = 91 };
            }
            else if (character.Race == Character.CharacterRace.Dwarf)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 129, Intellect = 82, Spirit = 88 };
            }
            else if (character.Race == Character.CharacterRace.Human)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 126, Intellect = 83, Spirit = 90, BonusSpiritMultiplier = 1.1f };
            }
            else
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 118, Intellect = 86, Spirit = 88 };
            }
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round((1.1f * (statsTotal.Intellect)) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (0.35f * statsTotal.Intellect) + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + (statsTotal.Intellect * 15);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsHolyPriest calc = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
            if (calc == null) calc = new CharacterCalculationsHolyPriest();
            ComparisonCalculationHolyPriest FoL = new ComparisonCalculationHolyPriest("Flash of Light");
            ComparisonCalculationHolyPriest HL11 = new ComparisonCalculationHolyPriest("Holy Light 11");
            ComparisonCalculationHolyPriest HL10 = new ComparisonCalculationHolyPriest("Holy Light 10");
            ComparisonCalculationHolyPriest HL9 = new ComparisonCalculationHolyPriest("Holy Light 9");
            ComparisonCalculationHolyPriest HL8 = new ComparisonCalculationHolyPriest("Holy Light 8");
            ComparisonCalculationHolyPriest HL7 = new ComparisonCalculationHolyPriest("Holy Light 7");
            ComparisonCalculationHolyPriest HL6 = new ComparisonCalculationHolyPriest("Holy Light 6");
            ComparisonCalculationHolyPriest HL5 = new ComparisonCalculationHolyPriest("Holy Light 5");
            ComparisonCalculationHolyPriest HL4 = new ComparisonCalculationHolyPriest("Holy Light 4");

            CalculationOptionsHolyPriest calcOpts = character.CalculationOptions as CalculationOptionsHolyPriest;
            if (calcOpts == null) calcOpts = new CalculationOptionsHolyPriest();

            calc[0] = new Spell("Flash of Light", 7, calcOpts.BoL);
            calc[1] = new Spell("Holy Light", 11, calcOpts.BoL);
            calc[2] = new Spell("Holy Light", 10, calcOpts.BoL);
            calc[3] = new Spell("Holy Light", 9, calcOpts.BoL);
            calc[4] = new Spell("Holy Light", 8, calcOpts.BoL);
            calc[5] = new Spell("Holy Light", 7, calcOpts.BoL);
            calc[6] = new Spell("Holy Light", 6, calcOpts.BoL);
            calc[7] = new Spell("Holy Light", 5, calcOpts.BoL);
            calc[8] = new Spell("Holy Light", 4, calcOpts.BoL);

            switch (chartName)
            {
                case "Healing per second":
                    FoL.OverallPoints = FoL.ThroughputPoints = calc[0].Hps;
                    HL11.OverallPoints = HL11.ThroughputPoints = calc[1].Hps;
                    HL10.OverallPoints = HL10.ThroughputPoints = calc[2].Hps;
                    HL9.OverallPoints = HL9.ThroughputPoints = calc[3].Hps;
                    HL8.OverallPoints = HL8.ThroughputPoints = calc[4].Hps;
                    HL7.OverallPoints = HL7.ThroughputPoints = calc[5].Hps;
                    HL6.OverallPoints = HL6.ThroughputPoints = calc[6].Hps;
                    HL5.OverallPoints = HL5.ThroughputPoints = calc[7].Hps;
                    HL4.OverallPoints = HL4.ThroughputPoints = calc[8].Hps;
                    break;
                case "Average heal":
                    FoL.OverallPoints = FoL.ThroughputPoints = calc[0].AverageHeal;
                    HL11.OverallPoints = HL11.ThroughputPoints = calc[1].AverageHeal;
                    HL10.OverallPoints = HL10.ThroughputPoints = calc[2].AverageHeal;
                    HL9.OverallPoints = HL9.ThroughputPoints = calc[3].AverageHeal;
                    HL8.OverallPoints = HL8.ThroughputPoints = calc[4].AverageHeal;
                    HL7.OverallPoints = HL7.ThroughputPoints = calc[5].AverageHeal;
                    HL6.OverallPoints = HL6.ThroughputPoints = calc[6].AverageHeal;
                    HL5.OverallPoints = HL5.ThroughputPoints = calc[7].AverageHeal;
                    HL4.OverallPoints = HL4.ThroughputPoints = calc[8].AverageHeal;
                    break;
                case "Healing per mana":
                    FoL.OverallPoints = FoL.LongevityPoints = calc[0].Hpm;
                    HL11.OverallPoints = HL11.LongevityPoints = calc[1].Hpm;
                    HL10.OverallPoints = HL10.LongevityPoints = calc[2].Hpm;
                    HL9.OverallPoints = HL9.LongevityPoints = calc[3].Hpm;
                    HL8.OverallPoints = HL8.LongevityPoints = calc[4].Hpm;
                    HL7.OverallPoints = HL7.LongevityPoints = calc[5].Hpm;
                    HL6.OverallPoints = HL6.LongevityPoints = calc[6].Hpm;
                    HL5.OverallPoints = HL5.LongevityPoints = calc[7].Hpm;
                    HL4.OverallPoints = HL4.LongevityPoints = calc[8].Hpm;
                    break;
                case "Mana per second":
                    FoL.OverallPoints = FoL.LongevityPoints = calc[0].Mps;
                    HL11.OverallPoints = HL11.LongevityPoints = calc[1].Mps;
                    HL10.OverallPoints = HL10.LongevityPoints = calc[2].Mps;
                    HL9.OverallPoints = HL9.LongevityPoints = calc[3].Mps;
                    HL8.OverallPoints = HL8.LongevityPoints = calc[4].Mps;
                    HL7.OverallPoints = HL7.LongevityPoints = calc[5].Mps;
                    HL6.OverallPoints = HL6.LongevityPoints = calc[6].Mps;
                    HL5.OverallPoints = HL5.LongevityPoints = calc[7].Mps;
                    HL4.OverallPoints = HL4.LongevityPoints = calc[8].Mps;
                    break;
            }

            return new ComparisonCalculationBase[] { FoL, HL11, HL10, HL9, HL8, HL7, HL6, HL5, HL4 };
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                Healing = stats.Healing,
                SpellCritRating = stats.SpellCritRating,
                SpellHasteRating = stats.SpellHasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                FoLBoL = stats.FoLBoL,
                FoLCrit = stats.FoLCrit,
                FoLHeal = stats.FoLHeal,
                FoLMultiplier = stats.FoLMultiplier,
                HLHeal = stats.HLHeal,
                HLCrit = stats.HLCrit,
                HLCost = stats.HLCost,
                HLBoL = stats.HLBoL,
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Intellect + stats.Spirit + stats.Mp5 + stats.Healing + stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.FoLMultiplier + stats.FoLHeal + stats.FoLCrit + stats.FoLBoL + stats.HLBoL + stats.HLCost
                + stats.HLCrit + stats.HLHeal + stats.MementoProc + stats.AverageHeal) > 0;
        }
    }
}
