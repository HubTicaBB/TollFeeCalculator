using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTest
{
    class MockFileReader : IFileReader
    {
        public string Read(string path)
        {            
            if (path == "invalidPath")
            {
                throw new FileNotFoundException();
            }
            return "File Data";
        }
    }
}
