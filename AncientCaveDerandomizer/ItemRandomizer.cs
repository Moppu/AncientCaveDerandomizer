using AncientCaveDerandomizer.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    public class ItemRandomizer
    {
        // use alternate flags
        public static int[] cursedItemIndexes = new int[]
        {
            0x45, // deadly sword
            0x4C, // beserk blade
            0x56, // deadly sword
            0x58, // luck rapier
            0x5C, // lucky blade
            0x7D, // deadly rod
            0x92, // fatal pick
            0xA2, // cursed bow
            0xD2, // deadly armor
            0x17D, // evil jewel
        };
        // 0 useless, 10 awesome
        public static int[] itemDesirability = new int[]
        {
            0, // nothing
            1, // charred newt
            3, // potion
            5, // hipotion
            7, // expotion
            3, // magic
            5, // himagic
            7, // exmagic
            7, // regain
            8, // miracle
            4, // antidote
            2, // awake
            2, // shriek
            2, // mystery pin
            7, // stat boosters
            1,
            1,2,2,7,7, // ->20
            1,4,1,1,1,
            1,4,4,4,4,
            4,2,2,4,0, // ear pick?
            4,4,4,7,0, // ->40, green tea
            0,0,0,8,4, // escape, warp, dragon egg, curselifter
            4,5,5,2,2, // fruits
            2,4,8,4,1,
            1,1,4,4,4, // ->60
            4,4,4,4,4,
            4,4,4,7,0, // deadly swords
            4,4,4,4,4,
            1,0,6,4,4, // ->80 multi sword, beserk blade - annoying cursed weapon
            4,4,4,9,6, // dekar blade, crazy blade
            7,0,2,0,4, // deadly swords
            8,2,0,4,7, // deadly swords
            6,6,6,6,6, // ->100
            7,7,7,7,8, // swords
            8,8,9,8,8, // swords
            8,0,4,7,4, // swords, egg sword
            4,8,4,5,6, // ->120 axes
            7,4,1,1,7, // deadly rod
            0,4,4,4,4, // deadly rod
            5,6,6,7,7, // rods n stuff
            7,1,1,4,4, // ->140 spark staff
            4,4,4,4,6, // air whip
            7,0,7,7,7, // spears, some deadly
            7,8,8,4,4, // spears
            4,4,4,4,7, // ->160
            7,7,8,1,1, // bows
            1,4,1,1,1,
            1,1,4,4,4,
            4,4,4,4,6, // ->180
            4,4,4,4,4,
            4,4,6,7,4, // armors
            4,4,4,4,4,
            4,4,8,6,4, // ->200 power cape
            7,7,4,4,4,
            4,4,8,4,4, // heal armor
            0,4,7,6,8,
            4,8,7,4,7, // ->220
            7,7,7,9,7,
            8,1,1,1,1,
            1,1,1,6,4, // power brace
            4,4,4,4,4, // ->240
            4,6,4,4,4,
            4,4,4,4,4,
            4,4,4,4,4,
            4,4,4,7,7, // ->260
            7,7,7,4,4,
            4,8,0,7,7,
            1,1,1,1,1,
            1,1,4,4,1, // ->280
            1,1,4,4,1,
            7,4,4,4,4,
            4,4,1,4,4,
            9,4,4,4,6, // ->300
            4,4,4,4,4,
            4,4,4,4,4,
            4,7,7,4,7, // zirco helmets
            7,7,7,7,7, // ->320
            7,7,1,1,1, // rings
            1,6,6,6,2,
            6,2,6,6,6,
            6,2,2,6,6, // ->340
            6,6,6,6,6,
            6,6,6,6,2,
            6,6,6,2,6,
            6,6,6,1,0, // ->360 accessories, egg ring=cheat mode
            6,6,6,6,6,
            7,2,0,6,6, // uni jewel
            6,6,2,2,6,
            2,6,6,2,6, // ->380
            6,0,6,6,6, // evil jewel
            2,6,2,6,6,
            6,6,6,0,0, // 394 - casino
            0,0,0,0,0, // ->400
            0,0,0,0,0,
            0,0,0,0,0,
            0,1,1,1,1, // 412 - iris
            1,1,1,1,1, // ->420
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0, // ->440
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0, // ->460
            0,0,0,0,0,
            0,
            //
        };

        public static string[] itemNames = new string[]
        {
            "No equip",
            "Charred newt",
            "Potion",
            "Hi-Potion",
            "Ex-Potion",
            "Magic jar",
            "Hi-Magic",
            "Ex-Magic",
            "Regain",
            "Miracle",
            "Antidote",
            "Awake",
            "Shriek",
            "Mystery pin",
            "Power gourd",
            "Mind gourd",
            "Magic guard",
            "Life potion",
            "Spell potion",
            "Power potion",
            "Speed potion",
            "Mind potion",
            "Brave",
            "Pear cider",
            "Sour cider",
            "Lime cider",
            "Plum cider",
            "Apple cider",
            "Sleep ball",
            "Confuse ball",
            "Freeze ball",
            "Smoke ball",
            "Ice ball",
            "Fire ball",
            "Terror ball",
            "Ear pick",
            "Boomerang",
            "Big boomer",
            "Ex-boomer",
            "Dragon tooth",
            "Green tea",
            "Escape",
            "Warp",
            "Dragon egg",
            "Curselifter",
            "Providence",
            "Secret fruit",
            "Holy fruit",
            "Breeze fruit",
            "Charm fruit",
            "Dark fruit",
            "Earth fruit",
            "Flame fruit",
            "Magic fruit",
            "Dual blade",
            "Frypan",
            "Knife",
            "Small knife",
            "Rapier",
            "Battle knife",
            "Dagger",
            "Insect crush",
            "Long knife",
            "Short sword",
            "Light knife",
            "Kukri",
            "Gladius",
            "Cold rapier",
            "Scimitar",
            "Deadly sword (cursed)",
            "Deadly sword (uncursed)",
            "SuhrCustom11",
            "Bronze sword",
            "Fire dagger",
            "War rapier",
            "Long sword",
            "Beserk blade (cursed)",
            "Beserk blade (uncursed)",
            "Multi sword",
            "Rockbreaker",
            "Broadsword",
            "Estok",
            "Silvo rapier",
            "Burn sword",
            "Dekar blade",
            "Crazy blade",
            "Deadly sword (cursed)",
            "Deadly sword (uncursed)",
            "Luck rapier (cursed)",
            "Luck rapier (uncursed)",
            "Aqua sword",
            "Red saber",
            "Lucky blade (cursed)",
            "Lucky blade (uncursed)",
            "Mist rapier",
            "Boom sword",
            "Freeze sword",
            "Silver sword",
            "Flying blow",
            "Super sword",
            "Buster sword",
            "Rune rapier",
            "Old sword",
            "Lizard blow",
            "Zirco sword",
            "Sizzle sword",
            "Blaze sword",
            "Myth blade",
            "Gades blade",
            "Sky sword",
            "Snow sword",
            "Fry sword",
            "Egg sword",
            "Franshiska",
            "Thunder ax",
            "Hand ax",
            "Bronze ax",
            "Flying ax",
            "Rainy ax",
            "Great ax",
            "Zirco ax",
            "Mega ax",
            "Mace",
            "Rod",
            "Staff",
            "Deadly rod (cursed)",
            "Deadly rod (uncursed)",
            "Sleep rod",
            "Long staff",
            "Holy staff",
            "Morning star",
            "Pounder rod",
            "Crystal wand",
            "Silver rod",
            "Zirco rod",
            "Zirco flail",
            "Spark staff",
            "Whip",
            "Wire",
            "Chain",
            "Aqua whip",
            "Cutter whip",
            "Royal whip",
            "Holy whip",
            "Zirco whip",
            "Air whip",
            "Fatal pick (cursed)",
            "Fatal pick (uncursed)",
            "Spear",
            "Trident",
            "Halberd",
            "Heavy lance",
            "Water spear",
            "Dragon spear",
            "Vice pliers",
            "Coma hit",
            "Figgoru",
            "Superdriver",
            "Stun gun",
            "Battledriver",
            "Launcher",
            "Freeze bow",
            "Cursed bow",
            "Arty's bow",
            "Apron",
            "Dress",
            "Cloth",
            "Lab-coat",
            "Hide armor",
            "Frock",
            "Robe",
            "Cloth armor",
            "Coat",
            "Tough hide",
            "Light dress",
            "Light armor",
            "Camu armor",
            "Baggy",
            "Tight dress",
            "Chainmail",
            "Holy wings",
            "Ironmail",
            "Toga",
            "Chain armor",
            "Thick cloth",
            "Stone plate",
            "Long robe",
            "Plated cloth",
            "Iron plate",
            "Metal mail",
            "Silk toga",
            "Silver armor",
            "Light jacket",
            "Metal coat",
            "Silver mail",
            "Power jacket",
            "Quilted silk",
            "Metal armor",
            "Power cape",
            "Magic bikini",
            "Silver robe",
            "Evening gown",
            "Plate armor",
            "Plati plate",
            "Silk robe",
            "Revive armor",
            "Crystal mail",
            "Crystal robe",
            "Heal armor",
            "Metal jacket",
            "Deadly armor (cursed)",
            "Deadly armor (uncursed)",
            "Eron dress",
            "Bright armor",
            "Bright cloth",
            "Power robe",
            "Magic scale",
            "Holy robe",
            "Ghostclothes",
            "Royal dress",
            "Full mail",
            "Old armor",
            "Zircon plate",
            "Zircon armor",
            "Mirak plate",
            "Ruse armor",
            "Pearl armor",
            "Chop board",
            "Small shield",
            "Hide shield",
            "Buckler",
            "Mini shield",
            "Wood shield",
            "Bracelet",
            "Power brace",
            "Kite shield",
            "Tough gloves",
            "Brone shield",
            "Anger brace",
            "Block shield",
            "Tecto gloves",
            "Round shield",
            "Pearl brace",
            "Fayza shield",
            "Big shield",
            "Tall shield",
            "Silvo shield",
            "Spike shield",
            "Slash shield",
            "Mage shield",
            "Tuff buckler",
            "Tect buckler",
            "Gold gloves",
            "Gold shield",
            "Plati gloves",
            "Plati shield",
            "Gauntlet",
            "Rune gloves",
            "Holy shield",
            "Zirco gloves",
            "Zirco shield",
            "Old shield",
            "Flame shield",
            "Water gaunt",
            "Bolt shield",
            "Cryst shield",
            "Mega shield",
            "Dark mirror (cursed)",
            "Dark mirror (uncursed)",
            "Apron shield",
            "Pearl shield",
            "Pot",
            "Beret",
            "Cap",
            "Cloth helmet",
            "Hairband",
            "Headband",
            "Hide helmet",
            "Jet helm",
            "Red beret",
            "Glass cap",
            "Wood helmet",
            "Blue beret",
            "Brone helmet",
            "Stone helmet",
            "Cloche",
            "Fury helmet",
            "Iron helmet",
            "Tight helmet",
            "Turban",
            "Plate cap",
            "Roomy helmet",
            "Tight turban",
            "Glass cloche",
            "Plate helmet",
            "Rock helmet",
            "Jute helmet",
            "Shade hat",
            "Metal cloche",
            "SilverHelmet",
            "Fury ribbon",
            "Silver hat",
            "Eron hat",
            "Circlet",
            "Golden helm",
            "Gold band",
            "Plati band",
            "Plati helm",
            "Crysto beret",
            "Crysto helm",
            "Holy cap",
            "Safety hat",
            "Zirco band",
            "Zirco helmet",
            "Old helmet",
            "Agony helm",
            "Boom turban",
            "Aqua helm",
            "Ice hairband",
            "Legend helm",
            "Hairpin",
            "Brill helm",
            "Pearl helmet",
            "Ear jewel",
            "Glass brace",
            "Glass ring",
            "Earring",
            "Speedy ring",
            "Power ring",
            "Muscle ring",
            "Protect ring",
            "Mind ring",
            "Witch ring",
            "Fire ring",
            "Water ring",
            "Ice ring",
            "Thunder ring",
            "Fury ring",
            "Mystery ring",
            "Sonic ring",
            "Hipower ring",
            "Trick ring",
            "Fake ring",
            "S-fire ring",
            "S-water ring",
            "S-ice ring",
            "S-thun ring",
            "S-power ring",
            "S-mind ring",
            "S-pro ring",
            "S-witch ring",
            "Undead ring",
            "Rocket ring",
            "Ghost ring",
            "Angry ring",
            "S-myst ring",
            "Dia ring",
            "Sea ring",
            "Dragon ring",
            "Engage ring",
            "Egg ring",
            "Horse rock",
            "Eagle rock",
            "Lion fang",
            "Bee rock",
            "Snake rock",
            "Cancer rock",
            "Pumkin jewel",
            "Uni jewel",
            "Mysto jewel",
            "Samu jewel",
            "Bat rock",
            "Hidora rock",
            "Flame jewel",
            "Water jewel",
            "Thundo jewel",
            "Earth jewel",
            "Twist jewel",
            "Gloom jewel",
            "Tidal jewel",
            "Magma rock",
            "Evil jewel (cursed)",
            "Evil jewel (uncursed)",
            "Gorgon rock",
            "Song rock",
            "Kraken rock",
            "Catfish jwl.",
            "Camu jewel",
            "Spido jewel",
            "Gorgan rock",
            "Light jewel",
            "Black eye",
            "Silver eye",
            "Gold eye",
            "1 coin",
            "10 coin set",
            "50 coin set",
            "100 coin set",
            "Flame charm",
            "Zap charm",
            "Magic lamp",
            "Statue",
            "Rage knife",
            "Fortune whip",
            "Dragon blade",
            "Bunny ring",
            "Bunny ears",
            "Bunnylady",
            "Bunny sword",
            "Bunnysuit",
            "Seethru cape",
            "Seethru silk",
            "Iris sword",
            "Iris shield",
            "Iris helmet",
            "Iris armor",
            "Iris ring",
            "Iris jewel",
            "Iris staff",
            "Iris pot",
            "Iris tiara",
            "Power jelly",
            "Jewel sonar",
            "Hook",
            "Bomb",
            "Arrow",
            "Fire arrow",
            "Hammer",
            "Treas. sword",
            "Door key",
            "Shrine key",
            "Sky key",
            "Lake key",
            "Ruby key",
            "Wind key",
            "Cloud key",
            "Light key",
            "Sword key",
            "Tree key",
            "Flower key",
            "Magma key",
            "Heart key",
            "Ghost key",
            "Trial key",
            "Dankirk key",
            "Basement key",
            "Narcysus key",
            "Truth key",
            "Mermaid jade",
            "Engine",
            "Ancient key",
            "Pretty flwr.",
            "Glass angel",
            "VIP card",
            "Key26",
            "Key27",
            "Key28",
            "Key29",
            "Key30",
            "Crown",
            "Ruby apple",
            "PURIFIA",
            "Tag ring",
            "Tag ring",
            "RAN-RAN step",
            "Tag candy",
            "Last",
        };
        public static void setRedChestItemQuality(byte[] origData, byte[] newData, int quality, int bottomFloor, int inclusionType)
        {
            if(bottomFloor != 99)
            {
                int goldScale = (int)((99.0 / bottomFloor) * 1000);
                if(goldScale > 32767)
                {
                    goldScale = 32767;
                }
                newData[0x19059] = (byte)goldScale;
                newData[0x1905E] = (byte)(goldScale>>8);
                Logging.logDebug("Scaling item values for max floor " + bottomFloor + "; new scale value (original 1K) = " + goldScale, "items");
            }
            // no change
            if (quality == 5)
            {
                Logging.logDebug("No change in item quality", "items");
                return;
            }

            // scale down the value of the good items, and up the quality of the bad items, if quality > 5
            double sF = quality - 5.0;
            Logging.logDebug("Desired item quality where 1=shit, 5=normal, 9=awesome = " + quality, "items");

            for (int i = 0; i < 467; i++)
            {
                int offset = origData[0xB4F69 + i * 2] + origData[0xB4F69 + i * 2 + 1] * 256;
                int itemValue = origData[0xB4F69 + offset + 5] + origData[0xB4F69 + offset + 6] * 256;
                // at quality 10,
                //   when itemD=10, scaleFactor = 25
                //   when itemD=5, scaleFactor = 0
                //   when itemD=0, scaleFactor = -25
                // at quality 5,
                //   when itemD=10, scaleFactor = 0
                //   when itemD=5, scaleFactor = 0
                //   when itemD=0, scaleFactor = 0
                // at quality 0,
                //   when itemD=10, scaleFactor = -25
                //   when itemD=5, scaleFactor = 0
                //   when itemD=0, scaleFactor = 25

                // range -1-1, 1 to have more, -1 to have less
                double scaleFactor = ((itemDesirability[i] - 5.0) * (double)sF) / 25.0;

                int oldItemValue = itemValue;
                if(inclusionType == 1)
                {
                    // needs work
                    double val = (-scaleFactor + 1) * (-scaleFactor + 1) * 10000;
                    itemValue = val > 65535 ? 65535 : val < 0 ? 0 : (int)val;
                }
                else
                { 
                    itemValue = (int)(itemValue - scaleFactor * itemValue - scaleFactor * 5000);
                }
                if (itemValue > 65535)
                {
                    itemValue = 65535;
                }
                if (itemValue < 0)
                {
                    itemValue = 0;
                }
                Logging.logDebug("Item#" + i + "/466 [" + itemNames[i] + "] desirability (1-10) = " + itemDesirability[i] + " old value = " + oldItemValue + " new value = " + itemValue, "items");
                newData[0xB4F69 + offset + 5] = (byte)itemValue;
                newData[0xB4F69 + offset + 6] = (byte)(itemValue >> 8);
            }
        }

        public static void randomizeChestDistribution(byte[] origData, byte[] newData, Random r)
        {
            // this code is at $83/9142 .. load 8 randomish items
            // $54 == x24, divide by two == x12 == 18
            // -> 211c
            // value from random stuff == x54 == 84 -> 211b
            // x12 * x54 = 5E8 or something, grab the 5, mul by 2 (a) and index into 7Fx000 for an item ID (0x3C == dagger).
            // x for item type 0 = 0000
            // x for item type 1 = 1000 (item ID AC == coat for example)
            // only 7F0000 and 7F1000 used for weapons/armor due to value stuff .. everything else pulled directly?
            Logging.logDebug("Random chest distribution", "items");
            byte[] originalValues = new byte[] { 0xAE, 0x81, 0x63, 0x5E, 0x24 };
            byte[] changeMax = new byte[] { 0x18, 0x18, 0x05, 0x10, 0x18 };
            int[] offsets = new int[] { 0x19147, 0x1914B, 0x1914F, 0x19153, 0x19157 };
            for(int i=0; i < 5; i++)
            {
                int changeAmount = (r.Next() % (changeMax[i] * 2 + 1)) - changeMax[i];
                Logging.logDebug("Randomized existing value at " + offsets[i].ToString("X") + " by " + changeAmount, "items");
                newData[offsets[i]] = (byte)(originalValues[i] + changeAmount);
                Logging.logDebug("Wrote " + newData[offsets[i]].ToString("X"), "items");
            }
        }

        public static void setManualChestDistribution(byte[] origData, byte[] newData, int[] manualValues)
        {
            Logging.logDebug("Manual chest distribution", "items");
            int[] offsets = new int[] { 0x19147, 0x1914B, 0x1914F, 0x19153, 0x19157 };
            int sum = 0;
            for(int i=0; i < 6; i++)
            {
                sum += manualValues[i];
            }

            byte[] newValues = new byte[5];

            for(int i=0; i < 5; i++)
            {
                Logging.logDebug("Converting manual value " + manualValues[i] + " to byte with sum " + sum, "items");
                double newValue = 0;
                for(int j=i + 1; j < 6; j++)
                {
                    double jValue = (manualValues[j] / (double)sum) * 256.0;
                    Logging.logDebug("Adding " + jValue, "items");
                    newValue += jValue;
                }
                newValues[i] = (byte)newValue;
                Logging.logDebug("Ended up with " + newValue, "items");
            }

            for (int i = 0; i < 5; i++)
            {
                newData[offsets[i]] = newValues[i];
                Logging.logDebug("Setting manual chest distribution value at " + offsets[i].ToString("X") + " to " + newValues[i].ToString("X"), "items");
            }

        }

        public static void setRedChestItemInclusion(byte[] origData, byte[] newData, int inclusionType, Random r)
        {
            if (inclusionType == 1)
            {
                Logging.logDebug("Item inclusion type = include everything that isn't totally useless", "items");
            }
            else if(inclusionType == 2)
            {
                Logging.logDebug("Item inclusion type = randomize but keep basic usables", "items");
            }

            for (int i = 54; i < 467; i++)
            {
                int offset = origData[0xB4F69 + i * 2] + origData[0xB4F69 + i * 2 + 1] * 256;
                if (itemDesirability[i] > 0)
                {
                    if (inclusionType == 1)
                    {
                        // all
                        if (cursedItemIndexes.Contains(i))
                        {
                            newData[0xB4F69 + offset] = 0x0A;
                            newData[0xB4F69 + offset + 1] = 0x40;
                        }
                        else
                        {
                            newData[0xB4F69 + offset] = 0x02;
                            newData[0xB4F69 + offset + 1] = 0x00;
                        }
                        Logging.logDebug("Item #" + i + "/466 [" + itemNames[i] + "] included", "items");
                    }
                    else if (inclusionType == 2)
                    {
                        // random
                        if((r.Next() % 100) > 50)
                        {
                            if (cursedItemIndexes.Contains(i))
                            {
                                newData[0xB4F69 + offset] = 0x0A;
                                newData[0xB4F69 + offset + 1] = 0x40;
                            }
                            else
                            {
                                newData[0xB4F69 + offset] = 0x02;
                                newData[0xB4F69 + offset + 1] = 0x00;
                            }
                            Logging.logDebug("Item #" + i + "/466 [" + itemNames[i] + "] included", "items");
                        }
                        else
                        {
                            newData[0xB4F69 + offset] = 0x02;
                            newData[0xB4F69 + offset + 1] = 0x60;
                            Logging.logDebug("Item #" + i + "/466 [" + itemNames[i] + "] not included", "items");
                        }
                    }
                }
                else
                {
                    newData[0xB4F69 + offset] = 0x02;
                    newData[0xB4F69 + offset + 1] = 0x60;
                    Logging.logDebug("Item #" + i + "/466 [" + itemNames[i] + "] not included due to being useless", "items");
                }
            }
        }
    }
}
