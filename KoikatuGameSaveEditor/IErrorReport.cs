using JetBrains.Annotations;

namespace KGSE {
    public interface IErrorReport {

        void Add([NotNull] ErrorEntry entry);

    }
}
