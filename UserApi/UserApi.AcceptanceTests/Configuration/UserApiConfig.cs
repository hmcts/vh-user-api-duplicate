﻿using Testing.Common.Configuration;
using UserApi.Common;

namespace UserApi.AcceptanceTests.Configuration
{
    public class UserApiConfig
    {
        public AzureAdConfiguration AzureAdConfiguration { get; set; }
        public TestSettings TestConfig { get; set; }
        public VhServicesConfig VhServices { get; set; }
    }
}
