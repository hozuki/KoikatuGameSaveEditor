using JetBrains.Annotations;

namespace KGSE.Localization {
    internal interface ITranslateable {

        void ApplyTranslation([NotNull] Translation t);

    }
}
