using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ARX;

namespace FEN
{

    public delegate bool BoolCondition();

    public enum VICTORYSTATE { WIN, LOSS, ETC };

    public enum TERRAINS { SHORELINE = 0, START = 1, TRAIL = 2, NEST = 3, FORTRESS = 4, WARREN = 5, UNUSED = 6, MOUNTAINS = 7, IMPASSE = 8, VALLEY = 9 }

    public enum CARDTYPE { SKILL = 0, POWER, ATTACK, GEAR, CONSUMABLE, STATUS, USELESS }
    public enum BLURBTYPE { HEAL, BLOCKED, UNBLOCKED };


    public enum DECKTYPE { HAND = 0, DISCARD = 1, EQUIPPED = 2, POWERS = 3, RELICS = 4, DRAW = 5, EXHAUST = 6, GRAVE = 7, INVENTORY = 8, NONE = 9 }
    public enum STATPLACEMENT{BONUS, BASE, CURRENT, BASEANDCURRENT}

    public enum ALIGNMENT {NONE = 0, NEUTRAL, ALLY, ENEMY }

    public static partial class IDs
    {
        public const int MAXCOPIES = 5;
        public const int VALIDDECKSTATCOST = 10;
        public const int MINDECKSIZE = 10, MAXDECKSIZE = 25;
        public const int MAXHANDSIZE = 10;
        public const int MAXENEMYACTIONPREVIEW = 5;
        public const int MAXEQUIPPEDACCESSORIES = 4;

        public static readonly string[] EditorAttributes = {};

        public static readonly string[] Classes = { };

        public const string Brutes = "brutes",
            Adrenal = "adrenal",
            Bugs = "bugs",
            Crystal = "crystal",
            ShowOfForce = "showofforce",
            Storm = "storm",
            Volatile = "volatile";

        public const string
            Text = "text",
            ID = "id",
            Content = "content",
            Attribute = "attribute",
            Money = "money",
            None = "none",
            HP = "hp",
            Heal = "heal",
            DamageMultiplier = "mult",
            Amount = "amount",
            UniqueID = "uniqueid",
            Scene = "scene",
            StatName = "statname",
            OriginUniqueID = "originuniqueid",
            Position = "position",
            Trek = "trek", TrekLoot = "trekloot", Tutorial = "tutorial", Name = "name", 
            Title = "title", Description = "description", ValueX = "x", ValueY = "y",
            Min = "min", Max = "max", 
            MissionName = "missionname", Target = "target", Block = "block", Damage = "damage", 
            Strength = "strength", 
            StatusEffect = "statuseffect", 
            Battle = "battle", AbilityID = "abilityid", Attack = "attack", Buff = "buff", Debuff = "debuff", Weapon = "weapon",
            Armor = "armor", Burn = "burn", Price = "price", Dodge = "dodge", Scaling = "scaling", Accuracy = "accuracy", 
            AccuracyCost = "accuracycost", Stamina = "stamina",
            PerStrikeDamage = "perstrikedamage", Strikes = "strikes", State = "state", Corruption = "corruption", 
            Spikes = "spikes", Metallicize = "metallicize", Threat = "threat", CriticalChance = "criticalchance";

        public const string Act = "act", Pistol = "pistol", Shotgun = "shotgun", Heavy = "heavy", Melee = "melee", MachineGun = "machinegun",
            Chem = "chem", RandomChem = "randomchem", VialEffect = "vialeffect", Firearm = "firearm", GunMod = "gunmod", mod = "mod", MeleeMod = "meleemod", 
            PistolAmmo = "pistolammo", ShotgunAmmo = "shotgunammo", MachineGunAmmo = "machinegunammo", HeavyAmmo = "heavyammo",
            ThreshholdOne = "threshholdone", ThreshholdTwo = "threshholdtwo", ThreshholdThree = "threshholdthree", AllyAbility = "allyability";

        public const string ActionMenu = "actionmenu", ExecuteMenu = "executemenu";
    }

    public static class Blurbs
    {
        public const string Bright = "bright", BigBulletImpact = "bigbullet", MachineGunImpact = "manybullets", Rays = "rays",
            Crowned = "crowned", Gooey = "gooey", Spiky = "spiky", Punch = "punch", Damage = "damageblurb";
    }
}
