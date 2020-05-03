﻿using System.Linq;
using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Shared.Components;
using Xunit;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Tests.Shared.Components
{
    public class BadgeTests : BlazorTestBase
    {
        [Fact]
        public void BadgeEmpty()
        {
            var cut = RenderComponent<Badge>();
            Assert.Empty(cut.Nodes);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(null, false)]
        [InlineData(1, true)]
        public void BadgeWorksShown(int? value, bool shown)
        {
            var cut = RenderComponent<Badge>(
                (nameof(Badge.Number), value));
            Assert.Equal(shown, cut.Nodes.Count() != 0);
        }
    }
}