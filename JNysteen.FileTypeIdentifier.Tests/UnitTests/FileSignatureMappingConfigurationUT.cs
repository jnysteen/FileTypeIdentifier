using System;
using System.Linq;
using JNysteen.FileTypeIdentifier.Exceptions;
using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class FileSignatureMappingConfigurationUT
    {
        [Fact]
        public void AddFileSignature_FileSignatureEmpty_Negative()
        {
            var fileSignature = new byte?[0];
            var fileType = "test";

            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);

            Assert.Throws<ArgumentException>(() => fileSignatureMapping.AddFileSignature(fileSignature, fileType));
        }

        [Fact]
        public void AddFileSignature_FileSignatureNull_Negative()
        {
            byte?[] fileSignature = null;
            var fileType = "test";

            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);

            Assert.Throws<ArgumentNullException>(() => fileSignatureMapping.AddFileSignature(fileSignature, fileType));
        }

        [Fact]
        public void AddFileSignature_FileTypeEmpty_Negative()
        {
            var fileSignature = new byte?[] {1, null, 3};
            var fileType = " ";

            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);

            Assert.Throws<ArgumentException>(() => fileSignatureMapping.AddFileSignature(fileSignature, fileType));
        }

        [Fact]
        public void AddFileSignature_FileTypeNull_Negative()
        {
            var fileSignature = new byte?[] {1, null, 3};
            string fileType = null;

            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);

            Assert.Throws<ArgumentNullException>(() => fileSignatureMapping.AddFileSignature(fileSignature, fileType));
        }

        [Fact]
        public void AddFileSignature_Positive()
        {
            var fileSignature = new byte?[] {1, null, 3};
            var fileType = "test";

            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);

            fileSignatureMapping.AddFileSignature(fileSignature, fileType);

            Assert.Single(fileSignatureMapping.FileSignaturesMappingTable);
        }

        [Fact]
        public void GetLongestFileSignature_NoSignaturesDefined_Negative()
        {
            var fileSignatureMapping = new FileSignatureMapping();
            Assert.Empty(fileSignatureMapping.FileSignaturesMappingTable);
            Assert.Throws<InvalidConfigurationException>(() => fileSignatureMapping.GetLongestSignature());
        }

        [Fact]
        public void GetLongestFileSignature_Positive()
        {
            var longestSignatureToAdd = 10;
            var fileSignatureMapping = new FileSignatureMapping();

            for (var i = 1; i <= longestSignatureToAdd; i++)
            {
                var signature = Enumerable.Repeat(new byte?(1), i).ToArray();
                var type = i.ToString();
                fileSignatureMapping.AddFileSignature(signature, type);
            }

            var longestSignatureRetrieved = fileSignatureMapping.GetLongestSignature();
            Assert.Equal(longestSignatureToAdd, longestSignatureRetrieved);
        }
    }
}