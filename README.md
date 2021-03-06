# [KubeUI.com](https://KubeUI.com)

![](https://github.com/IvanJosipovic/KubeUI/workflows/CI/CD/badge.svg)

## What is this?
KubeUI is a experimental user interface for Kubernetes. It's built using [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) and the [official C# Kubernetes Client](https://github.com/kubernetes-client/csharp).

![](KubeUI.gif)

## How to run?


### Web
**Please Note:** As this version of KubeUI operates in the browser, it's limited by [CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS).

There are two options to get around CORS:
1. Download KubeUI Desktop

2. Kubectl Proxy + API Server change
    - kubectl proxy
    - Add the following line to the [Kube API Server](https://kubernetes.io/docs/reference/command-line-tools-reference/kube-apiserver/) configuration:
      - --cors-allowed-origins=https://KubeUI.com


### Desktop
**Please Note:** Windows users will need to install the new [Edge (Chromium)](https://www.microsoft.com/en-us/edge) or be on Windows 10 build 2004 or higher.

Go to [Releases](https://github.com/IvanJosipovic/KubeUI/releases) and download the version for your OS.

## How to build?

1. [Download .Net Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. [Download an IDE](https://dotnet.microsoft.com/platform/tools)
3. Build away!


### PRs are welcome!