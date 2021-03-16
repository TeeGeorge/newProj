﻿using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using FirstBoilerPlateApp.Configuration.Dto;

namespace FirstBoilerPlateApp.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FirstBoilerPlateAppAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
