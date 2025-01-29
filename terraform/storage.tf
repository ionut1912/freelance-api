resource "azurerm_storage_account" "logs" {
  name                     = "frelancelogsacct"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_blob_public_access = false
  min_tls_version          = "TLS1_2"
}

resource "azurerm_storage_container" "logs_container" {
  name                  = "webapp-logs"
  storage_account_name  = azurerm_storage_account.logs.name
  container_access_type = "private"
}

resource "azurerm_storage_account_sas" "sas" {
  storage_account_name = azurerm_storage_account.logs.name
  https_only           = true
  start                = timestamp()
  expiry               = timeadd(timestamp(), "168h")

  resource_types = ["service", "container", "object"]
  services       = ["b"]
  permissions    = ["rw"]
}

locals {
  sas_url = "https://${azurerm_storage_account.logs.name}.blob.core.windows.net/${azurerm_storage_container.logs_container.name}?${azurerm_storage_account_sas.sas.sas}"
}
