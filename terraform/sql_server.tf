resource "azurerm_mssql_server" "sql_server" {
  name                = "frelance-db"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  version             = "12.0"
  administrator_login = azurerm_key_vault_secret.sql_admin_username.value
  administrator_login_password = azurerm_key_vault_secret.sql_admin_password.value
}

resource "azurerm_mssql_database" "sql_database" {
  name            = "frelance-db"
  server_id       = azurerm_mssql_server.sql_server.id
  sku_name        = var.sql_sku_name
  max_size_gb     = 2
  zone_redundant  = false
}
