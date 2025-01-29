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

data "azurerm_storage_account_sas" "sas" {
  connection_string = azurerm_storage_account.logs.primary_connection_string

  https_only = true
  start       = timestamp()
  expiry      = timeadd(timestamp(), "168h")

  permissions     = "rw"
  resource_types  = "sco"
  services        = "b"
}

locals {
  sas_url = "https://${azurerm_storage_account.logs.name}.blob.core.windows.net/${azurerm_storage_container.logs_container.name}?${data.azurerm_storage_account_sas.sas.sas}"
}
