﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RapidPayTest.Application.DTOs.Settings
{
    public class AuthenticationResponse
    {
        public string JWToken { get; set; }
        public DateTime Expiration { get; set; }
        public bool? ValidationContrato { get; set; }
    }
}
