resource "azurerm_service_plan" "app_service_plan" {
  name                = "frelance-api-plan"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Linux"
  sku_name            = var.app_service_plan_sku
}

resource "azurerm_linux_web_app" "app_service" {
  name                = "frelance-api"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.app_service_plan.id

  site_config {
    always_on        = false
    app_command_line = ""
    container_registry {
      use_managed_identity = false
      server_url           = "https://${azurerm_container_registry.acr.login_server}"
      username             = azurerm_container_registry.acr.admin_username
      password_secret_name = "DOCKER_REGISTRY_SERVER_PASSWORD"
    }
  }

  app_settings = {
    "DATABASE_CONNECTION_STRING" = azurerm_key_vault_secret.db_connection_string.value
    "DOCKER_REGISTRY_SERVER_URL" = "https://${azurerm_container_registry.acr.login_server}"
    "DOCKER_REGISTRY_SERVER_USERNAME" = azurerm_container_registry.acr.admin_username
    "DOCKER_REGISTRY_SERVER_PASSWORD" = azurerm_container_registry.acr.admin_password
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = "1800"
  }
}
