using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientCaveDerandomizer
{
    public class CodeGenerationUtils
    {
        /// <summary>
        /// Make sure we don't overrun bank boundaries
        /// </summary>
        /// <param name="newCodeOffset"></param>
        /// <param name="numBytes"></param>
        public static bool ensureSpaceInBank(ref int newCodeOffset, int numBytes)
        {
            byte bank1 = (byte)(newCodeOffset >> 16);
            byte bank2 = (byte)((newCodeOffset + numBytes) >> 16);
            if (bank1 != bank2)
            {
                newCodeOffset = (bank2 << 16);
                return true;
            }
            return false;
        }

        public static int fileOffsetToLoRom(int fileOffset)
        {
            // ROM offset -> Lufia's weird MSB-set LoROM offsets
            int bank = ((fileOffset >> 16) * 2) | 0x80;
            int shortOffset = fileOffset & 0xFFFF;
            if (shortOffset >= 0x8000)
            {
                bank++;
            }
            else
            {
                shortOffset += 0x8000;
            }
            return (bank << 16) + shortOffset;
        }

    }
}
