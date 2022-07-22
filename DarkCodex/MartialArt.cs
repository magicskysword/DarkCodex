﻿using CodexLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkCodex
{
    public class MartialArt
    {
        public static void CreatePaladinVirtuousBravo() // TODO: 
        {
            var paladin = Helper.ToRef<BlueprintCharacterClassReference>("bfa11238e7ae3544bbeb4d0b92e897ec"); //PaladinClass

            var archetype = Helper.CreateBlueprintArchetype(
                "VirtuousBravoArchetype",
                "Virtuous Bravo",
                "Although no less a beacon of hope and justice than other paladins, virtuous bravos rely on their wit and grace rather than might and strong armor.",
                removeSpellbook: true
                );

            var f1_prof = Helper.CreateBlueprintFeature(
                "VirtuousBravoProficiencies",
                "Virtuous Bravo Proficiencies",
                "Virtuous bravos are proficient with all simple and martial weapons, with light and medium armor, and with bucklers."
                ).SetComponents(
                Helper.CreateAddFacts("6d3728d4e9c9898458fe5e9532951132", "46f4fb320f35704488ba3d513397789d", "e70ecf1ed95ca2f40b754f1adb22bbdd", "203992ef5b35c864390b4e4a1e200629", "7c28228ce4eed1543a6b670fd2a88e72")
                );

            var f1_finesse = Helper.CreateBlueprintFeature(
                "VirtuousBravoFinesse",
                "Bravo’s Finesse",
                "A virtuous bravo gains Weapon Finesse as a bonus feat. She can use her Charisma score in place of her Intelligence score to meet prerequisites of combat feats."
                ).SetComponents(
                Helper.CreateAddFeatureOnApply("90e54424d682d104ab36436bd527af09"), //WeaponFinesse
                new ReplaceStatForPrerequisites { Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat, OldStat = StatType.Intelligence, NewStat = StatType.Charisma }
                );

            var f3_nimble = Helper.CreateBlueprintFeature(
                "VirtuousBravoNimble",
                "Nimble",
                "At 3rd level, a virtuous bravo gains a +1 dodge bonus to AC while wearing light armor or no armor.\nAnything that causes the virtuous bravo to lose her Dexterity bonus to AC also causes her to lose this dodge bonus. This bonus increases by 1 for every 4 paladin levels beyond 3rd (to a maximum of +5 at 19th level).\nThis ability replaces mercy."
                ).SetComponents(
                new CreateAddStatBonusInArmor(Resource.ValueRank, StatType.AC, ModifierDescriptor.Dodge, ArmorProficiencyGroup.None, ArmorProficiencyGroup.Light),
                Helper.CreateContextRankConfig(ContextRankBaseValueType.ClassLevel, ContextRankProgression.StartPlusDivStep, AbilityRankType.Default, 
                    startLevel: 3, stepLevel: 4, classes: paladin.ObjToArray())
                );

            archetype.SetAddFeatures(
                Helper.CreateLevelEntry(1, f1_prof),
                Helper.CreateLevelEntry(1, f1_finesse),
                Helper.CreateLevelEntry(3, f3_nimble)
                );

            archetype.SetRemoveFeatures(
                Helper.CreateLevelEntry(1, "b10ff88c03308b649b50c31611c2fefb"), //PaladinProficiencies
                Helper.CreateLevelEntry(3, "02b187038a8dce545bb34bbfb346428d"), //SelectionMercy
                Helper.CreateLevelEntry(11, "9f13fdd044ccb8a439f27417481cb00e"), //AuraOfJusticeFeature
                Helper.CreateLevelEntry(20, "eff3b63f744868845a2f511e9929f0de") //HolyChampion
                );

            Helper.AppendAndReplace(ref paladin.Get().m_Archetypes, archetype.ToRef());
        }

        /*
        Bladed Brush (Combat)
        Note: This is associated with a specific deity.
        You know how to balance a polearm perfectly, striking with artful, yet deadly precision.
        Prerequisite(s): Weapon Focus (glaive), must be a worshiper of the associated deity.
        Benefit(s): You can use the Weapon Finesse feat to apply your Dexterity modifier instead of your Strength modifier to attack rolls with a glaive sized for you, even though it isn’t a light weapon. When wielding a glaive, you can treat it as a one-handed piercing or slashing melee weapon and as if you were not making attacks with your off-hand for all feats and class abilities that require such a weapon (such as a duelist’s or swashbuckler’s precise strike).
        As a move action, you can shorten your grip on the glaive, treating it as though it lacked the reach weapon property. You can adjust your grip to grant the weapon the reach property as a move action.
        
        Virtuous Bravo

        Panache and Deeds (Ex)
        At 4th level, a virtuous bravo gains the swashbuckler’s panache class feature along with the following swashbuckler deeds: dodging panache, menacing swordplay, opportune parry and riposte, precise strike, and swashbuckler initiative. The virtuous bravo’s paladin levels stack with any swashbuckler levels when using these deeds.
        This ability replaces the paladin’s spellcasting.

        *Dodging Panache (Ex): At 1st level, when an opponent attempts a melee attack against the swashbuckler, the swashbuckler can as an immediate action spend 1 panache point to move 5 feet; doing so grants the swashbuckler a dodge bonus to AC equal to her Charisma modifier (minimum 0) against the triggering attack. This movement doesn’t negate the attack, which is still resolved as if the swashbuckler had not moved from the original square. This movement is not a 5-foot step; it provokes attacks of opportunity from creatures other than the one who triggered this deed. The swashbuckler can only perform this deed while wearing light or no armor, and while carrying no heavier than a light load.
        *Menacing Swordplay (Ex): At 3rd level, while she has at least 1 panache point, when a swashbuckler hits an opponent with a light or one-handed piercing melee weapon, she can choose to use Intimidate to demoralize that opponent as a swift action instead of a standard action.
        *Opportune Parry and Riposte (Ex): At 1st level, when an opponent makes a melee attack against the swashbuckler, she can spend 1 panache point and expend a use of an attack of opportunity to attempt to parry that attack. The swashbuckler makes an attack roll as if she were making an attack of opportunity; for each size category the attacking creature is larger than the swashbuckler, the swashbuckler takes a –2 penalty on this roll. If her result is greater than the attacking creature’s result, the creature’s attack automatically misses. The swashbuckler must declare the use of this ability after the creature’s attack is announced, but before its attack roll is made. Upon performing a successful parry and if she has at least 1 panache point, the swashbuckler can as an immediate action make an attack against the creature whose attack she parried, provided that creature is within her reach. This deed’s cost cannot be reduced by any ability or effect that reduces the number of panache points a deed costs.
        *Precise Strike (Ex): At 3rd level, while she has at least 1 panache point, a swashbuckler gains the ability to strike precisely with a light or one-handed piercing melee weapon (though not natural weapon attacks), adding her swashbuckler level to the damage dealt. To use this deed, a swashbuckler cannot attack with a weapon in her other hand or use a shield other than a buckler. She can even use this ability with thrown light or one-handed piercing melee weapons, so long as the target is within 30 feet of her. Any creature that is immune to sneak attacks is immune to the additional damage granted by precise strike, and any item or ability that protects a creature from critical hits also protects a creature from the additional damage of a precise strike. This additional damage is precision damage, and isn’t multiplied on a critical hit. As a swift action, a swashbuckler can spend 1 panache point to double her precise strike’s damage bonus on the next attack. This benefit must be used before the end of her turn, or it is lost. This deed’s cost cannot be reduced by any ability or effect that reduces the amount of panache points a deed costs (such as the Signature Deed feat).
        *Swashbuckler Initiative (Ex): At 3rd level, while the swashbuckler has at least 1 panache point, she gains a +2 bonus on initiative checks. In addition, if she has the Quick Draw feat, her hands are free and unrestrained, and she has any single light or one-handed piercing melee weapon that isn’t hidden, she can draw that weapon as part of the initiative check.
        
        Advanced Deeds (Ex)
        At 11th level, a virtuous bravo gains the following swashbuckler deeds: bleeding wound, evasive, subtle blade, superior feint, swashbuckler’s grace, and targeted strike.
        This ability replaces aura of justice.

        *Bleeding Wound (Ex): At 11th level, when the swashbuckler hits a living creature with a light or one-handed piercing melee weapon attack, as a free action she can spend 1 panache point to have that attack deal additional bleed damage. The amount of bleed damage dealt is equal to the swashbuckler’s Dexterity modifier (minimum 1). Alternatively, the swashbuckler can spend 2 panache points to deal 1 point of Strength, Dexterity, or Constitution bleed damage instead (swashbuckler’s choice). Creatures that are immune to sneak attacks are also immune to these types of bleed damage.
        *Evasive (Ex): At 11th level, while a swashbuckler has at least 1 panache point, she gains the benefits of the evasion, uncanny dodge, and improved uncanny dodge rogue class features. She uses her swashbuckler level as her rogue level for improved uncanny dodge.
        *Subtle Blade (Ex): At 11th level, while a swashbuckler has at least 1 panache point, she is immune to disarm, steal, and sunder combat maneuvers made against a light or one-handed piercing melee weapon she is wielding.
        *Superior Feint (Ex): At 7th level, a swashbuckler with at least 1 panache point can, as a standard action, purposefully miss a creature she could make a melee attack against with a wielded light or one-handed piercing weapon. When she does, the creature is denied its Dexterity bonus to AC until the start of the swashbuckler’s next turn.
        *Swashbuckler’s Grace (Ex): At 7th level, while the swashbuckler has at least 1 panache point, she takes no penalty for moving at full speed when she uses Acrobatics to attempt to move through a threatened area or an enemy’s space.
        *Targeted Strike (Ex): At 7th level, as a full-round action the swashbuckler can spend 1 panache point to make an attack with a single light or one-handed piercing melee weapon that cripples part of a foe’s body. The swashbuckler chooses a part of the body to target. If the attack succeeds, in addition to the attack’s normal damage, the target suffers one of the following effects based on the part of the body targeted. If a creature doesn’t have one of the listed body locations, that body part cannot be targeted. Creatures that are immune to sneak attacks are also immune to targeted strikes. Items or abilities that protect a creature from critical hits also protect a creature from targeted strikes.
            *Arms: The target takes no damage from the attack, but it drops one carried item of the swashbuckler’s choice, even if the item is wielded with two hands. Items held in a locked gauntlet cannot be chosen.
            *Head: The target is confused for 1 round. This is a mind-affecting effect.
            *Legs: The target is knocked prone. Creatures with four or more legs or that are immune to trip attacks are immune to this effect.
            *Torso or Wings: The target is staggered for 1 round.


        Bravo’s Holy Strike (Su)
        At 20th level, a virtuous bravo becomes a master at dispensing holy justice with her blade.
        When the virtuous bravo confirms a critical hit with a light or one-handed piercing melee weapon, she can choose one of the following three effects in addition to dealing damage: the target is rendered unconscious for 1d4 hours, the target is paralyzed for 2d6 rounds, or the target is slain. Regardless of the effect chosen, the target can attempt a Fortitude save.
        On a success, the target is instead stunned for 1 round (it still takes damage). The DC of this save is equal to 10 + 1/2 the virtuous bravo’s paladin level + her Charisma modifier. Once a creature has been the target of a bravo’s holy strike, regardless of whether or not it succeeds at the save, that creature is immune to that bravo’s holy strike for 24 hours. Creatures that are immune to critical hits are also immune to this ability.
        This ability replaces holy champion.

        */
    }
}
