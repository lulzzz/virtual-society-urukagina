﻿using Microsoft.AspNetCore.Components;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Shared.Components.FormElements
{
    public partial class Label
    {
        [Parameter]
        public string Name { get; set; }
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public string TagText { get; set; }

        private bool _showTag => !string.IsNullOrWhiteSpace(TagText);
    }
}
