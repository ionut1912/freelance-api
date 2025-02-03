output "container_registry_url" {
  value = azurerm_container_registry.acr.login_server
}

output "app_service_url" {
  value = azurerm_linux_web_app.app_service.default_hostname
}

output "app_service_client_id" {
  value       = azurerm_linux_web_app.app_service.identity[0].principal_id
  description = "Client ID of the App Service Managed Identity"
}