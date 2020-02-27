using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.Localization;

namespace KGSE.UI {
    // Don't be abstract. That will break the designer.
    public class CharacterControl : UserControl, ICharacterControl, ITranslateable {

        public virtual void LoadFromCharacter() {
            throw new NotImplementedException();
        }

        public virtual void SaveToCharacter() {
            throw new NotImplementedException();
        }

        public virtual void ValidateInput(IErrorReport report) {
            throw new NotImplementedException();
        }

        // DO NOT call before contents are added.
        public virtual void ApplyTranslation(Translation t) {
            throw new NotImplementedException();
        }

        [CanBeNull]
        protected virtual CharacterBase GetCharacter() {
            throw new NotImplementedException();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            var rect = ClientRectangle;
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        }

        CharacterBase ICharacterControl.Character {
            [DebuggerStepThrough]
            get => GetCharacter();
        }

    }
}
