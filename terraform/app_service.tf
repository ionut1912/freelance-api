resource "azurerm_service_plan" "app_service_plan" {
  name                = "freelance-app-service-plan"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku_name            = var.app_service_plan_sku
  os_type             = "Linux"

  lifecycle {
    create_before_destroy = true
  }
}

resource "azurerm_linux_web_app" "app_service" {
  name                = "freelance-api"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.app_service_plan.id
  https_only          = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on                               = false
    container_registry_use_managed_identity = true
  }

  app_settings = {
    "DOCKER_CUSTOM_IMAGE_NAME"                  = "${azurerm_container_registry.acr.login_server}/freelance-api:latest",
    "AzureKeyVault__VaultUrl"                   = azurerm_key_vault.keyvault.vault_uri,
    "AzureKeyVault__ConnectionStringSecretName" = "db-connection-string",
    "AzureKeyVault__StorageConnectionString"    = "storage-connection-string",
    "AzureKeyVault__JWTTokenSecretName"         = "jwt-token-key",
    "AZURE_AUTHORITY_HOST"                      = "https://login.microsoftonline.com/",
    "AZURE_IDENTITY_DISABLE_IMDS"               = "0",
    "WEBSITES_CONTAINER_START_TIME_LIMIT"       = "1300",
    "PORT"                                      = "80",
    "WEBSITES_PORT"                             = "80"
  }
}
