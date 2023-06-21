using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    // i don't actually remember whether any of this stuff really works, it should probably be checked out/fixed
    public class LayoutRandomizer
    {
        public static void randomizeRoomSize(byte[] rom, Random r, ref int workingOffset)
        {
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int valueOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);
            for(int i=0; i < 100; i++)
            {
                byte val = (byte)((r.Next() % 10) - 5);
                rom[workingOffset++] = val;
            }

            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int codeOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            // hijack 83/9e0c to add random -5 -> 5, cap to zero
            // generate table of 99 values; x is corruptable, note we're in 8bit accum mode and 16bit x/y
            rom[0x19E0C] = 0x22;
            rom[0x19E0D] = (byte)codeOffset_lorom;
            rom[0x19E0E] = (byte)(codeOffset_lorom >> 8);
            rom[0x19E0F] = (byte)(codeOffset_lorom >> 16);

            // original code first
            rom[workingOffset++] = 0x18; // CLC

            rom[workingOffset++] = 0x65; // ADC $54
            rom[workingOffset++] = 0x54;

            rom[workingOffset++] = 0x4A; // LSR

            // PHA
            rom[workingOffset++] = 0x48;

            // REP 20 - 16bit A
            rom[workingOffset++] = 0xC2;
            rom[workingOffset++] = 0x20;

            // AF 96 E6 7F   LDA $7FE696  -- load floor number
            rom[workingOffset++] = 0xAF;
            rom[workingOffset++] = 0x96;
            rom[workingOffset++] = 0xE6;
            rom[workingOffset++] = 0x7F;

            // AND #00FF
            rom[workingOffset++] = 0x29;
            rom[workingOffset++] = 0xFF;
            rom[workingOffset++] = 0x00;

            // AA            TAX
            rom[workingOffset++] = 0xAA;

            // SEP 20 - 8bit A
            rom[workingOffset++] = 0xE2;
            rom[workingOffset++] = 0x20;

            // PLA
            rom[workingOffset++] = 0x68;

            // CLC
            rom[workingOffset++] = 0x18;

            // 7F 00 C0 D0   ADC $D0C000,x
            rom[workingOffset++] = 0x7F;
            rom[workingOffset++] = (byte)valueOffset_lorom;
            rom[workingOffset++] = (byte)(valueOffset_lorom >> 8);
            rom[workingOffset++] = (byte)(valueOffset_lorom >> 16);

            // BPL xx
            rom[workingOffset++] = 0x10;
            rom[workingOffset++] = 0x02;

            // LDA #$00
            rom[workingOffset++] = 0xA9;
            rom[workingOffset++] = 0x00;

            // RTL
            rom[workingOffset++] = 0x6B;

            // so let's also replace this:
            //$83 / 9213 C9 09       CMP #$09                A:0002 X:1014 Y:0010 P:envMxdIzc
            //$83 / 9215 B0 30       BCS $30[$9247]      A: 0002 X: 1014 Y: 0010 P: eNvMxdIzc
            // with a thing that sets A to 8 if it's >= 9
            // FA83DD43C05BDDB7 2nd Floor

            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int codeOffset2_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            rom[0x19213] = 0x22;
            rom[0x19214] = (byte)codeOffset2_lorom;
            rom[0x19215] = (byte)(codeOffset2_lorom >> 8);
            rom[0x19216] = (byte)(codeOffset2_lorom >> 16);

            // CMP 09
            rom[workingOffset++] = 0xC9;
            rom[workingOffset++] = 0x09;

            // BCC 02
            rom[workingOffset++] = 0x90;
            rom[workingOffset++] = 0x02;

            // LDA #08
            rom[workingOffset++] = 0xA9;
            rom[workingOffset++] = 0x08;

            // RTL
            rom[workingOffset++] = 0x6B;
        }

        public static void manualRoomSize(byte[] origData, byte[] newData, int size)
        {
            // like above except instead of table we use fixed value

            // looks like i never finished this when this was originally made
            // i'll disable the radio button for it for now
        }

        public static void randomizeRoomNum(byte[] origData, byte[] newData, Random r, ref int workingOffset)
        {
            // default 0x08
            byte[] values = new byte[] { 0x02, 0x03, 0x04, 0x05, 0x06, 0x08, 0x0A, 0x0D, 0x12, 0x18, 0x30 };

            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int valueOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);
            for (int i = 0; i < 100; i++)
            {
                byte val = values[r.Next() % values.Length];
                newData[workingOffset++] = val;
            }

            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int codeOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(workingOffset);

            // generate table of 99 values; x is corruptable, note we're in 8bit accum mode and 16bit x/y
            newData[0x1923E] = 0x22;
            newData[0x1923F] = (byte)codeOffset_lorom;
            newData[0x19240] = (byte)(codeOffset_lorom >> 8);
            newData[0x19241] = (byte)(codeOffset_lorom >> 16);

            // replacing: 
            // $83/923E 85 54       STA $54    [$00:0054]   A:0002 X:0062 Y:0010 P:envMxdIzC
            // $83/9240 C9 08       CMP #$08                A:0002 X:0062 Y:0010 P:envMxdIzC

            newData[workingOffset++] = 0x85; // old code
            newData[workingOffset++] = 0x54;

            // REP 20 - 16bit A
            newData[workingOffset++] = 0xC2;
            newData[workingOffset++] = 0x20;

            // AF 96 E6 7F   LDA $7FE696  -- load floor number
            newData[workingOffset++] = 0xAF;
            newData[workingOffset++] = 0x96;
            newData[workingOffset++] = 0xE6;
            newData[workingOffset++] = 0x7F;

            // AND #00FF
            newData[workingOffset++] = 0x29;
            newData[workingOffset++] = 0xFF;
            newData[workingOffset++] = 0x00;

            // AA            TAX
            newData[workingOffset++] = 0xAA;

            // SEP 20 - 8bit A
            newData[workingOffset++] = 0xE2;
            newData[workingOffset++] = 0x20;

            // LDA $56
            newData[workingOffset++] = 0xA5;
            newData[workingOffset++] = 0x56;

            // CMP table,x
            newData[workingOffset++] = 0xDF;
            newData[workingOffset++] = (byte)valueOffset_lorom;
            newData[workingOffset++] = (byte)(valueOffset_lorom >> 8);
            newData[workingOffset++] = (byte)(valueOffset_lorom >> 16);

            // RTL
            newData[workingOffset++] = 0x6B;
        }

        public static void manualRoomNum(byte[] origData, byte[] newData, int num)
        {
            // looks like i never finished this when this was originally made
            // i'll disable the radio button for it for now
        }
    }
}
