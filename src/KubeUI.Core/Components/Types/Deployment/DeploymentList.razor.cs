﻿using k8s;
using k8s.Models;
using KubeUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KubeUI.Core.Components.Types
{
    public partial class DeploymentList : IDisposable
    {
        [Parameter]
        public string Namespace { get; set; }

        [Parameter]
        public Expression<Func<V1Deployment, bool>> Filter { get; set; }

        [Inject]
        protected ILogger<DeploymentList> Logger { get; set; }

        [Inject]
        protected IState State { get; set; }


        private List<V1Deployment> Items = new List<V1Deployment>();

        private Watcher<V1Deployment> watcher;

        protected override void OnParametersSet()
        {
            Task<HttpOperationResponse<V1DeploymentList>> task;

            if (Namespace == null)
            {
                task = State.Client.ListDeploymentForAllNamespacesWithHttpMessagesAsync(watch: true);
            }
            else
            {
                task = State.Client.ListNamespacedDeploymentWithHttpMessagesAsync(Namespace, watch: true);
            }

            watcher = task.Watch<V1Deployment, V1DeploymentList>((type, item) =>
            {
                switch (type)
                {
                    case WatchEventType.Added:
                        if (!Items.Any(x => x.Metadata.Uid == item.Metadata.Uid))
                            Items.Add(item);
                        else
                            Items[Items.FindIndex(x => x.Metadata.Uid == item.Metadata.Uid)] = item;
                        break;
                    case WatchEventType.Modified:
                        Items[Items.FindIndex(x => x.Metadata.Uid == item.Metadata.Uid)] = item;
                        break;
                    case WatchEventType.Deleted:
                        Items.RemoveAt(Items.FindIndex(x => x.Metadata.Uid == item.Metadata.Uid));
                        break;
                    case WatchEventType.Error:
                        break;
                    default:
                        break;
                }
                StateHasChanged();
            });
        }

        private async Task Delete(V1Deployment item)
        {
            await State.Client.DeleteNamespacedDeploymentAsync(item.Metadata.Name, item.Metadata.NamespaceProperty);
        }

        private async Task ScaleUp(V1Deployment item)
        {
            var patch = new JsonPatchDocument<V1Deployment>();
            patch.Replace(e => e.Spec.Replicas, item.Spec.Replicas.GetValueOrDefault() + 1);

            await State.Client.PatchNamespacedDeploymentScaleAsync(new V1Patch(patch), item.Metadata.Name, item.Metadata.NamespaceProperty);
        }

        private async Task ScaleDown(V1Deployment item)
        {
            if (!item.Spec.Replicas.HasValue || item.Spec.Replicas.Value == 0)
            {
                return;
            }

            var patch = new JsonPatchDocument<V1Deployment>();
            patch.Replace(e => e.Spec.Replicas, item.Spec.Replicas.Value - 1);

            await State.Client.PatchNamespacedDeploymentScaleAsync(new V1Patch(patch), item.Metadata.Name, item.Metadata.NamespaceProperty);
        }

        public void Dispose()
        {
            watcher?.Dispose();
        }
    }
}
