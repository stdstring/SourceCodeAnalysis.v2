using Microsoft.CodeAnalysis.Text;

namespace SourceCodeCheckApp.Utils
{
    internal class CollectedData<T> : IEquatable<CollectedData<T>>
    {
        public CollectedData(T data, LinePosition startPosition, LinePosition endPosition)
        {
            Data = data;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public T Data { get; }

        public LinePosition StartPosition { get; }

        public LinePosition EndPosition { get; }

        public Boolean Equals(CollectedData<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Data, other.Data) && StartPosition.Equals(other.StartPosition) && EndPosition.Equals(other.EndPosition);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CollectedData<T>)obj);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hashCode = Data != null ? Data.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ StartPosition.GetHashCode();
                hashCode = (hashCode * 397) ^ EndPosition.GetHashCode();
                return hashCode;
            }
        }

        public override String ToString()
        {
            return $"CollectedData: Data = \"{Data}\", StartPosition = {StartPosition}, EndPosition = {EndPosition}";
        }
    }
}