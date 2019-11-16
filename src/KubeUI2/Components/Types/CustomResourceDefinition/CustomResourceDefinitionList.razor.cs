﻿using k8s;
using k8s.Models;
using KubeUI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KubeUI2.Components.Types
{
    public partial class CustomResourceDefinitionList
    {
        [Inject]
        protected IState State { get; set; }

        [Inject]
        protected IKubernetes Client { get; set; }

        private IList<V1CustomResourceDefinition> Items;

        protected override async Task OnInitializedAsync()
        {
            await Update();
        }

        private async Task Update()
        {
            Items = (await Client.ListCustomResourceDefinitionAsync())?.Items;
        }

        public async Task Delete(V1CustomResourceDefinition crd)
        {
            await Client.DeleteCustomResourceDefinitionAsync(crd.Metadata.Name);

            await Update();
        }
    }
}