using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	[System.ComponentModel.DisplayName("Cat|Ability_Druid_CatForm")]
	public class CalculationsCat : CalculationsBase
	{
		//my insides all turned to ash / so slow
		//and blew away as i collapsed / so cold

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelCat();
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
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Strength",
					"Basic Stats:Crit Rating",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration",
					"Basic Stats:Weapon Damage",
					"Complex Stats:Avoided Attacks",
					"Complex Stats:White Crit",
					"Complex Stats:Yellow Crit",
					"Complex Stats:Attack Speed",
					"Complex Stats:Armor Mitigation",
					"Complex Stats:Shreds Per Cycle",
					"Complex Stats:Cycle Time",
					"Complex Stats:4cp Finishers",
					"Complex Stats:5cp Finishers",
					"Complex Stats:Mangle Damage",
					"Complex Stats:Shred Damage",
					"Complex Stats:Melee Damage",
					"Complex Stats:Rip Damage",
					"Complex Stats:Bite Damage",
					"Complex Stats:DPS Points*DPS Points is your theoretical DPS.",
					"Complex Stats:Overall Points*Rawr is designed to support an Overall point value, comprised of one or more sub point values. Cats only have DPS, so Overall Points will always be identical to DPS Points."
				};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
		public override string[] OptimizableCalculationLabels
		{
			get
			{
				if (_optimizableCalculationLabels == null)
					_optimizableCalculationLabels = new string[] {
					"Health",
					"Avoided Attacks %"
					};
				return _optimizableCalculationLabels;
			}
		}

		private string[] _customChartNames = null;
		public override string[] CustomChartNames
		{
			get
			{
				if (_customChartNames == null)
					_customChartNames = new string[] {
					"Combat Table (White)",
					"Combat Table (Yellow)",
					"Relative Stat Values"
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
					_subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
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
						Item.ItemType.None,
						Item.ItemType.Leather,
						Item.ItemType.Idol,
						Item.ItemType.Staff,
						Item.ItemType.TwoHandMace
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationCat(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsCat(); }

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			_cachedCharacter = character;
			int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
			float targetArmor = int.Parse(character.CalculationOptions["TargetArmor"]);
			float exposeWeaknessAPValue = int.Parse(character.CalculationOptions["ExposeWeaknessAPValue"]);
			float bloodlustUptime = float.Parse(character.CalculationOptions["BloodlustUptime"], System.Globalization.CultureInfo.InvariantCulture) / 100f;
			int powershift = int.Parse(character.CalculationOptions["Powershift"]);
			string primaryAttack = character.CalculationOptions["PrimaryAttack"];
			string finisher = character.CalculationOptions["Finisher"];
			string shattrathFaction = character.CalculationOptions["ShattrathFaction"];
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - 70f) * 0.2f;
			CharacterCalculationsCat calculatedStats = new CharacterCalculationsCat();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;

			if (stats.ShatteredSunMightProc > 0)
			{
				switch (shattrathFaction)
				{
					case "Aldor":
						stats.AttackPower += 39.13f;
						break;

					case "Scryer":
						stats.WeaponDamage += 4.35f;
						break;
				}
			}

			//Begin Toskk's 
			
			#region Calculate Basic Chances, Costs, and Damage

			float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
			float modArmor = (baseArmor / (baseArmor + 10557.5f )) * 100f;

			float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
			float attackPower = stats.AttackPower + (stats.ExposeWeakness * exposeWeaknessAPValue * (1 + stats.BonusAttackPowerMultiplier));

			float hitBonus = stats.HitRating * 52f / 82f / 1000f;
			float expertiseBonus = stats.ExpertiseRating * 52f / 82f / 2.5f * 0.0025f;

			float glancingRate = 0.2335774f; // Glancing rate data from Toskk

			float chanceCrit = Math.Min(0.75f, (stats.CritRating / 22.08f + (stats.Agility / 25f)) / 100f) - 0.042f; // Crit Reduction data from Toskk
			float chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
			float chanceMiss = Math.Max(0f, 0.09f - hitBonus) + chanceDodge;
						
			float meleeDamage = stats.WeaponDamage + (768f + attackPower) / 14f;
			float mangleCost = 40f - stats.MangleCostReduction;
			float totalMangleCost = 1f / (1f - chanceMiss) * (mangleCost * (1f - chanceMiss) + mangleCost / 5f * chanceMiss);
			float mangleDamage = 1.2f * (meleeDamage * 1.6f + 264f + stats.BonusMangleDamage);

			float shredCost = 42f;
			float totalShredCost = chanceMiss * shredCost / 5f + (1f - chanceMiss) * shredCost;
			float shredDamage = meleeDamage * 2.25f + 405f + stats.BonusShredDamage;

			float ripCost = 30f;
			float totalRipCost = 1f / (1f - chanceMiss) * ripCost;

			float chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceMiss);

			float hasteBonus = stats.HasteRating / 15.76f / 100f;
			float attackSpeed = (1f - (stats.Bloodlust * bloodlustUptime)) / (1f + hasteBonus);
			float meleeTicker = attackSpeed;

			#endregion

			float dmgMelee = 0f;
			float cycleTime = 0f;
			float segmentTime = 0f;
			float energyCount = 10f;
			float terrorTicker = 0f;
			float comboPoints = 0f;
			float dmgMangles = 0f;
					
			
			#region Mangle

			if (primaryAttack == "Mangle" || primaryAttack == "Both")
			{
				// Starting immediately after Ripping, with energy to Mangle and GCD running
				dmgMelee = 0f;
				cycleTime = 1.0f;
				segmentTime = 1.0f;
				energyCount = mangleCost + 10f;

				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) +
					critMultiplier * chanceWhiteCrit);
				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				//GCD ends, Mangle hits

				energyCount -= totalMangleCost * (1f - segmentTime / 30f);
				dmgMangles = mangleDamage * ((1f - chanceCrit) + critMultiplier * chanceCrit);
				comboPoints = 1f + chanceCrit;
				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				if (stats.TerrorProc > 0)
				{
					attackPower += 65f * 0.85f * (1f + stats.BonusAgilityMultiplier) * (1f + stats.BonusAttackPowerMultiplier);
					chanceCrit += 65f * 0.85f * stats.BonusAgilityMultiplier / 25 / 100;
					terrorTicker = 10;
				}
			}

			#endregion

			#region Shreds

			float numberAttacks = (4f - 3f * chanceCrit + 2f * (float)Math.Pow(chanceCrit, 2) - (float)Math.Pow(chanceCrit, 3)) / (1f - chanceMiss);
			float numberShreds = (numberAttacks - (1f / (1f - chanceMiss)));

			// Mangle Debuff running, waiting for energy to Shred number_shreds times

			energyCount += (numberShreds - 1f) * stats.BloodlustProc;

			segmentTime = (numberShreds * totalShredCost * (1f - numberShreds * totalShredCost / 10f / numberShreds / 30f)
				- energyCount) / 10f;
			segmentTime -= segmentTime / attackSpeed * stats.BloodlustProc / 10f;
			cycleTime += segmentTime;

			terrorTicker -= segmentTime;

			dmgMelee += segmentTime / attackSpeed * (stats.WeaponDamage + (768f + attackPower) / 14f) *
				(0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier * chanceWhiteCrit);

			float dmgPerShred = ((stats.WeaponDamage + (768f + attackPower) / 14f) * 2.25f + 405f + stats.BonusShredDamage);
			float dmgShreds = 0;
			if (segmentTime <= 12f)
				dmgShreds = numberShreds * dmgPerShred * 1.3f * ((1f - chanceMiss) * (1f - chanceCrit) + critMultiplier *
					(1f - chanceMiss) * chanceCrit);
			else
				dmgShreds = ((12f / segmentTime) * numberShreds * dmgPerShred * 1.3f * ((1f - chanceMiss) * (1f - chanceCrit) + 
					critMultiplier * (1f - chanceMiss) * chanceCrit)) + ((1f - 12f / segmentTime) * numberShreds * 
					dmgPerShred * ((1f - chanceMiss) * (1f - chanceCrit) + critMultiplier * (1f - chanceMiss) * chanceCrit));

			energyCount = 0;
			energyCount += stats.BloodlustProc;
			
			#endregion

			#region Powershifting

			if (powershift > 0)
			{
				energyCount = 17f / powershift;
			}
			//Ignoring the wolf helm since it's pretty craptacular

			// 5 Combo points generated, waiting for energy to Rip + Mangle

			segmentTime = (((primaryAttack == "Mangle" || primaryAttack == "Both") ? mangleCost : 0) + totalRipCost * (1f - totalRipCost / 10f / 30f) - energyCount) / 10f;
			segmentTime -= segmentTime / attackSpeed * stats.BloodlustProc / 10f;

			cycleTime += segmentTime;

			if (terrorTicker > segmentTime) terrorTicker = segmentTime;

			if (stats.TerrorProc > 0)
				dmgMelee += (terrorTicker / segmentTime) * (segmentTime / attackSpeed * (stats.WeaponDamage + (768f +
					attackPower) / 14f) * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier *
					chanceWhiteCrit)) + (1f - terrorTicker / segmentTime) * (segmentTime / attackSpeed * meleeDamage *
					(0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier * chanceWhiteCrit));
			else
				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss -
					chanceWhiteCrit) + critMultiplier * chanceWhiteCrit);
			#endregion

			#region Rip

			float rip5 = (chanceCrit - (float)Math.Pow(chanceCrit, 2) + (float)Math.Pow(chanceCrit, 3) - (float)Math.Pow(chanceCrit, 4));
			float rip4 = 1f - rip5;

			// Checking on the possibility of completing the cycle too quickly

			if (cycleTime < 12f)
			{
				segmentTime = 12f - cycleTime;
				cycleTime += segmentTime;
				energyCount = segmentTime * 10f;

				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) +
					critMultiplier * chanceWhiteCrit);

				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				float extraShred = energyCount / (totalShredCost * (1f - segmentTime / 30f));

				dmgShreds += extraShred * shredDamage * 1.3f * ((1f - chanceMiss) * (1 - chanceCrit) +
					critMultiplier * (1f - chanceMiss) * chanceCrit);

				rip5 += extraShred * rip4;
				rip4 -= extraShred * rip4;

				numberShreds += extraShred;
			}

			float dmgRips = (rip5 * (attackPower * 0.24f + 1553f + (stats.BonusRipDamagePerCPPerTick * 6f * 5f)) + rip4 * 
				(attackPower * 0.24f + 1272f + (stats.BonusRipDamagePerCPPerTick * 6f * 4f))) * 1.3f * (1f + stats.BonusRipDamageMultiplier);

			#endregion

			float dps = 1.1f * (((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) / cycleTime);

			calculatedStats.DPSPoints = dps;// *100f;
			calculatedStats.OverallPoints = dps;// *100f;
			calculatedStats.AvoidedAttacks = chanceMiss * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;
			calculatedStats.WhiteCrit = chanceWhiteCrit * 100f;
			calculatedStats.YellowCrit = (1f - chanceMiss) * chanceCrit * 100f;
			calculatedStats.ShredsPerCycle = numberShreds;
			calculatedStats.AttackSpeed = attackSpeed;
			calculatedStats.ArmorMitigation = modArmor;
			calculatedStats.MangleDamage = dmgMangles * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.MeleeDamage = dmgMelee * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.RipDamage = dmgRips / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.Finishers4cp = rip4 * 100f;
			calculatedStats.Finishers5cp = rip5 * 100f;
			calculatedStats.ShredDamage = dmgShreds * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.CycleTime = cycleTime;

			/*

			*/

			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = character.Race == Character.CharacterRace.NightElf ? 
				new Stats() { 
					Health = 3434f, 
					Strength = 73f, 
					Agility = 75f,
					Stamina = 82f,
					DodgeRating = 59f,
					AttackPower = 225f,
					BonusCritMultiplier = 0.1f,
					CritRating = 264.0768f, 
					BonusAttackPowerMultiplier = 0.1f,
					BonusAgilityMultiplier = 0.03f,
					BonusStrengthMultiplier = 0.03f,
					BonusStaminaMultiplier = 0.03f} : 
				new Stats() { 
					Health = 3434f,
					Strength = 81f,
					Agility = 64.5f,
					Stamina = 85f,
					DodgeRating = 40f,
					AttackPower = 227f,
					BonusCritMultiplier = 0.1f,
					CritRating = 264.0768f, 
					BonusAttackPowerMultiplier = 0.1f,
					BonusAgilityMultiplier = 0.03f,
					BonusStrengthMultiplier = 0.03f,
					BonusStaminaMultiplier = 0.03f}; 
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

			Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

			statsGearEnchantsBuffs.AttackPower += statsGearEnchantsBuffs.DrumsOfWar * (float.Parse(character.CalculationOptions["DrumsOfWarUptime"], System.Globalization.CultureInfo.InvariantCulture) / 100f);
			statsGearEnchantsBuffs.HasteRating += statsGearEnchantsBuffs.DrumsOfBattle * (float.Parse(character.CalculationOptions["DrumsOfBattleUptime"], System.Globalization.CultureInfo.InvariantCulture) / 100f);

			float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
			float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
			float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
			float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
			float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));
						
			Stats statsTotal = new Stats();
			statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
			statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
			statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
			statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));
			statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));
			statsTotal.DefenseRating = statsRace.DefenseRating + statsGearEnchantsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.Armor = (float)Math.Round((statsGearEnchantsBuffs.Armor + statsRace.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));
			statsTotal.Miss = statsBuffs.Miss;
			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
			statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + statsTotal.Agility + (statsTotal.Strength * 2)) *  (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
			statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
			statsTotal.BonusMangleDamage = statsRace.BonusMangleDamage + statsGearEnchantsBuffs.BonusMangleDamage;
			statsTotal.BonusRipDamageMultiplier = ((1 + statsRace.BonusRipDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusRipDamageMultiplier)) - 1;
			statsTotal.BonusShredDamage = statsRace.BonusShredDamage + statsGearEnchantsBuffs.BonusShredDamage;
			statsTotal.BonusRipDamagePerCPPerTick = statsRace.BonusRipDamagePerCPPerTick + statsGearEnchantsBuffs.BonusRipDamagePerCPPerTick;
			statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
			statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
			statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
			statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.MangleCostReduction = statsRace.MangleCostReduction + statsGearEnchantsBuffs.MangleCostReduction;
			statsTotal.TerrorProc = statsRace.TerrorProc + statsGearEnchantsBuffs.TerrorProc;
			statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;

			return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				case "Combat Table (White)":
					CharacterCalculationsCat currentCalculationsCatWhite = GetCharacterCalculations(character) as CharacterCalculationsCat;
					ComparisonCalculationCat calcMissWhite = new ComparisonCalculationCat()		{ Name = "    Miss    " };
					ComparisonCalculationCat calcDodgeWhite = new ComparisonCalculationCat()	{ Name = "   Dodge   " };
					ComparisonCalculationCat calcCritWhite = new ComparisonCalculationCat()		{ Name = "  Crit  " };
					ComparisonCalculationCat calcGlanceWhite = new ComparisonCalculationCat()	{ Name = " Glance " };
					ComparisonCalculationCat calcHitWhite = new ComparisonCalculationCat()		{ Name = "Hit" };
					if (currentCalculationsCatWhite != null)
					{
						calcMissWhite.OverallPoints = calcMissWhite.DPSPoints = currentCalculationsCatWhite.MissedAttacks;
						calcDodgeWhite.OverallPoints = calcDodgeWhite.DPSPoints = currentCalculationsCatWhite.DodgedAttacks;
						calcCritWhite.OverallPoints = calcCritWhite.DPSPoints = currentCalculationsCatWhite.WhiteCrit;
						calcGlanceWhite.OverallPoints = calcGlanceWhite.DPSPoints = 23.35774f;
						calcHitWhite.OverallPoints = calcHitWhite.DPSPoints = (100f - calcMissWhite.OverallPoints - 
							calcDodgeWhite.OverallPoints - calcCritWhite.OverallPoints - calcGlanceWhite.OverallPoints);
					}
					return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

				case "Combat Table (Yellow)":
					CharacterCalculationsCat currentCalculationsCatYellow = GetCharacterCalculations(character) as CharacterCalculationsCat;
					ComparisonCalculationCat calcMissYellow = new ComparisonCalculationCat()	{ Name = "    Miss    " };
					ComparisonCalculationCat calcDodgeYellow = new ComparisonCalculationCat()	{ Name = "   Dodge   " };
					ComparisonCalculationCat calcCritYellow = new ComparisonCalculationCat()	{ Name = "  Crit  " };
					ComparisonCalculationCat calcGlanceYellow = new ComparisonCalculationCat()	{ Name = " Glance " };
					ComparisonCalculationCat calcHitYellow = new ComparisonCalculationCat()		{ Name = "Hit" };
					if (currentCalculationsCatYellow != null)
					{
						calcMissYellow.OverallPoints = calcMissYellow.DPSPoints = currentCalculationsCatYellow.MissedAttacks;
						calcDodgeYellow.OverallPoints = calcDodgeYellow.DPSPoints = currentCalculationsCatYellow.DodgedAttacks;
						calcCritYellow.OverallPoints = calcCritYellow.DPSPoints = currentCalculationsCatYellow.YellowCrit;
						calcGlanceYellow.OverallPoints = calcGlanceYellow.DPSPoints = 0f;
						calcHitYellow.OverallPoints = calcHitYellow.DPSPoints = (100f - calcMissYellow.OverallPoints -
							calcDodgeYellow.OverallPoints - calcCritYellow.OverallPoints - calcGlanceYellow.OverallPoints);
					}
					return new ComparisonCalculationBase[] { calcMissYellow, calcDodgeYellow, calcCritYellow, calcGlanceYellow, calcHitYellow };

				case "Relative Stat Values":
					float dpsBase =		GetCharacterCalculations(character).OverallPoints;
					//float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }).OverallPoints - dpsBase) / 10f;
					float dpsCrit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }).OverallPoints - dpsBase);
					float dpsExp =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }).OverallPoints - dpsBase);
					float dpsHaste =	(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }).OverallPoints - dpsBase);
					float dpsHit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBase);
					float dpsDmg =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetration = 1 } }).OverallPoints - dpsBase);

					//Differential Calculations for Agi
					float dpsAtAdd = dpsBase;
					float agiToAdd = 0f;
					while (dpsBase == dpsAtAdd && agiToAdd < 2)
					{
						agiToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }).OverallPoints;
					}

					float dpsAtSubtract = dpsBase;
					float agiToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && agiToSubtract > -2)
					{
						agiToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }).OverallPoints;
					}
					agiToSubtract += 0.01f;

					ComparisonCalculationCat comparisonAgi = new ComparisonCalculationCat()
					{
						Name = "Agility",
						OverallPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
					};


					//Differential Calculations for Str
					dpsAtAdd = dpsBase;
					float strToAdd = 0f;
					while (dpsBase == dpsAtAdd && strToAdd < 2)
					{
						strToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float strToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && strToSubtract > -2)
					{
						strToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }).OverallPoints;
					}
					strToSubtract += 0.01f;

					ComparisonCalculationCat comparisonStr = new ComparisonCalculationCat()
					{
						Name = "Strength",
						OverallPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
					};


					//Differential Calculations for AP
					dpsAtAdd = dpsBase;
					float apToAdd = 0f;
					while (dpsBase == dpsAtAdd && apToAdd < 2)
					{
						apToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float apToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && apToSubtract > -2)
					{
						apToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToSubtract } }).OverallPoints;
					}
					apToSubtract += 0.01f;

					ComparisonCalculationCat comparisonAP = new ComparisonCalculationCat()
					{
						Name = "Attack Power",
						OverallPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
					};



					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonStr,
						comparisonAP,
						new ComparisonCalculationCat() { Name = "Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
						new ComparisonCalculationCat() { Name = "Expertise Rating", OverallPoints = dpsExp, DPSPoints = dpsExp },
						new ComparisonCalculationCat() { Name = "Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
						new ComparisonCalculationCat() { Name = "Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
						new ComparisonCalculationCat() { Name = "Weapon Damage", OverallPoints = dpsDmg, DPSPoints = dpsDmg },
						new ComparisonCalculationCat() { Name = "Armor Penetration", OverallPoints = dpsPen, DPSPoints = dpsPen }
					};

				default:
					return new ComparisonCalculationBase[0];
			}
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
				{
					Agility = stats.Agility,
					Strength = stats.Strength,
					AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
					ExpertiseRating = stats.ExpertiseRating,
					ArmorPenetration = stats.ArmorPenetration,
					BloodlustProc = stats.BloodlustProc,
					TerrorProc = stats.TerrorProc,
					BonusMangleDamage = stats.BonusMangleDamage,
					BonusShredDamage = stats.BonusShredDamage,
					BonusRipDamagePerCPPerTick = stats.BonusRipDamagePerCPPerTick,
					WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusRipDamageMultiplier = stats.BonusRipDamageMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
					BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
					Health = stats.Health,
					MangleCostReduction = stats.MangleCostReduction,
					ExposeWeakness = stats.ExposeWeakness,
					Bloodlust = stats.Bloodlust,
					DrumsOfBattle = stats.DrumsOfBattle,
					DrumsOfWar = stats.DrumsOfWar,
					ShatteredSunMightProc = stats.ShatteredSunMightProc
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BloodlustProc +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
				stats.BonusMangleDamage + stats.BonusRipDamageMultiplier + stats.BonusShredDamage +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
				stats.HasteRating + /*stats.Health +*/ stats.HitRating + stats.MangleCostReduction + /*stats.Stamina +*/
				stats.Strength + stats.TerrorProc + stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust +
				stats.DrumsOfBattle + stats.DrumsOfWar + stats.BonusRipDamagePerCPPerTick + stats.ShatteredSunMightProc) > 0;
		}
	}

    public class CharacterCalculationsCat : CharacterCalculationsBase
    {
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

		private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
		}

		private float _avoidedAttacks;
		public float AvoidedAttacks
		{
			get { return _avoidedAttacks; }
			set { _avoidedAttacks = value; }
		}

		private float _dodgedAttacks;
		public float DodgedAttacks
		{
			get { return _dodgedAttacks; }
			set { _dodgedAttacks = value; }
		}

		private float _missedAttacks;
		public float MissedAttacks
		{
			get { return _missedAttacks; }
			set { _missedAttacks = value; }
		}

		private float _whiteCrit;
		public float WhiteCrit
		{
			get { return _whiteCrit; }
			set { _whiteCrit = value; }
		}

		private float _yellowCrit;
		public float YellowCrit
		{
			get { return _yellowCrit; }
			set { _yellowCrit = value; }
		}

		private float _attackSpeed;
		public float AttackSpeed
		{
			get { return _attackSpeed; }
			set { _attackSpeed = value; }
		}

		private float _armorMitigation;
		public float ArmorMitigation
		{
			get { return _armorMitigation; }
			set { _armorMitigation = value; }
		}

		private float _shredsPerCycle;
		public float ShredsPerCycle
		{
			get { return _shredsPerCycle; }
			set { _shredsPerCycle = value; }
		}

		private float _cycleTime;
		public float CycleTime
		{
			get { return _cycleTime; }
			set { _cycleTime = value; }
		}

		private float _finishers4cp;
		public float Finishers4cp
		{
			get { return _finishers4cp; }
			set { _finishers4cp = value; }
		}

		private float _finishers5cp;
		public float Finishers5cp
		{
			get { return _finishers5cp; }
			set { _finishers5cp = value; }
		}

		private float _mangleDamage;
		public float MangleDamage
		{
			get { return _mangleDamage; }
			set { _mangleDamage = value; }
		}

		private float _shredDamage;
		public float ShredDamage
		{
			get { return _shredDamage; }
			set { _shredDamage = value; }
		}

		private float _meleeDamage;
		public float MeleeDamage
		{
			get { return _meleeDamage; }
			set { _meleeDamage = value; }
		}

		private float _ripDamage;
		public float RipDamage
		{
			get { return _ripDamage; }
			set { _ripDamage = value; }
		}

		private float _ferociousBiteDamage;
		public float FerociousBiteDamage
		{
			get { return _ferociousBiteDamage; }
			set { _ferociousBiteDamage = value; }
		}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			float critRating = BasicStats.CritRating;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Judgement of the Crusade"))
				critRating -= 66.24f;
			critRating -= 264.0768f; //Base 5% + 6% from talents
			
			float hitRating = BasicStats.HitRating;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Faerie Fire"))
				hitRating -= 47.3077f;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Heroic Presence"))
				hitRating -= 15.769f;

			float armorPenetration = BasicStats.ArmorPenetration;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Faerie Fire"))
				armorPenetration -= 610f;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Sunder Armor (x5)"))
				armorPenetration -= 2600f;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Curse of Recklessness"))
				armorPenetration -= 800f;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Expose Armor (5cp)"))
				armorPenetration -= 2000f;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Expose Armor (5cp)"))
				armorPenetration -= 1000f;

			float attackPower = BasicStats.AttackPower;
			if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Hunter's Mark"))
				attackPower -= 121f;
			
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Attack Power", attackPower.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Strength", BasicStats.Strength.ToString());
			dictValues.Add("Crit Rating", critRating.ToString());
			dictValues.Add("Hit Rating", hitRating.ToString());
			dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString());
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
			dictValues.Add("Armor Penetration", armorPenetration.ToString());
			dictValues.Add("Weapon Damage", "+" + BasicStats.WeaponDamage.ToString());
			dictValues.Add("Avoided Attacks", string.Format("{0}%*{1}% Dodged, {2}% Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
			dictValues.Add("White Crit", WhiteCrit.ToString() + "%");
			dictValues.Add("Yellow Crit", YellowCrit.ToString() + "%");
			dictValues.Add("Attack Speed", AttackSpeed.ToString() + "s");
			dictValues.Add("Armor Mitigation", ArmorMitigation.ToString() + "%");
			dictValues.Add("Shreds Per Cycle", ShredsPerCycle.ToString());
			dictValues.Add("Cycle Time", CycleTime.ToString() + "s");
			dictValues.Add("4cp Finishers", Finishers4cp.ToString() + "%");
			dictValues.Add("5cp Finishers", Finishers5cp.ToString() + "%");
			dictValues.Add("Mangle Damage", MangleDamage.ToString() + "%");
			dictValues.Add("Shred Damage", ShredDamage.ToString() + "%");
			dictValues.Add("Melee Damage", MeleeDamage.ToString() + "%");
			dictValues.Add("Rip Damage", RipDamage.ToString() + "%");
			dictValues.Add("Bite Damage", FerociousBiteDamage.ToString() + "%");
			dictValues.Add("DPS Points", DPSPoints.ToString());
			dictValues.Add("Overall Points", OverallPoints.ToString());
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Avoided Attacks %": return AvoidedAttacks;
			}
			return 0f;
		}
    }

	public class ComparisonCalculationCat : ComparisonCalculationBase
	{
		private string _name = string.Empty;
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		private Item _item = null;
		public override Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
	}
}