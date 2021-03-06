﻿using Vs.BurgerPortaal.Core.Objects.FormElements;
using Xunit;

namespace Vs.BurgerPortaal.Core.Tests.Objects.FormElements
{
    public class TextFormElementDataTests
    {
        [Fact]
        public void CheckValidEmptyFromParent()
        {
            var sut = new TextFormElementData();
            sut.CustomValidate();
            Assert.False(sut.IsValid);
            Assert.Equal("Vul een waarde in.", sut.ErrorText);
            sut.Value = string.Empty;
            sut.CustomValidate();
            Assert.False(sut.IsValid);
            Assert.Equal("Vul een waarde in.", sut.ErrorText);
            sut.Value = " ";
            sut.CustomValidate();
            Assert.False(sut.IsValid);
            Assert.Equal("Vul een waarde in.", sut.ErrorText);
            sut.Value = "\t";
            sut.CustomValidate();
            Assert.False(sut.IsValid);
            Assert.Equal("Vul een waarde in.", sut.ErrorText);
        }

        [Fact]
        public void CheckValidFilledDouble()
        {
            var sut = new TextFormElementData();
            sut.Value = "test";
            sut.CustomValidate();
            Assert.True(sut.IsValid);
            Assert.Empty(sut.ErrorText);
        }
    }
}
