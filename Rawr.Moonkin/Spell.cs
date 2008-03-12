using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    enum SpellSchool
    {
        Arcane,
        Nature
    }


    class DotSpell
    {
        public float numTicks = 0.0f;
        public float damagePerTick = 0.0f;
        public float tickLength = 0.0f;
    }
    class Spell
    {
        // This field will become modifiable via a property in 2.4, according to spell haste
        public static float GlobalCooldown = 1.5f;
        public SpellSchool school = SpellSchool.Nature;
        public float manaCost = 0.0f;
        public float trueCastTime = 0.0f;
        public float castTime
        {
            get
            {
                return trueCastTime;
            }
            set
            {
                if (value < GlobalCooldown)
                    trueCastTime = GlobalCooldown;
                else
                    trueCastTime = value;
            }
        }
        public float damagePerHit = 0.0f;
        public DotSpell dotEffect = null;
        public float critBonus = 150.0f;
        public float extraCritChance = 0.0f;
    }

    class MoonkinSpells : IEnumerable<KeyValuePair<string, Spell>>
    {
        private Dictionary<string, Spell> spellList = null;
        public string lastRotationName = "";
        public Spell this[string spellName]
        {
            get
            {
                if (spellList == null)
                {
                    spellList = new Dictionary<string, Spell>();
                    spellList.Add("Wrath", new Spell()
                    {
                        manaCost = 255.0f,
                        school = SpellSchool.Nature,
                        castTime = 2.0f,
                        damagePerHit = (381.0f + 429.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 1.50f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Starfire", new Spell()
                    {
                        manaCost = 370.0f,
                        school = SpellSchool.Arcane,
                        castTime = 3.5f,
                        damagePerHit = (540.0f + 636.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 1.50f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Moonfire", new Spell()
                    {
                        manaCost = 495.0f,
                        school = SpellSchool.Arcane,
                        // Instant cast, but GCD is limiting factor
                        castTime = 1.5f,
                        damagePerHit = (305.0f + 357.0f) / 2.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 3.0f,
                            numTicks = 4.0f,
                            damagePerTick = 150.0f
                        },
                        critBonus = 1.50f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Insect Swarm", new Spell()
                    {
                        manaCost = 175.0f,
                        school = SpellSchool.Nature,
                        // Instant cast, but GCD is limiting factor
                        castTime = 1.5f,
                        // Using a 0% damage per hit should ensure that a "critical" insect swarm doesn't do any extra damage
                        damagePerHit = 0.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 2.0f,
                            numTicks = 6.0f,
                            damagePerTick = 132.0f
                        },
                        critBonus = 0.0f,
                        extraCritChance = 0.0f
                    });
                }
                return spellList[spellName];
            }
            set
            {
                spellList[spellName] = value;
            }
        }

        private Dictionary<string, List<Spell>> BuildSpellRotations()
        {
            // Build each spell rotation
            List<Spell> MFSFx3W = new List<Spell>(new Spell[] {
                spellList["Moonfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Wrath"]
            });
            List<Spell> ISMFSFx3 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Moonfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"]
            });
            List<Spell> SFSpam = new List<Spell>(new Spell[] {
                spellList["Starfire"]
            });
            List<Spell> MFWx8 = new List<Spell>(new Spell[] {
                spellList["Moonfire"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });
            List<Spell> ISSFx4 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"]
            });
            List<Spell> ISMFWx7 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Moonfire"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });
            List<Spell> ISSFx3W = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Wrath"]
            });
            List<Spell> WrathSpam = new List<Spell>(new Spell[] {
                spellList["Wrath"]
            });
            List<Spell> ISWx8 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });

            // Create a "master list" of spell rotations
            Dictionary<string, List<Spell>> spellRotations = new Dictionary<string, List<Spell>>();
            spellRotations.Add("MF/SFx3/W", MFSFx3W);
            spellRotations.Add("MF/Wx8", MFWx8);
            spellRotations.Add("IS/MF/SFx3", ISMFSFx3);
            spellRotations.Add("IS/MF/Wx7", ISMFWx7);
            spellRotations.Add("IS/SFx3/W", ISSFx3W);
            spellRotations.Add("IS/SFx4", ISSFx4);
            spellRotations.Add("IS/Wx8", ISWx8);
            spellRotations.Add("Starfire Spam", SFSpam);
            spellRotations.Add("Wrath Spam", WrathSpam);
            return spellRotations;
        }

        private float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            // Innervate calculations
            int numInnervates = character.CalculationOptions["Innervate"] == "Yes" ? ((int)fightLength / (int)innervateCooldown + 1) : 0;
            // Innervate mana rate increases only spirit-based regen
            float innervateManaRate = (calcs.ManaRegen - calcs.BasicStats.Mp5 / 5f) * 4 + calcs.BasicStats.Mp5 / 5f;
            float innervateTime = numInnervates * 20.0f;
            float totalInnervateMana = innervateManaRate * innervateTime - (numInnervates * calcs.BasicStats.Mana * 0.04f);

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen;
        }

        public void GetRotation(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            // Nature's Grace haste time
            bool naturesGrace = int.Parse(character.CalculationOptions["NaturesGrace"]) > 0 ? true : false;

            // Calculate spell resists due to level
            int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            float missRate = 0.05f;
            switch (targetLevel)
            {
                case 70:
                    missRate = 0.04f;
                    break;
                case 71:
                    missRate = 0.05f;
                    break;
                case 72:
                    missRate = 0.06f;
                    break;
                case 73:
                    missRate = 0.17f;
                    break;
            }
            missRate -= calcs.SpellHit;
            if (missRate < 0.01f) missRate = 0.01f;

            // Get total effective mana pool and total effective dps time
            float totalMana = GetEffectiveManaPool(character, calcs);
            float fightLength = calcs.FightLength * 60;

            Dictionary<string, List<Spell>> spellRotations = BuildSpellRotations();

            foreach (KeyValuePair<string, List<Spell>> rotation in spellRotations)
            {
                float averageCritChance = 0.0f;
                int spellCount = 0;
                foreach (Spell sp in rotation.Value)
                {
                    // Spells that do 0 damage are considered uncrittable in this simulation
                    if (sp.damagePerHit > 0.0f)
                        averageCritChance += calcs.SpellCrit + sp.extraCritChance;
                    ++spellCount;
                }
                averageCritChance /= spellCount;

                float damageDone = 0.0f;
                float manaUsed = 0.0f;
                float duration = 0.0f;
                float dotDuration = 0.0f;
                foreach (Spell sp in rotation.Value)
                {
                    DoSpellCalculations(sp, naturesGrace, averageCritChance, missRate, calcs, ref damageDone, ref manaUsed, ref duration, ref dotDuration);
                }
                // Handle the case where there is sufficient haste to add another spell cast in the DoT time
                // Right now, just automagically casts the last spell in the rotation
                while (dotDuration > rotation.Value[rotation.Value.Count - 1].castTime)
                {
                    DoSpellCalculations(rotation.Value[rotation.Value.Count - 1], naturesGrace, averageCritChance, missRate, calcs, ref damageDone, ref manaUsed, ref duration, ref dotDuration);
                }
                // Handle the case where DoTs overflow the cast times
                if (dotDuration > 0)
                    duration += dotDuration;
                // Calculate how long we will burn through all our mana
                float secsToOom = totalMana / (manaUsed / duration);
                // This dps calc takes into account time spent not doing dps due to OOM issues
                float dps = damageDone / duration * (secsToOom >= fightLength ? fightLength : secsToOom) / fightLength;
                float dpm = damageDone / manaUsed;
                if (dps > calcs.DPS)
                {
                    calcs.DamageDone = damageDone;
                    calcs.DPS = dps;
                    calcs.DPM = dpm;
                    calcs.RotationName = rotation.Key;
                    if (secsToOom >= fightLength)
                        calcs.TimeToOOM = new TimeSpan(0, 0, 0);
                    else
                        calcs.TimeToOOM = new TimeSpan(0, (int)Math.Floor(secsToOom) / 60, (int)Math.Floor(secsToOom) % 60);
                }
            }
            calcs.SubPoints = new float[] { calcs.DamageDone };
            calcs.OverallPoints = calcs.SubPoints[0];
        }

        private void DoSpellCalculations(Spell sp, bool naturesGrace, float averageCritChance, float missRate, CharacterCalculationsMoonkin calcs, ref float damageDone, ref float manaUsed, ref float duration, ref float dotDuration)
        {
            // Save/restore casting time because we only want to apply the effect once
            float oldCastTime = sp.castTime;
            if (naturesGrace)
            {
                sp.castTime -= 0.5f * averageCritChance;
            }
            float oldSpellDamage = 0.0f;
            if (dotDuration > 0 && sp.school == SpellSchool.Arcane && sp.dotEffect == null)
            {
                oldSpellDamage = sp.damagePerHit;
                // Add 4pc T5 bonus to Starfire spell
                sp.damagePerHit *= 1.1f;
            }
            // Calculate hits/crits/misses
            // Use a 2-roll system; crits only count if the spell hits, it's either a hit or a crit (not both)
            // Note: sp.DamagePerHit makes allowance for a DoT not being able to crit.
            float normalHitDamage = sp.damagePerHit * (1 - missRate - sp.extraCritChance - calcs.SpellCrit);
            float critHitDamage = sp.damagePerHit * sp.critBonus * ((1 - missRate) * (sp.extraCritChance + calcs.SpellCrit));

            damageDone += normalHitDamage + critHitDamage;
            manaUsed += sp.manaCost;
            if (sp.dotEffect != null)
            {
                damageDone += sp.dotEffect.damagePerTick * sp.dotEffect.numTicks;
                dotDuration = sp.dotEffect.numTicks * sp.dotEffect.tickLength;
            }
            if (dotDuration > 0)
            {
                dotDuration -= sp.castTime;
            }
            duration += sp.castTime;

            if (naturesGrace)
            {
                sp.castTime = oldCastTime;
            }
            if (oldSpellDamage > 0)
            {
                sp.damagePerHit = oldSpellDamage;
                oldSpellDamage = 0.0f;
            }
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return spellList.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,Spell>> Members

        IEnumerator<KeyValuePair<string, Spell>> IEnumerable<KeyValuePair<string, Spell>>.GetEnumerator()
        {
            return spellList.GetEnumerator();
        }

        #endregion

    }
}