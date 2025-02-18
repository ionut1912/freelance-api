resource "azurerm_storage_account" "storage" {
  name                     = "usersprofiles"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  # Terraform will create this if it doesn't already exist; 
  # if you have an existing storage account with the same name, import it.

  tags = {
    environment = "dev"
  }
}

resource "azurerm_storage_container" "user_images" {
  name                  = "userimagescontainer"
  storage_account_id    = azurerm_storage_account.storage.id
  container_access_type = "blob"

  # If this container already exists, import it; otherwise, Terraform creates it.
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