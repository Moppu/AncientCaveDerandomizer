using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AncientCaveDerandomizer
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            webBrowser1.DocumentText = Properties.Resources.main;
            webBrowser1.DocumentTitleChanged += WebBrowser1_DocumentTitleChanged;
        }

        private void WebBrowser1_DocumentTitleChanged(object sender, EventArgs e)
        {
            string browserText = webBrowser1.DocumentText;
            // when i click links, it doesn't go to them, but replaces the document with the name of the file + html blocks.  fix that here by loading the named thing
            string minusHtmlBlocks = browserText.ToLower().Replace("<html>", "").Replace("</html>", "").Replace(".html", "").Trim(new char[] { ' ', '\t', '\n', '\r', '\0' });
            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = ((string)entry.Key).Trim();
                string resource = (string)entry.Value;
                if (minusHtmlBlocks == resourceKey)
                {
                    if (resource.Contains("%itemqualitytable"))
                    {
                        string itemQualityTable = "<table border=\"1\"><tr><th>Item ID</th><th>Item name</th> <th>Desirability value</th></tr>";
                        for (int i = 0; i < ItemRandomizer.itemNames.Length; i++)
                        {
                            itemQualityTable += "<tr><td>" + i + "</td><td>" + ItemRandomizer.itemNames[i] + "</td> <td>" + ItemRandomizer.itemDesirability[i] + "</td></tr>";
                        }
                        itemQualityTable += "</table>";
                        resource = resource.Replace("%itemqualitytable", itemQualityTable);
                    }
                    webBrowser1.DocumentText = resource;
                }
            }
        }
    }
}
