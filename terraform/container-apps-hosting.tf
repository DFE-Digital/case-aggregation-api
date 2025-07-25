module "azure_container_apps_hosting" {
  source = "github.com/DFE-Digital/terraform-azurerm-container-apps-hosting?ref=v1.19.1"

  environment    = local.environment
  project_name   = local.project_name
  azure_location = local.azure_location
  tags           = local.tags

  enable_container_registry             = local.enable_container_registry
  registry_admin_enabled                = local.registry_admin_enabled
  registry_use_managed_identity         = local.registry_use_managed_identity
  registry_managed_identity_assign_role = local.registry_managed_identity_assign_role
  registry_server                       = local.registry_server

  enable_dns_zone      = local.enable_dns_zone
  dns_zone_domain_name = local.dns_zone_domain_name
  dns_ns_records       = local.dns_ns_records
  dns_txt_records      = local.dns_txt_records
  dns_mx_records       = local.dns_mx_records

  image_name                             = local.image_name
  image_tag                              = local.image_tag
  container_command                      = local.container_command
  container_secret_environment_variables = local.container_secret_environment_variables
  container_scale_http_concurrency       = local.container_scale_http_concurrency
  container_apps_allow_ips_inbound       = local.container_apps_allow_ips_inbound
  container_min_replicas                 = local.container_min_replicas
  container_port                         = local.container_port
  enable_health_insights_api             = local.enable_health_insights_api
  health_insights_api_cors_origins       = local.health_insights_api_cors_origins
  health_insights_api_ipv4_allow_list    = local.health_insights_api_ipv4_allow_list

  existing_container_app_environment = local.existing_container_app_environment
  existing_virtual_network           = local.existing_virtual_network
  existing_resource_group            = local.existing_resource_group
  launch_in_vnet                     = false
  container_app_name_override        = "case-api"

  enable_cdn_frontdoor                            = local.enable_cdn_frontdoor
  cdn_frontdoor_forwarding_protocol               = local.cdn_frontdoor_forwarding_protocol
  cdn_frontdoor_enable_rate_limiting              = local.cdn_frontdoor_enable_rate_limiting
  cdn_frontdoor_rate_limiting_duration_in_minutes = local.cdn_frontdoor_rate_limiting_duration_in_minutes
  cdn_frontdoor_rate_limiting_threshold           = local.cdn_frontdoor_rate_limiting_threshold
  cdn_frontdoor_host_add_response_headers         = local.cdn_frontdoor_host_add_response_headers
  cdn_frontdoor_custom_domains                    = local.cdn_frontdoor_custom_domains
  cdn_frontdoor_origin_fqdn_override              = local.cdn_frontdoor_origin_fqdn_override
  cdn_frontdoor_origin_host_header_override       = local.cdn_frontdoor_origin_host_header_override
  enable_cdn_frontdoor_vdp_redirects              = local.enable_cdn_frontdoor_vdp_redirects
  cdn_frontdoor_vdp_destination_hostname          = local.cdn_frontdoor_vdp_destination_hostname
  cdn_frontdoor_waf_custom_rules                  = local.cdn_frontdoor_waf_custom_rules
  enable_cdn_frontdoor_health_probe               = local.enable_cdn_frontdoor_health_probe

  enable_event_hub                          = local.enable_event_hub
  enable_logstash_consumer                  = local.enable_logstash_consumer
  eventhub_export_log_analytics_table_names = local.eventhub_export_log_analytics_table_names

  enable_monitoring               = local.enable_monitoring
  monitor_email_receivers         = local.monitor_email_receivers
  container_health_probe_path     = local.container_health_probe_path
  cdn_frontdoor_health_probe_path = local.cdn_frontdoor_health_probe_path
  monitor_endpoint_healthcheck    = local.monitor_endpoint_healthcheck
  existing_logic_app_workflow     = local.existing_logic_app_workflow
  monitor_http_availability_verb  = local.monitor_http_availability_verb
  monitor_http_availability_fqdn  = local.monitor_http_availability_fqdn
  enable_monitoring_traces        = local.enable_monitoring_traces
}
