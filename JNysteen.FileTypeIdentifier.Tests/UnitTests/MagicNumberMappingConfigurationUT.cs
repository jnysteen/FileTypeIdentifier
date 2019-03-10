using System;
using System.Linq;
using JNysteen.FileTypeIdentifier.Exceptions;
using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class MagicNumberMappingConfigurationUT
    {
        [Fact]
        public void AddMagicNumber_MagicNumberEmpty_Negative()
        {
            var magicNumber = new byte?[0];
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentException>(() => magicNumberMapping.AddMagicNumber(magicNumber, fileType));
        }
        
        [Fact]
        public void AddMagicNumbers_MagicNumberEmpty_Negative()
        {
            var magicNumbers = new byte?[][] {new byte?[0]};
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentException>(() => magicNumberMapping.AddMagicNumbers(magicNumbers, fileType));
        }

        [Fact]
        public void AddMagicNumber_MagicNumberNull_Negative()
        {
            byte?[] magicNumber = null;
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentNullException>(() => magicNumberMapping.AddMagicNumber(magicNumber, fileType));
        }
        
        [Fact]
        public void AddMagicNumbers_MagicNumberNull_Negative()
        {
            var magicNumbers = new byte?[][]{null};
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentNullException>(() => magicNumberMapping.AddMagicNumbers(magicNumbers, fileType));
        }

        [Fact]
        public void AddMagicNumber_FileTypeEmpty_Negative()
        {
            var magicNumber = new byte?[] {1, null, 3};
            var fileType = " ";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentException>(() => magicNumberMapping.AddMagicNumber(magicNumber, fileType));
        }
        
        [Fact]
        public void AddMagicNumbers_FileTypeEmpty_Negative()
        {
            var magicNumbers = new byte?[][] {new byte?[] {1, null, 3}};
            var fileType = " ";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentException>(() => magicNumberMapping.AddMagicNumbers(magicNumbers, fileType));
        }

        [Fact]
        public void AddMagicNumber_FileTypeNull_Negative()
        {
            var magicNumber = new byte?[] {1, null, 3};
            string fileType = null;

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentNullException>(() => magicNumberMapping.AddMagicNumber(magicNumber, fileType));
        }
        
        [Fact]
        public void AddMagicNumbers_FileTypeNull_Negative()
        {
            var magicNumbers = new byte?[][] {new byte?[] {1, null, 3}};
            string fileType = null;

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            Assert.Throws<ArgumentNullException>(() => magicNumberMapping.AddMagicNumbers(magicNumbers, fileType));
        }

        [Fact]
        public void AddMagicNumber_Positive()
        {
            var magicNumber = new byte?[] {1, null, 3};
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            magicNumberMapping.AddMagicNumber(magicNumber, fileType);

            Assert.Single(magicNumberMapping.FileMagicNumberMappingTable);
        }
        
        [Fact]
        public void GetLongestMagicNumber_Positive()
        {
            var longestMagicNumberToAdd = 10;
            var fileMagicNumberMapping = new MagicNumberMapping();

            for (var i = 1; i <= longestMagicNumberToAdd; i++)
            {
                var magicNumber = Enumerable.Repeat(new byte?(1), i).ToArray();
                var type = i.ToString();
                fileMagicNumberMapping.AddMagicNumber(magicNumber, type);
            }

            var longestMagicNumber = fileMagicNumberMapping.GetLongestMagicNumber();
            Assert.Equal(longestMagicNumberToAdd, longestMagicNumber);
        }
        
        [Fact]
        public void AddMagicNumbers_Positive()
        {
            var magicNumbers = new byte?[][]{new byte?[]{1, null, 3}, new byte?[]{1, null, 4} } ;
            var fileType = "test";

            var magicNumberMapping = new MagicNumberMapping();
            Assert.Empty(magicNumberMapping.FileMagicNumberMappingTable);

            magicNumberMapping.AddMagicNumbers(magicNumbers, fileType);

            Assert.Equal(2, magicNumberMapping.FileMagicNumberMappingTable.Count);
        }

        [Fact]
        public void GetLongestMagicNumber_NoMagicNumberDefined_Negative()
        {
            var magicNumber = new MagicNumberMapping();
            Assert.Empty(magicNumber.FileMagicNumberMappingTable);
            Assert.Throws<InvalidConfigurationException>(() => magicNumber.GetLongestMagicNumber());
        }
    }
}