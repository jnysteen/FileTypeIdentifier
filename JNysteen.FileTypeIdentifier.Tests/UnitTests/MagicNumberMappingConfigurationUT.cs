using System;
using System.Collections;
using System.Linq;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Exceptions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    internal class MagicNumberMappingConfigurationUT
    {
        [Theory]
        public void AddMagicNumber_Positive()
        {
            // Arrange
            var magicNumberDefinition = new FileMagicNumberDefinition("test", ValidMagicNumber);

            // Act
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.AddMagicNumberDefinition(magicNumberDefinition);

            // Assert
            magicNumberMapping.FileMagicNumberMappingTable.Should().HaveCount(1, "a mapping should have been added");
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
                var def = new FileMagicNumberDefinition(type, magicNumber);
                fileMagicNumberMapping.AddMagicNumberDefinition(def);
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
        public void AddMagicNumber_ArgumentException_Tests(FileMagicNumberDefinition definition)
        {
            // Arrange
            var magicNumberMapping = new MagicNumberMapping();
            magicNumberMapping.FileMagicNumberMappingTable.Should().BeEmpty("there should not be any existing mappings");
            
            // Act
            Action act = () => magicNumberMapping.AddMagicNumberDefinition(definition);

            // Assert
            act.Should().Throw<ArgumentException>("the input is bad and should provoke an exception");
        }
        
        public static IEnumerable AddMagicNumberNegativeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new object[]{null}
                ).SetName("Magic number definition is null");
            }
        }

        public static byte?[] ValidMagicNumber = {1, null, 3};
    }
}