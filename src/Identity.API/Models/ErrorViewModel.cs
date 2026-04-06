// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace ICLAco.Identity.API.Models
{
    public record ErrorViewModel
    {
        public ErrorMessage Error { get; set; }
    }

    public record ErrorMessage
    {
        public string? Error { get; set; }
        public string? ErrorDescription { get; set; }
        public string? RequestId { get; set; }
    }
}
