using GeoNerves.Models;
using Xunit;

namespace GeoNerves.Tests.Unit
{
  public class AddressTests
  {
    /// <summary>
    /// Verify that the overriden Equals() evaluates equal contents as true and unequal contents as false
    /// </summary>
    [Fact]
    public void SameAddressesAreEqual()
    {
      var address1 = new Address
      {
        Id = 1,
        Street = "661 Main St",
        City = "Cambridge",
        State = "MA",
        Zip = "02140"
      };

      var address2 = new Address
      {
        Id = 1,
        Street = "661 Main St",
        City = "Cambridge",
        State = "MA",
        Zip = "02140"
      };

      Assert.True(address1.Equals(address2));

      address2.Id = 2;
      Assert.False(address1.Equals(address2));
    }

    /// <summary>
    /// Verify that overriden GetHashCode() evaluates the same for two addresses with the same values
    /// for all properties, and evaluates as false for two with different values (that should not collide)
    /// </summary>
    [Fact]
    public void SameAddressesMatchHash()
    {
      var address1 = new Address
      {
        Id = 1,
        Street = "661 Main St",
        City = "Cambridge",
        State = "MA",
        Zip = "02140"
      };

      var address2 = new Address
      {
        Id = 1,
        Street = "661 Main St",
        City = "Cambridge",
        State = "MA",
        Zip = "02140"
      };

      Assert.Equal(address1.GetHashCode(), address2.GetHashCode());

      address2.Id = 2;
      Assert.NotEqual(address1.GetHashCode(), address2.GetHashCode());
    }
  }
}