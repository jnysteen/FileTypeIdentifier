using System;
using System.Collections;
using System.Linq;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Exceptions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class MagicNumberMappingConfigurationUT
    {
        [Theory]
        public void AddMagicNumber_Positive()
        {
            // Arrange
            var magicNumberDefinition = new FileMagicNumberDefinition(ValidMagicNumber, "test");

            // Act
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.AddMagicNumberDefinition(magicNumberDefinition);

            // Assert
            magicNumberMapping.FileMagicNumberMappingTable.Should().HaveCount(1, "a mapping should have been added");
        }

        [Theory]
        public void AddMagicNumbers_Positive()
        {
            // Arrange
            var magicNumbers = new[] {new byte?[]{1, null, 3}, new byte?[]{1, null, 4} } ;
            var fileType = "test";
            var magicNumberDefinition = new FileMagicNumberDefinition(magicNumbers, "test");

            // Act
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.AddMagicNumberDefinition(magicNumberDefinition);

            // Assert
            magicNumberMapping.FileMagicNumberMappingTable.Should().HaveCount(2, "two mappings should have been added");
        }
        
        [Theory]
        public void GetLongestMagicNumber_Positive()
        {
            // Arrange
            var longestMagicNumberToAdd = 10;
            var fileMagicNumberMapping = new MagicNumberMapping();

            for (var i = 1; i <= longestMagicNumberToAdd; i++)
            {
                var magicNumber = Enumerable.Repeat(new byte?(1), i).ToArray();
                var type = i.ToString();
                fileMagicNumberMapping.AddMagicNumberDefinition(magicNumber, type);
            }

            // Act
            var longestMagicNumber = fileMagicNumberMapping.GetLongestMagicNumber();
            
            // Assert
            longestMagicNumber.Should().Be(longestMagicNumberToAdd);
        }

        [Theory]
        public void GetLongestMagicNumber_NoMagicNumberDefined_Negative()
        {
            // Arrange
            var magicNumber = new MagicNumberMapping();
            
            // Act
            Action act = () => magicNumber.GetLongestMagicNumber();
            
            // Assert
            act.Should().Throw<InvalidConfigurationException>("there are no magic numbers defined at all, hence no longest number");
        }
        
        [Theory]
        [TestCaseSource(nameof(AddMagicNumberNegativeTestCases))]
        public void AddMagicNumber_ArgumentException_Tests(byte?[] magicNumber, string fileType)
        {
            // Arrange
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.FileMagicNumberMappingTable.Should().BeEmpty("there should not be any existing mappings");
            
            // Act
            Action act = () => magicNumberMapping.AddMagicNumberDefinition(magicNumber, fileType);

            // Assert
            act.Should().Throw<ArgumentException>("the input is bad and should provoke an exception");
        }
        
        [Theory]
        [TestCaseSource(nameof(AddMagicNumberNegativeTestCases))]
        public void AddMagicNumbers_SingleElement_ArgumentException_Tests(byte?[] magicNumber, string fileType)
        {
            // Arrange
            var magicNumbers = new byte?[][] {magicNumber};
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.FileMagicNumberMappingTable.Should().BeEmpty("there should not be any existing mappings");

            // Act
            Action act = () => magicNumberMapping.AddMagicNumbers(magicNumbers, fileType);
            
            // Assert
            act.Should().Throw<ArgumentException>("the input is bad and should provoke an exception");
        }
        
        public static IEnumerable AddMagicNumberNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new object[]{new byte?[0], "test"}
                ).SetName("Empty magic number byte array");
                
                yield return new TestCaseData(
                    new object[]{null, "test"}
                ).SetName("Magic number byte array is null");
                
                yield return new TestCaseData(
                    new object[]{ValidMagicNumber, " "}
                ).SetName("File type is empty");
            }
        }

        public static byte?[] ValidMagicNumber = {1, null, 3};
    }
}