using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AncientCaveDerandomizer
{
    // this was taken straight from mana rando years ago and is super out of date
    public class OptionsManager
    {
        Dictionary<int, object> options = new Dictionary<int, object>();
        int totalSize = 0;
        public void addCheckbox(CheckBox c)
        {
            options[totalSize] = c;
            totalSize++;
        }

        public void addRadioButtonGroup(RadioButton[] buttons)
        {
            int numBitsNeeded = (int)Math.Ceiling(Math.Log(buttons.Length, 2));
            options[totalSize] = buttons;
            totalSize += numBitsNeeded;
        }

        public void addTrackbar(TrackBar bar)
        {
            int numBitsNeeded = (int)Math.Ceiling(Math.Log(bar.Maximum, 2));
            options[totalSize] = bar;
            totalSize += numBitsNeeded;
        }

        public void addNumericUpDown(NumericUpDown upDown)
        {
            int numBitsNeeded = (int)Math.Ceiling(Math.Log((double)upDown.Maximum, 2));
            options[totalSize] = upDown;
            totalSize += numBitsNeeded;
        }


        public string getOptionsString()
        {
            if(!options.ContainsKey(0))
            {
                return "";
            }

            byte[] allBytes = new byte[(int)(Math.Ceiling((totalSize / 8.0) + 1))];

            int currentBit = 0;
            object currentControl = options[currentBit];
            while(currentControl != null)
            {
                int currentByte = currentBit / 8;
                if(currentControl is CheckBox)
                {
                    if(((CheckBox)currentControl).Checked)
                    {
                        allBytes[currentByte] |= (byte)((1 << (currentBit % 8)));
                    }
                    else
                    {
                        allBytes[currentByte] &= (byte)~((1 << (currentBit % 8)));
                    }
                    currentBit++;
                }
                else if(currentControl is RadioButton[])
                {
                    RadioButton[] buttons = (RadioButton[])currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log(buttons.Length, 2));
                    for(int i=0; i < buttons.Length; i++)
                    {
                        if(buttons[i].Checked)
                        {
                            int value = i;
                            for(int j=numBitsNeeded - 1; j >= 0; j--)
                            {
                                currentByte = currentBit / 8;
                                if ((value & (1 << j)) > 0)
                                {
                                    allBytes[currentByte] |= (byte)((1 << (currentBit % 8)));
                                }
                                else
                                {
                                    allBytes[currentByte] &= (byte)~((1 << (currentBit % 8)));
                                }
                                currentBit++;
                            }
                            break;
                        }
                    }
                }
                else if(currentControl is TrackBar)
                {
                    TrackBar bar = (TrackBar)currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log(bar.Maximum, 2));
                    int value = bar.Value;
                    for (int j = numBitsNeeded - 1; j >= 0; j--)
                    {
                        currentByte = currentBit / 8;
                        if ((value & (1 << j)) > 0)
                        {
                            allBytes[currentByte] |= (byte)((1 << (currentBit % 8)));
                        }
                        else
                        {
                            allBytes[currentByte] &= (byte)~((1 << (currentBit % 8)));
                        }
                        currentBit++;
                    }
                }
                else if(currentControl is NumericUpDown)
                {
                    NumericUpDown upDown = (NumericUpDown)currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log((double)upDown.Maximum, 2));
                    int value = (int)upDown.Value;
                    for (int j = numBitsNeeded - 1; j >= 0; j--)
                    {
                        currentByte = currentBit / 8;
                        if ((value & (1 << j)) > 0)
                        {
                            allBytes[currentByte] |= (byte)((1 << (currentBit % 8)));
                        }
                        else
                        {
                            allBytes[currentByte] &= (byte)~((1 << (currentBit % 8)));
                        }
                        currentBit++;
                    }
                }
                currentControl = options.ContainsKey(currentBit) ? options[currentBit] : null;
            }

            string result = "";
            foreach(byte b in allBytes)
            {
                result += b.ToString("X2");
            }
            return result;
        }

        public void setOptions(string enteredString)
        {
            if (!options.ContainsKey(0))
            {
                return;
            }

            byte[] allBytes = new byte[(int)(Math.Ceiling((totalSize / 8.0) + 1))];
            for(int i=0; i < allBytes.Length; i++)
            {
                string hexString = enteredString.Substring(i * 2, 2);
                allBytes[i] = Byte.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            int currentBit = 0;
            object currentControl = options[currentBit];
            while (currentControl != null)
            {
                int currentByte = currentBit / 8;
                if (currentControl is CheckBox)
                {
                    ((CheckBox)currentControl).Checked = (allBytes[currentByte] & (byte)((1 << (currentBit % 8)))) > 0;
                    currentBit++;
                }
                else if (currentControl is RadioButton[])
                {
                    RadioButton[] buttons = (RadioButton[])currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log(buttons.Length, 2));
                    int value = 0;
                    for (int j = numBitsNeeded - 1; j >= 0; j--)
                    {
                        currentByte = currentBit / 8;
                        if ((allBytes[currentByte] & (byte)((1 << (currentBit % 8)))) > 0)
                        {
                            value |= (1 << j);
                        }
                        currentBit++;
                    }

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].Checked = i == value;
                    }
                }
                else if (currentControl is TrackBar)
                {
                    TrackBar bar = (TrackBar)currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log(bar.Maximum, 2));
                    int value = 0;
                    for (int j = numBitsNeeded - 1; j >= 0; j--)
                    {
                        currentByte = currentBit / 8;
                        if((allBytes[currentByte] & (byte)((1 << (currentBit % 8)))) > 0)
                        {
                            value |= (1 << j);
                        }
                        currentBit++;
                    }
                    bar.Value = value > bar.Maximum ? bar.Maximum : value < bar.Minimum ? bar.Minimum : value;
                }
                else if (currentControl is NumericUpDown)
                {
                    NumericUpDown upDown = (NumericUpDown)currentControl;
                    int numBitsNeeded = (int)Math.Ceiling(Math.Log((double)upDown.Maximum, 2));
                    int value = 0;
                    for (int j = numBitsNeeded - 1; j >= 0; j--)
                    {
                        currentByte = currentBit / 8;
                        if ((allBytes[currentByte] & (byte)((1 << (currentBit % 8)))) > 0)
                        {
                            value |= (1 << j);
                        }
                        currentBit++;
                    }
                    upDown.Value = value > upDown.Maximum ? upDown.Maximum : value < upDown.Minimum ? upDown.Minimum : value;
                }
                currentControl = options.ContainsKey(currentBit) ? options[currentBit] : null;
            }
        }
    }

}
