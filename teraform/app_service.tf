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
    always_on           = false
    app_command_line    = ""
    docker_registry_url = azurerm_container_registry.acr.login_server
    docker_image_name   = "${azurerm_container_registry.acr.login_server}/frelance-api:latest"
  }

  app_settings = {
    "DATABASE_CONNECTION_STRING" = azurerm_key_vault_secret.db_connection_string.value
    "DOCKER_REGISTRY_SERVER_URL" = azurerm_container_registry.acr.login_server
    "DOCKER_REGISTRY_SERVER_USERNAME" = azurerm_container_registry.acr.admin_username
    "DOCKER_REGISTRY_SERVER_PASSWORD" = azurerm_container_registry.acr.admin_password
  }
}
