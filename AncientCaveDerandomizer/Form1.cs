using AncientCaveDerandomizer.logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// ~~~ things to do still:
// * replace free space allocator thing with running offset
// * keep settings (vanilla rom location) in appdata
// * move some code stuff out of form1
// - text options instead of hexadecimal ones, like we did for mana rando
namespace AncientCaveDerandomizer
{
    public partial class Form1 : Form
    {
        private bool showCustom = false;
        private HelpForm helpForm = null;
        private OptionsManager optionsManager = new OptionsManager();
        private byte[] watermarkCharSelect = new byte[] { 0x05, 0x50, 0x20, 0x88, 0x20, 0x03, 0x7B, 0x44, 0x65, 0x72, 0x61, 0x6E, 0x64, 0x6F, 0x6D, 0x69, 0x7A, 0x65, 0x64, 0x7D, 0x03, 0x05, 0x5D, 0x20, 0x43, 0x85, 0x21, 0x01, 0x1A, 0xC4, 0x00 };
        private byte[] watermarkNoCharSelect = new byte[] { 0x05, 0x50, 0x20, 0x88, 0x20, 0x03, 0x7B, 0x44, 0x65, 0x72, 0x61, 0x6E, 0x64, 0x6F, 0x6D, 0x69, 0x7A, 0x65, 0x64, 0x7D, 0x03, 0x05, 0x5D, 0x20, 0x43, 0x85, 0x21, 0x01, 0x1A, 0x00 };
        public Form1()
        {
            InitializeComponent();
            updateSize();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox2.TextChanged += TextBox2_TextChanged;
            textBox3.TextChanged += TextBox3_TextChanged;
            numericUpDown2.Value = 0x52;
            numericUpDown4.Value = 0x2D;
            numericUpDown5.Value = 0x1E;
            numericUpDown8.Value = 0x05;
            numericUpDown7.Value = 0x3A;
            numericUpDown6.Value = 0x24;
            radioButton8.CheckedChanged += RadioButton8_CheckedChanged;
            radioButton9.CheckedChanged += RadioButton8_CheckedChanged;
            radioButton10.CheckedChanged += RadioButton8_CheckedChanged;

            radioButton11.CheckedChanged += RadioButton11_CheckedChanged;
            radioButton12.CheckedChanged += RadioButton11_CheckedChanged;
            radioButton13.CheckedChanged += RadioButton11_CheckedChanged;

            radioButton14.CheckedChanged += RadioButton14_CheckedChanged;
            radioButton15.CheckedChanged += RadioButton14_CheckedChanged;
            radioButton16.CheckedChanged += RadioButton14_CheckedChanged;

            deriveTableWidthsFromEntries();
            ToolTip t = new ToolTip();
            t.SetToolTip(textBox1, "Please only use \"Lufia II - Rise of the Sinistrals (U).smc\".\nCountry: US\nVersion: 1.0\nSize: 20 Megabit (2.5 MB)");
            t.SetToolTip(pictureBox1, "Version 1.4\nBy Mop!\numokumok@gmail.com\ntwitch.tv/moppleton\n\nClick for github: https://github.com/Moppu");
            t.SetToolTip(pictureBox2, "Open the help thingy");
            t.SetToolTip(trackBar1, "Accelerate or decelerate the increase in enemy difficulty as you descend.");
            t.SetToolTip(trackBar2, "Attempts to increase or decrease the appearance of useful items in normal chests.");
            t.SetToolTip(trackBar3, "Adjust the HP of the Jelly boss on the final floor.  1 HP -> Normal HP -> Double HP.");
            t.SetToolTip(checkBox4, "Show all enemies as the same sprite when outside of a battle.");
            t.SetToolTip(checkBox3, "The primary function of this tool.  Uncheck to keep Lufia's on-the-fly randomization.");
            t.SetToolTip(checkBox2, "Randomize tilesets and music.  Adds a few new songs to the lineup, too.\nAdditionally, this disables the chest fanfare music for better musical continuity.");
            t.SetToolTip(numericUpDown1, "Decrease for a shorter Ancient Cave.");
            t.SetToolTip(checkBox5, "Enable gift mode regardless of the state of your Save RAM, so Ancient Cave can always be played.");
            t.SetToolTip(radioButton1, "No change to items you can get from chests.");
            t.SetToolTip(radioButton2, "Everything except absolutely useless stuff can be found in chests.");
            t.SetToolTip(radioButton3, "Consumables can still be found; gear may or may not be found randomly.");
            t.SetToolTip(numericUpDown3, "Characters will start at this level when entering ancient cave.");
            t.SetToolTip(checkBox8, "If selected with fewer than 99 floors, the cave will scale up such that your new bottom acts like floor 99.  Jelly HP will also be adjusted.");
            t.SetToolTip(radioButton4, "The usual amount of cores will appear.");
            t.SetToolTip(radioButton5, "More cores than normal will appear.");
            t.SetToolTip(radioButton6, "No cores will appear.");
            t.SetToolTip(radioButton7, "The only enemies you'll see are cores.");
            t.SetToolTip(trackBar5, "Increase for more healing tiles hidden under bushes (max 1 per floor), decrease for fewer.");
            t.SetToolTip(trackBar6, "Increase for more enemies walking around.  Decrease for fewer.");
            t.SetToolTip(radioButton8, "Use the standard distribution of chests.");
            t.SetToolTip(radioButton9, "Randomize the distribution of chests.");
            t.SetToolTip(radioButton10, "Specify a manual distribution of chests below; defaults are the standard values.");
            t.SetToolTip(numericUpDown2, "Increase for more weapon type chests.  Decrease for fewer.");
            t.SetToolTip(numericUpDown4, "Increase for more armor type chests.  Decrease for fewer.");
            t.SetToolTip(numericUpDown5, "Increase for more spell type chests.  Decrease for fewer.");
            t.SetToolTip(numericUpDown8, "Increase for more blue chests.  Decrease for fewer.");
            t.SetToolTip(numericUpDown7, "Increase for more chests with healing consumables.  Decrease for fewer.");
            t.SetToolTip(numericUpDown6, "Increase for more chests with misc. consumables.  Decrease for fewer.");
            t.SetToolTip(checkBox6, "In addition to the usual 10 potions, you'll get a Providence when you start Ancient Cave.");

            t.SetToolTip(label15, "Distribution of chests containing weapons.");
            t.SetToolTip(label16, "Distribution of chests containing non-weapon equips.");
            t.SetToolTip(label17, "Distribution of chests containing spells.");
            t.SetToolTip(label18, "Distribution of chests containing non-restorative consumables.");
            t.SetToolTip(label19, "Distribution of chests containing restorative consumables.");
            t.SetToolTip(label20, "Distribution of blue chests.");

            t.SetToolTip(checkBox7, "Instead of allowing party selection, use a randomly chosen party of four.");

            t.SetToolTip(textBox1, "Original Lufia 2 ROM.  Will not be modified.");
            t.SetToolTip(button1, "Browse for source ROM.");
            t.SetToolTip(textBox2, "Location of ROM to write to with chosen settings.");
            t.SetToolTip(button2, "Browse for destination ROM.");
            t.SetToolTip(textBox3, "Enter any string from which all the randomness will be seeded.");
            t.SetToolTip(button4, "Generate a random hexadecimal seed.");
            t.SetToolTip(textBox4, "An easy way of copy/pasting all the settings below.  Typing manually here is not recommended.");
            t.SetToolTip(button3, "Write to the destination ROM with the chosen settings.");
            pictureBox1.Click += PictureBox1_Click;
            pictureBox2.Click += PictureBox2_Click;

            numericUpDown2.ValueChanged += itemDistribution_ValueChanged;
            numericUpDown4.ValueChanged += itemDistribution_ValueChanged;
            numericUpDown5.ValueChanged += itemDistribution_ValueChanged;
            numericUpDown8.ValueChanged += itemDistribution_ValueChanged;
            numericUpDown7.ValueChanged += itemDistribution_ValueChanged;
            numericUpDown6.ValueChanged += itemDistribution_ValueChanged;
            randomizeSeed();
            // Hide presets tab for now, i'll fill it in later
            tabControl1.TabPages.RemoveAt(4);
            FileLogger fileLogger = new FileLogger("./log.txt");
            Logging.AddDebugLogger(fileLogger);
            Logging.debugEnabled = true;

            initCheckbox(checkBox4);
            initCheckbox(checkBox8);
            initCheckbox(checkBox5);
            initCheckbox(checkBox7);
            initCheckbox(checkBox3);
            initCheckbox(checkBox2);
            initCheckbox(checkBox6);

            initRadioButtons(new RadioButton[] { radioButton4, radioButton5, radioButton6, radioButton7 });

            initRadioButtons(new RadioButton[] { radioButton1, radioButton2, radioButton3 });

            initRadioButtons(new RadioButton[] { radioButton8, radioButton9, radioButton10 });

            // 13625
            initTrackBar(trackBar1);
            initTrackBar(trackBar3);
            initTrackBar(trackBar6);
            initTrackBar(trackBar2);
            initTrackBar(trackBar5);

            initNumericUpDown(numericUpDown2);
            initNumericUpDown(numericUpDown4);
            initNumericUpDown(numericUpDown5);
            initNumericUpDown(numericUpDown8);
            initNumericUpDown(numericUpDown7);
            initNumericUpDown(numericUpDown6);

            initNumericUpDown(numericUpDown1);
            initNumericUpDown(numericUpDown3);

            initRadioButtons(new RadioButton[] { radioButton11, radioButton12, radioButton13 });
            initRadioButtons(new RadioButton[] { radioButton14, radioButton15, radioButton16 });

            initTrackBar(trackBar4);
            initTrackBar(trackBar7);

            textBox4.Text = optionsManager.getOptionsString();

            textBox4.TextChanged += TextBox4_TextChanged;

            try
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(appDataFolder, "AC_DeRandomizer");
                string configFile = Path.Combine(specificFolder, "config.ini");

                Dictionary<string, string> configProperties = PropertyFileUtil.readFile(configFile);
                if (configProperties.ContainsKey("inRom"))
                {
                    textBox1.Text = configProperties["inRom"];
                    textBox2.Text = configProperties["inRom"];
                    processOutputFilename();
                }
            }
            catch (Exception e)
            {
                // ignore - config doesn't exist
            }

        }

        private void RadioButton14_CheckedChanged(object sender, EventArgs e)
        {
            trackBar7.Enabled = radioButton15.Checked;
        }

        private void RadioButton11_CheckedChanged(object sender, EventArgs e)
        {
            trackBar4.Enabled = radioButton12.Checked;
        }

        private bool handleOptionChanges = true;
        private bool handleIndividualChanges = true;

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if(handleOptionChanges)
            {
                handleIndividualChanges = false;
                optionsManager.setOptions(textBox4.Text);
                handleIndividualChanges = true;
            }
        }

        private void initCheckbox(CheckBox cb)
        {
            optionsManager.addCheckbox(cb);
            cb.CheckedChanged += CheckboxChange;
        }

        private void initRadioButtons(RadioButton[] rbs)
        {
            optionsManager.addRadioButtonGroup(rbs);
            foreach(RadioButton rb in rbs)
            {
                rb.CheckedChanged += CheckboxChange;
            }
        }

        private void initTrackBar(TrackBar bar)
        {
            optionsManager.addTrackbar(bar);
            bar.ValueChanged += CheckboxChange;
        }

        private void initNumericUpDown(NumericUpDown upDown)
        {
            optionsManager.addNumericUpDown(upDown);
            upDown.ValueChanged += CheckboxChange;
        }

        private void CheckboxChange(object sender, EventArgs e)
        {
            if (handleIndividualChanges)
            {
                handleOptionChanges = false;
                textBox4.Text = optionsManager.getOptionsString();
                handleOptionChanges = true;
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            if (helpForm != null)
            {
                helpForm.BringToFront();
            }
            else
            {
                helpForm = new HelpForm();
                helpForm.Icon = Icon;
                helpForm.Show();
                helpForm.FormClosing += HelpForm_FormClosing;
            }
        }

        private void HelpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            helpForm = null;
        }
        private void RadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = radioButton10.Checked;
            numericUpDown4.Enabled = radioButton10.Checked;
            numericUpDown5.Enabled = radioButton10.Checked;
            numericUpDown8.Enabled = radioButton10.Checked;
            numericUpDown7.Enabled = radioButton10.Checked;
            numericUpDown6.Enabled = radioButton10.Checked;
        }

        private void deriveTableWidthsFromEntries()
        {
            int[] values = new int[6];
            values[0] = (int)numericUpDown2.Value;
            values[1] = (int)numericUpDown4.Value;
            values[2] = (int)numericUpDown5.Value;
            values[3] = (int)numericUpDown8.Value;
            values[4] = (int)numericUpDown7.Value;
            values[5] = (int)numericUpDown6.Value;
            int sum = 0;
            for(int i=0; i < values.Length; i++)
            {
                sum += values[i];
            }
            for(int i=0; i < 6; i++)
            { 
                dataGridView1.Columns[i].Width = (int)(dataGridView1.Width * values[i] / (double)sum);
            }
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Moppu");
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            processOutputFilename();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            processOutputFilename();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("No ROM selected.");
                return;
            }
            else if(textBox1.Text == textBox2.Text)
            {
                DialogResult dialogResult = MessageBox.Show("Source and destination are the same.  Sure you want to overwrite?", "Overwrite", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else if(File.Exists(textBox2.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Destination file already exists.  Sure you want to overwrite?", "Overwrite", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            byte[] origRom = File.ReadAllBytes(textBox1.Text);
            if(origRom == null)
            {
                Logging.logDebug("Unable to read source file", "file");
                MessageBox.Show("Cannot open source file");
                return;
            }
            if(origRom.Length != 2621440 && origRom.Length != 2621440 + 0x200)
            {
                Logging.logDebug("Source file was " + origRom.Length + " bytes; expected 2621440 or that + 0x200", "file");
                MessageBox.Show("Source file had unexpected size; should be 20 Megabit ROM");
                return;
            }

            if(origRom.Length == 2621440 + 0x200)
            {
                Logging.logDebug("Found header on file", "file");
                byte[] headerLess = new byte[2621440];
                for(int i=0; i < 2621440; i++)
                {
                    headerLess[i] = origRom[i + 0x200];
                }
                origRom = headerLess;
            }
            byte[] outFile = new byte[3 * 1024 * 1024];
            for(int i=0; i < origRom.Length; i++)
            {
                outFile[i] = origRom[i];
            }

            char[] nameChars = new char[8];
            Random random = new Random();
            for (int i = 0; i < nameChars.Length; i++)
            {
                nameChars[i] = (char)origRom[i + 0x7FC0];
            }
            if(new string(nameChars) != "Lufia II")
            {
                Logging.logDebug("Unexpected game name: " + new string(nameChars) + "; expected Lufia II", "file");
                MessageBox.Show("Name of game did not match the expected \"Lufia II\"");
                return;
            }
            if(origRom[0x7FD9] != 1)
            {
                Logging.logDebug("Unexpected region: " + origRom[0x7FD9] + "; expected 1", "file");
                MessageBox.Show("Please use US ROM only.");
                return;
            }
            if (origRom[0x7FDB] != 0)
            {
                Logging.logDebug("Unexpected version: " + origRom[0x7FDB] + "; expected 0", "file");
                MessageBox.Show("Please use version 1.0 only.");
                return;
            }

            Logging.logDebug("Starting processing of file", "file");

            // where we start writing custom stuff - after all vanilla rom data
            int workingOffset = 0x280000;

            string seed = textBox3.Text;
            Random r = new Random(seed.GetHashCode());
            if (checkBox3.Checked)
            {
                MiscCodeChanges.derandomize(outFile, ref workingOffset, r);
            }

            if (checkBox5.Checked)
            {
                MiscCodeChanges.enableGiftMode(outFile);
            }

            int numFloors = (int)numericUpDown1.Value;
            if (numFloors != 99)
            {
                MiscCodeChanges.setNumFloors(outFile, (byte)numFloors);
            }

            double jellyHpScale = trackBar3.Value / 5.0;
            int jellyHp = (int)(9980 * jellyHpScale);

            // if we've adjusted the hp directly, or indirectly by scaling max floors
            if(trackBar3.Value != 5 || (checkBox8.Checked && numFloors != 99))
            {
                if(checkBox8.Checked)
                {
                    jellyHp = (int)(jellyHp * (numFloors / 99.0));
                }
                if(jellyHp < 1)
                {
                    jellyHp = 1;
                }
                if (jellyHp > 65535)
                {
                    jellyHp = 65535;
                }

                EnemyRandomizer.setJellyHealth(outFile, jellyHp);
            }

            int coresType = 0;
            if(radioButton5.Checked)
            {
                coresType = 1;
            }
            else if (radioButton6.Checked)
            {
                coresType = 2;
            }
            else if (radioButton7.Checked)
            {
                coresType = 3;
            }

            EnemyRandomizer.randomizeEnemies(origRom, outFile, coresType, trackBar1.Value, checkBox4.Checked, checkBox8.Checked ? numFloors : 99, r);
            EnemyRandomizer.setNumberOfEnemies(outFile, trackBar6.Value);

            if(checkBox2.Checked)
            {
                ThemeRandomizer.randomizeThemes(origRom, outFile, r, ref workingOffset);
            }

            int startingLevel = (int)numericUpDown3.Value;
            if (startingLevel != 1)
            {
                MiscCodeChanges.setStartingLevels(outFile, (byte)startingLevel);
            }

            int itemInclusionType = 0;
            if(radioButton2.Checked)
            {
                itemInclusionType = 1;
            }
            else if(radioButton3.Checked)
            {
                itemInclusionType = 2;
            }
            ItemRandomizer.setRedChestItemQuality(origRom, outFile, trackBar2.Value, checkBox8.Checked ? numFloors : 99, itemInclusionType);
            ItemRandomizer.setRedChestItemInclusion(origRom, outFile, itemInclusionType, r);

            if(radioButton9.Checked)
            {
                ItemRandomizer.randomizeChestDistribution(origRom, outFile, r);
            }
            else if(radioButton10.Checked)
            {
                ItemRandomizer.setManualChestDistribution(origRom, outFile, new int[] {
                    (int)numericUpDown2.Value,
                    (int)numericUpDown4.Value,
                    (int)numericUpDown5.Value,
                    (int)numericUpDown8.Value,
                    (int)numericUpDown7.Value,
                    (int)numericUpDown6.Value,
                });
            }

            byte[] healTileValues = new byte[] { 0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0xC0, 0xFF };
            if(trackBar5.Value != 5)
            {
                // higher values are better chance of healing tile
                // original value x10
                Logging.logDebug("Setting heal tile comparator to " + healTileValues[trackBar5.Value], "misc");
                MiscCodeChanges.setHealingTileChances(outFile, healTileValues[trackBar5.Value]);
            }

            // "Start with providence" option
            if (checkBox6.Checked)
            {
                MiscCodeChanges.startWithProvidence(outFile, ref workingOffset);
            }

            // Add some sort of visible watermark for anti-cheating purposes
            // Welcome to "Derandomized" Ancient Cave!
            byte[] watermark = watermarkCharSelect;
            if(checkBox7.Checked)
            {
                // this takes off the event setter thing that lets you pick your party
                watermark = watermarkNoCharSelect;
            }

            // always set this so it's obvious you're running with a modified rom
            MiscCodeChanges.setDialogueWatermark(outFile, watermark);

            if (checkBox7.Checked)
            {
                MiscCodeChanges.randomizeParty(outFile, r);
            }

            if(radioButton13.Checked)
            {
                LayoutRandomizer.randomizeRoomSize(outFile, r, ref workingOffset);
            }
            else if(radioButton12.Checked)
            {
                LayoutRandomizer.manualRoomSize(origRom, outFile, trackBar4.Value);
            }

            if (radioButton14.Checked)
            {
                LayoutRandomizer.randomizeRoomNum(origRom, outFile, r, ref workingOffset);
            }
            else if (radioButton15.Checked)
            {
                LayoutRandomizer.manualRoomNum(origRom, outFile, trackBar7.Value);
            }

            // write processed rom
            File.WriteAllBytes(textBox2.Text, outFile);
            Logging.logDebug("Finished processing of file", "file");

            // save settings - input rom path
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(appDataFolder, "AC_DeRandomizer");
            string configFile = Path.Combine(specificFolder, "config.ini");
            try
            {
                Directory.CreateDirectory(specificFolder);
            }
            catch (Exception ee)
            {
                Logging.logDebug("Failed to save settings on successful ROM generation: " + ee.Message, "file");
            }

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties["inRom"] = textBox1.Text;
            try
            {
                PropertyFileUtil.writePropertyFile(configFile, properties);
            }
            catch (Exception ee)
            {
                Logging.logDebug("Failed to save settings on successful ROM generation: " + ee.Message, "file");
            }

            MessageBox.Show("Done!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            DialogResult dr = of.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox1.Text = of.FileName;
                if (textBox2.Text == null || textBox2.Text.Length == 0)
                {
                    textBox2.Text = textBox1.Text;
                    processOutputFilename();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            DialogResult dr = of.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox2.Text = of.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            randomizeSeed();
            processOutputFilename();
        }

        private void randomizeSeed()
        {
            textBox3.Text = randomString();
        }

        private string randomString()
        {
            string chars = "0123456789ABCDEF";
            char[] stringChars = new char[16];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            processOutputFilename();
        }

        private void processOutputFilename()
        {
            string outFilename = textBox2.Text;
            string seed = textBox3.Text;
            int periodIndex = outFilename.LastIndexOf(".");
            string beforeExtension = outFilename;
            string extension = "";

            if (periodIndex >= 0)
            {
                beforeExtension = outFilename.Substring(0, periodIndex);
                extension = outFilename.Substring(periodIndex);
            }

            int oBracketIndex = beforeExtension.LastIndexOf("[");
            if(oBracketIndex >= 0)
            {
                beforeExtension = beforeExtension.Substring(0, oBracketIndex);
            }

            if (checkBox1.Checked && beforeExtension.Length > 0)
            {
                beforeExtension += "[" + seed + "]";
            }
            textBox2.Text = beforeExtension + extension;
        }

        private void updateSize()
        {
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeightT = screenRectangle.Top - this.Top;
            int titleHeightL = screenRectangle.Left - this.Left;
            int titleHeightB = this.Bottom - screenRectangle.Bottom;
            int titleHeightR = this.Right - screenRectangle.Right;
            // 39 vertical
            // 16 horizontal
            int extraHeight = titleHeightT + titleHeightB;
            int extraWidth = titleHeightL + titleHeightR;
            if (showCustom)
            {
                this.Size = new Size(494 - 16 + extraWidth, 451 - 39 + extraHeight);
            }
            else
            {
                this.Size = new Size(494 - 16 + extraWidth, 208 - 39 + extraHeight);
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showCustom = !showCustom;
            updateSize();
        }

        private void itemDistribution_ValueChanged(object sender, EventArgs e)
        {
            deriveTableWidthsFromEntries();
        }
    }
}
