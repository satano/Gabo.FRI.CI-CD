using './main.bicep'

param deploymentEnvironment = 'test'

param appService_ASPNETCORE_ENVIRONMENT = 'Test'
param appServicePlanSku = {
  name: 'B1'
  tier: 'Basic'
}
