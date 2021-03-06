﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KubeUI.Core.Components
{
    public partial class FieldValidationResult
    {
        [Parameter]
        public ValidationResult Results { get; set; }

        [Parameter]
        public string Field { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Maximum number of errors to display
        /// </summary>
        [Parameter]
        public int ErrorCount { get; set; } = 3;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IEnumerable<string> Errors
        {
            get
            {
                List<string> errors = new List<string>();

                if (Enabled)
                {
                    try
                    {
                        if (Results != null && !Results.IsValid)
                        {
                            foreach (var failure in Results.Errors
                                .Where(x => x.PropertyName == Field)
                                .Take(ErrorCount)
                                )
                            {
                                errors.Add(failure.ErrorMessage);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add($"Validator error: {e}");
                    }
                }

                return errors;
            }
        }
    }
}
