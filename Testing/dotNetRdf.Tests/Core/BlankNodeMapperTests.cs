using FluentAssertions;
using Xunit;

namespace VDS.RDF;

public class BlankNodeMapperTests
{
    private const string GivenPrefix = "mine";

    [Fact]
    public void DefaultPrefixUsed() =>
        new BlankNodeMapper().GetNextID().Should().StartWith("autos");

    [Fact]
    public void GivenPrefixNotUsed() =>
        new BlankNodeMapper(GivenPrefix).GetNextID().Should().StartWith("autos");

    [Fact]
    public void NullPrefixDoesNotThrow() =>
        new BlankNodeMapper(null).GetNextID().Should().StartWith("autos");

    [Fact]
    public void EmptyPrefixDoesNotThrow() =>
        new BlankNodeMapper(string.Empty).GetNextID().Should().StartWith("autos");

    [Fact]
    public void WhitespacePrefixDoesNotThrow() =>
        new BlankNodeMapper(" ").GetNextID().Should().StartWith("autos");
}
