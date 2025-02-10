resource "azurerm_storage_account" "storage" {
  name                     = "usersprofiles"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = "dev"
  }
}

resource "azurerm_storage_container" "user_images" {
  name               = "userimages"
  storage_account_id = azurerm_storage_account.storage.id
  container_access_type = "blob"
}

resource "azurerm_storage_container" "invoices" {
  name               = "invoices"
  storage_account_id = azurerm_storage_account.storage.id
  container_access_type = "blob"
}