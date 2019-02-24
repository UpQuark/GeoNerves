using System.Collections.Generic;
using GeoNerves.Models;

namespace GeoNerves
{
  public static class AddressChunkGenerator
  {
    public static List<List<Address>> SplitAddressChunks(AddressList addressList, int chunkSize)
    {
      var addressListChunked = new List<List<Address>>();
      var addressCount       = addressList.Addresses.Count;

      var remainder  = addressCount % chunkSize;
      var chunkCount = addressCount / chunkSize;

      if (remainder > 1)
        chunkCount++;

      for (var i = 0; i < chunkCount; i++)
      {
        // On last iteration, use remainder rather than chunkSize for addressCounts that don't evenly divide by chunkSize
        if (i == chunkCount - 1 && remainder > 0)
        {
          addressListChunked.Add(
            new List<Address>(addressList.Addresses.GetRange(i * chunkSize, remainder))
          );
        }
        else
        {
          addressListChunked.Add(
            new List<Address>(addressList.Addresses.GetRange(i * chunkSize, chunkSize))
          );
        }
      }

      return addressListChunked;
    }
  }
}