@allowed([
  'test'
  'prod'
])
@description('The environment to create infrastructure for.')
param deploymentEnvironment string = 'test'

@description('Prefix for resource names.')
param prefix string = ''

@description('The location for resources.')
param location string = resourceGroup().location

@description('App service plan SKU definition')
param appServicePlanSku appServicePlanSkus = {
  name: 'B1'
  tier: 'Basic'
}

@allowed([
  'Development'
  'Test'
  'Production'
])
@description('ASPNETCORE_ENVIRONMENT value for app services')
param appService_ASPNETCORE_ENVIRONMENT string = 'Development'

@description('URLs of the allowed CORS origins.')
param corsOrigins string[] = []

// User defined types
type appServicePlanSkus = { name: 'B1', tier: 'Basic' } | { name: 'S1', tier: 'Standard' }

// App service plan
resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: '${prefix}fri-contacts-${deploymentEnvironment}-asp'
  location: location
  sku: appServicePlanSku
  kind: 'app'
}

var defaultAppSettings = [
  {
    name: 'WEBSITE_ENABLE_SYNC_UPDATE_SITE'
    value: 'true'
  }
  {
    name: 'WEBSITE_RUN_FROM_PACKAGE'
    value: '1'
  }
  {
    name: 'XDT_MicrosoftApplicationInsights_Mode'
    value: 'default'
  }
]

var siteProperties_api = {
  serverFarmId: appServicePlan.id
  clientAffinityEnabled: false
  httpsOnly: true
  publicNetworkAccess: 'Enabled'
  siteConfig: {
    netFrameworkVersion: 'v9.0'
    metadata: [
      {
        name: 'CURRENT_STACK'
        value: 'dotnet'
      }
    ]
    appSettings: union(
      defaultAppSettings,
      [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: appService_ASPNETCORE_ENVIRONMENT
        }
      ]
    )
  }
}

var siteProperties_webapp = {
  serverFarmId: appServicePlan.id
  clientAffinityEnabled: false
  httpsOnly: true
  publicNetworkAccess: 'Enabled'
  siteConfig: {
    nodeVersion: '~20'
    appSettings: union(
      defaultAppSettings,
      [
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION'
          value: '~20'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_NodeJS'
          value: '1'
        }
      ]
    )
  }
}

var webConfig_default = {
  alwaysOn: true
  ftpsState: 'Disabled'
  http20Enabled: true
  use32BitWorkerProcess: true
  webSocketsEnabled: false
}

var webConfig_api = union(
  webConfig_default,
  {
    netFrameworkVersion: 'v9.0'
    cors: {
      allowedOrigins: corsOrigins
    }
  }
)

var webConfig_webapp = union(
  webConfig_default,
  {
    nodeVersion: '~20'
  }
)

resource api 'Microsoft.Web/sites@2024-04-01' = {
  name: '${prefix}fri-contacts-${deploymentEnvironment}-api'
  location: location
  kind: 'app'
  properties: siteProperties_api

  resource webConfig 'config' = {
    name: 'web'
    properties: webConfig_api
  }
}

resource webAppPayrolls 'Microsoft.Web/sites@2023-01-01' = {
  name: '${prefix}fri-contacts-${deploymentEnvironment}-app'
  location: location
  kind: 'app'
  properties: siteProperties_webapp

  resource webConfig 'config' = {
    name: 'web'
    properties: webConfig_webapp
  }
}
