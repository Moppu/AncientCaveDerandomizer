using AncientCaveDerandomizer.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    public class ThemeRandomizer
    {
        private class Tileset
        {
            public Tileset(ushort t1, ushort t2, ushort t3, ushort p1, ushort p2, uint o, byte b)
            {
                tilesetvalue1 = t1;
                tilesetvalue2 = t2;
                tilesetvalue3 = t3;
                palettevalue1 = p1;
                palettevalue2 = p2;
                offsetThingy = o;
                battleBg = b;
            }
            // 19f49
            public ushort tilesetvalue1;
            // 19f5f
            public ushort tilesetvalue2;
            // 19f59
            public ushort tilesetvalue3;
            // 19f6b
            public ushort palettevalue1;
            // 19f7f
            public ushort palettevalue2;
            // 19f73, 24 bit
            public uint offsetThingy;
            // 2bcd, byte
            public byte battleBg;
        }

        public static void randomizeThemes(byte[] origData, byte[] newData, Random r, ref int workingOffset)
        {
            Tileset[] tilesets = new Tileset[]
            {
                new Tileset(0x13b, 0x156, 0x169, 0, 0x1E, 0x94D54E, 0x17),
                new Tileset(0x139, 0x154, 0x16a, 0x8EE0, 0x12, 0x94D2BF, 0x09),
                new Tileset(0x134, 0x14f, 0x16b, 0x8BD0, 6, 0x94D61F, 0x01),
            };

            byte[] musicChoices = new byte[] { 0x01, 0x05, 0x08, 0x17, 0x09, 0x0A, 0x18, 0x19, 0x1B, 0x38 };

            // change the divisor to 1, one theme per floor
            newData[0x19E4E] = 0x01;

            Logging.logDebug("Randomizing themes", "randomization");
            // inject new tables in expanded section
            int musicNum = musicChoices[r.Next() % musicChoices.Length];
            // 8bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int indexOffset = workingOffset;
            workingOffset += 100;
            int indexOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(indexOffset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int musicOffset = workingOffset;
            workingOffset += 200;
            int musicOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(musicOffset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int t1Offset = workingOffset;
            workingOffset += 200;
            int t1Offset_lorom = CodeGenerationUtils.fileOffsetToLoRom(t1Offset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int t2Offset = workingOffset;
            workingOffset += 200;
            int t2Offset_lorom = CodeGenerationUtils.fileOffsetToLoRom(t2Offset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int t3Offset = workingOffset;
            workingOffset += 200;
            int t3Offset_lorom = CodeGenerationUtils.fileOffsetToLoRom(t3Offset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int p1Offset = workingOffset;
            workingOffset += 200;
            int p1Offset_lorom = CodeGenerationUtils.fileOffsetToLoRom(p1Offset);
            // 16bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 200);
            int p2Offset = workingOffset;
            workingOffset += 200;
            int p2Offset_lorom = CodeGenerationUtils.fileOffsetToLoRom(p2Offset);
            // 24bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 300);
            int oOffset = workingOffset;
            workingOffset += 300;
            int oOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(oOffset);
            // 8bit * 100
            CodeGenerationUtils.ensureSpaceInBank(ref workingOffset, 100);
            int bgOffset = workingOffset;
            workingOffset += 100;
            int bgOffset_lorom = CodeGenerationUtils.fileOffsetToLoRom(bgOffset);

            for (int i = 0; i < 100; i++)
            {
                // index table from 19F4F .. now with one for each floor instead of one for every ten
                newData[indexOffset + i] = (byte)i;
                int tilesetIndex = r.Next() % tilesets.Length;

                Logging.logDebug("Floor #" + i + " tileset index = " + tilesetIndex + " music = " + musicNum, "randomization");
                // music
                newData[musicOffset + i * 2] = (byte)musicNum;
                newData[musicOffset + i * 2 + 1] = 0;

                // t1
                newData[t1Offset + i * 2] = (byte)tilesets[tilesetIndex].tilesetvalue1;
                newData[t1Offset + i * 2 + 1] = (byte)(tilesets[tilesetIndex].tilesetvalue1>>8);
                // t2
                newData[t2Offset + i * 2] = (byte)tilesets[tilesetIndex].tilesetvalue2;
                newData[t2Offset + i * 2 + 1] = (byte)(tilesets[tilesetIndex].tilesetvalue2 >> 8);
                // t3
                newData[t3Offset + i * 2] = (byte)tilesets[tilesetIndex].tilesetvalue3;
                newData[t3Offset + i * 2 + 1] = (byte)(tilesets[tilesetIndex].tilesetvalue3 >> 8);
                // p1
                newData[p1Offset + i * 2] = (byte)tilesets[tilesetIndex].palettevalue1;
                newData[p1Offset + i * 2 + 1] = (byte)(tilesets[tilesetIndex].palettevalue1 >> 8);
                // p2
                newData[p2Offset + i * 2] = (byte)tilesets[tilesetIndex].palettevalue2;
                newData[p2Offset + i * 2 + 1] = (byte)(tilesets[tilesetIndex].palettevalue2 >> 8);

                // offset thingy
                newData[oOffset + i * 3] = (byte)tilesets[tilesetIndex].offsetThingy;
                newData[oOffset + i * 3 + 1] = (byte)(tilesets[tilesetIndex].offsetThingy >> 8);
                newData[oOffset + i * 3 + 2] = (byte)(tilesets[tilesetIndex].offsetThingy >> 16);

                newData[bgOffset + i] = tilesets[tilesetIndex].battleBg;

                // change music every now and then
                if ((r.Next() % 5) == 0)
                {
                    musicNum = musicChoices[r.Next() % musicChoices.Length];
                    Logging.logDebug("Floor #" + i + " changing music to " + musicNum, "randomization");
                }
            }

            // TO REMOVE CHEST FANFARE MUSIC:
            // four EAs at 7426E
            newData[0x7426E] = 0xEA;
            newData[0x7426F] = 0xEA;
            newData[0x74270] = 0xEA;
            newData[0x74271] = 0xEA;
            // four EAs at 742BC
            newData[0x742BC] = 0xEA;
            newData[0x742BD] = 0xEA;
            newData[0x742BE] = 0xEA;
            newData[0x742BF] = 0xEA;
            // this makes for fewer music restarts, and it sounds better overall

            // now point to the new data

            // index table
            newData[0x19eae] = (byte)indexOffset_lorom;
            newData[0x19eaf] = (byte)(indexOffset_lorom >> 8);
            newData[0x19eb0] = (byte)(indexOffset_lorom >> 16);

            // music
            newData[0x19eba] = (byte)musicOffset_lorom;
            newData[0x19ebb] = (byte)(musicOffset_lorom >> 8);
            newData[0x19ebc] = (byte)(musicOffset_lorom >> 16);

            // t1
            newData[0x19ec0] = (byte)t1Offset_lorom;
            newData[0x19ec1] = (byte)(t1Offset_lorom >> 8);
            newData[0x19ec2] = (byte)(t1Offset_lorom >> 16);

            // t2
            newData[0x19ec8] = (byte)t2Offset_lorom;
            newData[0x19ec9] = (byte)(t2Offset_lorom >> 8);
            newData[0x19eca] = (byte)(t2Offset_lorom >> 16);

            // t3
            newData[0x19ed0] = (byte)t3Offset_lorom;
            newData[0x19ed1] = (byte)(t3Offset_lorom >> 8);
            newData[0x19ed2] = (byte)(t3Offset_lorom >> 16);

            // p1
            newData[0x19ed8] = (byte)p1Offset_lorom;
            newData[0x19ed9] = (byte)(p1Offset_lorom >> 8);
            newData[0x19eda] = (byte)(p1Offset_lorom >> 16);

            // p2
            newData[0x19ee0] = (byte)p2Offset_lorom;
            newData[0x19ee1] = (byte)(p2Offset_lorom >> 8);
            newData[0x19ee2] = (byte)(p2Offset_lorom >> 16);

            // o
            newData[0x19f0f] = (byte)oOffset_lorom;
            newData[0x19f10] = (byte)(oOffset_lorom >> 8);
            newData[0x19f11] = (byte)(oOffset_lorom >> 16);

            newData[0x19f19] = (byte)(oOffset_lorom + 2);
            newData[0x19f1a] = (byte)((oOffset_lorom + 2) >> 8);
            newData[0x19f1b] = (byte)((oOffset_lorom + 2) >> 16);

            // battle bg
            newData[0x2bc1] = (byte)bgOffset_lorom;
            newData[0x2bc2] = (byte)(bgOffset_lorom >> 8);
            newData[0x2bc3] = (byte)(bgOffset_lorom >> 16);

        }
    }
}
