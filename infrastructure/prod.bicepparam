using './main.bicep'

param deploymentEnvironment = 'prod'

param appService_ASPNETCORE_ENVIRONMENT = 'Production'
param appServicePlanSku = {
  name: 'S1'
  tier: 'Standard'
}
