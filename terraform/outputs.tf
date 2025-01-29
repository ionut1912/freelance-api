output "container_registry_url" {
  value = azurerm_container_registry.acr.login_server
}

output "app_service_url" {
  value = azurerm_linux_web_app.app_service.default_hostname
}
