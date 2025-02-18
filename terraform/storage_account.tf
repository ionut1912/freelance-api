resource "azurerm_storage_account" "storage" {
  name                     = "usersprofiles"
  resource_group_name      = var.resource_group_name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = "dev"
  }
}

resource "azurerm_storage_container" "user_images" {
  name                  = "userimagescontainer"
  storage_account_id    = azurerm_storage_account.storage.id
  container_access_type = "blob"
}

resource "azurerm_storage_container" "invoices" {
  name                  = "invoicescontainer"
  storage_account_id    = azurerm_storage_account.storage.id
  container_access_type = "blob"
}

resource "azurerm_storage_container" "contracts" {
  name                  = "contractscontainer"
  storage_account_id    = azurerm_storage_account.storage.id
  container_access_type = "blob"
}
