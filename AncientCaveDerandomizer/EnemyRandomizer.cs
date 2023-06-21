using AncientCaveDerandomizer.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    public class EnemyRandomizer
    {
        // 192 of these
        private static int[] enemyDifficulty = new int[]
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, //0x
            1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1, //1x
            1,2,2,2,1,1,1,1,1,2,2,2,2,2,2,2, //2x
            2,2,2,2,2,3,2,2,2,2,2,2,2,2,3,3, //3x
            3,3,3,3,2,3,3,3,3,3,3,2,3,3,3,3, //4x
            3,5,4,4,4,3,4,4,4,4,4,4,4,5,5,4, //5x
            5,5,5,4,5,6,4,7,5,6,6,5,6,3,7,6, //6x
            3,5,4,5,1,4,5,6,4,6,5,6,5,5,5,7, //7x
            6,6,6,6,4,6,7,6,6,7,7,7,7,3,7,7, //8x
            7,8,8,7,7,7,7,7,7,7,7,7,8,8,7,8, //9x
            7,7,4,7,9,8,7,8,8,8,8,8,8,8,8,8, //ax
            7,8,9,9,8,9,9,9,9,9,9,9,9,9,6,9  //bx
        };
        // 0 for never include (sprites fucked up), 1 for not normally included, 2 for included
        private static int[] enemyInclusion = new int[]
        {
            2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1, //0x
            2,2,2,2,2,1,2,1,2,2,1,1,2,1,2,1, //1x
            2,2,2,2,2,2,2,1,2,2,2,2,1,1,1,1, //2x
            2,1,1,1,2,2,2,2,0,2,1,1,2,2,1,2, //3x
            1,2,1,2,2,1,1,2,2,1,1,2,2,2,2,2, //4x
            2,2,1,2,1,2,2,2,2,2,2,2,2,2,2,2, //5x
            2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2, //6x
            2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2, //7x
            2,2,2,2,2,2,2,2,2,2,2,2,0,2,0,2, //8x
            0,2,0,2,0,0,0,0,0,0,0,2,2,2,2,2, //9x
            2,2,2,2,2,0,2,2,2,2,2,2,2,2,2,2, //ax
            2,2,2,2,2,2,2,2,2,0,0,0,0,0,2,2, //bx
        };

        private static byte[] coreIds = new byte[] { 0x74, 0x78, 0x7C, 0x7F };
 
        public static void randomizeEnemies(byte[] origRom, byte[] newRom, int coresType, int difficulty, bool conceal, int bottomFloor, Random r)
        {
            if (coresType == 0 && bottomFloor == 99 && difficulty == 5 && !conceal)
            {
                Logging.logDebug("No changes to enemies", "enemies");
                // no changes! don't do shit
                return;
            }

            if (conceal)
            {
                Logging.logDebug("Replacing enemy sprites at 0xA5DF6 with 0x91", "enemies");
                for (int i = 0; i < 0xC0; i++)
                {
                    // i think this also dictates how the enemy behaves, ie chases/runs away/speed
                    // so maybe base this on the difficulty rating?
                    newRom[0xA5DF6 + i] = 0x91;
                }
            }

            // a595c
            // only cores
            if (coresType == 3)
            {
                int threshold1 = (int)(bottomFloor * 1.8);
                int threshold2 = (int)(bottomFloor * 1.5);
                int threshold3 = (int)(bottomFloor * 1.2);
                Logging.logDebug("Setting enemies to cores only; bottom floor = " + bottomFloor + "; thresholds = " + threshold1 + "," + threshold2 + "," + threshold3, "enemies");

                for (int i = 0; i < 202; i++)
                {
                    // A595C
                    // At floor 1, (i=0)          range 200 -> 300
                    // At floor 10 (i~20)         range 180  -> 280
                    // At floor 100 (i near 200), range 0 -> 100
                    // 0x74 red core [80+]
                    // 0x78 blue core [20-79]
                    // 0x7C green core [-40-20]
                    // 0x7F no core [<-40]
                    int result = ((r.Next() % bottomFloor) + bottomFloor * 2) - i;
                    Logging.logDebug("Enemy #" + i + "/201 randomized core result value = " + result, "enemies");
                    if (result > threshold1) // 
                    {
                        Logging.logDebug("Enemy #" + i + "/201 = red core [x74]", "enemies");
                        newRom[0xA595C + i] = coreIds[0];
                    }
                    else if (result > threshold2)
                    {
                        Logging.logDebug("Enemy #" + i + "/201 = blue core [x78]", "enemies");
                        newRom[0xA595C + i] = coreIds[1];
                    }
                    else if (result > threshold3)
                    {
                        Logging.logDebug("Enemy #" + i + "/201 = green core [x7C]", "enemies");
                        newRom[0xA595C + i] = coreIds[2];
                    }
                    else
                    {
                        Logging.logDebug("Enemy #" + i + "/201 = no core [x7F]", "enemies");
                        newRom[0xA595C + i] = coreIds[3];
                    }
                }
            }
            else
            {
                byte[] originalEnemies = new byte[202];
                for (int i = 0; i < 202; i++)
                {
                    originalEnemies[i] = origRom[0xA595C + i];
                    Logging.logDebug("Original Enemy #" + i + "/201 = " + originalEnemies[i], "enemies");
                }

                byte[] newEnemies = new byte[202];
                double increment = (0.5 + difficulty * .15) / (bottomFloor / 99.0);
                if(difficulty == 5 && bottomFloor == 99)
                {
                    increment = 1;
                }
                Logging.logDebug("Enemy increment amount = " + increment, "enemies");
                int coresMod = (int)(10 * bottomFloor / 99.0);
                if (coresMod < 2)
                {
                    coresMod = 2;
                }
                int threshold1 = (int)(bottomFloor * 1.8);
                int threshold2 = (int)(bottomFloor * 1.5);
                int threshold3 = (int)(bottomFloor * 1.2);

                Logging.logDebug("Cores appearance value if more cores selected = every " + coresMod + "; threshold values = " + threshold1 + "," + threshold2 + "," + threshold3, "enemies");
                for (int i = 0; i < 202; i++)
                {
                    // difficulty 0  -> 0.5
                    // difficulty 10 -> 2.0
                    int targetIndex = (int)(increment * i);
                    Logging.logDebug("Enemy #" + i + "/201 changing to index: " + targetIndex, "enemies");
                    if (targetIndex > 201)
                    {
                        // repeat the final eight enemies
                        targetIndex = 201 - (targetIndex%8);
                        Logging.logDebug("Enemy #" + i + "/201 index beyond bounds, changing to " + targetIndex, "enemies");
                    }
                    newEnemies[i] = originalEnemies[targetIndex];

                    if (coresType == 2)
                    {
                        // remove cores
                        for (int j = 0; j < coreIds.Length; j++)
                        {
                            if (newEnemies[i] == coreIds[j])
                            {
                                newEnemies[i] = newEnemies[i - 1];
                                Logging.logDebug("Removing core-type enemy#" + i + "/201 [" + coreIds[j] + "]; replacing with previous enemy " + newEnemies[i - 1], "enemies");
                            }
                        }
                    }
                    if (coresType == 1 && ((i % coresMod) == 0))
                    {
                        // 0x74 red core [80+]
                        // 0x78 blue core [20-79]
                        // 0x7C green core [-40-20]
                        // 0x7F no core [<-40]
                        int result = ((r.Next() % bottomFloor) + bottomFloor * 2) - i;
                        Logging.logDebug("Enemy #" + i + "/201 randomized core result value = " + result, "enemies");
                        if (result > threshold1) // 
                        {
                            Logging.logDebug("Injecting additional cores; replacing enemy#" + i + "/201 [" + newEnemies[i] + "] with red core [0x74]", "enemies");
                            newEnemies[i] = coreIds[0];
                        }
                        else if (result > threshold2)
                        {
                            Logging.logDebug("Injecting additional cores; replacing enemy#" + i + "/201 [" + newEnemies[i] + "] with blue core [0x78]", "enemies");
                            newEnemies[i] = coreIds[1];
                        }
                        else if (result > threshold3)
                        {
                            Logging.logDebug("Injecting additional cores; replacing enemy#" + i + "/201 [" + newEnemies[i] + "] with green core [0x7C]", "enemies");
                            newEnemies[i] = coreIds[2];
                        }
                        else
                        {
                            Logging.logDebug("Injecting additional cores; replacing enemy#" + i + "/201 [" + newEnemies[i] + "] with no core [0x7F]", "enemies");
                            newEnemies[i] = coreIds[3];
                        }
                    }
                }

                // stick it back
                for (int i = 0; i < 202; i++)
                {
                    newRom[0xA595C + i] = newEnemies[i];
                }
            }
        }

        public static void setNumberOfEnemies(byte[] rom, int sliderValue)
        {
            // default 04
            byte[] enemyValues = new byte[] { 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x06, 0x08, 0x10, 0x20, 0x7F };
            if(sliderValue != 5)
            {
                Logging.logDebug("Setting enemy appearance value to " + enemyValues[sliderValue], "enemies");
                rom[0x1951C] = enemyValues[sliderValue];
            }
        }

        public static void setJellyHealth(byte[] rom, int jellyHp)
        {
            Logging.logDebug("Changing Jelly HP from 9980 to " + jellyHp, "enemies");
            rom[0xB4F02] = (byte)jellyHp;
            rom[0xB4F03] = (byte)(jellyHp >> 8);
        }
    }
}
