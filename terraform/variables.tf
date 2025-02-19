variable "resource_group_name" { default = "freelance-api" }
variable "location" { default = "northeurope" }
variable "app_service_plan_sku" { default = "B1" }
variable "sql_sku_name" { default = "Basic" }
variable "sql_sku_tier" { default = "Basic" }
variable "sql_storage_mb" { default = 5120 }
