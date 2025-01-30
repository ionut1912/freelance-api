resource "azurerm_storage_account" "terraform_storage" {
  name                     = "frelanceterraformsa"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = "West Europe"
  account_tier             = "Standard"
  account_replication_type = "LRS"
  min_tls_version          = "TLS1_2"
}

resource "azurerm_storage_container" "terraform_state" {
  name                  = "terraform-state"
  storage_account_name  = azurerm_storage_account.terraform_storage.name
  container_access_type = "private"
}
