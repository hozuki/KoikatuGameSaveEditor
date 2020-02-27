using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.IO;
using KGSE.Localization;
using Newtonsoft.Json;

namespace KGSE.UI {
    public partial class Form1 : Form, ITranslateable {

        public Form1() {
            InitializeComponent();
        }

        public void ApplyTranslation(Translation t) {
            if (_saveData != null && _lastSaveFileName != null) {
                Text = string.Format(TranslationManager.Instance.Current["app.title.loaded"], _saveData.SchoolName, _lastSaveFileName);
            } else {
                Text = t["app.title"];
            }

            mnuFile.Text = t["app.menu.file"];
            mnuFileOpen.Text = t["app.menu.file.open"];
            mnuFileSave.Text = t["app.menu.file.save"];
            mnuFileSaveAs.Text = t["app.menu.file.saveas"];
            mnuFileExit.Text = t["app.menu.file.exit"];
            mnuLanguages.Text = t["app.menu.languages"];
            mnuHelp.Text = t["app.menu.help"];
            mnuHelpAbout.Text = t["app.menu.help.about"];

            foreach (var item in mainPanel.Tiles) {
                if (item is ITranslateable translateable) {
                    translateable.ApplyTranslation(t);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            LoadTranslations();
            SelectMostAppropriateTranslation();
            ApplyTranslation(TranslationManager.Instance.Current);
        }

        private void mnuFileExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void mnuFileOpen_Click(object sender, EventArgs e) {
            ofd.Multiselect = false;
            ofd.ShowReadOnly = false;
            ofd.ReadOnlyChecked = false;
            ofd.Filter = OpenFilter;
            ofd.CheckFileExists = true;
            ofd.ValidateNames = true;
            ofd.DereferenceLinks = true;

            var r = ofd.ShowDialog(this);

            if (r == DialogResult.Cancel) {
                return;
            }

            SaveData saveData;

            using (var memory = new MemoryStream()) {
                try {
                    using (var fileStream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        fileStream.CopyTo(memory);
                    }
                } catch (IOException ex) {
                    MessageBox.Show($"Cannot open file '{ofd.FileName}':{Environment.NewLine}{ex.ToString()}", AppCommon.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                memory.Position = 0;

                try {
                    saveData = SaveDataIo.Read(memory);
                } catch (Exception ex) {
                    Debug.Print(ex.StackTrace);
                    MessageBox.Show($"Cannot open save data:{Environment.NewLine}{ex.ToString()}", AppCommon.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            _saveData = saveData;
            _lastSaveFileName = ofd.FileName;

            Text = string.Format(TranslationManager.Instance.Current["app.title.loaded"], saveData.SchoolName, ofd.FileName);

            UpdateControls();
        }

        private void mnuFileSave_Click(object sender, EventArgs e) {
            if (_saveData == null) {
                return;
            }

            if (!ValidateAllInputs()) {
                return;
            }

            Trace.Assert(_lastSaveFileName != null, nameof(_lastSaveFileName) + " != null");

            SaveCurrentSaveGameTo(_lastSaveFileName);
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e) {
            if (_saveData == null) {
                return;
            }

            if (!ValidateAllInputs()) {
                return;
            }

            sfd.OverwritePrompt = true;
            sfd.ValidateNames = true;
            sfd.CheckPathExists = true;
            sfd.Filter = SaveFilter;

            var r = sfd.ShowDialog(this);

            if (r == DialogResult.Cancel) {
                return;
            }

            SaveCurrentSaveGameTo(sfd.FileName);
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e) {
            FAbout.ShowModal(this);
        }

        private void TranslationSelectionMenuOnClick(object sender, EventArgs e) {
            var translation = (Translation)((ToolStripMenuItem)sender).Tag;

            if (translation != TranslationManager.Instance.Current) {
                TranslationManager.Instance.Current = translation;
                ApplyTranslation(translation);
            }
        }

        private void LoadTranslations() {
            var currentDirectory = Environment.CurrentDirectory;
            var languagesDirectory = Path.Combine(currentDirectory, "Resources/Lang");

            do {
                if (Directory.Exists(languagesDirectory)) {
                    var dir = new DirectoryInfo(languagesDirectory);

                    const string langJsonFilter = "*.lang.json";
                    var langFiles = dir.GetFiles(langJsonFilter, SearchOption.TopDirectoryOnly);

                    foreach (var fileInfo in langFiles) {
                        var fileData = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
                        var translation = JsonConvert.DeserializeObject<Translation>(fileData);
                        TranslationManager.Instance.AddTranslation(translation);
                    }
                }
            } while (false);

            foreach (var kv in TranslationManager.Instance.Translations) {
                var menuItem = new ToolStripMenuItem(kv.Value.DisplayName);
                mnuLanguages.DropDownItems.Add(menuItem);
                menuItem.Click += TranslationSelectionMenuOnClick;
                menuItem.Tag = kv.Value;
            }
        }

        private static void SelectMostAppropriateTranslation() {
            var culture = CultureInfo.CurrentUICulture;
            var name = culture.Name.ToLowerInvariant();

            Translation t = null;

            foreach (var kv in TranslationManager.Instance.Translations) {
                if (kv.Key == name) {
                    t = kv.Value;
                    break;
                }
            }

            if (t != null) {
                TranslationManager.Instance.Current = t;
            }
        }

        private void UpdateControls() {
            Trace.Assert(_saveData != null);

            mainPanel.ClearControls();

            var added = new List<CharacterControl>();

            foreach (var chara in _saveData.Characters) {
                switch (chara.Gender) {
                    case Gender.Male: {
                            var male = (MaleCharacter)chara;
                            var c = new MaleCharacterControl();
                            c.Character = male;
                            added.Add(c);
                            break;
                        }
                    case Gender.Female: {
                            var female = (FemaleCharacter)chara;
                            var c = new FemaleCharacterControl();
                            c.Character = female;
                            added.Add(c);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var c in added) {
                c.BackColor = SystemColors.Control;
                c.Anchor = AnchorStyles.None;
                c.LoadFromCharacter();
            }

            var controlArray = added.ToArray();

            mainPanel.EnterBatchMode();
            // ReSharper disable once CoVariantArrayConversion
            mainPanel.AddControls(controlArray);
            mainPanel.ExitBatchMode();

            foreach (var control in controlArray) {
                if (control is ITranslateable translateable) {
                    translateable.ApplyTranslation(TranslationManager.Instance.Current);
                }
            }
        }

        private bool ValidateAllInputs() {
            var report = new ErrorReport();

            foreach (var control in mainPanel.Tiles) {
                var c = control as ICharacterControl;

                Trace.Assert(c != null, nameof(c) + " != null");
                c.ValidateInput(report);
            }

            if (report.Entries.Count > 0) {
                FErrorLog.ShowModal(this, report);
                return false;
            } else {
                return true;
            }
        }

        private void SaveCurrentSaveGameTo([NotNull] string path) {
            Trace.Assert(_saveData != null, nameof(_saveData) + " != null");

            try {
                using (var fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Write)) {
                    SaveDataIo.Write(_saveData, fileStream);
                }
            } catch (IOException ex) {
                Debug.Print(ex.StackTrace);
                MessageBox.Show($"Failed to save to '{path}':{Environment.NewLine}{ex.ToString()}", AppCommon.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _lastSaveFileName = path;
            Text = string.Format(TranslationManager.Instance.Current["app.title.loaded"], _saveData.SchoolName, path);
            MessageBox.Show($"File saved to '{path}'.", AppCommon.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private const string OpenFilter = "Koikatu Save Data (file*.dat)|file*.dat";
        private const string SaveFilter = "Koikatu Save Data (*.dat)|*.dat";

        [CanBeNull]
        private SaveData _saveData;

        [CanBeNull]
        private string _lastSaveFileName;

    }
}
