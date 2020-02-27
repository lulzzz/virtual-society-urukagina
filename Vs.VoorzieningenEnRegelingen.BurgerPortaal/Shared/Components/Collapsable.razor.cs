﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Shared.Components
{
    public partial class Collapsable
    {
        [Parameter]
        public string Name { get; set; }
        [Parameter]
        public string CollapsedText { get; set; } = "ingeklapt";
        [Parameter]
        public string UnfoldedText { get; set; } = "uitgeklapt";
        [Parameter]
        public RenderFragment Title { get; set; }
        [Parameter]
        public RenderFragment CollapsableContent { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await InitCollapsable();
        }

        public async Task InitCollapsable()
        {
            await JSRuntime.InvokeAsync<string>("InitCollapsable");
        }

    }
}