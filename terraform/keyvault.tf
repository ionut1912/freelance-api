resource "azurerm_key_vault" "keyvault" {
  name                = "freelance-api-keyvault"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku_name            = "standard"
  tenant_id           = "5c384fed-84cc-44a6-b34a-b060bf102a6e"
}

resource "azurerm_key_vault_access_policy" "app_service_access" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = azurerm_key_vault.keyvault.tenant_id
  object_id    = "df66e3ec-c251-4294-8587-4721f1ac225e"

  secret_permissions      = ["Get", "List"]
  key_permissions         = ["Get", "List"]
  certificate_permissions = ["Get", "List"]
}

resource "azurerm_key_vault_secret" "jwt_token_key" {
  name         = "jwt-token-key"
  value        = "this is a secret key and needs to be a long string"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_access_policy" "user_access" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = "5c384fed-84cc-44a6-b34a-b060bf102a6e"
  object_id    = "0f7db1d5-76d4-4759-91ea-7d335f63e491"

  secret_permissions      = ["Get", "List", "Set", "Delete", "Purge", "Recover"]
  key_permissions         = ["Get", "List"]
  certificate_permissions = ["Get", "List"]
}

resource "azurerm_key_vault_secret" "sql_admin_username" {
  name         = "sql-admin-username"
  value        = "adminuser"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "sql_admin_password" {
  name         = "sql-admin-password"
  value        = "SuperSecurePass123!"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "db_connection_string" {
  name         = "db-connection-string"
  value        = "Server=tcp:${azurerm_mssql_server.sql_server.fully_qualified_domain_name},1433;Database=${azurerm_mssql_database.sql_database.name};User ID=${azurerm_key_vault_secret.sql_admin_username.value};Password=${azurerm_key_vault_secret.sql_admin_password.value};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "storage_connection_string" {
  name         = "storage-connection-string"
  value        = "DefaultEndpointsProtocol=https;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
  key_vault_id = azurerm_key_vault.keyvault.id

  depends_on = [azurerm_storage_account.storage]
}
