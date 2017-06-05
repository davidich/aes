using System.Linq;
using Xunit.Sdk;

namespace Xunit
{
    public partial class Assert
    {
        public static void EqualBytes(byte[] expectedBytes, byte[] actualBytes)
        {

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                if (expectedBytes[i] != actualBytes[i])
                {
                    var message = $"Expected bytes: {string.Join("_", expectedBytes.Select(k => k.ToString("X")))}\n" +
                                  $"Actual bytes  : {string.Join("_", actualBytes.Select(k => k.ToString("X")))}\n"+ 
                                  $"Mismatch fount at byte #{i}";

                    throw new AesKeyEqualException(expectedBytes[i], actualBytes[i], message, "Expected byte", "Actual byte  ");
                }
            }
        }
    }

    public class AesKeyEqualException : AssertActualExpectedException
    {
        public AesKeyEqualException(
            object expected, 
            object actual, 
            string userMessage, 
            string expectedTitle = null, 
            string actualTitle = null) : base(expected, actual, userMessage, expectedTitle, actualTitle)
        {
        }
    }
}
