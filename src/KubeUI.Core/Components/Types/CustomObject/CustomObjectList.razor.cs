﻿using k8s;
using k8s.Models;
using KubeUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace KubeUI.Core.Components.Types
{
    public partial class CustomObjectList : IDisposable
    {
        [Inject]
        protected IKubernetes Client { get; set; }

        [Parameter]
        public string Namespace { get; set; }

        [Parameter]
        public string Group { get; set; }

        [Parameter]
        public string Version { get; set; }

        [Parameter]
        public string Plural { get; set; }

        private List<JObject> Items = new List<JObject>();

        private Watcher<JObject> watcher;

        protected override void OnParametersSet()
        {
            Task<HttpOperationResponse<object>> task;

            if (Namespace?.Equals(State.AllNameSpace) != false)
            {
                task = Client.ListClusterCustomObjectWithHttpMessagesAsync(Group, Version, Plural, watch: true);
            }
            else
            {
                task = Client.ListNamespacedCustomObjectWithHttpMessagesAsync(Group, Version, Namespace, Plural, watch: true);
            }

            watcher = task.Watch<JObject, object> ((type, item) =>
            {
                switch (type)
                {

                    case WatchEventType.Added:
                        if (!Items.Any(x => (x as JObject)["metadata"]["uid"].Value<string>() == (item as JObject)["metadata"]["uid"].Value<string>()))
                            Items.Add(item);
                        else
                            Items[Items.FindIndex(x => (x as JObject)["metadata"]["uid"].Value<string>() == (item as JObject)["metadata"]["uid"].Value<string>())] = item;
                        break;
                        //case WatchEventType.Modified:
                        //    Items[Items.FindIndex(x => x.Metadata.Uid == item.Metadata.Uid)] = item;
                        //    break;
                        //case WatchEventType.Deleted:
                        //    Items.RemoveAt(Items.FindIndex(x => x.Metadata.Uid == item.Metadata.Uid));
                        //    break;
                        //case WatchEventType.Error:
                        //    break;
                        //default:
                        //    break;
                }
                StateHasChanged();
            });
        }

        private async Task Delete(object crd)
        {
            //await Client.DeleteClusterCustomObjectAsync(crd.Metadata.Name);
        }

        public void Dispose()
        {
            watcher?.Dispose();
        }
    }
}
