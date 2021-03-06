﻿using KubeUI.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace KubeUI.Core.Pages
{
    public partial class Connect
    {
        [Inject]
        protected IState State { get; set; }

        private string Config { get; set; }

        [Inject]
        protected Updater Updater { get; set; }

        private Updater.GithubRelease GithubRelease;

        protected override async Task OnInitializedAsync()
        {
            GithubRelease = await Updater.GetRelease();
        }

        private void LoadConfig()
        {
            State.SetK8SConfiguration(Config);
        }
    }
}
