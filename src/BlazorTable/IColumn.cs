﻿using Microsoft.AspNetCore.Components;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorTable
{
    public interface IColumn<TableItem>
    {
        string Title { get; set; }

        string Width { get; set; }

        bool Sortable { get; set; }

        bool Filterable { get; set; }

        bool FilterOpen { get; }

        void ToggleFilter();

        MemberInfo GetPropertyMemberInfo();

        Expression<Func<TableItem, object>> Property { get; set; }

        RenderFragment<TableItem> EditorTemplate { get; set; }

        RenderFragment<TableItem> Template { get; set; }
    }
}
