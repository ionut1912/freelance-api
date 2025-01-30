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
    "DATABASE_CONNECTION_STRING"          = azurerm_key_vault_secret.db_connection_string.value
    "JWT_TOKEN_KEY"                        = azurerm_key_vault_secret.jwt_token_key.value
    "DOCKER_REGISTRY_SERVER_URL"          = "https://${azurerm_container_registry.acr.login_server}"
    "DOCKER_REGISTRY_SERVER_USERNAME"     = azurerm_container_registry.acr.admin_username
    "DOCKER_REGISTRY_SERVER_PASSWORD"     = azurerm_container_registry.acr.admin_password
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = "1800"
  }
}

resource "azurerm_role_assignment" "keyvault_secret_access" {
  scope                = azurerm_key_vault.keyvault.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_linux_web_app.app_service.identity[0].principal_id
}
