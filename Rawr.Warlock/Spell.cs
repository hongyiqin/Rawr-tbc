using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    enum MagicSchool
    {
        Arcane,
        Fire,
        Frost,
        Shadow,
        Nature
    }

    abstract class Spell
    {
        public string Name;
        public int Rank;
        public float BaseCastTime;
        public float BasePeriodicDamage;
        public float BasePeriodicDuration;
        public float BasePeriodTickInterval;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BaseCritPercent;
        public bool Channeled;
        public bool Instant;
        public bool AreaEffect;
        public int BaseCost;
        public int BaseRange;
        public MagicSchool MagicSchool;
        public float BaseSpellDamageCoefficient;
        public float BaseCritModifier;

        public float MinDamage;
        public float MaxDamage;
        public float AverageDamage;
        public float PeriodicDamage;
        public float PeriodicDuration;
        public float PeriodicTickInterval;
        public int Cost;
        public int Range;
        public float SpellDamageCoefficient;
        public float CastTime;
        public float CritPercent;
        public float CritModifier = 1.5f;
        public float ThreatModifier;
        public float HealthReturnFactor = 0f;
        public float DamageModifier = 1f;
        public float SpellDamage;
        public float ShadowDamage;
        public float FireDamage;

    }


    
    internal class ShadowBolt : Spell
    {
        public ShadowBolt(Character character, Stats stats)
        {
            Name = "Shadowbolt";
            BaseCost = 420;
            BaseCastTime = 3.0f;
            BaseMinDamage = 541;
            BaseMaxDamage = 603;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 0.8571f;
            BaseCritPercent = stats.SpellCritRating / 22.08f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            
            
            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;
            
            //destruction talents
            Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
            CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);
            CritPercent +=  tal.GetTalent("Devastation").PointsInvested;
            HealthReturnFactor = tal.GetTalent("SoulLeech").PointsInvested / 10f;
            SpellDamageCoefficient += tal.GetTalent("ShadowandFlame").PointsInvested * 0.04f;
            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
            if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
        }

        private void Calculate(Character character, Stats stats)
        {
            CastTime /= (1 + (stats.SpellHasteRating / 1570f));
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            float minCrit = MinDamage * CritModifier;
            float maxCrit = MaxDamage * CritModifier;
            AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
        }

    }

    internal class CurseOfAgony : Spell
    {
        public CurseOfAgony(Character character, Stats stats)
        {
            Name = "CurseOfAgony";
            BaseCost = 265;
            BaseCastTime = 0f;
            BaseMinDamage = 1356;
            BaseMaxDamage = 1356;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 1.2f;
            BaseCritPercent = 0f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            BasePeriodTickInterval = 2f;
            BasePeriodicDamage = 113f;
            BasePeriodicDuration = 24f;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;

            //bost in base damage only
            if (tal.GetTalent("ImprovedCurseOfAgony").PointsInvested > 0)
            {
                MinDamage = MaxDamage = BaseMinDamage * (1f + (tal.GetTalent("ImprovedCurseOfAgony").PointsInvested) * 0.1f);
            }

            //Shadow Mastery
            MinDamage = MaxDamage *= (1f + tal.GetTalent("ShadowMastery").PointsInvested * 0.2f);

            //Contagion
            MinDamage = MaxDamage *= (1f + tal.GetTalent("Contagion").PointsInvested * 0.1f);


            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
        }

        private void Calculate(Character character, Stats stats)
        {
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            AverageDamage = MinDamage;
        }

    }

    internal class CurseOfDoom : Spell
    {
        public CurseOfDoom(Character character, Stats stats)
        {
            Name = "CurseOfDoom";
            BaseCost = 380;
            BaseCastTime = 0f;
            BaseMinDamage = 4200;
            BaseMaxDamage = 4200;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 2f;
            BaseCritPercent = 0f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            BasePeriodTickInterval = 60f;
            BasePeriodicDamage = 4200f;
            BasePeriodicDuration = 60f;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;

            //Shadow Mastery - should apply but apparently doesn't
           // MinDamage = MaxDamage *= (1f + tal.GetTalent("Contagion").PointsInvested * 0.2f);

            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
        }

        private void Calculate(Character character, Stats stats)
        {
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            AverageDamage = MinDamage;
        }

    }


    //assumes immolate is up
    internal class Incinerate : Spell
    {
        public Incinerate(Character character, Stats stats)
        {
            Name = "Incinerate";
            BaseCost = 355;
            BaseCastTime = 2.5f;
            BaseMinDamage = 444f + 111f;
            BaseMaxDamage = 514f + 129f;
            MagicSchool = MagicSchool.Fire;
            BaseSpellDamageCoefficient = 0.7143f;
            BaseCritPercent = stats.SpellCritRating / 22.08f;
            SpellDamage = stats.SpellDamageRating;
            FireDamage = stats.SpellFireDamageRating;
            BaseRange = 30;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;

            //destruction talents
            Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
            
            //CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);
            
            CritPercent += tal.GetTalent("Devastation").PointsInvested;
            
            HealthReturnFactor = tal.GetTalent("SoulLeech").PointsInvested / 10f;
            SpellDamageCoefficient += tal.GetTalent("ShadowandFlame").PointsInvested * 0.04f;
            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Imp")
                DamageModifier += 0.15f;
            if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
            DamageModifier += (tal.GetTalent("Emberstorm").PointsInvested * 0.02f);
        }

        private void Calculate(Character character, Stats stats)
        {
            CastTime /= (1 + (stats.SpellHasteRating / 1570f));
            DamageModifier *= (stats.BonusFireSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
            MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
            float minCrit = MinDamage * CritModifier;
            float maxCrit = MaxDamage * CritModifier;
            AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
        }

    }

    internal class Immolate : Spell
    {
        float PeriodicSpellDamageCoefficient = 0.38f;

        public Immolate(Character character, Stats stats)
        {
            Name = "Immolate";
            BaseCost = 445;
            BaseCastTime = 2.0f;
            BaseMinDamage = 327f;
            BaseMaxDamage = 327f;
            MagicSchool = MagicSchool.Fire;
            BaseSpellDamageCoefficient = 0.19f;
            BaseCritPercent = stats.SpellCritRating / 22.08f;
            SpellDamage = stats.SpellDamageRating;
            FireDamage = stats.SpellFireDamageRating;
            BaseRange = 30;
            BasePeriodicDamage = 615f / 5f;
            BasePeriodicDuration = 15f;
            BasePeriodTickInterval = 3f;
            


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;

            //destruction talents
            Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);

            CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);

            CritPercent += tal.GetTalent("Devastation").PointsInvested;

            MinDamage = MaxDamage *= tal.GetTalent("ImprovedImmolate").PointsInvested * 0.05f + 1f;

            HealthReturnFactor = tal.GetTalent("SoulLeech").PointsInvested / 10f;
            //SpellDamageCoefficient += tal.GetTalent("ShadowandFlame").PointsInvested * 0.04f;
            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Imp")
                DamageModifier += 0.15f;
            if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
            DamageModifier += (tal.GetTalent("Emberstorm").PointsInvested * 0.02f);
        }

        private void Calculate(Character character, Stats stats)
        {
            CastTime /= (1 + (stats.SpellHasteRating / 1570f));
            DamageModifier *= (stats.BonusFireSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
            MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
            float minCrit = MinDamage * CritModifier;
            float maxCrit = MaxDamage * CritModifier;
            AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
            //dot portion
            AverageDamage += PeriodicDamage * (PeriodicDuration / PeriodicTickInterval) + (SpellDamage + FireDamage) * PeriodicSpellDamageCoefficient;
        }

    }

    internal class Corruption : Spell
    {
        public Corruption(Character character, Stats stats)
        {
            Name = "Corruption";
            BaseCost = 370;
            BaseCastTime = 2f;
            BaseMinDamage = 900;
            BaseMaxDamage = 900;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 0.936f;
            BaseCritPercent = 0f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            BasePeriodTickInterval = 3f;
            BasePeriodicDamage = 900f / 6f;
            BasePeriodicDuration = 18f;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;

            //Empowered corruption
            SpellDamageCoefficient += tal.GetTalent("EmpoweredCorruption").PointsInvested * 0.12f;

            //Shadow Mastery
            MinDamage = MaxDamage *= (1f + tal.GetTalent("ShadowMastery").PointsInvested * 0.2f);

            //Contagion
            MinDamage = MaxDamage *= (1f + tal.GetTalent("Contagion").PointsInvested * 0.1f);

            //Improved Corruption
            CastTime -= tal.GetTalent("ImprovedCorruption").PointsInvested * 0.4f;

            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
        }

        private void Calculate(Character character, Stats stats)
        {
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            AverageDamage = MinDamage;
        }

    }


    internal class UnstableAffliction : Spell
    {
        public UnstableAffliction(Character character, Stats stats)
        {
            Name = "UnstableAffliction";
            BaseCost = 400;
            BaseCastTime = 1.5f;
            BaseMinDamage = 1050;
            BaseMaxDamage = 1050;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 1.80f;
            BaseCritPercent = 0f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            BasePeriodTickInterval = 3f;
            BasePeriodicDamage = 1050f / 6f;
            BasePeriodicDuration = 18f;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;


            //Shadow Mastery
            MinDamage = MaxDamage *= (1f + tal.GetTalent("ShadowMastery").PointsInvested * 0.2f);

       
            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
        }

        private void Calculate(Character character, Stats stats)
        {
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            AverageDamage = MinDamage;
        }

    }

    internal class SiphonLife : Spell
    {
        public SiphonLife(Character character, Stats stats)
        {
            Name = "SiphonLife";
            BaseCost = 410;
            BaseCastTime = 0f;
            BaseMinDamage = 630;
            BaseMaxDamage = 630;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 1f;
            BaseCritPercent = 0f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            BasePeriodTickInterval = 3f;
            BasePeriodicDamage = 630f / 10f;
            BasePeriodicDuration = 30f;


            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;


            //Shadow Mastery
            MinDamage = MaxDamage *= (1f + tal.GetTalent("ShadowMastery").PointsInvested * 0.2f);

       
            if (tal.GetTalent("DemonicSacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
        }

        private void Calculate(Character character, Stats stats)
        {
            DamageModifier *= (stats.BonusShadowSpellPowerMultiplier + 1f);
            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            AverageDamage = MinDamage;
        }

    }


}
