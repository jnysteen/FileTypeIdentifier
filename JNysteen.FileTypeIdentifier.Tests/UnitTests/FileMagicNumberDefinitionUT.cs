using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class FileMagicNumberDefinitionUT
    {
        [Test]
        [TestCaseSource(nameof(FileMagicNumberDefinitionCreation_SingleNumber_BadArguments))]
        public void FileMagicNumberDefinitionCreationTests_SingleNumber_ArgumentExceptionExpected(byte?[] magicNumber, string fileType)
        {
            Action act = () => new FileMagicNumberDefinition(fileType, magicNumber);
            act.Should().Throw<ArgumentException>();
        }

        public static IEnumerable FileMagicNumberDefinitionCreation_SingleNumber_BadArguments
        {
            get
            {
                yield return new TestCaseData(
                    new object[]
                    {
                        null, null
                    }
                    ).SetName("Magic number and file type are both null");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[]{1, 2, 3}, null
                    }
                ).SetName("Magic number is valid, file type is null");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        null, "test"
                    }
                ).SetName("Magic number is null, file type is valid");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[]{}, "test"
                    }
                ).SetName("Magic number is empty, file type is valid");
            }
        }
    }
}