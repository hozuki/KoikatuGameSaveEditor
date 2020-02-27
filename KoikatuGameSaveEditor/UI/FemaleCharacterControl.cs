using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.Localization;

namespace KGSE.UI {
    public partial class FemaleCharacterControl : CharacterControl {

        public FemaleCharacterControl() {
            InitializeComponent();

            btnAnswers.DropDown.Closing += BlockDefaultDropDownClosing;
            btnPreferences.DropDown.Closing += BlockDefaultDropDownClosing;
            btnTraits.DropDown.Closing += BlockDefaultDropDownClosing;
        }

        ~FemaleCharacterControl() {
            btnAnswers.DropDown.Closing -= BlockDefaultDropDownClosing;
            btnPreferences.DropDown.Closing -= BlockDefaultDropDownClosing;
            btnTraits.DropDown.Closing -= BlockDefaultDropDownClosing;
        }

        [CanBeNull]
        public FemaleCharacter Character { get; set; }


        public override void ApplyTranslation(Translation t) {
            lblLastName.Text = t["female.last_name"];
            lblFirstName.Text = t["female.first_name"];
            lblNickname.Text = t["female.nickname"];
            lblGender.Text = t["misc.gender"];
            lblPersonality.Text = t["female.pers"];
            lblWeakPoint.Text = t["female.weak_point"];

            void ApplyComboBoxItems(ComboBox comboBox, string prefix) {
                var items = comboBox.Items;
                var count = items.Count;
                for (var i = 0; i < count; i += 1) {
                    items[i] = t.Get($"{prefix}{i.ToString()}", "?");
                }
            }

            ApplyComboBoxItems(cboGender, "misc.gender.");
            ApplyComboBoxItems(cboPersonality, "female.pers.");
            ApplyComboBoxItems(cboWeakPoint, "female.weak_point.");

            btnAnswers.Text = t["female.answers"];
            btnPreferences.Text = t["female.pref"];
            btnTraits.Text = t["female.traits"];

            void ApplyDropDownItems(ToolStripDropDown dropDown, string prefix) {
                foreach (var item in dropDown.Items) {
                    var m = item as ToolStripMenuItem;
                    Trace.Assert(m != null);

                    var key = (string)m.Tag;

                    m.Text = t.Get($"{prefix}{key}", "?");
                }
            }

            ApplyDropDownItems(btnAnswers.DropDown, "female.answers.");
            ApplyDropDownItems(btnPreferences.DropDown, "female.pref.");
            ApplyDropDownItems(btnTraits.DropDown, "female.traits.");

            lblFeeling.Text = t["female.feeling"];
            lblRelation.Text = t["female.relation"];
            lblHDegree.Text = t["female.h_degree"];
            lblHCount.Text = t["female.h_count"];
            lblIntimacy.Text = t["female.intimacy"];

            ApplyComboBoxItems(cboRelation, "female.relation.");

            chkIsAngry.Text = t["female.is_angry"];
            chkIsClubMember.Text = t["female.is_club_member"];
            chkHasDate.Text = t["female.has_date"];

            lblDBreasts.Text = t["female.d.breasts"];
            lblDCrotch.Text = t["female.d.crotch"];
            lblDAnal.Text = t["female.d.anal"];
            lblDButts.Text = t["female.d.butts"];
            lblDNipples.Text = t["female.d.nipples"];
            lblDVInsertion.Text = t["female.d.vi"];
            lblDAInsertion.Text = t["female.d.ai"];
            lblDServing.Text = t["female.d.serving"];
        }

        public override void LoadFromCharacter() {
            var character = Character;

            if (character == null) {
                return;
            }

            Bitmap newImage;

            using (var imageStream = new MemoryStream(character.GetSecondaryCardImageData(), false)) {
                newImage = (Bitmap)Image.FromStream(imageStream);
            }

            var oldImage = photo.Image;
            photo.Image = newImage;

            oldImage?.Dispose();

            InitializeOptionData();

            txtLastName.Text = character.LastName;
            txtFirstName.Text = character.FirstName;
            txtNickname.Text = character.Nickname;
            cboGender.SelectedIndex = (int)character.Gender;
            cboPersonality.SelectedIndex = (int)character.Personality;
            cboWeakPoint.SelectedIndex = (int)character.WeakPoint;

            void SetDropDownItemsChecked(ToolStripDropDown dropDown, BooleanPropertyBag bag) {
                foreach (var item in dropDown.Items) {
                    var m = item as ToolStripMenuItem;
                    Trace.Assert(m != null);

                    var key = (string)m.Tag;
                    m.Checked = bag.Get(key);
                }
            }

            SetDropDownItemsChecked(btnAnswers.DropDown, character.Answers);
            SetDropDownItemsChecked(btnPreferences.DropDown, character.IntercoursePreferences);
            SetDropDownItemsChecked(btnTraits.DropDown, character.Traits);

            txtFeeling.Text = character.Feeling.ToString();
            cboRelation.SelectedIndex = character.IsLover ? 1 : 0;
            txtHDegree.Text = character.EroticDegree.ToString();
            txtHCount.Text = character.IntercourseCount.ToString();
            txtIntimacy.Text = character.Intimacy.ToString();
            chkIsAngry.Checked = character.IsAngry;
            chkIsClubMember.Checked = character.IsClubMember;
            chkHasDate.Checked = character.HasDate;

            cboDBreasts.SelectedIndex = (int)character.Development.Breasts.Value;
            cboDCrotch.SelectedIndex = (int)character.Development.Crotch.Value;
            cboDAnal.SelectedIndex = (int)character.Development.Anal.Value;
            cboDButts.SelectedIndex = (int)character.Development.Butts.Value;
            cboDNipples.SelectedIndex = (int)character.Development.Nipples.Value;
            cboDVInsertion.SelectedIndex = (int)character.Development.VaginaInsertion.Value;
            cboDAInsertion.SelectedIndex = (int)character.Development.AnalInsertion.Value;
            cboDServing.SelectedIndex = (int)character.Development.Serving.Value;
        }

        public override void SaveToCharacter() {
            var character = Character;

            if (character == null) {
                return;
            }

            character.LastName = txtLastName.Text;
            character.FirstName = txtFirstName.Text;
            character.Nickname = txtNickname.Text;
            character.Personality = (Personality)cboPersonality.SelectedIndex;
            character.WeakPoint = (WeakPoint)cboWeakPoint.SelectedIndex;

            void MapDropDownItemsChecked(ToolStripDropDown dropDown, BooleanPropertyBag bag) {
                foreach (var item in dropDown.Items) {
                    var m = item as ToolStripMenuItem;
                    Trace.Assert(m != null);

                    var key = (string)m.Tag;
                    bag.Set(key, m.Checked);
                }
            }

            MapDropDownItemsChecked(btnAnswers.DropDown, character.Answers);
            MapDropDownItemsChecked(btnPreferences.DropDown, character.IntercoursePreferences);
            MapDropDownItemsChecked(btnTraits.DropDown, character.Traits);

            character.Feeling = int.Parse(txtFeeling.Text);
            character.IsLover = cboRelation.SelectedIndex != 0;
            character.EroticDegree = int.Parse(txtHDegree.Text);
            character.IntercourseCount = int.Parse(txtHCount.Text);
            character.Intimacy = int.Parse(txtIntimacy.Text);
            character.IsAngry = chkIsAngry.Checked;
            character.IsClubMember = chkIsClubMember.Checked;
            character.HasDate = chkHasDate.Checked;

            character.Development.Breasts.Value = (Percentage)cboDBreasts.SelectedIndex;
            character.Development.Crotch.Value = (Percentage)cboDCrotch.SelectedIndex;
            character.Development.Anal.Value = (Percentage)cboDAnal.SelectedIndex;
            character.Development.Butts.Value = (Percentage)cboDButts.SelectedIndex;
            character.Development.Nipples.Value = (Percentage)cboDNipples.SelectedIndex;
            character.Development.VaginaInsertion.Value = (Percentage)cboDVInsertion.SelectedIndex;
            character.Development.AnalInsertion.Value = (Percentage)cboDAInsertion.SelectedIndex;
            character.Development.Serving.Value = (Percentage)cboDServing.SelectedIndex;
        }

        public override void ValidateInput(IErrorReport report) {
            var firstName = txtFirstName.Text;
            var lastName = txtLastName.Text;

            void Err(string description) {
                report.Add(ErrorEntry.New(firstName, lastName, description));
            }

            void CheckString(string value, string description) {
                //if (value.Length == 0)
                //{
                //    Err($"{description} cannot be empty.");
                //}

                if (Utf8.Encoding.GetBytes(value).Length > 0xff) {
                    Err($"{description} is too long.");
                }
            }

            void CheckInt32(string stringValue, int minInc, int maxInc, string description) {
                var b = int.TryParse(stringValue, out var n);

                if (!b || (n < minInc || n > maxInc)) {
                    Err($"Invalid {description}, should be integer between {minInc.ToString()} and {maxInc.ToString()}.");
                }
            }

            CheckString(firstName, "Given name");
            CheckString(lastName, "Family name");
            CheckString(txtNickname.Text, "Nickname");

            CheckInt32(txtFeeling.Text, 0, 100, "feeling");
            CheckInt32(txtHDegree.Text, 0, 100, "H degree");
            CheckInt32(txtHCount.Text, 0, int.MaxValue, "H count");
            CheckInt32(txtIntimacy.Text, 0, 100, "intimacy");
        }

        protected override CharacterBase GetCharacter() {
            return Character;
        }

        private void InitializeOptionData() {
            if (_optionDataInitialized) {
                return;
            }

            cboGender.Items.AddRange(new object[] { "Male", "Female" });

            AddAllEnumValues(cboPersonality, typeof(Personality));
            AddAllEnumValues(cboWeakPoint, typeof(WeakPoint));

            cboRelation.Items.AddRange(new object[] { "Friend", "Lover" });

            AddPercentages(cboDBreasts);
            AddPercentages(cboDCrotch);
            AddPercentages(cboDAnal);
            AddPercentages(cboDButts);
            AddPercentages(cboDNipples);
            AddPercentages(cboDVInsertion);
            AddPercentages(cboDAInsertion);
            AddPercentages(cboDServing);

            AddYesNoDropDowns(btnAnswers.DropDown, Character.Answers);
            AddYesNoDropDowns(btnPreferences.DropDown, Character.IntercoursePreferences);
            AddYesNoDropDowns(btnTraits.DropDown, Character.Traits);

            _optionDataInitialized = true;

            foreach (var o in Controls) {
                if (o is ComboBox cb) {
                    if (cb.Items.Count > 0) {
                        cb.SelectedIndex = 0;
                    }
                }
            }
        }

        private static void AddAllEnumValues([NotNull] ComboBox comboBox, [NotNull] Type enumType) {
            var arr = Enum.GetValues(enumType);
            var count = arr.Length;

            for (var i = 0; i < count; i += 1) {
                var description = arr.GetValue(i).ToString();
                comboBox.Items.Add(description);
            }
        }

        private static void AddPercentages([NotNull] ComboBox comboBox) {
            for (var i = 0; i <= 10; i += 1) {
                var s = $"{(i * 10).ToString()}%";
                comboBox.Items.Add(s);
            }
        }

        private static void AddYesNoDropDowns([NotNull] ToolStripDropDown dropDown, [NotNull] BooleanPropertyBag propertyBag) {
            var properties = propertyBag.GetAllProperties();

            foreach (var prop in properties) {
                var item = new ToolStripMenuItem(prop.Key) {
                    Tag = prop.Key,
                    CheckOnClick = true
                };

                dropDown.Items.Add(item);
            }
        }

        private static void BlockDefaultDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e) {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) {
                e.Cancel = true;
            }
        }

        private void FemaleCharacterControl_Load(object sender, EventArgs e) {
            InitializeOptionData();
        }

        private bool _optionDataInitialized;

    }
}
