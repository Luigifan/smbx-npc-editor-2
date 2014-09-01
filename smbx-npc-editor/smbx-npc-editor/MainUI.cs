﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using smbx_npc_editor.IO;
using System.IO;
using Setting;
using smbx_npc_editor.SpriteHandling;
using smbxnpceditor;

namespace smbx_npc_editor
{
    public partial class MainUI : Form
    {
        //
        IniFile settingsFile = new IniFile(Environment.CurrentDirectory + @"\Data\settings.ini");
        public IniFile curConfig;
        public string currentConfig;
        bool showAnimationPane;
        bool runPortable = false;
        //
        NpcConfigFile npcfile = new NpcConfigFile(true);
        string curNpcId = "blank";
        //
        public MainUI()
        {
            Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
            loadSettings();
            compileConfigs();
            GetUserControls(this.Controls);
        }

        private void GetUserControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is UserControl)
                {
                    if (c is SpinnerControlValue)
                    {
                        SpinnerControlValue svc = (SpinnerControlValue)c;
                        svc.valueSpinner.ValueChanged += valueSpinner_ValueChanged;
                        svc.enabledCheckBox.CheckedChanged += (sender, e) => enabledCheckBox_CheckedChanged(sender, e, svc);
                    }
                    else if (c is ComboBoxControlValue)
                    {
                        ComboBoxControlValue cbcv = (ComboBoxControlValue)c;
                        cbcv.ComboValue.SelectedIndexChanged += comboValue_SelectedIndexChanged;
                        cbcv.enabledCheckBox.CheckedChanged += (sender, e) => combo_enabledCheckBox_CheckedChanged(sender, e, cbcv);
                    }
                    else if (c is CheckBoxValue)
                    {
                        CheckBoxValue cbv = (CheckBoxValue)c;
                        cbv.checkValue.CheckedChanged += checkValue_CheckedChanged;
                        cbv.enabledCheckBox.CheckedChanged += (sender, e) => cb_enabledCheckBox_CheckedChanged(sender, e, cbv);
                    }
                }
                if (c.Controls.Count > 0)
                    GetUserControls(c.Controls);
            }
        }
        #region Control Events
        void enabledCheckBox_CheckedChanged(object sender, object e, SpinnerControlValue sv)
        {
            CheckBox check = (CheckBox)sender;
            if(check.Checked)
            {
                npcfile.AddValue(sv.valueSpinner.Tag.ToString(), sv.valueSpinner.Value.ToString());
            }
            else
            {
                npcfile.RemoveValue(sv.valueSpinner.Tag.ToString());
            }
        }

        void valueSpinner_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown updown = (NumericUpDown)sender;
            npcfile.AddValue(updown.Tag.ToString(), updown.Value.ToString());
        }
        //
        void cb_enabledCheckBox_CheckedChanged(object sender, object e, CheckBoxValue cb)
        {
            CheckBox check = (CheckBox)sender;
            if(check.Checked)
            {
                switch(cb.checkValue.Checked)
                {
                    case(true):
                        npcfile.AddValue(cb.checkValue.Tag.ToString(), "1");
                        break;
                    case(false):
                        npcfile.AddValue(cb.checkValue.Tag.ToString(), "0");
                        break;
                }
            }
            else
            {
                npcfile.RemoveValue(cb.checkValue.Tag.ToString());
            }
        }
        void checkValue_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            switch(check.Checked)
            {
                case(true):
                    npcfile.AddValue(check.Tag.ToString(), "1");
                    break;
                case(false):
                    npcfile.AddValue(check.Tag.ToString(), "0");
                    break;
            }
        }
        //
        void combo_enabledCheckBox_CheckedChanged(object sender, EventArgs e, ComboBoxControlValue cbcv)
        {
            CheckBox check = (CheckBox)sender;
            switch(check.Checked)
            {
                case(true):
                    if(cbcv.GetSelectedIndex().ToString() != "-1")
                        npcfile.AddValue(cbcv.ComboValue.Tag.ToString(), cbcv.GetSelectedIndex().ToString());
                    break;
                case(false):
                    npcfile.RemoveValue(cbcv.ComboValue.Tag.ToString());
                    break;
            }
        }
        void comboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if(combo.SelectedIndex.ToString() != "-1")
                npcfile.AddValue(combo.Tag.ToString(), combo.SelectedIndex.ToString());
        }
        #endregion
        #region crying
        void ResetItems(Control.ControlCollection controls)
        {
            foreach(Control c in controls)
            {
                if(c is UserControl)
                {
                    if(c is SpinnerControlValue)
                    {
                        SpinnerControlValue svc = (SpinnerControlValue)c;
                        svc.Reset();
                    }
                    if(c is ComboBoxControlValue)
                    {
                        ComboBoxControlValue cbcv = (ComboBoxControlValue)c;
                        cbcv.Reset();
                    }
                    if(c is CheckBoxValue)
                    {
                        CheckBoxValue cbv = (CheckBoxValue)c;
                        cbv.Reset();
                    }
                }
                if (c.Controls.Count > 0)
                    ResetItems(c.Controls);
            }
            ///TODO: Reset the animator and NPC Name stuff. Along with the title and any other loose variables.
            npcfile.Clear();
        }

        void compileConfigs()
        {
            if (Directory.Exists(Environment.CurrentDirectory + @"\Data"))
            {
                if(currentConfig == "null")
                {
                    noConfig();
                }
                int count = 0;
                List<string> dirs = new List<string>();
                foreach (var dir in Directory.GetDirectories(Environment.CurrentDirectory + @"\Data"))
                {
                    count++;
                    dirs.Add(dir);
                }
                MenuItem[] items = new MenuItem[count];
                bool add = true;
                for (int i = 0; i < items.Length; i++)
                {
                    string dirName = Path.GetFileName(dirs[i]);
                    if (dirName == "tools")
                        add = false;
                    if (!File.Exists(dirs[i] + @"\lvl_npc.ini"))
                        add = false;
                    items[i] = new MenuItem();
                    items[i].Name = "configItem" + i;
                    items[i].Text = dirName;
                    items[i].RadioCheck = true;
                    items[i].Click += new EventHandler(ConfigMenuHandler);
                    if (add)
                        selectConfigMenuItem.MenuItems.Add(items[i]);
                    add = true;
                }
            }
            else
            {
                noConfig();
            }
        }
        void noConfig()
        {
            Console.WriteLine("Data folder was moved or non existent!");
            MenuItem nullConfig = new MenuItem("No configuration available");
            nullConfig.Enabled = false;
            selectConfigMenuItem.MenuItems.Add(nullConfig);
        }
        void loadSettings()
        {
            if(File.Exists(Environment.CurrentDirectory + @"\Data\settings.ini"))
            {
                try
                {
                    currentConfig = settingsFile.ReadValue("Settings", "lastConfig");
                    if(currentConfig == "null")
                    {
                        //do null stuff
                    }
                    showAnimationPane = bool.Parse(settingsFile.ReadValue("Settings", "showAnimation"));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Utility.ModifyRegistry.ModifyRegistry mr = new Utility.ModifyRegistry.ModifyRegistry();
                try
                {
                    string runPortableStr = mr.Read("runPortable");
                    Console.WriteLine("RegKey 'runPortable' is equal to {0}", runPortableStr.ToString());
                    if (runPortableStr == null)
                        throw new InvalidDataException();
                    else if (runPortableStr == "true")
                    {
                        runPortable = true;
                        runPortableEvents();
                    }
                    else if (runPortableStr == "false")
                    { runPortable = false; writeInitialIni(); loadSettings(); }
                    else
                        throw new InvalidDataException();
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("We can't seem to find a Data directory alongside the NPC Editor executable. Would you like to run this application as a \"Portable\" app?", "No Data Directory Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (dr)
                    {
                        case (DialogResult.Yes):
                            mr.Write("runPortable", "true");
                            break;
                        case (DialogResult.No):
                            mr.Write("runPortable", "false");
                            writeInitialIni();
                            loadSettings();
                            break;
                        case (DialogResult.Cancel):
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }
        void runPortableEvents()
        {
            MenuItem runPortableMenu = new MenuItem("Run portable (stored in registry)", runPortableMenu_Clicked);
            if (runPortable)
            {
                runPortableMenu.Checked = true;
                MenuItem sep = new MenuItem("-");
                editMenu.MenuItems.Add(sep);
                editMenu.MenuItems.Add(runPortableMenu);
            }
        }

        private void runPortableMenu_Clicked(object sender, EventArgs e)
        {
            Utility.ModifyRegistry.ModifyRegistry mr = new Utility.ModifyRegistry.ModifyRegistry();
            MenuItem portMenu = (MenuItem)sender;
            if(portMenu.Checked == true)
            {
                portMenu.Checked = false;
                mr.Write("runPortable", "false");
                Console.WriteLine("Writing key 'runPortable' to registry with value 'false'");
            }
            else
            {
                portMenu.Checked = true;
                mr.Write("runPortable", true);
                Console.WriteLine("Writing key 'runPortable' to registry with value 'true'");
            }
        }

        void writeInitialIni()
        {
            try
            {
                if (!Directory.Exists(Environment.CurrentDirectory + @"\Data"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Data");
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Data\settings.ini");
                sw.WriteLine("[Settings]");
                sw.WriteLine("showAnimation=true");
                sw.WriteLine("lastConfig=null");
                sw.WriteLine("lastConfigItem=null");
                sw.Flush();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Looks like we can't get write permission to write the default configurations and settings files!\nTry running this program as administrator before proceeding.\nException output: \nPlease press OK so the program can crash" + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        void saveSettings()
        {
            //write settings file
            if (showAnimationPane)
                settingsFile.WriteValue("Settings", "showAnimation", showAnimationPane.ToString());
            else
                settingsFile.WriteValue("Settings", "showAnimation", "false");
            settingsFile.WriteValue("Settings", "lastConfig", currentConfig);
            foreach (MenuItem menu in selectConfigMenuItem.MenuItems)
            {
                if (menu.Checked == true)
                    settingsFile.WriteValue("Settings", "lastConfigItem", menu.Name);
            }
        }
        private void unCheckOthers(MenuItem keep)
        {
            foreach(MenuItem eachMenu in selectConfigMenuItem.MenuItems)
            {
                eachMenu.Checked = false;
            }
            keep.Checked = true;
        }
        #endregion

        private void ConfigMenuHandler(object sender, EventArgs e)
        {
            MenuItem selected = (MenuItem)sender;
            selected.Checked = true;
            MenuItem parent = (MenuItem)selected.Parent;
            unCheckOthers(selected);
            currentConfig = Environment.CurrentDirectory + @"\Data\" + selected.Text;
        }

        private void MainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveSettings();
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            if (showAnimationPane)
                showAnimationMenuItem.Checked = true;
            else
                showAnimationMenuItem.Checked = false;
            try
            {
                foreach (MenuItem menu in selectConfigMenuItem.MenuItems)
                {
                    if (menu.Name == settingsFile.ReadValue("Settings", "lastConfigItem"))
                        menu.Checked = true;
                    else
                        menu.Checked = false;
                }
            }
            catch (Exception ex) { Console.WriteLine("Not a problem but, {0}", ex.Message); }
            currentConfig = settingsFile.ReadValue("Settings", "lastConfig");
            curConfig = new IniFile(currentConfig + @"\lvl_npc.ini");
        }

        private void showAnimationMenuItem_Click(object sender, EventArgs e)
        {
            if (showAnimationMenuItem.Checked == true)
            {
                showAnimationMenuItem.Checked = false;
                showAnimationPane = false;
                //update UI when the time comes
            }
            else
            {
                showAnimationMenuItem.Checked = true;
                showAnimationPane = true;
                //update UI when the time comes
            }
        }
        private void menuItem9_Click(object sender, EventArgs e)
        {
            if (currentConfig != "null")
            {
                NewConfig nc = new NewConfig(currentConfig);
                DialogResult dr = nc.ShowDialog();
                switch(dr)
                {
                    case(DialogResult.OK):
                        ResetItems(this.Controls);
                        curNpcId = nc.npcId;
                        Console.WriteLine("Selected NPC ID was {0}", curNpcId);
                        break;
                    case(DialogResult.Cancel):
                        break;
                }
            }
            else
            {    //reset items, when this is actually implemented of course
                ResetItems(this.Controls);
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        //

        //
    }
}
