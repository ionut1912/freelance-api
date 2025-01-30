terraform {
  backend "azurerm" {
    resource_group_name   = "frelance-api"
    storage_account_name  = "frelanceterraformsa"
    container_name        = "terraform-state"
    key                   = "terraform.tfstate"
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
