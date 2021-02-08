﻿using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;

namespace eg_03_csharp_auth_code_grant_core.Examples
{
    public static class CreateBrand
    {
        /// <summary>
        /// Creates a brand
        /// </summary>
        /// <param name="brandName">The name of new brand</param>
        /// <param name="defaultBrandLanguage">Default brand's language</param>
        /// <param name="accessToken">Access Token for API call (OAuth)</param>
        /// <param name="basePath">BasePath for API calls (URI)</param>
        /// <param name="accountId">The DocuSign Account ID (GUID or short version) for which the APIs call would be made</param>
        /// <returns>A brand</returns>
        public static BrandsResponse Create(string brandName, string defaultBrandLanguage, string accessToken, string basePath, string accountId)
        {
            // Construct your API headers
            var config = new Configuration(new ApiClient(basePath));
            config.AddDefaultHeader("Authorization", "Bearer " + accessToken);

            // Construct your request body
            Brand newBrand = new Brand
            {
                BrandName = brandName,
                DefaultBrandLanguage = defaultBrandLanguage
            };

            // Call the eSignature REST API
            AccountsApi accountsApi = new AccountsApi(config);

            return accountsApi.CreateBrand(accountId, newBrand);
        }
    }
}
