resource "azurerm_service_plan" "app_service_plan" {
  name                = "frelance-app-service-plan"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku_name            = var.app_service_plan_sku
  os_type             = "Linux"

  lifecycle {
    create_before_destroy = true
  }
}

resource "azurerm_linux_web_app" "app_service" {
  name                = "frelance-api"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.app_service_plan.id
  https_only          = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on        = false
    app_command_line = ""

    application_stack {
      docker_image     = "${azurerm_container_registry.acr.login_server}/frelance-api"
      docker_image_tag = "latest"
    }
  }

  app_settings = {
    "AzureKeyVault__VaultUrl"                   = "https://frelance-api-keyvault.vault.azure.net/"
    "AzureKeyVault__ConnectionStringSecretName" = "db-connection-string"
    "AzureKeyVault__JWTTokenSecretName"         = "jwt-token-key"
    "AZURE_AUTHORITY_HOST"                      = "https://login.microsoftonline.com/"
    "AZURE_IDENTITY_DISABLE_IMDS"               = "0"
    "DOCKER_REGISTRY_SERVER_URL"                = "https://${azurerm_container_registry.acr.login_server}"
    "DOCKER_REGISTRY_SERVER_USERNAME"           = azurerm_container_registry.acr.admin_username
    "DOCKER_REGISTRY_SERVER_PASSWORD"           = azurerm_container_registry.acr.admin_password
    "WEBSITES_CONTAINER_START_TIME_LIMIT"       = "900"
    "PORT"                                      = "80"
  }
}