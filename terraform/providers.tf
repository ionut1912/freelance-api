terraform {
  backend "azurerm" {
    resource_group_name  = "terraform-tf-state"
    storage_account_name = "terraformdetails"
    container_name       = "terraform-data"
    key                  = "terraform.tfstate"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }

  required_version = ">= 1.0"
}

provider "azurerm" {
  features {}
}