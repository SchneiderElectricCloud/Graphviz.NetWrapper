using FluentAssertions;

namespace Rubjerg.Graphviz.Test
{
    internal class Assert
    {
        internal static void AreEqual(object v1, object v2)
        {
            v1.Should().Be(v2);
        }


        internal static void IsNotNull(object v)
        {
            v.Should().NotBeNull();
        }

        internal static void IsFalse(bool v)
        {
            v.Should().Be(false);
        }

        internal static void IsTrue(bool v)
        {
            v.Should().Be(true);
        }

        internal static void True(bool v)
        {
            v.Should().Be(true);

        }

        internal static void False(bool v)
        {
            v.Should().Be(false);

        }

        internal static void AreNotEqual(object v1, object v2)
        {
            v1.Should().NotBe(v2);
        }

        internal static void NotNull(object v)
        {
            v.Should().NotBeNull();
        }

        internal static void Null(object v)
        {
            v.Should().BeNull();
        }
    }
}
