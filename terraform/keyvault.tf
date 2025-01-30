resource "azurerm_key_vault_secret" "jwt_token_key" {
  name         = "jwt-token-key"
  value        = "this is a secret key"
  key_vault_id = azurerm_key_vault.keyvault.id

  lifecycle {
    ignore_changes = [value]
  }
}

resource "azurerm_key_vault_access_policy" "user_access" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = "5c384fed-84cc-44a6-b34a-b060bf102a6e"
  object_id    = "fe4c5ebe-84a4-4bea-9a09-57fbfe9b0bcb"

  secret_permissions = ["Get", "List", "Set", "Delete", "Purge", "Recover"]
  key_permissions    = ["Get", "List"]
  certificate_permissions = ["Get", "List"]
}
