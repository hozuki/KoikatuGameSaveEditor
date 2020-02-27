using System.Drawing;
using System.IO;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.Localization;

namespace KGSE.UI {
    public partial class MaleCharacterControl : CharacterControl, ICharacterControl {

        public MaleCharacterControl() {
            InitializeComponent();
        }

        [CanBeNull]
        public MaleCharacter Character { get; set; }

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

            txtLastName.Text = character.LastName;
            txtFirstName.Text = character.FirstName;
            txtNickname.Text = character.Nickname;
            txtIntellect.Text = character.Intelligence.ToString();
            txtStrength.Text = character.Strength.ToString();
            txtH.Text = character.Hentai.ToString();
        }

        public override void SaveToCharacter() {
            var character = Character;

            if (character == null) {
                return;
            }

            character.LastName = txtLastName.Text;
            character.FirstName = txtFirstName.Text;
            character.Nickname = txtNickname.Text;
            character.Intelligence = int.Parse(txtIntellect.Text);
            character.Strength = int.Parse(txtStrength.Text);
            character.Hentai = int.Parse(txtH.Text);
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

            CheckInt32(txtIntellect.Text, 0, 100, "intellect");
            CheckInt32(txtStrength.Text, 0, 100, "strength");
            CheckInt32(txtH.Text, 0, 100, "H");
        }

        public override void ApplyTranslation(Translation t) {
            lblFirstName.Text = t["male.first_name"];
            lblLastName.Text = t["male.last_name"];
            lblNickname.Text = t["male.nickname"];
            lblIntellect.Text = t["male.intellect"];
            lblStrength.Text = t["male.strength"];
            lblH.Text = t["male.hentai"];
        }

        protected override CharacterBase GetCharacter() {
            return Character;
        }

    }
}
