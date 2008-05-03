using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    enum MagicSchool
    {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane
    }

    enum SpellId
    {
        None,
        Wand,
        LightningBolt,
        ArcaneMissiles,
        ArcaneMissilesCC,
        ArcaneMissilesNoProc,
        //ArcaneMissilesFTF,
        //ArcaneMissilesFTT,
        Frostbolt,
        FrostboltNoCC,
        Fireball,
        Pyroblast,
        FireBlast,
        Scorch,
        ScorchNoCC,
        ArcaneBlast33,
        ArcaneBlast33NoCC,
        ArcaneBlast00,
        ArcaneBlast00NoCC,
        ArcaneBlast10,
        ArcaneBlast01,
        ArcaneBlast11,
        ArcaneBlast11NoCC,
        ArcaneBlast22,
        ArcaneBlast22NoCC,
        ArcaneBlast12,
        ArcaneBlast23,
        ArcaneBlast30,
        ABAM,
        ABAMP,
        AB3AMSc,
        ABAM3Sc,
        ABAM3Sc2,
        ABAM3FrB,
        ABAM3FrB2,
        AB3FrB,
        ABFrB3FrB,
        ABFrB3FrB2,
        ABFrB3FrBSc,
        ABFB3FBSc,
        AB3Sc,
        FireballScorch,
        FireballFireBlast,
        ABAM3ScCCAM,
        ABAM3Sc2CCAM,
        ABAM3FrBCCAM,
        ABAM3FrBCCAMFail,
        ABAM3FrBScCCAM,
        ABAMCCAM,
        ABAM3CCAM,
        ArcaneExplosion,
        FlamestrikeSpammed,
        FlamestrikeSingle,
        Blizzard,
        BlastWave,
        DragonsBreath,
        ConeOfCold,
    }

    abstract class Spell
    {
        public string Name;
        public float DamagePerSecond;
        public float ThreatPerSecond;
        public float CostPerSecond;
        public float ManaRegenPerSecond;

        public bool AffectedByFlameCap;
        public bool ABCycle;

        public bool AreaEffect;

        protected string sequence = null;
        public virtual string Sequence
        {
            get
            {
                return sequence;
            }
        }

        public bool Channeled;
        public float HitProcs;
        public float CastProcs;
        public float CastTime;
    }

    abstract class BaseSpell : Spell
    {
        public bool Instant;
        public bool Binary;
        public int BaseCost;
        public int BaseRange;
        public float BaseCastTime;
        public float BaseCooldown;
        public MagicSchool MagicSchool;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BasePeriodicDamage;
        public float SpellDamageCoefficient;
        public float DotDamageCoefficient;
        public float DotDuration;
        public bool SpammedDot;
        public float TargetProcs;
        public float AoeDamageCap;
        public float MinHitDamage;
        public float MaxHitDamage;
        public float MinCritDamage;
        public float MaxCritDamage;
        public float DotDamage;

        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, spellDamageCoefficient, 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, hitProcs, castProcs, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs, float spellDamageCoefficient, float dotDamageCoefficient, float dotDuration, bool spammedDot)
        {
            Name = name;
            Channeled = channeled;
            Binary = binary;
            Instant = instant;
            AreaEffect = areaEffect;
            BaseCost = cost;
            BaseRange = range;
            BaseCastTime = castTime;
            BaseCooldown = cooldown;
            MagicSchool = magicSchool;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            SpellDamageCoefficient = spellDamageCoefficient;
            HitProcs = hitProcs;
            CastProcs = castProcs;
            TargetProcs = hitProcs;
            DotDamageCoefficient = dotDamageCoefficient;
            DotDuration = dotDuration;
            SpammedDot = spammedDot;
        }

        public float CostModifier;
        public float SpellModifier;
        public float RealResistance;
        public float CritRate;
        public float ThreatMultiplier;
        public float CritBonus;
        public float HitRate;
        public float CastingSpeed;
        public float GlobalCooldown;
        public float PartialResistFactor;
        public float RawSpellDamage;
        public float SpellDamage;
        public float AverageDamage;
        public bool ManualClearcasting = false;
        public bool ClearcastingAveraged;
        public bool ClearcastingActive;
        public bool ClearcastingProccing;
        public float InterruptProtection;

        public float Cooldown;
        public float Cost;

        public virtual void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            if (AreaEffect) TargetProcs *= calculations.CalculationOptions.AoeTargets;
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01f * calculations.CalculationOptions.Pyromaniac;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01f * calculations.CalculationOptions.ElementalPrecision;
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05f * calculations.CalculationOptions.FrostChanneling;
            if (calculations.ArcanePower) CostModifier += 0.3f;
            if (MagicSchool == MagicSchool.Fire) AffectedByFlameCap = true;
            if (MagicSchool == MagicSchool.Fire) InterruptProtection += 0.35f * calculations.CalculationOptions.BurningSoul;

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    SpellModifier = calculations.ArcaneSpellModifier;
                    CritRate = calculations.ArcaneCritRate;
                    CritBonus = calculations.ArcaneCritBonus;
                    RawSpellDamage = calculations.ArcaneDamage;
                    HitRate = calculations.ArcaneHitRate;
                    RealResistance = calculations.CalculationOptions.ArcaneResist;
                    ThreatMultiplier = calculations.ArcaneThreatMultiplier;
                    break;
                case MagicSchool.Fire:
                    SpellModifier = calculations.FireSpellModifier;
                    CritRate = calculations.FireCritRate;
                    CritBonus = calculations.FireCritBonus;
                    RawSpellDamage = calculations.FireDamage;
                    HitRate = calculations.FireHitRate;
                    RealResistance = calculations.CalculationOptions.FireResist;
                    ThreatMultiplier = calculations.FireThreatMultiplier;
                    break;
                case MagicSchool.Frost:
                    SpellModifier = calculations.FrostSpellModifier;
                    CritRate = calculations.FrostCritRate;
                    CritBonus = calculations.FrostCritBonus;
                    RawSpellDamage = calculations.FrostDamage;
                    HitRate = calculations.FrostHitRate;
                    RealResistance = calculations.CalculationOptions.FrostResist;
                    ThreatMultiplier = calculations.FrostThreatMultiplier;
                    break;
                case MagicSchool.Nature:
                    SpellModifier = calculations.NatureSpellModifier;
                    CritRate = calculations.NatureCritRate;
                    CritBonus = calculations.NatureCritBonus;
                    RawSpellDamage = calculations.NatureDamage;
                    HitRate = calculations.NatureHitRate;
                    RealResistance = calculations.CalculationOptions.NatureResist;
                    ThreatMultiplier = calculations.NatureThreatMultiplier;
                    break;
                case MagicSchool.Shadow:
                    SpellModifier = calculations.ShadowSpellModifier;
                    CritRate = calculations.ShadowCritRate;
                    CritBonus = calculations.ShadowCritBonus;
                    RawSpellDamage = calculations.ShadowDamage;
                    HitRate = calculations.ShadowHitRate;
                    RealResistance = calculations.CalculationOptions.ShadowResist;
                    ThreatMultiplier = calculations.ShadowThreatMultiplier;
                    break;
            }

            int targetLevel = AreaEffect ? calculations.CalculationOptions.AoeTargetLevel : calculations.CalculationOptions.TargetLevel;

            // do not count debuffs for aoe effects, can't assume it will be up on all
            // do not include molten fury (molten fury relates to boss), instead amplify all by average
            if (AreaEffect)
            {
                SpellModifier = (1 + 0.01f * calculations.CalculationOptions.ArcaneInstability) * (1 + 0.01f * calculations.CalculationOptions.PlayingWithFire);
                if (calculations.ArcanePower)
                {
                    SpellModifier *= 1.3f;
                }
                if (calculations.CalculationOptions.MoltenFury > 0)
                {
                    SpellModifier *= (1 + 0.1f * calculations.CalculationOptions.MoltenFury * calculations.CalculationOptions.MoltenFuryPercentage);
                }
                if (MagicSchool == MagicSchool.Fire) SpellModifier *= (1 + 0.02f * calculations.CalculationOptions.FirePower);
                if (MagicSchool == MagicSchool.Frost) SpellModifier *= (1 + 0.02f * calculations.CalculationOptions.PiercingIce);

                if (MagicSchool == MagicSchool.Arcane) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.02f * calculations.CalculationOptions.ArcaneFocus);
                if (MagicSchool == MagicSchool.Fire) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.01f * calculations.CalculationOptions.ElementalPrecision);
                if (MagicSchool == MagicSchool.Frost) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.01f * calculations.CalculationOptions.ElementalPrecision);
                if (MagicSchool == MagicSchool.Nature) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit);
                if (MagicSchool == MagicSchool.Shadow) HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit);
            }

            if (ManualClearcasting && !ClearcastingAveraged)
            {
                CritRate -= 0.01f * calculations.CalculationOptions.ArcanePotency; // replace averaged arcane potency with actual % chance
                if (ClearcastingActive) CritRate += 0.1f * calculations.CalculationOptions.ArcanePotency;
            }

            PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - calculations.BasicStats.SpellPenetration / 350f * 0.75f) - ((targetLevel > 70 && !Binary) ? ((targetLevel - 70) * 0.02f) : 0f));
        }

        private float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        protected void CalculateDerivedStats(Character character, CharacterCalculationsMage calculations)
        {
            CastingSpeed = calculations.CastingSpeed;

            if (Instant) InterruptProtection = 1;
            if (calculations.IcyVeins) InterruptProtection = 1;
            float InterruptFactor = calculations.CalculationOptions.InterruptFrequency * Math.Max(0, 1 - InterruptProtection);

            float Haste = calculations.BasicStats.SpellHasteRating;
            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);

            // TODO consider converting to discrete model for procs

            GlobalCooldown = Math.Max(calculations.GlobalCooldownLimit, 1.5f / CastingSpeed);
            CastTime = BaseCastTime / CastingSpeed + calculations.Latency;
            CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
            if (CastTime < GlobalCooldown + calculations.Latency) CastTime = GlobalCooldown + calculations.Latency;

            // Quagmirran
            if (calculations.BasicStats.SpellHasteFor6SecOnHit_10_45 > 0 && HitProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + calculations.BasicStats.SpellHasteFor6SecOnHit_10_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(calculations.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + calculations.Latency;
                cast = cast * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (cast < gcd + calculations.Latency) cast = gcd + calculations.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                //discrete model
                float castsAffected = 0;
                for (int c = 0; c < HitProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / HitProcs) / cast) / HitProcs;
                Haste += calculations.BasicStats.SpellHasteFor6SecOnHit_10_45 * castsAffected * cast / (45f + CastTime / HitProcs / 0.1f);
                //continuous model
                //Haste += calculations.BasicStats.SpellHasteFor6SecOnHit_10_45 * 6f / (45f + CastTime / HitProcs / 0.1f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(calculations.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + calculations.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + calculations.Latency) CastTime = GlobalCooldown + calculations.Latency;
            }

            // MSD
            if (calculations.BasicStats.SpellHasteFor6SecOnCast_15_45 > 0 && CastProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + calculations.BasicStats.SpellHasteFor6SecOnCast_15_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(calculations.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + calculations.Latency;
                cast = cast * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (cast < gcd + calculations.Latency) cast = gcd + calculations.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float castsAffected = 0;
                for (int c = 0; c < CastProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / CastProcs) / cast) / CastProcs;
                Haste += calculations.BasicStats.SpellHasteFor6SecOnCast_15_45 * castsAffected * cast / (45f + CastTime / CastProcs / 0.15f);
                //Haste += calculations.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(calculations.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + calculations.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + calculations.Latency) CastTime = GlobalCooldown + calculations.Latency;
            }


            // AToI, first cast after proc is not affected for non-instant
            if (calculations.BasicStats.SpellHasteFor5SecOnCrit_50 > 0)
            {
                float rawHaste = Haste;

                // 1st order approximation
                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                int chancesToProc = (int)Math.Floor(5f / (CastTime / HitProcs));
                if (!Instant) chancesToProc -= 1;
                chancesToProc *= (int)(TargetProcs / HitProcs);
                Haste = rawHaste + calculations.BasicStats.SpellHasteFor5SecOnCrit_50 * (1 - (float)Math.Pow(1 - 0.5f * CritRate, chancesToProc));
                //Haste = rawHaste + calculations.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);
                GlobalCooldown = Math.Max(calculations.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + calculations.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + calculations.Latency) CastTime = GlobalCooldown + calculations.Latency;

                // 2nd order approximation
                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                chancesToProc = (int)Math.Floor(5f / (CastTime / HitProcs));
                if (!Instant) chancesToProc -= 1;
                chancesToProc *= (int)(TargetProcs / HitProcs);
                Haste = rawHaste + calculations.BasicStats.SpellHasteFor5SecOnCrit_50 * (1 - (float)Math.Pow(1 - 0.5f * CritRate, chancesToProc));
                //Haste = rawHaste + calculations.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);
                GlobalCooldown = Math.Max(calculations.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + calculations.Latency;
                CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
                if (CastTime < GlobalCooldown + calculations.Latency) CastTime = GlobalCooldown + calculations.Latency;
            }

            Cost = BaseCost * CostModifier;

            CritRate = Math.Min(1, CritRate);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) Cost *= (1f - CritRate * 0.1f * calculations.CalculationOptions.MasterOfElements);

            CostPerSecond = Cost / CastTime;

            if (!ManualClearcasting || ClearcastingAveraged)
            {
                CostPerSecond *= (1 - 0.02f * calculations.CalculationOptions.ArcaneConcentration);
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            // hit & crit ranges, do it before passive spell damage effects for cleaner comparison in game
            MinHitDamage = (BaseMinDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier;
            MaxHitDamage = (BaseMaxDamage + RawSpellDamage * SpellDamageCoefficient) * SpellModifier;
            MinCritDamage = MinHitDamage * CritBonus;
            MaxCritDamage = MaxHitDamage * CritBonus;
            DotDamage = (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier;

            if (calculations.BasicStats.SpellDamageFor10SecOnHit_5 > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnHit_5 * ProcBuffUp(1 - (float)Math.Pow(0.95, TargetProcs), 10, CastTime);
            if (calculations.BasicStats.SpellDamageFor6SecOnCrit > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor6SecOnCrit * ProcBuffUp(1 - (float)Math.Pow(1 - CritRate, HitProcs), 6, CastTime);
            if (calculations.BasicStats.SpellDamageFor10SecOnHit_10_45 > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
            if (calculations.BasicStats.SpellDamageFor10SecOnResist > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnResist * ProcBuffUp(1 - (float)Math.Pow(HitRate, HitProcs), 10, CastTime);
            if (calculations.BasicStats.SpellDamageFor15SecOnCrit_20_45 > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor15SecOnCrit_20_45 * 15f / (45f + CastTime / HitProcs / 0.2f / CritRate);
            if (calculations.BasicStats.SpellDamageFor10SecOnCrit_20_45 > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnCrit_20_45 * 10f / (45f + CastTime / HitProcs / 0.2f / CritRate);

            SpellDamage = RawSpellDamage * SpellDamageCoefficient;
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - calculations.ResilienceCritRateReduction);
            float resistMultiplier = HitRate * PartialResistFactor;
            int targets = 1;
            if (AreaEffect) targets = calculations.CalculationOptions.AoeTargets;
            AverageDamage = baseAverage * SpellModifier * targets * HitRate;
            if (AreaEffect && AverageDamage > AoeDamageCap) AverageDamage = AoeDamageCap;
            AverageDamage = AverageDamage * critMultiplier * PartialResistFactor;
            if (SpammedDot)
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier * CastTime / DotDuration;
            }
            else
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier;
            }
            DamagePerSecond = AverageDamage / CastTime;
            ThreatPerSecond = DamagePerSecond * ThreatMultiplier;

            if (Name != "Lightning Bolt" && calculations.BasicStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = (BaseSpell)calculations.GetSpell(SpellId.LightningBolt);
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / HitProcs));
                float avgCritsPerHit = CritRate * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = LightningBolt.AverageDamage / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
                DamagePerSecond += boltDps;
                ThreatPerSecond += boltDps * calculations.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }

            /*float casttimeHash = calculations.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance, Name, casttimeHash);
            }*/

            float OO5SR;

            if (Cost > 0)
            {
                OO5SR = FSRCalc.CalculateSimpleOO5SR(calculations.ClearcastingChance, CastTime - calculations.Latency, calculations.Latency, Channeled);
            }
            else
            {
                OO5SR = 1;
            }

            ManaRegenPerSecond = calculations.ManaRegen5SR + OO5SR * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;
            ThreatPerSecond += (calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime) * 0.5f * (1 + calculations.BasicStats.ThreatIncreaseMultiplier) * (1 - calculations.BasicStats.ThreatReductionMultiplier);

            if (calculations.Mp5OnCastFor20Sec > 0 && CastProcs > 0)
            {
                float totalMana = calculations.Mp5OnCastFor20Sec / 5f / CastTime * 0.5f * (20 - CastTime / HitProcs / 2f) * (20 - CastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
                ThreatPerSecond += totalMana / 20f * 0.5f * (1 + calculations.BasicStats.ThreatIncreaseMultiplier) * (1 - calculations.BasicStats.ThreatReductionMultiplier);
            }
        }
    }

    class Wand : BaseSpell
    {
        public Wand(Character character, CharacterCalculationsMage calculations, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0, false)
        {
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            Calculate(character, calculations);
            CastTime = speed;
            CritRate = calculations.SpellCrit;
            CritBonus = (1 + (1.5f * (1 + calculations.BasicStats.BonusSpellCritMultiplier) - 1)) * calculations.ResilienceCritDamageReduction;
            SpellModifier = (1 + 0.01f * calculations.CalculationOptions.ArcaneInstability) * (1 + 0.01f * calculations.CalculationOptions.PlayingWithFire) * (1 + calculations.BasicStats.BonusSpellPowerMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Fire:
                    SpellModifier *= (1 + calculations.BasicStats.BonusFireSpellPowerMultiplier);
                    break;
                case MagicSchool.Frost:
                    SpellModifier *= (1 + calculations.BasicStats.BonusFrostSpellPowerMultiplier);
                    break;
                case MagicSchool.Nature:
                    SpellModifier *= (1 + calculations.BasicStats.BonusNatureSpellPowerMultiplier);
                    break;
                case MagicSchool.Shadow:
                    SpellModifier *= (1 + calculations.BasicStats.BonusShadowSpellPowerMultiplier);
                    break;
            }
            if (calculations.CalculationOptions.WandSpecialization > 0)
            {
                SpellModifier *= 1 + 0.01f + 0.12f * calculations.CalculationOptions.WandSpecialization;
            }
            CalculateDerivedStats(character, calculations);
        }
    }

    class FireBlast : BaseSpell
    {
        public FireBlast(Character character, CharacterCalculationsMage calculations)
            : base("Fire Blast", false, false, true, false, 465, 20, 0, 8, MagicSchool.Fire, 664, 786, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            Cooldown -= 0.5f * calculations.CalculationOptions.ImprovedFireBlast;
            CritRate += 0.02f * calculations.CalculationOptions.Incinerate;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Scorch : BaseSpell
    {

        public Scorch(Character character, CharacterCalculationsMage calculations, bool clearcastingActive)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            Calculate(character, calculations);
        }

        public Scorch(Character character, CharacterCalculationsMage calculations)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * calculations.CalculationOptions.Incinerate;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Flamestrike : BaseSpell
    {
        public Flamestrike(Character character, CharacterCalculationsMage calculations, bool spammedDot)
            : base("Flamestrike", false, false, false, true, 1175, 30, 3, 0, MagicSchool.Fire, 480, 585, 424, 1, 1, 0.2363f, 0.12f, 8f, spammedDot)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 7830;
            CritRate += 0.05f * calculations.CalculationOptions.ImprovedFlamestrike;
            CalculateDerivedStats(character, calculations);
        }
    }

    class FrostNova : BaseSpell
    {
        public FrostNova(Character character, CharacterCalculationsMage calculations)
            : base("Frost Nova", false, true, true, true, 185, 0, 0, 25, MagicSchool.Frost, 100, 113, 0, 1.5f / 3.5f * 0.5f * 0.13f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            Cooldown -= 2 * calculations.CalculationOptions.ImprovedFrostNova;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Frostbolt : BaseSpell
    {
        public Frostbolt(Character character, CharacterCalculationsMage calculations, bool clearcastingActive)
            : base("Frostbolt", false, true, false, false, 330, 30, 3, 0, MagicSchool.Frost, 600, 647, 0, 0.95f * 3f / 3.5f)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            Calculate(character, calculations);
        }

        public Frostbolt(Character character, CharacterCalculationsMage calculations)
            : base("Frostbolt", false, true, false, false, 330, 30, 3, 0, MagicSchool.Frost, 600, 647, 0, 0.95f * 3f / 3.5f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            BaseCastTime -= 0.1f * calculations.CalculationOptions.ImprovedFrostbolt;
            CritRate += 0.01f * calculations.CalculationOptions.EmpoweredFrostbolt;
            SpellDamageCoefficient += 0.02f * calculations.CalculationOptions.EmpoweredFrostbolt;
            int targetLevel = calculations.CalculationOptions.TargetLevel;
            HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.02f * calculations.CalculationOptions.ElementalPrecision); // bugged Elemental Precision
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Fireball : BaseSpell
    {
        public Fireball(Character character, CharacterCalculationsMage calculations)
            : base("Fireball", false, false, false, false, 425, 35, 3.5f, 0, MagicSchool.Fire, 649, 821, 84)
        {
            Calculate(character, calculations);
            SpammedDot = true;
            DotDuration = 8;
            BaseCastTime -= 0.1f * calculations.CalculationOptions.ImprovedFireball;
            SpellDamageCoefficient += 0.03f * calculations.CalculationOptions.EmpoweredFireball;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Pyroblast : BaseSpell
    {
        public Pyroblast(Character character, CharacterCalculationsMage calculations)
            : base("Pyroblast", false, false, false, false, 500, 35, 6f, 0, MagicSchool.Fire, 939, 1191, 356)
        {
            Calculate(character, calculations);
            SpammedDot = false;
            DotDuration = 12;
            SpellDamageCoefficient = 1.15f;
            DotDamageCoefficient = 0.2f;
            CalculateDerivedStats(character, calculations);
        }
    }

    class ConeOfCold : BaseSpell
    {
        public ConeOfCold(Character character, CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, true, 645, 0, 0, 10, MagicSchool.Frost, 418, 457, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 6500;
            int ImprovedConeOfCold = calculations.CalculationOptions.ImprovedConeOfCold;
            SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0));
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneBlast : BaseSpell
    {
        public ArcaneBlast(Character character, CharacterCalculationsMage calculations, int timeDebuff, int costDebuff, bool clearcastingActive)
            : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(character, calculations);
        }

        public ArcaneBlast(Character character, CharacterCalculationsMage calculations, int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(character, calculations);
        }

        private int timeDebuff;
        private int costDebuff;

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            BaseCastTime -= timeDebuff / 3f;
            CostModifier += 0.75f * costDebuff + calculations.BasicStats.ArcaneBlastBonus;
            SpellModifier *= (1 + calculations.BasicStats.ArcaneBlastBonus);
            CritRate += 0.02f * calculations.CalculationOptions.ArcaneImpact;
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissiles : BaseSpell
    {
        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing)
            : base("Arcane Missiles", true, false, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0, 5, 6)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = clearcastingAveraged;
            ClearcastingProccing = clearcastingProccing;
            Calculate(character, calculations);
        }

        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0, 5, 1)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CostModifier += 0.02f * calculations.CalculationOptions.EmpoweredArcaneMissiles;

            // CC double dipping
            if (!ManualClearcasting) CritRate += 0.01f * calculations.CalculationOptions.ArcanePotency;
            else if (ClearcastingProccing) CritRate += 0.1f * calculations.CalculationOptions.ArcanePotency;

            SpellDamageCoefficient += 0.15f * calculations.CalculationOptions.EmpoweredArcaneMissiles;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            InterruptProtection += 0.2f * calculations.CalculationOptions.ImprovedArcaneMissiles;
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissilesCC : Spell
    {
        public ArcaneMissilesCC(Character character, CharacterCalculationsMage calculations)
        {
            Name= "Arcane Missiles CC";

            //AM?1-AM11-AM11-...=0.9*0.1*...
            //...
            //AM?1-AM10=0.9

            //TIME = T * [1 + 1/0.9]
            //DAMAGE = AM?1 + AM10 + 0.1/0.9*AM11

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            Spell AMc1 = new ArcaneMissiles(character, calculations, true, false, true);
            Spell AM10 = new ArcaneMissiles(character, calculations, false, true, false);
            Spell AM11 = new ArcaneMissiles(character, calculations, false, true, true);

            CastProcs = AMc1.CastProcs * (1 + 1 / (1 - CC));
            CastTime = AMc1.CastTime * (1 + 1 / (1 - CC));
            HitProcs = AMc1.HitProcs * (1 + 1 / (1 - CC));
            Channeled = true;
            CostPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            DamagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            ThreatPerSecond = (AMc1.ThreatPerSecond + AM10.ThreatPerSecond + CC / (1 - CC) * AM11.ThreatPerSecond) / (1 + 1 / (1 - CC));
            ManaRegenPerSecond = 0;
        }
    }

    class ArcaneExplosion : BaseSpell
    {
        public ArcaneExplosion(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Explosion", false, false, true, true, 545, 0, 0, 0, MagicSchool.Arcane, 377, 407, 0, 1.5f / 3.5f * 0.5f)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * calculations.CalculationOptions.ArcaneImpact;
            AoeDamageCap = 10180;
            CalculateDerivedStats(character, calculations);
        }
    }

    class BlastWave : BaseSpell
    {
        public BlastWave(Character character, CharacterCalculationsMage calculations)
            : base("Blast Wave", false, false, true, true, 645, 0, 0, 30, MagicSchool.Fire, 616, 724, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 9440;
            CalculateDerivedStats(character, calculations);
        }
    }

    class DragonsBreath : BaseSpell
    {
        public DragonsBreath(Character character, CharacterCalculationsMage calculations)
            : base("Dragon's Breath", false, false, true, true, 700, 0, 0, 20, MagicSchool.Fire, 680, 790, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 10100;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Blizzard : BaseSpell
    {
        public Blizzard(Character character, CharacterCalculationsMage calculations)
            : base("Blizzard", true, false, false, true, 1645, 0, 8, 0, MagicSchool.Frost, 1476, 1476, 0, 1.1429f)
        {
            base.Calculate(character, calculations);
            CritBonus = 1;
            CritRate = 0;
            HitRate = 1;
            AoeDamageCap = 28950;
            CalculateDerivedStats(character, calculations);
        }
    }

    class LightningBolt : BaseSpell
    {
        public LightningBolt(Character character, CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, false, true, false, 0, 30, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0, 0, 0, false)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BasicStats.BonusSpellCritMultiplier) - 1)) * calculations.ResilienceCritDamageReduction;
            CalculateDerivedStats(character, calculations);
        }
    }

    class SpellCycle : Spell
    {
        public override string Sequence
        {
            get
            {
                if (sequence == null) sequence = string.Join("-", spellList.ToArray());
                return sequence;
            }
        }

        public float AverageDamage;
        public float AverageThreat;
        public float Cost;
        public float SpellCount = 0;

        private List<string> spellList = new List<string>();
        private FSRCalc fsr = new FSRCalc();

        public SpellCycle()
        {
            spellList = new List<string>();
            fsr = new FSRCalc();
        }

        public SpellCycle(int capacity)
        {
            spellList = new List<string>(capacity);
            fsr = new FSRCalc(capacity);
        }

        public void AddSpell(Spell spell, CharacterCalculationsMage calculations)
        {
            fsr.AddSpell(spell.CastTime - calculations.Latency, calculations.Latency, spell.Channeled);
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            AverageThreat += spell.ThreatPerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell.Name);
            SpellCount++;
        }

        public void AddPause(float duration)
        {
            fsr.AddPause(duration);
            spellList.Add("Pause");
        }

        public void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            CastTime = fsr.Duration;

            CostPerSecond = Cost / CastTime;
            DamagePerSecond = AverageDamage / CastTime;
            ThreatPerSecond = AverageThreat / CastTime;

            float OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance);

            ManaRegenPerSecond = calculations.ManaRegen5SR + OO5SR * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;

            if (calculations.Mp5OnCastFor20Sec > 0)
            {
                float averageCastTime = fsr.Duration / SpellCount;
                float totalMana = calculations.Mp5OnCastFor20Sec / 5f / averageCastTime * 0.5f * (20 - averageCastTime / HitProcs / 2f) * (20 - averageCastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
            }
        }
    }

    class ABAMP : SpellCycle
    {
        public ABAMP(Character character, CharacterCalculationsMage calculations) : base(3)
        {
            Name = "ABAMP";
            ABCycle = true;

            Spell AB = calculations.GetSpell(SpellId.ArcaneBlast10);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);

            AddSpell(AB, calculations);
            AddSpell(AM, calculations);
            AddPause(8 - AM.CastTime - AB.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM : SpellCycle
    {
        public ABAM(Character character, CharacterCalculationsMage calculations) : base(2)
        {
            Name = "ABAM";
            ABCycle = true;

            Spell AB = calculations.GetSpell(SpellId.ArcaneBlast33);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);

            AddSpell(AB, calculations);
            AddSpell(AM, calculations);

            Calculate(character, calculations);
        }
    }

    class AB3AMSc : SpellCycle
    {
        public AB3AMSc(Character character, CharacterCalculationsMage calculations) : base(12)
        {
            Name = "AB3AMSc";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = calculations.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = calculations.GetSpell(SpellId.ArcaneBlast12);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(AB01, calculations);
            AddSpell(AB12, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3Sc : SpellCycle
    {
        public ABAM3Sc(Character character, CharacterCalculationsMage calculations) : base(14)
        {
            Name = "ABAM3Sc";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3Sc2 : SpellCycle
    {
        public ABAM3Sc2(Character character, CharacterCalculationsMage calculations) : base(14)
        {
            Name = "ABAM3Sc2";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap >= AB30.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3FrB : SpellCycle
    {
        public ABAM3FrB(Character character, CharacterCalculationsMage calculations) : base(14)
        {
            Name = "ABAM3FrB";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3FrB2 : SpellCycle
    {
        public ABAM3FrB2(Character character, CharacterCalculationsMage calculations) : base(14)
        {
            Name = "ABAM3FrB2";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class AB3FrB : SpellCycle
    {
        public AB3FrB(Character character, CharacterCalculationsMage calculations) : base(11)
        {
            Name = "AB3FrB";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = calculations.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = calculations.GetSpell(SpellId.ArcaneBlast12);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, calculations);
            AddSpell(AB01, calculations);
            AddSpell(AB12, calculations);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFrB3FrB : SpellCycle
    {
        public ABFrB3FrB(Character character, CharacterCalculationsMage calculations) : base(13)
        {
            Name = "ABFrB3FrB";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFrB3FrB2 : SpellCycle
    {
        public ABFrB3FrB2(Character character, CharacterCalculationsMage calculations) : base(13)
        {
            Name = "ABFrB3FrB2";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFrB3FrBSc : SpellCycle
    {
        public ABFrB3FrBSc(Character character, CharacterCalculationsMage calculations) : base(13)
        {
            Name = "ABFrB3FrBSc";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFB3FBSc : SpellCycle
    {
        public ABFB3FBSc(Character character, CharacterCalculationsMage calculations) : base(13)
        {
            Name = "ABFB3FBSc";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = calculations.GetSpell(SpellId.ArcaneBlast11);
            Spell AB22 = calculations.GetSpell(SpellId.ArcaneBlast22);
            Spell FB = calculations.GetSpell(SpellId.Fireball);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(FB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap >= FB.CastTime)
            {
                AddSpell(FB, calculations);
                gap -= FB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class AB3Sc : SpellCycle
    {
        public AB3Sc(Character character, CharacterCalculationsMage calculations) : base(11)
        {
            Name = "AB3Sc";
            ABCycle = true;

            Spell AB30 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = calculations.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = calculations.GetSpell(SpellId.ArcaneBlast12);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            AddSpell(AB30, calculations);
            AddSpell(AB01, calculations);
            AddSpell(AB12, calculations);
            float gap = 8;
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class FireballScorch : SpellCycle
    {
        public FireballScorch(Character character, CharacterCalculationsMage calculations) : base(33)
        {
            Name = "FireballScorch";

            Spell FB = calculations.GetSpell(SpellId.Fireball);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            if (calculations.CalculationOptions.ImprovedScorch == 0)
            {
                // in this case just Fireball, scorch debuff won't be applied
                AddSpell(FB, calculations);
                Calculate(character, calculations);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)calculations.CalculationOptions.ImprovedScorch);
                double timeOnScorch = 30;
                int fbCount = 0;

                while (timeOnScorch > FB.CastTime + (averageScorchesNeeded + 1) * Sc.CastTime) // one extra scorch gap to account for possible resist
                {
                    AddSpell(FB, calculations);
                    fbCount++;
                    timeOnScorch -= FB.CastTime;
                }
                for (int i = 0; i < averageScorchesNeeded; i++)
                {
                    AddSpell(Sc, calculations);
                }

                Calculate(character, calculations);

                sequence = string.Format("{0}x Fireball : {1}x Scorch", fbCount, averageScorchesNeeded);
            }
        }
    }

    class FireballFireBlast : SpellCycle
    {
        public FireballFireBlast(Character character, CharacterCalculationsMage calculations)
            : base(33)
        {
            Name = "FireballFireBlast";

            Spell FB = calculations.GetSpell(SpellId.Fireball);
            BaseSpell Blast = (BaseSpell)calculations.GetSpell(SpellId.FireBlast);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            if (calculations.CalculationOptions.ImprovedScorch == 0)
            {
                // in this case just Fireball/Fire Blast, scorch debuff won't be applied
                float blastCooldown = Blast.Cooldown - Blast.CastTime;
                AddSpell(Blast, calculations);
                while (blastCooldown > 0)
                {
                    AddSpell(FB, calculations);
                    blastCooldown -= FB.CastTime;
                }
                Calculate(character, calculations);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)calculations.CalculationOptions.ImprovedScorch);
                double timeOnScorch = 30;
                int fbCount = 0;
                float blastCooldown = 0;

                do
                {
                    if (timeOnScorch - Sc.CastTime > Blast.Cooldown && blastCooldown <= 0)
                    {
                        AddSpell(Blast, calculations);
                        fbCount++;
                        timeOnScorch -= Blast.CastTime;
                        blastCooldown = Blast.Cooldown - Blast.CastTime;
                    }
                    else if (timeOnScorch > FB.CastTime + (averageScorchesNeeded + 1) * Sc.CastTime) // one extra scorch gap to account for possible resist
                    {
                        AddSpell(FB, calculations);
                        fbCount++;
                        timeOnScorch -= FB.CastTime;
                        blastCooldown -= FB.CastTime;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                for (int i = 0; i < averageScorchesNeeded; i++)
                {
                    AddSpell(Sc, calculations);
                    blastCooldown -= Sc.CastTime;
                }
                if (blastCooldown > 0) AddPause(blastCooldown);

                Calculate(character, calculations);
            }
        }
    }

    class ABAM3ScCCAM : Spell
    {
        public ABAM3ScCCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3ScCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = calculations.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(Sc0, calculations);
            float gap = 8 - AMc0.CastTime - Sc0.CastTime;
            while (gap - AB3.CastTime >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3Sc2CCAM : Spell
    {
        public ABAM3Sc2CCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3Sc2CC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = calculations.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(13);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle();
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(Sc0, calculations);
            float gap = 8 - AMc0.CastTime - Sc0.CastTime;
            while (gap >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAM : Spell
    {
        public ABAM3FrBCCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3FrBCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = calculations.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(FrB0, calculations);
            float gap = 8 - AMc0.CastTime - FrB0.CastTime;
            while (gap - AB3.CastTime >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAMFail : Spell
    {
        public ABAM3FrBCCAMFail(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3FrBCCFail";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = calculations.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast00);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(FrB0, calculations);
            float gap = 8 - AMc0.CastTime - FrB0.CastTime;
            while (gap - AB3.CastTime >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBScCCAM : Spell
    {
        public ABAM3FrBScCCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3FrBScCC";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = calculations.GetSpell(SpellId.FrostboltNoCC);

            Spell AM = calculations.GetSpell(SpellId.ArcaneMissiles);
            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast30);
            Spell FrB = calculations.GetSpell(SpellId.Frostbolt);
            Spell Sc = calculations.GetSpell(SpellId.Scorch);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle(2);
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle(4);
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle(6);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle(13);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            float gap = 8 - AMc0.CastTime;
            bool extraAM = false;
            while (gap >= AM.CastTime)
            {
                chain4.AddSpell(AM, calculations);
                gap -= AM.CastTime;
                extraAM = true;
            }
            if (!extraAM)
            {
                chain4.AddSpell(FrB0, calculations);
                gap -= FrB0.CastTime;
            }
            while (gap >= FrB.CastTime)
            {
                chain4.AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ThreatPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            commonChain = chain4;
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAMCCAM : Spell
    {
        public ABAMCCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAMCC";
            ABCycle = true;

            //AMCC-AB00-AB01-AB12-AB23       0.1
            //AM?0-AB33                      0.9

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB00 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB01 = calculations.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = calculations.GetSpell(SpellId.ArcaneBlast12);
            Spell AB23 = calculations.GetSpell(SpellId.ArcaneBlast23);
            Spell AB33 = calculations.GetSpell(SpellId.ArcaneBlast33NoCC);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AMc0, calculations);
                chain1.AddSpell(AB33, calculations);
                chain1.Calculate(character, calculations);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;

                commonChain = chain1;
            }
            else
            {

                //AMCC-AB00-AB01-AB12-AB23       0.1
                SpellCycle chain1 = new SpellCycle(5);
                chain1.AddSpell(AMCC, calculations);
                chain1.AddSpell(AB00, calculations);
                chain1.AddSpell(AB01, calculations);
                chain1.AddSpell(AB12, calculations);
                chain1.AddSpell(AB23, calculations);
                chain1.Calculate(character, calculations);

                //AM?0-AB33                      0.9
                SpellCycle chain2 = new SpellCycle(2);
                chain2.AddSpell(AMc0, calculations);
                chain2.AddSpell(AB33, calculations);
                chain2.Calculate(character, calculations);

                CastTime = CC * chain1.CastTime + (1 - CC) * chain2.CastTime;
                CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + (1 - CC) * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + (1 - CC) * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = (CC * chain1.CastTime * chain1.ThreatPerSecond + (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;

                commonChain = chain2;
            }
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3CCAM : Spell
    {
        public ABAM3CCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3CC";
            ABCycle = true;

            //AM?0-AB33-AMCC subcycle
            //starts with 3 AB debuffs, alternate AM-AB until AM procs CC, then AM chain and stop

            //AM?0-AB33-AM?0-AB33-...=0.1*0.9*...
            //...
            //AM?0-AB33-AM?0-AB33-AMCC=0.1*0.9*0.9
            //AM?0-AB33-AMCC=0.1*0.9
            //AMCC=0.1

            //V = AMCC + 0.1*0.9*AM?0AB33 + 0.1*0.9*0.9*2*AM?0AB33 + ... + 0.1*0.9^n*n*AM?0AB33 + ...
            //  = AMCC + 0.1*AM?0AB33 * sum_1_inf n*0.9^n
            //  = AMCC + 9*AM?0AB33

            // it is on average equivalent to (AM?0-AB33)x9+AMCC cycle


            //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
            //AB00-AM?0-AB11-AMCC                                 0.9*0.1
            //AB00-AMCC                                           0.1

            Spell AMc0 = calculations.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = calculations.GetSpell(SpellId.ArcaneMissilesCC);
            Spell AB0 = calculations.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = calculations.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = calculations.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell AB3 = calculations.GetSpell(SpellId.ArcaneBlast33NoCC);

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                SpellCycle chain1 = new SpellCycle(2);
                chain1.AddSpell(AMc0, calculations);
                chain1.AddSpell(AB3, calculations);
                chain1.Calculate(character, calculations);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;

                commonChain = chain1;
            }
            else
            {
                //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
                SpellCycle chain1 = new SpellCycle(24);
                chain1.AddSpell(AB0, calculations);
                chain1.AddSpell(AMc0, calculations);
                chain1.AddSpell(AB1, calculations);
                chain1.AddSpell(AMc0, calculations);
                chain1.AddSpell(AB2, calculations);
                for (int i = 0; i < (int)((1 - CC) / CC); i++)
                {
                    chain1.AddSpell(AMc0, calculations);
                    chain1.AddSpell(AB3, calculations);
                }
                chain1.AddSpell(AMCC, calculations);
                chain1.Calculate(character, calculations);

                //AB00-AM?0-AB11-AMCC                                 0.9*0.1
                SpellCycle chain2 = new SpellCycle(4);
                chain2.AddSpell(AB0, calculations);
                chain2.AddSpell(AMc0, calculations);
                chain2.AddSpell(AB1, calculations);
                chain2.AddSpell(AMCC, calculations);
                chain2.Calculate(character, calculations);

                //AB00-AMCC                                           0.1
                SpellCycle chain3 = new SpellCycle(2);
                chain3.AddSpell(AB0, calculations);
                chain3.AddSpell(AMCC, calculations);
                chain3.Calculate(character, calculations);


                CastTime = (1 - CC) * (1 - CC) * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * chain3.CastTime;
                CostPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * chain3.CastTime * chain3.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * chain3.CastTime * chain3.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.ThreatPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ThreatPerSecond + CC * chain3.CastTime * chain3.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - CC) * (1 - CC) * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * chain3.CastTime * chain3.ManaRegenPerSecond) / CastTime;

                commonChain = chain3;
            }
        }

        private SpellCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }
}
