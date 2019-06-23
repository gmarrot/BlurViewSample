namespace BlurViewSample.Services {
    public struct EdgeInsets {

        public EdgeInsets(double left, double top, double right, double bottom) : this() {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public EdgeInsets(double horizontalInset, double verticalInset) : this(horizontalInset, verticalInset, horizontalInset, verticalInset) {
        }

        public EdgeInsets(double uniformInset) : this(uniformInset, uniformInset, uniformInset, uniformInset) {
        }

        public double Top { get; set; }

        public double Left { get; set; }

        public double Bottom { get; set; }

        public double Right { get; set; }

        public override bool Equals(object obj) {
            if (obj is EdgeInsets insets) {
                return Equals(insets);
            }

            return false;
        }

        public bool Equals(EdgeInsets other) {
            return Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right) && Bottom.Equals(other.Bottom);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(EdgeInsets left, EdgeInsets right) {
            return left.Equals(right);
        }

        public static bool operator !=(EdgeInsets left, EdgeInsets right) {
            return !left.Equals(right);
        }

    }
}
