output "container_registry_url" {
  value = azurerm_container_registry.acr.login_server
}

output "app_service_url" {
  value = azurerm_linux_web_app.app_service.default_hostname
}

output "sas_url" {
  description = "The SAS URL for Blob Storage logging"
  value       = local.sas_url
  sensitive   = true
}
