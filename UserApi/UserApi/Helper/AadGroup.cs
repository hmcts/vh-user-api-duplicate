﻿using System.ComponentModel.DataAnnotations;

namespace UserApi.Helper
{
    public enum AadGroup
    {
        [Display(Name = "VirtualRoomAdministrator")]
        VirtualRoomAdministrator = 1,
        [Display(Name = "External")] External = 2,
        [Display(Name = "Internal")] Internal = 3,
        [Display(Name = "MoneyClaims")] MoneyClaims = 4,
        [Display(Name = "FinancialRemedy")] FinancialRemedy = 5,
        [Display(Name = "SSPR Enabled")] SsprEnabled = 6,
        [Display(Name = "UserApiTestGroup")] UserApiTestGroup = 7,
        [Display(Name = "NewUserApiTestGroup")] NewUserApiTestGroup = 8

    }
}