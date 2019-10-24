using GeoNerves.Models;
using Xunit;

namespace GeoNerves.Tests.Unit
{
  public class AddressTests
  {
    [Fact]
    public void CanBuildAddressFromXml()
    {
      // Address to compare against
      var correctAddress = new Address
      {
        Id = 1,
        Street = "667 Massachusetts Avenue",
        City = "Cambridge",
        State = "MA",
        Zip = "02139"
      };

      // Generate an address from an XML string using the factory method
      var address = Address.ParseAddressFromXml
      (
        @"<Address>
            <UniqueId>1</UniqueId>
            <Street>667 Massachusetts Avenue</Street>
            <City>Cambridge</City>
            <State>MA</State>
            <Zip>02139</Zip>
          </Address>"
      );

      Assert.True(address.Equals(correctAddress));
    }

    [Fact]
    public void CanBuildAddressFromJson()
    {
      var correctAddress = new Address
      {
        Id = 1,
        Street = "667 Massachusetts Avenue",
        City = "Cambridge",
        State = "MA",
        Zip = "02139"
      };

      // Generate an address from an XML string using the factory method
      var address = Address.ParseAddressFromJson
      (
        @"{
                    ""UniqueId"": 1,
                    ""Street"": ""667 Massachusetts Avenue"",
                    ""City"": ""Cambridge"",
	                ""State"": ""MA"",
	                ""Zip"": ""02139""
                }"
      );

      Assert.True(address.Equals(correctAddress));
    }

    [Fact]
    public void CanBuildAddressFromCsv()
    {
      var correctAddress = new Address
      {
        Id = 1,
        Street = "667 Massachusetts Avenue",
        City = "Cambridge",
        State = "MA",
        Zip = "02139"
      };

      var address = Address.ParseAddressFromCsv
      (
        "1,667 Massachusetts Avenue,Cambridge,MA,02139"
      );

      Assert.True(address.Equals(correctAddress));
    }

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