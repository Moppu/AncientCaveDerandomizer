using AncientCaveDerandomizer.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    public class MiscCodeChanges
    {
        public static void setNumFloors(byte[] rom, byte numFloors)
        {
            Logging.logDebug("Changing number of floors to " + numFloors, "misc");
            rom[0x19E82] = numFloors;
        }

        public static void setStartingLevels(byte[] rom, byte startingLevel)
        {
            Logging.logDebug("Changing starting levels to " + startingLevel, "misc");
            rom[0x2b395] = startingLevel;
            rom[0x2b3b0] = startingLevel;
            rom[0x2b3cb] = startingLevel;
            rom[0x2b3e6] = startingLevel;
            rom[0x2b401] = startingLevel;
            rom[0x2b41c] = startingLevel;
            rom[0x2b437] = startingLevel;
        }

        public static void setHealingTileChances(byte[] rom, byte chancesIn255)
        {
            rom[0x19477] = chancesIn255;
        }

        public static void startWithProvidence(byte[] rom, ref int workingOffset)
        {
            // "Start with providence" option
            // here's where it gives you potions
            // $84 / 88BB A2 02 14    LDX #$1402              A:FFFF X:0041 Y:000F P:envmxdIZC
            // $84 / 88BE 8E 8D 0A    STX $0A8D[$84:0A8D]     A:FFFF X:1402 Y:000F P:envmxdIzC

            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 0x20); // estimate
            int providenceCodeOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            // Replace with subroutine
            rom[0x208BB] = 0x22;
            rom[0x208BC] = (byte)providenceCodeOffset_lorom;
            rom[0x208BD] = (byte)(providenceCodeOffset_lorom >> 8);
            rom[0x208BE] = (byte)(providenceCodeOffset_lorom >> 16);
            rom[0x208BF] = 0xEA;
            rom[0x208C0] = 0xEA;

            // Do the original 10 potions thing, then providence, then 6B=RTL
            rom[workingOffset++] = 0xA2;
            rom[workingOffset++] = 0x02;
            rom[workingOffset++] = 0x14;
            rom[workingOffset++] = 0x8E;
            rom[workingOffset++] = 0x8D;
            rom[workingOffset++] = 0x0A;

            rom[workingOffset++] = 0xA2;
            rom[workingOffset++] = 0x2D;
            rom[workingOffset++] = 0x02;
            rom[workingOffset++] = 0x8E;
            rom[workingOffset++] = 0x8F;
            rom[workingOffset++] = 0x0A;

            rom[workingOffset++] = 0x6B;
        }

        public static void setDialogueWatermark(byte[] rom, byte[] watermark)
        {
            for (int i = 0; i < watermark.Length; i++)
            {
                rom[0x56AA8 + i] = watermark[i];
            }
        }

        public static void randomizeParty(byte[] rom, Random r)
        {
            Logging.logDebug("Randomizing party.", "misc");
            int[] charIds = new[] { 0, 1, 2, 3, 4, 5, 6 };
            List<int> idsList = charIds.OfType<int>().ToList();
            rom[0x2b2a1] = 4;
            for (int i = 0; i < 4; i++)
            {
                int id = r.Next() % idsList.Count;
                Logging.logDebug("Setting character " + i + " to " + idsList[id], "misc");
                rom[0x2b2a2 + i] = (byte)idsList[id];
                idsList.RemoveAt(id);
            }
        }

        public static void derandomize(byte[] rom, ref int workingOffset, Random r)
        {
            Logging.logDebug("Injecting code to do manual randomization of Ancient Cave", "misc");

            // first, random bytes for frame counter $40 replacement
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int frameCounterReplacementOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            for (int i = 0; i < 100; i++)
            {
                rom[workingOffset++] = (byte)(r.Next() & 0xFF);
                Logging.logDebug("7E0040 Replacement #" + i + " = " + rom[workingOffset - 1], "misc");
            }

            // then, random bytes for 520-557 block; shift forward one byte per floor
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 158);
            int rngReplacementOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            for (int i = 0; i < 158; i++)
            {
                rom[workingOffset++] = (byte)(r.Next() & 0xFF);
                Logging.logDebug("7E05xx Replacement #" + i + " = " + rom[workingOffset - 1], "misc");
            }

            // now, replace the main subr to call out to our other two
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 0x100); // estimate
            int derandomizeCodeOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            // 1B26D: new subroutine call; replaces 22 31 9E 83 -> 22 [D08000]
            rom[0x1B26D] = 0x22;
            rom[0x1B26E] = (byte)derandomizeCodeOffset_lorom;
            rom[0x1B26F] = (byte)(derandomizeCodeOffset_lorom >> 8);
            rom[0x1B270] = (byte)(derandomizeCodeOffset_lorom >> 16);

            // AF 96 E6 7F   LDA $7FE696  -- load floor number
            rom[workingOffset++] = 0xAF;
            rom[workingOffset++] = 0x96;
            rom[workingOffset++] = 0xE6;
            rom[workingOffset++] = 0x7F;

            // AA            TAX
            rom[workingOffset++] = 0xAA;

            // BF 00 C0 D0   LDA $D0C000,x
            rom[workingOffset++] = 0xBF;
            rom[workingOffset++] = (byte)frameCounterReplacementOffset_lorom;
            rom[workingOffset++] = (byte)(frameCounterReplacementOffset_lorom >> 8);
            rom[workingOffset++] = (byte)(frameCounterReplacementOffset_lorom >> 16);

            // 85 40         STA $40
            rom[workingOffset++] = 0x85;
            rom[workingOffset++] = 0x40;

            // A0 00 00      LDY #$00
            rom[workingOffset++] = 0xA0;
            rom[workingOffset++] = 0x00;
            rom[workingOffset++] = 0x00;

            // ***:
            // BF 00 C1 D0   LDA $D0C100,x
            rom[workingOffset++] = 0xBF;
            rom[workingOffset++] = (byte)rngReplacementOffset_lorom;
            rom[workingOffset++] = (byte)(rngReplacementOffset_lorom >> 8);
            rom[workingOffset++] = (byte)(rngReplacementOffset_lorom >> 16);

            // 99 20 05      STA $0520,y
            rom[workingOffset++] = 0x99;
            rom[workingOffset++] = 0x20;
            rom[workingOffset++] = 0x05;

            // E8            INX
            rom[workingOffset++] = 0xE8;

            // C8            INY
            rom[workingOffset++] = 0xC8;

            // C0 39 00      CMP #$39
            rom[workingOffset++] = 0xC0;
            rom[workingOffset++] = 0x3A;
            rom[workingOffset++] = 0x00;

            // 90 F2         BCC ^ *** $F2?
            rom[workingOffset++] = 0x90;
            rom[workingOffset++] = 0xF2;

            // 22 31 9E 83   JSL $839E31    -- call orig subr
            rom[workingOffset++] = 0x22;
            rom[workingOffset++] = 0x31;
            rom[workingOffset++] = 0x9E;
            rom[workingOffset++] = 0x83;

            // 6B            RTL
            rom[workingOffset++] = 0x6B;
        }

        public static void enableGiftMode(byte[] rom)
        {
            Logging.logDebug("Injecting code to enable Gift Mode", "misc");
            // Enable gift mode regardless of saveram state
            rom[0x1699E] = 0xA9;
            rom[0x1699F] = 0x02;
        }
    }
}
